﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ShatteredRealm.Content.Items.Weapons.Melee.Swords;
using System.Collections.Generic;
using System;
using Terraria.Localization;
using ShatteredRealm.Content.Items.Accessories.Lush;
using ShatteredRealm.Content.Globals;
using System.Linq;

namespace ShatteredRealm.Content.Items.Accessories.Shield
{
	[AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
	public class GolemShield : ModItem
	{
        public static LocalizedText TooltipWithVar { get; private set; }
        public override void SetStaticDefaults()
        {
            TooltipWithVar = this.GetLocalization(nameof(TooltipWithVar));
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            TooltipLine tooltip;
            tooltip = new TooltipLine(Mod, "tooltipWithVar", TooltipWithVar.Format(Item.shieldItem().durability * player.GetModPlayer<ShatteredPlayer>().shieldDurabilityMult, Math.Round(Item.shieldItem().absorption * 10000) / 100 + "%", Math.Round(Item.shieldItem().cooldown / player.GetModPlayer<ShatteredPlayer>().shieldCooldownMult / 60f * 100) / 100 + " seconds"));

            TooltipLine line = tooltips.FirstOrDefault((TooltipLine x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            if (line != null)
            {
                line.Text += tooltip.Text;
            }
        }


        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(gold: 14);
			Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;

            Item.shieldItem().shield = true;

			Item.shieldItem().absorption = 0.75f;
			Item.shieldItem().durability = 250;
			Item.shieldItem().cooldown = 60 * 100;

			Item.shieldItem().shieldType = "GolemShield";
            Item.shieldItem().shieldBreakColor = new Color(141, 56, 0);


            Item.defense = 8;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<ShatteredPlayer>().shieldCooldown <= 0)
            {
                player.moveSpeed *= 0.825f;
            }
        }

        public override void AddRecipes()
        {

        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}