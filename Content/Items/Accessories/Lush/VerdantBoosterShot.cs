using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.Items.Accessories.Lush
{
    public class VerdantBoosterShot : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 5, 45, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShatteredPlayer>().verdantBoosterShot = true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

    }

}