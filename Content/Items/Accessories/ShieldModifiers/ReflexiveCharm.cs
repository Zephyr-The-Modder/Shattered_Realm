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
    public class ReflexiveCharm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 13;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        int Timer = 0;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShatteredPlayer>().ReflexiveCharm = true;
            player.GetModPlayer<ShatteredPlayer>().ShieldEffectPower += 0.05f;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {

        }
    }

}