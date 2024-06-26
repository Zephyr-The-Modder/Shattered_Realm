﻿using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ShatteredRealm.Content.Globals;
using Microsoft.Xna.Framework;
using ShatteredRealm.Content.Items.Weapons.Ranged.Guns;

namespace ShatteredRealm.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting X_Arms.png, X_Body.png and X_FemaleBody.png sprite-sheet files to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class StaticBreastplate : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 0, silver: 75); // How many coins the item is worth
			Item.rare = ItemRarityID.Yellow; // The rarity of the item
			Item.defense = 19; // The amount of defense the item will give when equipped
		}
		int Timer = 0;
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<ShatteredPlayer>().ShockBonus1 = true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	}
	[AutoloadEquip(EquipType.Head)]
	public class StaticHelmet : ModItem
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
			Item.rare = ItemRarityID.Yellow; // The rarity of the item
			Item.defense = 9; // The amount of defense the item will give when equipped
		}
		public override void UpdateEquip(Player player)
		{
			player.buffImmune[BuffID.Electrified] = true;
			player.GetDamage(DamageClass.Ranged) *= 1.08f;
			player.GetCritChance(DamageClass.Ranged) *= 1.06f;
		}
		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<StaticBreastplate>() && legs.type == ModContent.ItemType<StaticLeggings>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player)
		{
			player.GetModPlayer<ShatteredPlayer>().StaticSet = true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

	}
	[AutoloadEquip(EquipType.Legs)]
	public class StaticLeggings : ModItem
	{

		public override void SetDefaults()
		{
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Yellow; // The rarity of the item
			Item.defense = 14; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Ranged) *= 1.06f;
			player.moveSpeed *= 1.24f;
			player.GetModPlayer<ShatteredPlayer>().StaticLegs = true;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

	}
}