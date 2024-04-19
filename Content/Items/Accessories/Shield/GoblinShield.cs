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
    public class GoblinShield : ModItem
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
            Item.rare = ItemRarityID.Green;

            Item.shieldItem().shield = true;

            Item.shieldItem().absorption = 0.20f;
            Item.shieldItem().durability = 55;
            Item.shieldItem().cooldown = 60 * 20;

            Item.shieldItem().shieldType = "GoblinShield";
            Item.shieldItem().shieldBreakColor = Color.Magenta;


            Item.defense = 3;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<ShatteredPlayer>().shieldDurability <= 0)
            {
                player.moveSpeed *= 1.2f;
                player.statDefense -= 2;
            }
            else
            {
                player.GetModPlayer<ShatteredPlayer>().GoblinShield = true;
                player.statDefense += 1;
            }    
            
        }

        public override void AddRecipes()
        {

        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}