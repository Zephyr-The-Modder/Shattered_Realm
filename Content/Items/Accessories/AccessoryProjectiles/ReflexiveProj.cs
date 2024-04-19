using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.Items.Accessories.AccessoryProjectiles
{

    public class ReflexiveProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wind"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
        }

        int StateTimer = 0;
        float turn = 0;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Default;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.light = 1f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.extraUpdates = 29;
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.damage = (int)(Projectile.damage * 0.95f);
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            int dust; 
            dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0, 0, 0, player.GetModPlayer<ShatteredPlayer>().shieldBreakColor, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2);
            Main.dust[dust].scale = Main.rand.NextFloat(1.1f, 1.3f);

            dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0, 0, 0, player.GetModPlayer<ShatteredPlayer>().shieldBreakColor, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Projectile.velocity.RotatedBy(-MathHelper.PiOver2);
            Main.dust[dust].scale = Main.rand.NextFloat(1.1f, 1.3f);
        }
    }
}
