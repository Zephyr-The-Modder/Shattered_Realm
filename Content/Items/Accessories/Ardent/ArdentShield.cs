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

namespace ShatteredRealm.Content.Items.Accessories.Ardent
{
    public class ArdentShield : ModItem
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
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.defense = 5;
            Item.shieldItem().shield = true;
            Item.shieldItem().absorption = 0.35f;
            Item.shieldItem().durability = 90;
            Item.shieldItem().cooldown = 60 * 18;
            Item.shieldItem().shieldType = "ArdentShield"; 
            Item.shieldItem().shieldBreakColor = Color.OrangeRed;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {  
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;
            player.GetModPlayer<ShatteredPlayer>().ArdentShieldStat = true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {

        }
    }

}