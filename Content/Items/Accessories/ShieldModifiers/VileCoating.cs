using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;
using ShatteredRealm.Content.Items.Accessories.Lush;
using System.Collections.Generic;
using System;
using Terraria.Localization;

namespace ShatteredRealm.Content.Items.Accessories.ShieldModifiers
{
    public class VileCoating : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 13;
            Item.value = Item.buyPrice(0, 8, 50);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        int Timer = 0;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<ShatteredPlayer>().shieldDurability > 0 && player.GetModPlayer<ShatteredPlayer>().shieldEquipped)
            {
                if (Timer == 90)
                {
                    for (int i = 0; i < 360; i += 90)
                    {
                           
                        Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), player.Center, Vector2.One.RotatedBy(MathHelper.ToRadians(i)), ProjectileID.TinyEater, 10, 0, Main.myPlayer);
                    }
                    Timer = 0;
                }
                Timer++;
            }
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {

        }
    }

}