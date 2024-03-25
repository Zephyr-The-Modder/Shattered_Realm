using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.Items.Accessories.Lush
{
    public class SwiftTrinketofPetals : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShatteredPlayer>().PetalTrinket = true;
            if (!player.HasBuff(BuffID.PotionSickness))
            {
                if (player.statLife <= player.statLifeMax2 / 10)
                {
                    player.QuickHeal();
                }
            }
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

    }

}