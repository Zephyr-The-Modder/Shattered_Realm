using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using ShatteredRealm.Content.Items.Ammos;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;


namespace ShatteredRealm.Content.Items.Weapons.Ranged.Bows
{
	public class VerdantBow : ModItem
	{
		public override void SetDefaults()
		{
			// Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

			// Common Properties
			Item.width = 20; // Hitbox width of the item.
			Item.height = 32; // Hitbox height of the item.
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 27; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 27; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

			// The sound that this item plays when used.

			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 11; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 5f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.

			// Gun Properties
			Item.shoot = ProjectileID.PurificationPowder; // For some reason, all the guns in the vanilla source have this.
			Item.shootSpeed = 11f; // The speed of the projectile (measured in pixels per frame.)
			Item.useAmmo = AmmoID.Arrow; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.
			Item.UseSound = SoundID.Item5;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.rand.Next(1, 3) == 1)
            {
				const int NumProjectiles = 1; // The humber of projectiles that this gun will shoot.
				float rotate = 26.5f;

				for (int i = 0; i < NumProjectiles; i++)
				{
					// Rotate the velocity randomly by 30 degrees at max.
					Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(rotate));
					newVelocity *= 1f - Main.rand.NextFloat(0.15f);
					// Create a projectile.
					Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<FlowerProjectile>(), damage, knockback, player.whoAmI, type);
					rotate += rotate * 2;
				}
			}

			return true; // Return false because we don't want tModLoader to shoot projectile
		}
		// This method lets you adjust position of the gun in the player's hands. Play with these values until it looks good with your graphics.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(2f, -2f);
		}

		//TODO: Move this to a more specifically named example. Say, a paint gun?
		public class FlowerProjectile : ModProjectile
		{
			int StateTimer = 0;
			public override void SetStaticDefaults()
			{
				ProjectileID.Sets.TrailCacheLength[Projectile.type] = 11;
				ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			}
			public override void SetDefaults()
			{
				Projectile.width = 24; // The width of projectile hitbox
				Projectile.height = 26; // The height of projectile hitbox
				Projectile.light = 0.45f;

				// Ccopy the ai of any given projectile using AIType, since we want
				// the projectile to essentially behave the same way as the vanilla projectile.
				AIType = ProjectileID.Bullet;

				Projectile.friendly = true; // Can the projectile deal damage to enemies?
				Projectile.DamageType = DamageClass.Ranged; // Is the projectile shoot by a ranged weapon?
				Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
				Projectile.tileCollide = false; // Can the projectile collide with tiles?
				Projectile.timeLeft = 60; // Each update timeLeft is decreased by 1. Once timeLeft hits 0, the Projectile will naturally despawn. (60 ticks = 1 second)

				Projectile.penetrate = 1;
				Projectile.ArmorPenetration = 8;
				// 1: Projectile.penetrate = 1; // Will hit even if npc is currently immune to player
				// 2a: Projectile.penetrate = -1; // Will hit and unless 3 is use, set 10 ticks of immunity
				// 2b: Projectile.penetrate = 3; // Same, but max 3 hits before dying
				// 5: Projectile.usesLocalNPCImmunity = true;
				// 5a: Projectile.localNPCHitCooldown = -1; // 1 hit per npc max
				// 5b: Projectile.localNPCHitCooldown = 20; // 20 ticks before the same npc can be hit again
			}
			public override void AI()
			{
				StateTimer++;
				Projectile.rotation += 2;
				Projectile.velocity *= 0.96f;
			}
			Vector2 StartingVelocity;
            public override void OnSpawn(IEntitySource source)
            {
				
            }
            public override void OnKill(int timeLeft)
			{
				SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
				for (int i = 0; i < 18; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleTorch, 0f, 0f, 100, default, 3f);
					dust.noGravity = true;
					dust.velocity *= 5f;
					dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.JungleTorch, 0f, 0f, 100, default, 2f);
					dust.velocity *= 3f;
				}

				// Large Smoke Gore spawn

				for (int g = 0; g < 1; g++)
				{
					var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
					Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
					gore.scale = 1.0f;
					gore.velocity.X += 1.5f;
					gore.velocity.Y += 1.5f;
					gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
					gore.scale = 1.0f;
					gore.velocity.X -= 1.5f;
					gore.velocity.Y += 1.5f;
					gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
					gore.scale = 1.0f;
					gore.velocity.X += 1.5f;
					gore.velocity.Y -= 1.5f;
					gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
					gore.scale = 1.0f;
					gore.velocity.X -= 1.5f;
					gore.velocity.Y -= 1.5f;
				}
				
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
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(Projectile.width / 2, Projectile.height / 2); ;
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
		}
	}
}