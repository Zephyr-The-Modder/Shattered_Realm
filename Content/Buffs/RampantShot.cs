using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShatteredRealm.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class RampantShot : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = false;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = false; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

		// Allows you to make this buff give certain effects to the given player
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<RampantShotBuffPlayer>().boosterShot = true;
		}
	}

	public class RampantShotBuffPlayer : ModPlayer
	{
		// Flag checking when life regen debuff should be activated
		public int FrameCounter = 0;
		public bool boosterShot; //Booster shot (equipped)

		public override void ResetEffects()
		{
			boosterShot = false;
		}

        // Allows you to give the player a negative life regeneration based on its state (for example, the "On Fire!" debuff makes the player take damage-over-time)
        // This is typically done by setting player.lifeRegen to 0 if it is positive, setting player.lifeRegenTime to 0, and subtracting a number from player.lifeRegen
        // The player will take damage at a rate of half the number you subtract per second
        public override void PostUpdateBuffs()
        {
			if (boosterShot)
            {
				Player.lifeRegenTime = 180000;
				Player.GetDamage(DamageClass.Generic) *= 1.35f;
			}
        }
	}
}