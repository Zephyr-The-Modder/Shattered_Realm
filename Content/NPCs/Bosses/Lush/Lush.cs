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
    public class Lush : ModNPC
    {
        public override void SetStaticDefaults()
        {

        }
        int LushPhase = 1;
        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.aiStyle = 0;
            NPC.height = 40;
            NPC.damage = 67;
            NPC.defense = 15;
            NPC.lifeMax = 2300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.value = 120000f;
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
            for (int i = 0; i < 8; i++)
            {
                NPC.NewNPCDirect(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<VerdantOrb>(), 0, i);
            }
  
        }

        int Timer = 0;
        int rand;
        public override void AI()
        {
            if (LushPhase == 1)
            {
                if (Timer == 80)
                {
                    rand = Main.rand.Next(1, 6);
                    randAtkPhase1(rand);
                    Timer = 0;
                }
            }
            Timer++;
        }
        public void randAtkPhase1(int rand)
        {
            if (rand == 1)
            {
                Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().CoordinatedDashOrbRand = Main.rand.Next(1, 9);
            }
            if (rand == 2)
            {
                Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().CoordinatedDashOrbRand = Main.rand.Next(1, 9);
            }
        }



    }
}
