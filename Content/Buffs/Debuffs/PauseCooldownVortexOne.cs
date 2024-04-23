using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.Buffs.Debuffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class PauseCooldownVortexOne : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = false; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = false; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = false; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

		// Allows you to make this buff give certain effects to the given player
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<ShatteredPlayer>().shieldCooldown = player.GetModPlayer<ShatteredPlayer>().shieldMaxCooldown;
			player.statDefense += 175;
			player.GetCritChance(DamageClass.Ranged) *= 1.35f;
			player.GetDamage(DamageClass.Ranged) *= 1.1f;
		}
	}

}