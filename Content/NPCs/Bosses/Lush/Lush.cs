using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using ShatteredRealm.Content.Globals;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ShatteredRealm.Content.Items.Accessories.Lush;
using ShatteredRealm.Content.Items.Armor;
using ShatteredRealm.Content.Items.Weapons.Mage.Books;
using ShatteredRealm.Content.Items.Weapons.Melee.Swords;
using ShatteredRealm.Content.Items.Weapons.Ranged.Bows;
using ShatteredRealm.Content.Items.Weapons.Ranged.Flamethrowers;
using System.Collections.Generic;

namespace ShatteredRealm.Content.NPCs.Bosses.Lush
{
    public class Lush : ModNPC
    {
        private enum ActionState
        {
            FloatTowardsPlayer,

            EightThorns,
            SporeDash,
            ThornyVines,
            TrappingVines,
            VineExplosion,
            Minions,

            OrbLeafCircle,
            OrbLeafShot,
            OrbCircleDash,
        }

        // Our texture is 36x36 with 2 pixels of padding vertically, so 38 is the vertical spacing.
        // These are for our benefit and the numbers could easily be used directly in the code below, but this is how we keep code organized.
        private enum Frame
        {

        }

        // These are reference properties. One, for example, lets us write AI_State as if it's NPC.ai[0], essentially giving the index zero our own name.
        // Here they help to keep our AI code clear of clutter. Without them, every instance of "AI_State" in the AI code below would be "npc.ai[0]", which is quite hard to read.
        // This is all to just make beautiful, manageable, and clean code.
        float AI_State;
        float AI_Timer;
        float bossPhase;
        Vector2 projVel;
        int atkUseCounter;
        int frame;
        int frameCounter;
        Vector2 targetArea;
        Vector2 direction;
        Vector2 storedVel;
        Vector2 storedPos;
        Vector2 recoil;
        List<Vector2> playerPos = new List<Vector2> { };

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flutter Slime"); // Automatic from localization files
            Main.npcFrameCount[NPC.type] = 5; // make sure to set this for your modnpcs.

