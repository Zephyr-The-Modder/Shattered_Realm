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
using System.Linq;

namespace ShatteredRealm.Content.Items.Accessories.Shield
{
	[AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
	public class SolarShield : ModItem
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
			Item.value = Item.buyPrice(gold: 30);
			Item.accessory = true;
            Item.rare = ItemRarityID.Blue;

            Item.shieldItem().overrideShieldDamage = false;
            Item.shieldItem().shield = true;

			Item.shieldItem().absorption = 0.45f;
			Item.shieldItem().durability = 200;
			Item.shieldItem().cooldown = 60 * 30;

			Item.shieldItem().shieldType = "SolarShield";
            Item.shieldItem().shieldBreakColor = Color.OrangeRed;

            Item.defense = 12;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.GetModPlayer<ShatteredPlayer>().InversePolarity)
            {
                if (player.GetModPlayer<ShatteredPlayer>().shieldCooldown < 0)
                {
                    player.moveSpeed *= 0.9f;
                    player.statDefense += 10;
                }
                else
                {
                    player.moveSpeed *= 1.2f;
                    player.statDefense -= 5;
                }
            }
            else
            {
                if (player.GetModPlayer<ShatteredPlayer>().shieldCooldown < 0)
                {
                    player.moveSpeed *= 1.2f;
                    player.statDefense -= 5;
                }
                else
                {
                    player.moveSpeed *= 0.9f;
                    player.statDefense += 10;
                }
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar, 8)
                .AddTile(TileID.MythrilAnvil)
                .AddCondition(Condition.TimeDay)
                .Register();
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}