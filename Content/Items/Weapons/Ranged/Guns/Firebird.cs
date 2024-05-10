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
using ShatteredRealm.Content.Globals;

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
            Item.useTime = 16; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useAnimation = 16; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.

            // The sound that this item plays when used.

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
            Item.damage = 39; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
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
            Item.useTime = player.GetModPlayer<FirebirdPlayer>().FirebirdSpeed;
            Item.useAnimation = player.GetModPlayer<FirebirdPlayer>().FirebirdSpeed;
        }


        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.38f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.GetModPlayer<FirebirdPlayer>().ResetTimer = 20;
            player.GetModPlayer<FirebirdPlayer>().Timer++;

            return true;
        }
        // The following method allows this gun to shoot when having no ammo, as long as the player has at least 10 example items in their inventory.
        // The gun will then shoot as if the default ammo for it, in this case the musket ball, is being used.

        // The following method makes the gun slightly inaccurate
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(player.GetModPlayer<FirebirdPlayer>().FirebirdSpeed + 5));
        }

        // This method lets you adjust position of the gun in the player's hands. Play with these values until it looks good with your graphics.
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6f, -2f);
        }

    }

    public class FirebirdPlayer : ModPlayer
    {
        public int ResetTimer = 20; 
        public int FirebirdSpeed = 16;
        public int Timer = 0;
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Timer >= 4)
            {
                if (FirebirdSpeed <= 4)
                {
                    FirebirdSpeed = 4;
                }
                else
                {
                    FirebirdSpeed--;
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
                FirebirdSpeed = 16;
                Timer = 0;
            }    
            ResetTimer--;
        }


    }


}
