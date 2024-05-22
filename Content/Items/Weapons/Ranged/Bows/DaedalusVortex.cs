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
	public class DaedalusVortex : ModItem
	{
		public override void SetDefaults()
		{
			// Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

			// Common Properties
			Item.width = 20; // Hitbox width of the item.
			Item.height = 32; // Hitbox height of the item.
			Item.rare = ItemRarityID.Green; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 5; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 5; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

			// The sound that this item plays when used.

			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 95; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
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
			int NumProjectiles = Main.rand.Next(2, 5); // The humber of projectiles that this gun will shoot.
			Vector2 playerPosition = player.Center;
			playerPosition.Y -= 750;

			for (int i = 0; i < NumProjectiles; i++)
			{
				// Rotate the velocity randomly by 30 degrees at max.
				Vector2 newVelocity = playerPosition.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.ToRadians(15)) * 18;

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.6f);
				if (Main.rand.NextBool(9))
                {
					Projectile.NewProjectileDirect(source, playerPosition, newVelocity, ProjectileID.HallowStar, damage * 3, knockback, player.whoAmI);
				}
				else
                {
					newVelocity = playerPosition.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.ToRadians(8)) * 25;
					Projectile.NewProjectileDirect(source, playerPosition, newVelocity, type, damage, knockback, player.whoAmI);
				}
				// Create a projectile.
				
			}

			return false; // Return false because we don't want tModLoader to shoot projectile
		}
		// This method lets you adjust position of the gun in the player's hands. Play with these values until it looks good with your graphics.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(2f, -2f);
		}
	}


	//TODO: Move this to a more specifically named example. Say, a paint gun?

}
