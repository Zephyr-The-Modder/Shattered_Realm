using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.NPCs.Bosses.Lush
{
    // Party Zombie is a pretty basic clone of a vanilla NPC. To learn how to further adapt vanilla NPC behaviors, see https://github.com/tModLoader/tModLoader/wiki/Advanced-Vanilla-Code-Adaption#example-npc-npc-clone-with-modified-projectile-hoplite
    public class VerdantOrb : ModNPC
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.aiStyle = 0;
            NPC.height = 40;
            NPC.damage = 34;
            NPC.defense = 5;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 0f;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 3; // Fighter AI, important to choose the aiStyle that matches the NPCID that we want to mimic
            NPC.noGravity = true;

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            npcLoot.Add(ItemDropRule.Common(ItemID.Confetti, 100)); // 1% chance to drop Confetti
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (NPC.ai[0] == 0)
            {
                NPC.velocity.X = 4;
                NPC.velocity.Y = 4;
            }
            if (NPC.ai[0] == 1)
            {
                NPC.velocity.X = 6;
                NPC.velocity.Y = 2;
            }
            if (NPC.ai[0] == 2)
            {
                NPC.velocity.X = 6;
                NPC.velocity.Y = -2;
            }
            if (NPC.ai[0] == 3)
            {
                NPC.velocity.X = 4;
                NPC.velocity.Y = -4;
            }
            if (NPC.ai[0] == 4)
            {
                NPC.velocity.X = -4;
                NPC.velocity.Y = 4;
            }
            if (NPC.ai[0] == 5)
            {
                NPC.velocity.X = -6;
                NPC.velocity.Y = 2;
            }
            if (NPC.ai[0] == 6)
            {
                NPC.velocity.X = -6;
                NPC.velocity.Y = -2;
            }
            if (NPC.ai[0] == 7)
            {
                NPC.velocity.X = -4;
                NPC.velocity.Y = -4;
            }
        }

        int Timer = 0;
        int rand;
        public override void AI()
        {
            NPC.TargetClosest();
            
            
            if (NPC.ai[0] == Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().CoordinatedDashOrbRand)
            {
                NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].position) * 8;
                Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().CoordinatedDashOrbRand = 0;
            }
            Timer++;
        }
        public void randAtk(int rand)
        {
            if (rand == 1)
            {

            }
        }



    }
}
