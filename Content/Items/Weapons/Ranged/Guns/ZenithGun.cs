using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;

namespace ShatteredRealm.Content.Items.Weapons.Ranged.Guns
{
	public class ZenithGun : ModItem
	{
		public override void SetDefaults()
		{
			// Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

			// Common Properties
			Item.width = 38; // Hitbox width of the item.
			Item.height = 20; // Hitbox height of the item.
			Item.rare = ItemRarityID.Red; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 7; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 7; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

			// The sound that this item plays when used.

			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 49; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 1f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.

			// Gun Properties
			Item.shoot = ProjectileID.SuperStar; // For some reason, all the guns in the vanilla source have this.
			Item.shootSpeed = 15f; // The speed of the projectile (measured in pixels per frame.)
			Item.UseSound = SoundID.Item41;
			Item.useAmmo = AmmoID.Bullet;
		}

		int shotNum = 1;
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(3, 8) == 3)
            {
				return true;
            }
			else
            {
				return false;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity;
			const int NumProjectiles = 5; // The humber of projectiles that this gun will shoot.
			const int NumStars = 3; // The humber of stars that this gun will shoot.
			const int NumRockets = 2; // The humber of stars that this gun will shoot.
			Projectile.NewProjectileDirect(source, position, newVelocity, ProjectileID.BlackBolt, damage, knockback, player.whoAmI);
			for (int i = 0; i < NumProjectiles; i++)
			{
				// Rotate the velocity randomly by 30 degrees at max.
				newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(13));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.3f);

				// Create a projectile.
				Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
			}
			newVelocity = velocity;
			for (int p = 0; p < NumStars; p++)
			{
				// Rotate the velocity randomly by 30 degrees at max.
				newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.1f);

				// Create a projectile.
				Projectile.NewProjectileDirect(source, position, newVelocity, ProjectileID.SuperStar, 85, knockback, player.whoAmI);
			}
			shotNum += 1;

			return false; // Return false because we don't want tModLoader to shoot projectile
		}

	}
}
