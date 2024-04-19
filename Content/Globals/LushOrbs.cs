using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ShatteredRealm.Content.Items.Weapons.Ranged.Flamethrowers;
using ShatteredRealm.Content.Buffs;
using ShatteredRealm.Content.NPCs.Bosses.Lush;

namespace ShatteredRealm.Content.Globals
{
	public class LushOrbs : GlobalNPC
	{
        public int attackNumber = 0; 
        public override void AI(NPC npc)
        {
            if (npc.type == ModContent.NPCType<VerdantOrb>())
            {
                npc.TargetClosest();
                Player player = Main.player[npc.target];
                if (attackNumber == 0)
                {
                    npc.position = player.Center;
                }
            }

        }

    }
}
