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
	public class ArachnidAegis : ModItem
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
			Item.value = Item.buyPrice(gold: 9);
			Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
			Item.shieldItem().shield = true;
			Item.shieldItem().absorption = 0.24f;
			Item.shieldItem().durability = 75;
			Item.shieldItem().cooldown = 60 * 18;
			Item.shieldItem().shieldType = "ArachnidAegis";
            Item.shieldItem().shieldBreakColor = new Color(181, 32, 9);

            Item.defense = 4;
		}
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (!player.GetModPlayer<ShatteredPlayer>().shieldEquipped)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpiderFang, 14)
                .AddRecipeGroup("TitaniumBar", 9)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}