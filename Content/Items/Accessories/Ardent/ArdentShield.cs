using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;
using ShatteredRealm.Content.Items.Accessories.Lush;

namespace ShatteredRealm.Content.Items.Accessories.Ardent
{
    public class ArdentShield : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.defense = 5;
            Item.shieldItem().shield = true;
            Item.shieldItem().absorption = 0.33f;
            Item.shieldItem().durability = 120;
            Item.shieldItem().cooldown = 60 * 20;
            Item.shieldItem().shieldType = "ArdentShield";
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