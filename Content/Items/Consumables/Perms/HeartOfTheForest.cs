using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.Items.Consumables.Perms
{
	// This file showcases how to create an item that increases the player's maximum health on use.
	// Within your ModPlayer, you need to save/load a count of usages. You also need to sync the data to other players.
	// The overlay used to display the custom life fruit can be found in Common/UI/ResourceDisplay/VanillaLifeOverlay.cs
	internal class HeartOfTheForest : ModItem
	{
		
		public override void SetStaticDefaults()
		{

		}

		public override void SetDefaults()
		{
			Item.expert = true;
			Item.width = 28;
			Item.height = 50;
			Item.value = 35000;
			Item.maxStack = 16;
		}

		public override bool CanUseItem(Player player)
		{
			// This check prevents this item from being used before vanilla health upgrades are maxed out.
			return player.ConsumedLifeCrystals == Player.LifeCrystalMax && player.ConsumedLifeFruit == Player.LifeFruitMax;
		}

		public override bool? UseItem(Player player)
		{
			// Moving the exampleLifeFruits check from CanUseItem to here allows this example fruit to still "be used" like Life Fruit can be
			// when at the max allowed, but it will just play the animation and not affect the player's max life
			
			if (player.GetModPlayer<ShatteredPlayer>().ConsumedForestHeart)
			{
				// Returning null will make the item not be consumed
				return null;
			}

			// This field tracks how many of the example fruit have been consumed
			player.GetModPlayer<ShatteredPlayer>().ConsumedForestHeart = true;

			return true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation
	}
}