            // Specify the debuffs it is immune to
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;

            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.width = 64; // The width of the npc's hitbox (in pixels)
            NPC.height = 64; // The height of the npc's hitbox (in pixels)
            NPC.aiStyle = -1; // This npc has a completely unique AI, so we set this to -1. The default aiStyle 0 will face the player, which might conflict with custom AI code.
            NPC.damage = 34; // The amount of damage that this npc deals
            NPC.defense = 55; // The amount of defense that this npc has
            NPC.lifeMax = 28000; // The amount of health that this npc has
            NPC.HitSound = SoundID.NPCHit3; // The sound the NPC will make when being hit.
            NPC.DeathSound = SoundID.NPCDeath52; // The sound the NPC will make when it dies.
            NPC.value = 500000; // How many copper coins the NPC will drop when killed.
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.npcSlots = 100f;
            NPC.noTileCollide = true;

            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/CandescenceTheme");
            }
            //NPC.BossBar = ModContent.GetInstance<TreeToadBossBar>();

        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				new FlavorTextBestiaryInfoElement("Mods.AwfulGarbageMod.Bestiary.Candescence")
            });
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = 34000;
            if (Main.masterMode)
            {
                NPC.lifeMax = 40000; // Increase by 5 if expert or master mode
                if (Main.getGoodWorld || Main.zenithWorld)
                {
                    NPC.lifeMax = 50000;
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else

            // Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
           // npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<>()));


            // Trophies are spawned with 1/10 chance
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<>(), 10));

            // ItemDropRule.MasterModeCommonDrop for the relic
            //npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<>()));

            // ItemDropRule.MasterModeDropOnAllPlayers for the pet
            ///npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MinionBossPetItem>(), 4));

            // All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            notExpertRule.OnSuccess(ItemDropRule.FewFromOptions(4, 1, ModContent.ItemType<SwiftTrinketofPetals>(), ModContent.ItemType<VerdantBoosterShot>(), ModContent.ItemType<ViperBreastplate>(), ModContent.ItemType<ViperHelmet>(), ModContent.ItemType<ViperLeggings>(), ModContent.ItemType<VerdantStaff>(), ModContent.ItemType<SwiftTrinketofPetals>(), ModContent.ItemType<VerdantBlade>(), ModContent.ItemType<VerdantBow>(), ModContent.ItemType<PlantBurner>()));

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            // Boss masks are spawned with 1/7 chance
            ///notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MinionBossMask>(), 7));

            // This part is not required for a boss and is just showcasing some advanced stuff you can do with drop rules to control how items spawn
            // We make 12-15 ExampleItems spawn randomly in all directions, like the lunar pillar fragments. Hereby we need the DropOneByOne rule,
            // which requires these parameters to be defined
            ///int itemType = ModContent.ItemType<ExampleItem>();
            ///var parameters = new DropOneByOne.Parameters()
            ///{
            ///    ChanceNumerator = 1,
            ///    ChanceDenominator = 1,
            ///    MinimumStackPerChunkBase = 1,
            ///    MaximumStackPerChunkBase = 1,
            ///    MinimumItemDropsCount = 12,
            ///    MaximumItemDropsCount = 15,
            ///};

            ///notExpertRule.OnSuccess(new DropOneByOne(itemType, parameters));

            // Finally add the leading rule
            npcLoot.Add(notExpertRule);
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void OnKill()
        {
            // Since this hook is only ran in singleplayer and serverside, we would have to sync it manually.
            // Thankfully, vanilla sends the MessageID.WorldData packet if a BOSS was killed automatically, shortly after this hook is ran

            // If your NPC is not a boss and you need to sync the world (which includes ModSystem, check DownedBossSystem), use this code:
            /*
			if (Main.netMode == NetmodeID.Server) {
				NetMessage.SendData(MessageID.WorldData);
			}
			*/
        }

        // Our AI here makes our NPC sit waiting for a player to enter range, jumps to attack, flutter mid-fall to stay afloat a little longer, then falls to the ground. Note that animation should happen in FindFrame
        public override void AI()
        {
            // The npc starts in the asleep state, waiting for a player to enter range

            Player player = Main.player[NPC.target];

            Lighting.AddLight(NPC.Center, Color.Orange.ToVector3() * 6.8f);


            if (player.dead)
            {
                NPC.position.Y += 999;
                NPC.EncourageDespawn(0);
                return;
            }

            if (bossPhase == 0)
            {
                NPC.TargetClosest(true);
                bossPhase = 1;
                AI_Timer = 0;
                AI_State = (float)ActionState.FloatTowardsPlayer;
            }

            switch (AI_State)
            {
                case (float)ActionState.FloatTowardsPlayer:
                    FloatTowardsPlayer();
                    break;
                case (float)ActionState.OrbLeafCircle:
                    OrbLeafCircle();
                    break;
                case (float)ActionState.OrbLeafShot:
                    OrbLeafShot();
                    break;
                case (float)ActionState.OrbCircleDash:
                    OrbCircleDash();
                    break;
                case (float)ActionState.EightThorns:
                    EightThorns();
                    break;
                case (float)ActionState.SporeDash:
                    SporeDash();
                    break;
                case (float)ActionState.ThornyVines:
                    ThornyVines();
                    break;
                case (float)ActionState.TrappingVines:
                    TrappingVines();
                    break;
                case (float)ActionState.VineExplosion:
                    VineExplosion();
                    break;
                case (float)ActionState.Minions:
                    Minions();
                    break;
            }
        }
        // Here in FindFrame, we want to set the animation frame our npc will use depending on what it is doing.
        // We set npc.frame.Y to x * frameHeight where x is the xth frame in our spritesheet, counting from 0. For convenience, we have defined a enum above.
        public override void FindFrame(int frameHeight)
        {
            // This makes the sprite flip horizontally in conjunction with the npc.direction.
            NPC.spriteDirection = NPC.direction;
            frameCounter++;
            if (frameCounter > 5)
            {
                frameCounter = 0;
                frame++;
                if (frame >= Main.npcFrameCount[NPC.type])
                {
                    frame = 0;
                }
            }

            NPC.frame.Y = frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            if (true)
            {
                Vector2 drawOrigin = NPC.frame.Size() / 2;
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Vector2 drawPos = NPC.oldPos[k] - screenPos + new Vector2(NPC.width / 2, NPC.height / 2) + new Vector2(0, NPC.gfxOffY); //.RotatedBy(NPC.rotation);
                    Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
                    spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos + new Vector2(0, -6), NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, spriteEffects, 0f);
                }
            }


            return true;
        }
        // Here, because we use custom AI (aiStyle not set to a suitable vanilla value), we should manually decide when Flutter Slime can fall through platforms
        public override bool? CanFallThroughPlatforms()
        {
            return true;
            // You could also return null here to apply vanilla behavior (which is the same as false for custom AI)
        }


        private void FloatTowardsPlayer()
        {
            Player player = Main.player[NPC.target];

            Vector2 direction = player.Center - NPC.Center;
            NPC.velocity = direction * 0.015f + direction.SafeNormalize(Vector2.Zero) * -5f;
            NPC.dontTakeDamage = true;
            AI_Timer += 1;
            if (AI_Timer == 180)
            {
                NPC.dontTakeDamage = false;
                AI_State = (float)ActionState.OrbLeafCircle;
                AI_Timer = 0;
                atkUseCounter = 0;
            }
        }
        private void OrbLeafCircle()
        {

            AI_Timer += 1;
            if (AI_Timer > 0)
            {

                if (AI_Timer % 90 == 0)
                {
                    atkUseCounter++;
                    if (atkUseCounter == 7)
                    {
                        AI_State = (float)ActionState.OrbLeafShot;
                        AI_Timer = 0;
                        atkUseCounter = 0;
                        playerPos.Clear();
                    }
                }
            }
        }
        private void OrbLeafShot()
        {
            AI_Timer += 1;
            if (AI_Timer == 480)
            {
                AI_State = (float)ActionState.OrbCircleDash;
                AI_Timer = 0;
                atkUseCounter = 0;
            }
        }
        private void OrbCircleDash()
        {
            if (AI_Timer == 120)
            {
                if (atkUseCounter == 5)
                {

                    NPC.velocity = new Vector2(0, 0);
                    AI_State = (float)ActionState.OrbLeafCircle;
                    AI_Timer = 0;
                    atkUseCounter = 0;

                }
                AI_Timer = 0;
            }
            AI_Timer++;
        }
        private void EightThorns()
        {
            AI_Timer += 1;
            int dmg = 25;
            if (Main.expertMode)
            {
                dmg = 40;
            }
            if (Main.masterMode)
            {
                dmg = 65;
            }
            if (AI_Timer == 480)
            {
                for (int i = 0; i < 360; i += 45)
                {
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(i)), ProjectileID.Stinger, dmg, 1.5f);
                }
                AI_State = (float)ActionState.OrbCircleDash;
                AI_Timer = 0;
            }
        }
        private void SporeDash()
        {

        }
        private void ThornyVines()
        {

        }
        private void TrappingVines()
        {

        }
        private void VineExplosion()
        {

        }
        private void Minions()
        {

        }
    }
}
