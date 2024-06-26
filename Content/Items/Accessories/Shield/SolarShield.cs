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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.CodeAnalysis;
using Terraria.DataStructures;
using Terraria.GameContent;

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
            Item.rare = ItemRarityID.Purple;

            Item.shieldItem().overrideShieldDamage = false;
            Item.shieldItem().shield = true;

			Item.shieldItem().absorption = 0.45f;
			Item.shieldItem().durability = 200;
			Item.shieldItem().cooldown = 60 * 30;

			Item.shieldItem().shieldType = "SolarShield";
            Item.shieldItem().shieldBreakColor = Color.OrangeRed;

            Item.defense = 12;
		}

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<ShatteredPlayer>().InversePolarity)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ShatteredRealm/Content/Items/Accessories/Shield/AltSolarShield");
                Item.shieldItem().shieldBreakColor = Color.LightCyan;
                spriteBatch.Draw(texture, position, default, drawColor, default, origin, scale, default, default);
                return false;
            }
            else
            {
                return true;
                Item.shieldItem().shieldBreakColor = Color.OrangeRed;
            }
            
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
                player.GetDamage(DamageClass.Melee) *= 1.1f;
                player.GetModPlayer<ShatteredPlayer>().solarShield = true;
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
                player.GetDamage(DamageClass.Summon) *= 1.1f;
                player.GetModPlayer<ShatteredPlayer>().reversedSolar = true;
                player.GetModPlayer<ShatteredPlayer>().solarShield = false;
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentSolar, 8)
                .AddTile(TileID.LunarCraftingStation)
                .AddCondition(Condition.TimeDay)
                .Register();
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}