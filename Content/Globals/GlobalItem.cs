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

namespace ShatteredRealm.Content.Globals
{
    public class Globalitem : GlobalItem
    {
		public override void SetDefaults(Item item)
		{
			// We type "item" here instead of "Item" because this item is a parameter named "item".
			if (item.type == ItemID.JungleSpores)
			{
				item.ammo = ItemID.JungleSpores;
				item.shoot = ModContent.ProjectileType<SporeGasFlamethrower>();
				item.consumable = true;
			}
			// and so on, for SilkRope and WebRope
		}

        public override void OnConsumeItem(Item item, Player player)
        {
			if (item.healLife > 0 && player.GetModPlayer<ShatteredPlayer>().rampantBoosterShot) // Rampant Booster Shot Code
			{
				player.AddBuff(ModContent.BuffType<BoosterShot>(), item.healLife);
				player.AddBuff(ModContent.BuffType<RampantShot>(), item.healLife * 5);
			}

			if (item.healLife > 0 && player.GetModPlayer<ShatteredPlayer>().verdantBoosterShot) // Verdant Booster Shot Code
			{
				player.ClearBuff(ModContent.BuffType<BoosterShot>()); // This is in the slim case that both accessories are used, in order to set the healing to this one.
				player.AddBuff(ModContent.BuffType<BoosterShot>(), item.healLife * 3);
			}
		}
		
    }
}
