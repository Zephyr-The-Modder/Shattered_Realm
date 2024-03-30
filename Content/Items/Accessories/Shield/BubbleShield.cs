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
	public class BubbleShield : ModItem
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
			Item.value = Item.buyPrice(gold: 14);
			Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;

            Item.shieldItem().shield = true;

			Item.shieldItem().absorption = 0.65f;
			Item.shieldItem().durability = 1;
			Item.shieldItem().cooldown = 60 * 16;

			Item.shieldItem().shieldType = "BubbleShield";
            Item.shieldItem().shieldBreakColor = Color.DarkTurquoise;

            Item.defense = 8;
		}
        int Timer = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<ShatteredPlayer>().shieldCooldown <= 0)
            {
                if (Timer >= 14)
                {
                    Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), player.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)), ProjectileID.FlaironBubble, 79, 0, Main.myPlayer, -10);
                    Timer = 0;
                }
                Timer++;
            }
        }

        public override void AddRecipes()
        {

        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}