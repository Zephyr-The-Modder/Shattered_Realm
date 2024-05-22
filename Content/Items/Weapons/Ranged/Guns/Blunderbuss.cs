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
    public class Blunderbuss : ModItem
    {
        public override void SetDefaults()
        {
            // Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

            // Common Properties
            Item.width = 34; // Hitbox width of the item.
            Item.height = 14; // Hitbox height of the item.
            Item.rare = ItemRarityID.LightPurple; // The color that the item's name will be in-game.

            // Use Properties
            Item.useTime = 36; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useAnimation = 36; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = false; // Whether or not you can hold click to automatically use it again.
            Item.reuseDelay = 40;

            // The sound that this item plays when used.

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
            Item.damage = 4; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            Item.knockBack = 7f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            Item.noMelee = true; // So the item's animation doesn't do damage.

            // Gun Properties
            Item.shoot = ProjectileID.SuperStar; // For some reason, all the guns in the vanilla source have this.
            Item.shootSpeed = 6.5f; // The speed of the projectile (measured in pixels per frame.)
            Item.UseSound = SoundID.Item41;
            Item.useAmmo = AmmoID.Bullet;
            Item.blunderbussItem().reloadShots = 8;
            Item.blunderbussItem().reloadTime = 4;
            Item.blunderbussItem().isShotgun = true;
        }

        int shotNum = 1;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item41, player.position);
            Vector2 newVelocity = velocity;
            int NumProjectiles = 3; // The humber of projectiles that this gun will shoot.
            for (int i = 0; i < NumProjectiles; i++)
            {
                // Rotate the velocity randomly by 30 degrees at max.
                newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(13));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }

            return false; // Return false because we don't want tModLoader to shoot projectile
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 42f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10f, -2f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddRecipeGroup("Wood", 32);
            recipe.AddRecipeGroup("IronBar", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}
