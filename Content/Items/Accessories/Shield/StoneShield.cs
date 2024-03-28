using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ShatteredRealm.Content.Items.Weapons.Melee.Swords;
using System.Collections.Generic;
using System;
using Terraria.Localization;
using ShatteredRealm.Content.Items.Accessories.Lush;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.Items.Accessories.Shield
{
	[AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
	public class StoneShield : ModItem
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
            tooltips.Add(tooltip);
        }


        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(silver: 10);
			Item.accessory = true;
            Item.rare = ItemRarityID.Blue;

            Item.shieldItem().shield = true;
			Item.shieldItem().absorption = 0.09f;
			Item.shieldItem().durability = 15;
			Item.shieldItem().cooldown = 60 * 30;
			Item.shieldItem().shieldType = "StoneShield";
            Item.shieldItem().shieldBreakColor = new Color(199, 195, 190);


            Item.defense = 1;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<ShatteredPlayer>().shieldCooldown > 0)
            {
                player.moveSpeed *= 0.25f;
                player.GetArmorPenetration(DamageClass.Generic) -= 5;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 30)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}