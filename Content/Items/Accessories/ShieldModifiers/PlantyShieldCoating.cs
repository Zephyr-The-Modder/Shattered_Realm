﻿using Microsoft.Xna.Framework;
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
    public class PlantyShieldCoating : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 13;
            Item.value = Item.buyPrice(0, 0, 4, 50);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        int Timer = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<ShatteredPlayer>().shieldEquipped)
            {
                if (player.GetModPlayer<ShatteredPlayer>().shieldDurability < player.GetModPlayer<ShatteredPlayer>().shieldMaxDurability)
                {
                    if (player.GetModPlayer<ShatteredPlayer>().shieldDurability > 0)
                    {
                        if (Timer >= 74)
                        {
                            player.GetModPlayer<ShatteredPlayer>().shieldDurability += 1;
                        }
                     
                    }
                }
            }
            if (Timer >= 74)
            {
                Timer = 0;
            }
            Timer++;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {

        }
    }

}