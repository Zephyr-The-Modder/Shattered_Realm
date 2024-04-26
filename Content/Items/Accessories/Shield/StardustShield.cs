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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.CodeAnalysis;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace ShatteredRealm.Content.Items.Accessories.Shield
{
	[AutoloadEquip(EquipType.Shield)] // Load the spritesheet you create as a shield for the player when it is equipped.
	public class StardustShield : ModItem
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

			Item.shieldItem().absorption = 0.30f;
			Item.shieldItem().durability = 125;
			Item.shieldItem().cooldown = 60 * 20;

			Item.shieldItem().shieldType = "StardustShield";
            Item.shieldItem().shieldBreakColor = Color.LightCyan;

            Item.defense = 7;
		}

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<ShatteredPlayer>().InversePolarity)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ShatteredRealm/Content/Items/Accessories/Shield/AltStardustShield");
                Item.shieldItem().shieldBreakColor = Color.Purple;
                spriteBatch.Draw(texture, position, default, drawColor, default, origin, scale, default, default);
                return false;
            }
            else
            {
                return true;
                Item.shieldItem().shieldBreakColor = Color.LightCyan;
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
                player.moveSpeed += 0.2f;
                player.GetDamage(DamageClass.Summon) *= 1.1f;
                Item.shieldItem().cooldown = 60 * 20;
                player.GetModPlayer<ShatteredPlayer>().stardustShield = true;
            }
            else
            {
                player.moveSpeed += 0.2f;
                player.GetDamage(DamageClass.Magic) *= 1.1f;
                Item.shieldItem().cooldown = 60 * 15;
                player.GetModPlayer<ShatteredPlayer>().reversedStardust = true;
                player.GetModPlayer<ShatteredPlayer>().stardustShield = false;
            }

        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FragmentStardust, 8)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
    }
}