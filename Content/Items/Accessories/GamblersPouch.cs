using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;
using ShatteredRealm.Content.Items.Accessories.Lush;
using Microsoft.Xna.Framework.Input;

namespace ShatteredRealm.Content.Items.Accessories
{
    public class GamblersPouch : ModItem
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
            player.GetModPlayer<GamblersPouchPlayer>().active = true;
        }


    }

    public class GamblersPouchPlayer : ModPlayer
    {
        public bool active = false;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (active)
            {
                int rand = Main.rand.Next(101);
                if (rand <= 20)
                {
                    hit.Damage /= 2;
                }
                if (rand >= 20 && rand <= 60)
                {
                    hit.Damage -= 10;
                }
                if (rand >= 60 && rand <= 79)
                {
                    
                }
                if (rand >= 79 && rand <= 91)
                {
                    hit.Damage += 10;
                }
                if (rand >= 91 && rand <= 97)
                {
                    hit.Damage += 20;
                }
                if (rand >= 97 && rand <= 100)
                {
                    hit.Damage *= 2;
                }
            }
        }
        public override void ResetEffects()
        {
            active = false;
        }
    }

}