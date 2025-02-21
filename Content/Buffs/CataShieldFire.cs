using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShatteredRealm.Content.Buffs
{
	// This class serves as an example of a debuff that causes constant loss of life
	// See ExampleLifeRegenDebuffPlayer.UpdateBadLifeRegen at the end of the file for more information
	public class CataShieldFire : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

        // Allows you to make this buff give certain effects to the given player
        public override void Update(NPC npc, ref int buffIndex)
        {
			Dust.NewDust(npc.Center, npc.getRect().Width, npc.getRect().Height, DustID.RedTorch);
            if (npc.lifeRegen >= 0)
			{
				npc.lifeRegen = -32;
			}
			else
			{
				npc.lifeRegen -= 32;
			}
        }
        public override void Update(Player player, ref int buffIndex)
        {
            Dust.NewDust(player.Center, player.getRect().Width, player.getRect().Height, DustID.RedTorch);
            if (player.lifeRegen >= 0)
            {
                player.lifeRegen = -24;
            }
            else
            {
                player.lifeRegen -= 24;
            }
        }
    }

}