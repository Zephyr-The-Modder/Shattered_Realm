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

namespace ShatteredRealm.Content.Items.Weapons.Ranged.Guns
{
	public class ApoideaBlaster : ModItem
	{
		public override void SetDefaults()
		{
			// Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

			// Common Properties
			Item.width = 26; // Hitbox width of the item.
			Item.height = 13; // Hitbox height of the item.
			Item.rare = ItemRarityID.Yellow; // The color that the item's name will be in-game.

			// Use Properties
			Item.useTime = 36; // The item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 36; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

			// The sound that this item plays when used.

			// Weapon Properties
			Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
			Item.damage = 18; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
			Item.knockBack = 4f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
			Item.noMelee = true; // So the item's animation doesn't do damage.

			// Gun Properties
			Item.shoot = ProjectileID.SuperStar; // For some reason, all the guns in the vanilla source have this.
			Item.shootSpeed = 14f; // The speed of the projectile (measured in pixels per frame.)
			Item.UseSound = SoundID.Item41;
			Item.useAmmo = AmmoID.Bullet;
			Item.blunderbussItem().reloadShots = 15;
			Item.blunderbussItem().reloadTime = 5;
			Item.blunderbussItem().isShotgun = true;
		}

		int shotNum = 1;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity;
			int NumProjectiles = Main.rand.Next(4, 9); // The humber of projectiles that this gun will shoot.
			for (int i = 0; i < NumProjectiles; i++)
			{
				// Rotate the velocity randomly by 30 degrees at max.
				newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(9));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.15f);

				if (Main.rand.NextBool(4))
				{
					Projectile.NewProjectileDirect(source, position, newVelocity, ProjectileID.Bee, (int)(damage * 1.3), knockback, player.whoAmI);
				}
				else
                {
					Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
				}
				// Create a projectile.
				
			}

			return false; // Return false because we don't want tModLoader to shoot projectile
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 42f;
			if (type == ProjectileID.Bullet)
            {
				if (Main.rand.NextBool())
                {
					type = ModContent.ProjectileType<StingerProjectile>();
				}
            }

		}
        public override Vector2? HoldoutOffset()
        {
			return new Vector2(2f, -2f);
		}

        public override void AddRecipes()
        {
			CreateRecipe()
			.AddIngredient(ItemID.Boomstick)
			.AddIngredient(ItemID.BeeWax, 12)
			.AddTile(TileID.WorkBenches)
			.Register();
        }

    }
    public class StingerProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Default;
            Projectile.width = 5;
            Projectile.height = 9;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 900;
            Projectile.light = 1f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.JungleGrass, oldVelocity.X, oldVelocity.Y, 0, default(Color), 1f);
            Main.dust[dust].noGravity = true;
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 120);
        }

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

		}
    }
}
