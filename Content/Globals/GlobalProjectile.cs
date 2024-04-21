using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ShatteredRealm.Content.Globals
{
    public class GlobalProjectiles : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.owner == Main.myPlayer)
            {
                if (player.GetModPlayer<ShatteredPlayer>().verdantSetBonus && Main.rand.Next(1, 7) == 1)
                {
                    if (Main.hardMode)
                    {
                        target.AddBuff(BuffID.Venom, 180);
                    }
                    else
                    {
                        target.AddBuff(BuffID.Poisoned, 300);
                    }

                }
                
            }
            if (player.GetModPlayer<ShatteredPlayer>().reversedSolar)
            {
                if (projectile.minion == true)
                {
                    target.AddBuff(BuffID.Daybreak, damageDone * 3);
                }
            }
        }
        

    }
}
