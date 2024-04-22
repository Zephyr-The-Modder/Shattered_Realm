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
    public class ReversePolarity : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 20;
            Item.value = Item.buyPrice(0, 16, 50);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
        }
        int Timer = 0;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShatteredPlayer>().InversePolarity = true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {

        }
    }

}