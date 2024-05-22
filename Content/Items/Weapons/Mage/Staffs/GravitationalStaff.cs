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

namespace ShatteredRealm.Content.Items.Weapons.Mage.Staffs
{

    public class GravitationalStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Acorn Staff"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("Hitting an ememy restores all used mana");
            Item.staff[Type] = true;
		}

        public override void SetDefaults()
        {
            Item.damage = 26;
            Item.mana = 3;
            Item.DamageType = DamageClass.Magic;
            Item.width = 42;
            Item.height = 46;
            Item.useTime = 29;
            Item.useAnimation = 29;
            Item.useStyle = 5;
            Item.knockBack = 0.5f;
            Item.value = 10000;
            Item.rare = 6;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GravitationalRock>();
            Item.shootSpeed = 5f;
            Item.noMelee = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                const int NumProjectiles = 3; // The humber of projectiles that this gun will shoot.
                int aiRand = Main.rand.Next(1, 8);
                for (int i = 0; i < NumProjectiles; i++)
                {
                    // Rotate the velocity randomly by 30 degrees at max.
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                    newVelocity *= 1f - Main.rand.NextFloat(0.15f);
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI, aiRand);
                }
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(4)), type, (int)(damage * 2.5f), knockback, player.whoAmI, 8);
            }
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 48f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
    }

    public class GravitationalRock : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wind"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 54;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Type] = 8;
        }

        string colorAI = "black";
        int StateTimer = 0;
        float turn = 0;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.light = 1f;
            Projectile.penetrate = 3;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 3;
            Projectile.alpha = 255;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] <= 1)
            {
                colorAI = "black";
                Projectile.frame = 0;
            }
            if (Projectile.ai[0] == 2)
            {
                colorAI = "purple";
                Projectile.frame = 1;
            }
            if (Projectile.ai[0] == 3)
            {
                colorAI = "pink";
                Projectile.frame = 2;
            }
            if (Projectile.ai[0] == 4)
            {
                colorAI = "red";
                Projectile.frame = 3;
            }
            if (Projectile.ai[0] == 5)
            {
                colorAI = "lightpurple";
                Projectile.frame = 4;
            }
            if (Projectile.ai[0] == 6)
            {
                colorAI = "yellow";
                Projectile.frame = 5;
            }
            if (Projectile.ai[0] == 7)
            {
                colorAI = "white";
                Projectile.frame = 6;
            }
            if (Projectile.ai[0] >= 8)
            {
                colorAI = "rainbow";
                Projectile.frame = 7;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * Projectile.Opacity;
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
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

        int Timer = 0;
        int DustTimer = 0;
        public override void AI()
        {
            switch (colorAI)
            {
                case "black":
                    Projectile.velocity *= 1.01f;
                    if (DustTimer >= 4)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DemonTorch);
                        DustTimer = 0;
                    }
                    break;
                case "purple":
                    Projectile.velocity.Y += 0.01f;
                    if (DustTimer >= 4)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Purple);
                        DustTimer = 0;
                    }
                    break;
                case "pink":
                    Projectile.velocity.Y -= 0.01f;
                    if (DustTimer >= 4)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkFairy);
                        DustTimer = 0;
                    }
                    break;
                case "red":
                    if (DustTimer >= 4)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Red);
                        DustTimer = 0;
                    }
                    Projectile.velocity *= 0.99f;
                    break;
                case "lightpurple":
                    if (DustTimer >= 4)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch);
                        DustTimer = 0;
                    }
                    if (Timer == 30)
                    {
                        Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5));
                        Timer = 0;
                    }
                    Timer++;
                    break;
                case "yellow":
                    if (DustTimer >= 4)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowStarDust);
                        DustTimer = 0;
                    }
                    if (Timer == 300)
                    {
                        Projectile.velocity *= -1;
                    }
                    Timer++;
                    break;
                case "white":
                    if (DustTimer >= 4)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch);
                        DustTimer = 0;
                    }
                    if (Timer == 20)
                    {
                        Projectile.damage++;
                        Timer = 0;
                    }
                    Timer++;
                    break;
                case "rainbow":
                    if (DustTimer == 1)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
                    }
                    if (DustTimer == 3)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch);
                    }
                    if (DustTimer == 5)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch);
                    }
                    if (DustTimer == 7)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch);
                    }
                    if (DustTimer == 9)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueTorch);
                    }
                    if (DustTimer >= 11)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
                        DustTimer = 0;
                    }
                    Projectile.velocity *= 1.02f;
                    Projectile.scale *= 1.01f;
                    break;

            }
            DustTimer++;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (colorAI)
            {
                case "black":
                    Projectile.velocity *= 0.65f;
                    target.AddBuff(BuffID.Poisoned, 500);
                    break;
                case "purple":
                    target.AddBuff(BuffID.Slow, 1080);
                    Projectile.velocity.Y *= 1.35f;
                    break;
                case "pink":
                    target.AddBuff(BuffID.Confused, 360);
                    break;
                case "red":
                    target.AddBuff(BuffID.OnFire, 500);
                    Projectile.velocity *= 0.95f;
                    break;
                case "lightpurple":
                    if (Timer == 30)
                    {
                        Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(5));
                        Timer = 0;
                    }
                    target.AddBuff(BuffID.OnFire3, 900);
                    Timer++;
                    break;
                case "yellow":
                    target.AddBuff(BuffID.Ichor, 500);
                    if (Timer == 300)
                    {
                        Projectile.velocity *= -1;
                    }
                    Timer++;
                    break;
                case "white":

                    if (Timer == 20)
                    {
                        Projectile.damage++;
                        Timer = 0;
                    }
                    Timer++;
                    break;
                case "rainbow":
                    Projectile.damage += 5;
                    break;

            }
        }
    }

}