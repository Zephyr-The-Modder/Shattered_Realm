using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;
using ShatteredRealm.Content.Items.Accessories.Lush;

namespace ShatteredRealm.Content.Items.Accessories.Combos
{
    public class EnrichedFlower : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShatteredPlayer>().PetalTrinket = true;
            player.GetModPlayer<ShatteredPlayer>().PetalTrinketUpgrade = true;
            if (!player.HasBuff(BuffID.PotionSickness))
            {
                if (player.statLife <= player.statLifeMax2 / 10)
                {
                    player.QuickHeal();
                }
            }

            player.manaCost *= 0.92f; // Mana flower effects
            player.manaFlower = true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ManaFlower)
                .AddIngredient<SwiftTrinketofPetals>()
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

}