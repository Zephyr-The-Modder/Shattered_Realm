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
	public class LaserPistol : ModItem
	{
		public override void SetDefaults()
		{
			// Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

			// Common Properties
			Item.width = 34; // Hitbox width of the item.
			Item.height = 14; // Hitbox height of the item.
			Item.rare = ItemRarityID.LightPurple; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 12; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 12; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.autoReuse = false; // Whether or not you can hold click to automatically use it again.
			Item.channel = true;
			Item.reuseDelay = 8;

			// The sound that this item plays when used.

			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 294; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 4f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.

			// Gun Properties
			Item.shoot = ProjectileID.SuperStar; // For some reason, all the guns in the vanilla source have this.
			Item.shootSpeed = 8f; // The speed of the projectile (measured in pixels per frame.)
			Item.UseSound = SoundID.Item41;
			Item.useAmmo = AmmoID.Bullet;
		}

		int shotNum = 1;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LaserPistolDummy>(), damage, knockback, player.whoAmI, Item.shootSpeed, type, Item.shootSpeed);
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			Vector2 offset = new Vector2(0, 0);
			return offset;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 42f;

			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
			recipe.AddIngredient(ItemID.OnyxBlaster, 1);
			recipe.AddIngredient(ItemID.Boomstick, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 3);
			recipe.AddIngredient(ItemID.SoulofMight, 3);
			recipe.AddIngredient(ItemID.SoulofSight, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

	}
	public class LaserPistolDummy : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("a"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
		}

		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = 9;
			Projectile.timeLeft = 3;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
		}
		int ChargeTime = 0;
		int chargeLevel = 0;

		public override bool PreAI()
		{
			Player owner = Main.player[Projectile.owner];

			// Like other whips, this whip updates twice per frame (Projectile.extraUpdates = 1), so 120 is equal to 1 second.
			Projectile.position = owner.Center;
			if (!owner.channel)
			{
				return true; // Let the vanilla whip AI run.
			}
			Projectile.timeLeft = 3;
			Projectile.rotation = (Main.MouseWorld - Projectile.position).ToRotation();

			if (ChargeTime % owner.HeldItem.useTime == 0) // 1 segment per 12 ticks of charge.
			{
				if (chargeLevel < 4)
				{
					chargeLevel++;
				}
				if (chargeLevel == 4)
                {
					for (int i = 0; i<10; i++)
                    {
						Dust.NewDustPerfect(owner.Center, DustID.GemRuby);
                    }
					chargeLevel = 5;
				}
			}
			ChargeTime++;
			// Reset the animation and item timer while charging.
			owner.itemAnimation = owner.itemAnimationMax;
			owner.itemTime = owner.itemTimeMax;
			owner.itemRotation = Projectile.rotation;
			if (owner.Center.X < Main.MouseWorld.X)
			{
				owner.direction = (int)MathHelper.ToRadians(90);
			}
			else
			{
				owner.direction = (int)MathHelper.ToRadians(-90);
				owner.itemRotation += (int)MathHelper.ToRadians(180);
			}


			return false; // Prevent the vanilla whip AI from running.
		}
		public override void OnKill(int timeLeft)
		{
			Player owner = Main.player[Projectile.owner];
			if (chargeLevel == 5)
            {
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, owner.Center.DirectionTo(SRUtils.GetClosestNPC(owner.Center, 1400).Center) * (Projectile.ai[2] * 2), (int)Projectile.ai[1], (int)(Projectile.damage * 2.25f), Projectile.knockBack, Projectile.owner);
			}
			else
            {
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Projectile.DirectionTo(Main.MouseWorld) * Projectile.ai[2], (int)Projectile.ai[1], Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			
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
			Projectile.penetrate = 7;
			Projectile.timeLeft = 999999;
			Projectile.light = 1f;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 3;
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
							break;
						}
						else
                        {
							continue;
                        }

					}
				}
			}
			if (targetWithLineOfSight == -1)
            {
				Projectile.Kill();
            }

			return targetWithLineOfSight;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int enemy1 = NewTarget(700f);
			if (enemy1 != -1)
            {
				enemy = Main.npc[enemy1];
				Projectile.velocity = Projectile.DirectionTo(enemy.Center) * 12f;
			}
			Projectile.damage = (int)(Projectile.damage * 0.945f);
		}

		public override void AI()
		{
			StateTimer++;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

	}
}
