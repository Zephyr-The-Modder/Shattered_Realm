using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ShatteredRealm.Content.Items.Weapons.Melee.Swords;
using System.Collections.Generic;
using System;
using Terraria.Localization;
using ShatteredRealm.Content.Items.Accessories.Lush;

namespace ShatteredRealm.Content.Items.Accessories.Shield
{
	[AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
	public class WoodShield : ModItem
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
            tooltip = new TooltipLine(Mod, "tooltipWithVar", TooltipWithVar.Format(Item.shieldItem().durability, Item.shieldItem().absorption * 100 + "%", Math.Round(Item.shieldItem().cooldown / 60f * 100) / 100 + " seconds"));
            tooltips.Add(tooltip);
        }


        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(silver: 1);
			Item.accessory = true;

			Item.shieldItem().shield = true;
			Item.shieldItem().absorption = 0.12f;
			Item.shieldItem().durability = 14;
			Item.shieldItem().cooldown = 60 * 30;
			Item.shieldItem().shieldType = "WoodShield";

			Item.defense = 1;
		}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("Wood", 25)
                .AddIngredient(ItemID.StoneBlock, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}