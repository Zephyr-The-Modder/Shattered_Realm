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
    public class FlashShielding : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 13;
            Item.value = Item.buyPrice(0, 6);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        int Timer = 0;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.05f;
            player.GetModPlayer<ShatteredPlayer>().shieldCooldownMult += 0.3f;
            player.GetModPlayer<ShatteredPlayer>().shieldDurabilityMult += 0.4f;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {

        }
    }

}