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

    public class GravitationalStaffUpgrade : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Acorn Staff"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("Hitting an ememy restores all used mana");
            Item.staff[Type] = true;
		}

        public override void SetDefaults()
        {
            Item.damage = 67;
            Item.mana = 3;
            Item.DamageType = DamageClass.Magic;
            Item.width = 42;
            Item.height = 46;
            Item.useTime = 13;
            Item.useAnimation = 13;
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
                const int NumProjectiles = 5; // The humber of projectiles that this gun will shoot.
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
            for (int i = 0; i < 3; i++)
            {
                Vector2 newPos = Main.MouseWorld;
                newPos.X += Main.rand.Next(-150, 151);
                newPos.Y += Main.rand.Next(-150, 151);
                Projectile.NewProjectile(source, newPos, newPos.DirectionTo(Main.MouseWorld) * 26, ProjectileID.SuperStar, (int)(damage * 1f), knockback, player.whoAmI);
                
            }
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
    }


}