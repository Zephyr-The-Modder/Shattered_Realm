using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ShatteredRealm.Content.Items.Weapons.Ranged.Flamethrowers;
using ShatteredRealm.Content.Buffs;
using ShatteredRealm.Content.Items.Accessories.Ardent;
using Microsoft.Xna.Framework;

namespace ShatteredRealm.Content.Globals
{
    public class ShieldItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool shield = false; //Whether the item is a shield or not
        public float absorption = 0f;
        public int durability = 0;
        public int cooldown = 0;
        public string shieldType = "";
        public Color shieldBreakColor = Color.White;

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (!item.shieldItem().shield)
            {
                return;
            }
            player.GetModPlayer<ShatteredPlayer>().shieldEquipped = true;
            player.GetModPlayer<ShatteredPlayer>().shieldMaxDurability = (int)(item.shieldItem().durability * player.GetModPlayer<ShatteredPlayer>().shieldDurabilityMult);
            player.GetModPlayer<ShatteredPlayer>().shieldMaxCooldown = item.shieldItem().cooldown;
            player.GetModPlayer<ShatteredPlayer>().ShieldDR = 1 - item.shieldItem().absorption;
            player.GetModPlayer<ShatteredPlayer>().shieldType = shieldType;
            player.GetModPlayer<ShatteredPlayer>().shieldBreakColor = shieldBreakColor;
        }
    }
}
