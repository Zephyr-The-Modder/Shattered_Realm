using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Audio;
using ShatteredRealm;
using System.Collections.Generic;

namespace ShatteredRealm.Content.Items.Weapons.Ranged.Guns
{
    public class XLR8 : ModItem
    {

        public override void SetDefaults()
        {
            // Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

            // Common Properties
            Item.width = 36; // Hitbox width of the item.
            Item.height = 23; // Hitbox height of the item.
            Item.rare = ItemRarityID.Yellow; // The color that the item's name will be in-game.

            // Use Properties
            Item.useTime = 36; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useAnimation = 36; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

            // The sound that this item plays when used.

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
            Item.damage = 25; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            Item.knockBack = 4f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            Item.noMelee = true; // So the item's animation doesn't do damage.

            // Gun Properties
            Item.shoot = ProjectileID.SuperStar; // For some reason, all the guns in the vanilla source have this.
            Item.channel = true;
            Item.shootSpeed = 12f; // The speed of the projectile (measured in pixels per frame.)
            Item.UseSound = SoundID.Item41;
            Item.useAmmo = AmmoID.Bullet;

        }

        int shotNum = 1;

        // The following method gives this gun a 38% chance to not consume ammo
        public override void HoldItem(Player player)
        {
            Item.useTime = player.GetModPlayer<XLR8Player>().Speed;
            Item.useAnimation = player.GetModPlayer<XLR8Player>().Speed;
        }


        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.66f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.GetModPlayer<XLR8Player>().ResetTimer = 20;
            player.GetModPlayer<XLR8Player>().Timer++;
			int NumProjectiles = Main.rand.Next(1, 3);
            for (int i = 0; i < NumProjectiles; i++)
            {
				Vector2 newVelocity = velocity;
                if (NumProjectiles == 2)
                {
                    newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                    newVelocity *= 1f - Main.rand.NextFloat(0.3f);
                }
                // Rotate the velocity randomly by 30 degrees at max.

                if (NumProjectiles == 1)
                {
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<LaserBolt>(), (int)(damage * 0.75f), knockback, player.whoAmI);
                }
                if (NumProjectiles == 2)
                {
                    // Create a projectile.
                    Projectile.NewProjectileDirect(source, position, newVelocity, type, damage / 2, knockback, player.whoAmI);
                }
            }
            return false;
        }


        // The following method allows this gun to shoot when having no ammo, as long as the player has at least 10 example items in their inventory.
        // The gun will then shoot as if the default ammo for it, in this case the musket ball, is being used.

        // The following method makes the gun slightly inaccurate

        // This method lets you adjust position of the gun in the player's hands. Play with these values until it looks good with your graphics.
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6f, -2f);
        }

    }

    public class XLR8Player : ModPlayer
    {
        public int ResetTimer = 20;
        public int Speed = 15;
        public int Timer = 0;
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Timer >= 2)
            {
                if (Speed <= 2)
                {
                    Speed = 2;
                }
                else
                {
                    Speed--;
                }
                Timer = 0;
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override void PreUpdate()
        {
            if (ResetTimer <= 0)
            {
                ResetTimer = 0;
                Speed = 15;
                Timer = 0;
            }
            ResetTimer--;
        }


    }
    public class LaserBolt : ModProjectile
	{
		NPC enemy;
		List<int> enemiesHit = new List<int>();
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wind"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 48;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		int StateTimer = 0;
		float turn = 0;
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 9;
			Projectile.timeLeft = 999999;
			Projectile.light = 1f;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.extraUpdates = 3;
			Projectile.ArmorPenetration = 15;
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

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SparksMech, oldVelocity.X, oldVelocity.Y, 0, default(Color), 1f);
			Main.dust[dust].noGravity = true;
			// If collide with tile, reduce the penetrate.
			// So the projectile can reflect at most 5 times
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

				int enemy1 = NewTarget(700f);
				if (enemy1 == -1)
				{
                    if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                    {
                        Projectile.velocity.X = -oldVelocity.X;
                    }

                    // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                    if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y;
                    }
                }
				else
				{
                    enemy = Main.npc[enemy1];
                    Projectile.velocity = Projectile.DirectionTo(enemy.Center) * 12f;
                }
			}
			return false;
		}
		int NewTarget(float range)
		{
			float num1 = range;
			int targetWithLineOfSight = -1;

			for (int index = 0; index < 200; ++index)
			{
				NPC npc = Main.npc[index];
				bool flag = npc.CanBeChasedBy(Projectile);
				if (Projectile.localNPCImmunity[index] != 0)
					flag = false;

				if (flag)
				{
					float num2 = Projectile.Distance(Main.npc[index].Center);
					if ((double)num2 < (double)num1 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
					{
						if (!enemiesHit.Contains(npc.whoAmI))
						{
							num1 = num2;
							targetWithLineOfSight = npc.whoAmI;
							enemiesHit.Add(npc.whoAmI);
							break;
						}
						else
                        {
							continue;
                        }

					}
				}
			}

			return targetWithLineOfSight;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int enemy1 = NewTarget(700f);
			if (enemy1 == -1)
            {
				if (Projectile.penetrate == 9)
                {
					hit.Damage = (int)(hit.Damage * 1.75f);
                }
				Projectile.Kill();
            }
			Projectile.penetrate--;
			if (enemy1 != -1)
            {
				enemy = Main.npc[enemy1];
				Projectile.velocity = Projectile.DirectionTo(enemy.Center) * 12f;
			}
			Projectile.damage = (int)(Projectile.damage * 0.975f);
		}

		public override void AI()
		{
			StateTimer++;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

	}
}
