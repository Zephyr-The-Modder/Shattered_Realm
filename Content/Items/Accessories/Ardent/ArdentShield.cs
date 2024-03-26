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
        float DR = 0.67f;
        int Durability = 115;
        int Cooldown = 1200;
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 24, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.accessory = true;
            Item.defense = 9;
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {  
            if (Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().ShieldDR < DR)
            {
                Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().ShieldDR = DR;
            }
            if (Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().shieldMaxDurability < Durability)
            {
                Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().shieldMaxDurability = Durability;
            }
            if (Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().shieldMaxCooldown > Cooldown)
            {
                Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().shieldMaxCooldown = Cooldown;
            }
            Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().shieldEquipped = true;
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