using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ShatteredRealm.Content.Globals;

namespace ShatteredRealm.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class ViperBreastplate : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 0, silver: 75); // How many coins the item is worth
			Item.rare = ItemRarityID.Blue; // The rarity of the item
			Item.defense = 6; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player)
		{
			player.lifeRegen += 4;
			player.GetCritChance(DamageClass.Generic) += 2.5f;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	}
	[AutoloadEquip(EquipType.Head)]
	public class ViperHelmet : ModItem
	{
		public static readonly int AdditiveGenericDamageBonus = 20;


		public override void SetStaticDefaults()
		{
			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			// ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;

			
		}

		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Blue; // The rarity of the item
			Item.defense = 5; // The amount of defense the item will give when equipped
		}
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Melee) += 0.06f;
			player.luck += 0.1f;
		}
		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ViperBreastplate>() && legs.type == ModContent.ItemType<ViperLeggings>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Makes you immune to poison and venom and attacks inflict these debuffs. +1 minion slot."; // This is the setbonus tooltip: "Increases dealt damage by 20%"
			player.buffImmune[BuffID.Venom] = true;
			player.buffImmune[BuffID.Poisoned] = true;
			Main.LocalPlayer.GetModPlayer<ShatteredPlayer>().verdantSetBonus = true;
			player.maxMinions += 1;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

	}
	[AutoloadEquip(EquipType.Legs)]
	public class ViperLeggings : ModItem
	{

		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 4; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Generic) += 1.5f;
			player.maxRunSpeed += 0.1f;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

	}
}