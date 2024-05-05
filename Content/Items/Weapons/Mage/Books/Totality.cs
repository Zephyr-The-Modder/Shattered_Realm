using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShatteredRealm.Content.Items.Weapons.Mage.Books
{

    public class Totality : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Acorn Staff"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("Hitting an ememy restores all used mana");
		}

        public override void SetDefaults()
        {
            Item.damage = 112;
            Item.mana = 24;
            Item.DamageType = DamageClass.Magic;
            Item.width = 42;
            Item.height = 46;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.reuseDelay = 175;
            Item.useStyle = 5;
            Item.knockBack = 0.5f;
            Item.value = 10000;
            Item.rare = 6;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<TotalityProj>();
            Item.shootSpeed = 6f;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(8)), type, damage, knockback, player.whoAmI);
            return false;
        }
    }

    public class TotalityProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wind"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 800;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        int StateTimer = 0;
        float turn = 0;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 800;
            Projectile.light = 1f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.extraUpdates = 3;
            Projectile.alpha = 255;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and verticalliy
            SpriteEffects spriteEffects = SpriteEffects.None;

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;
            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);

            Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = origin;
            spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length && k < StateTimer; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(4f, 4f);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                color.A = (byte)(color.A * 0.75f);
                Main.spriteBatch.Draw(projectileTexture, drawPos, sourceRectangle, color, Projectile.oldRot[k], drawOrigin, Projectile.scale - k * 0.02f, spriteEffects, 0f);
            }

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SparksMech, oldVelocity.X, oldVelocity.Y, 0, default(Color), 1f);
            Main.dust[dust].noGravity = true;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 120);
        }
        int timer = 0;
        public override void AI()
        {
            float speed = Projectile.Center.Distance(Main.MouseWorld) / 50;
            Projectile.velocity = Projectile.Center.DirectionTo(Main.MouseWorld) * speed;
            if (timer % 2 == 0)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.OrangeTorch, Vector2.Zero, Scale: 2f);
            }
            if (timer == 60)
            {
                if (speed > 3)
                {
                    for (int i = 0; i < 360; i += 4)
                    {
                        Vector2 CappedVelocity = Projectile.velocity;
                        MathHelper.Clamp(CappedVelocity.X, -3, 3);
                        MathHelper.Clamp(CappedVelocity.Y, -2, 2);
                        Vector2 randCircle = Vector2.One.RotatedBy(MathHelper.ToRadians(i));
                        Dust.NewDustPerfect(Projectile.Center, DustID.OrangeTorch, randCircle * CappedVelocity.RotatedBy(90) / 4, Scale: 2f);
                    }
                }
                timer = 0;
            }
            timer++;
        }
    }

}