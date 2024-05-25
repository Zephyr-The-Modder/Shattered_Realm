using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using ShatteredRealm.Content.Items.Weapons.Mage.Books;
using ShatteredRealm.Content.Buffs;

namespace ShatteredRealm.Content.Items.Weapons.Melee.Swords
{
	public class VerdantBlade : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 24;
			Item.useTime = 24;
			Item.damage = 19;
			Item.knockBack = 4.5f;
			Item.width = 34;
			Item.height = 58;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(gold: 1, silver: 50); // Sell price is 5 times less than the buy price.
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = false; // This is set the sword itself doesn't deal damage (only the projectile does).
			Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
			Item.autoReuse = true;
			
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			if (Main.rand.NextBool(3))
            {
				Projectile.NewProjectileDirect(Item.GetSource_FromThis(), target.Center, Item.velocity, ModContent.ProjectileType<SporeGasCloud>(), Item.damage - 10, Item.knockBack, player.whoAmI);
			}
			
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(3))
			{
				// Emit dusts when the sword is swung
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GreenMoss);
			}
		}


	}

}
