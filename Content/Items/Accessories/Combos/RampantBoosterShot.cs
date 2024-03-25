using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;
using ShatteredRealm.Content.Items.Accessories.Lush;

namespace ShatteredRealm.Content.Items.Accessories.Combos
{
    public class RampantBoosterShot : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 32, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShatteredPlayer>().rampantBoosterShot = true;
            player.GetDamage(DamageClass.Generic) *= 1.08f;
            player.GetCritChance(DamageClass.Generic) *= 1.02f;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerdantBoosterShot>());
            recipe.AddIngredient(ItemID.DestroyerEmblem);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }

}