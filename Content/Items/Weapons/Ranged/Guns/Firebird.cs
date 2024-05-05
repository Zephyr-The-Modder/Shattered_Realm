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
using ShatteredRealm.Content.Items.Ammos;

namespace ShatteredRealm.Content.Items.Weapons.Ranged.Guns
{
    public class Firebird : ModItem
    {
        public override void SetDefaults()
        {
            // Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

            // Common Properties
            Item.width = 36; // Hitbox width of the item.
            Item.height = 23; // Hitbox height of the item.
            Item.rare = ItemRarityID.Yellow; // The color that the item's name will be in-game.

            // Use Properties
            Item.useTime = 7; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useAnimation = 7; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

            // The sound that this item plays when used.

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
            Item.damage = 31; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
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

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FirebirdDummy>(), 0, knockback, player.whoAmI, type, damage, knockback);
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 16.5f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, -2f);
        }

    }
    public class FirebirdDummy : ModProjectile
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
        int Timer = 4;
        int chargeStrength = 10;
        int Timer2 = 0;
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
            // Reset the animation and item timer while charging.
            owner.itemAnimation = owner.itemAnimationMax;
            owner.itemTime = owner.itemTimeMax;

            if (ChargeTime % 5 == 0) // 1 segment per 12 ticks of charge.
            {
                Timer2++;
                if (Timer2 % 15 == 0)
                {
                    if (!(chargeStrength <= 4))
                    {
                        chargeStrength--;
                    }

                }
            }
            ChargeTime++;

            if (Timer >= chargeStrength)
            {
                Vector2 newVelocity = Projectile.Center.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.ToRadians(15)) * 8;
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
                Projectile.rotation = newVelocity.ToRotation();
                Vector2 muzzleOffset = Vector2.Normalize(Projectile.velocity) + new Vector2(0.15f, 0.15f) * 16.5f;
                Vector2 position = Projectile.position;
                position += muzzleOffset;
                if (Main.rand.NextBool(chargeStrength))
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, newVelocity, ModContent.ProjectileType<HellfireRoundsProj>(), (int)Projectile.ai[1], 5);
                }
                else
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, newVelocity, (int)Projectile.ai[0], (int)Projectile.ai[1], 5);
                }


                Timer = 0;
            }
            Timer++;
            return false; // Prevent the vanilla whip AI from running.
        }
        
        public override void AI()
        {

        }
    }

    
}
