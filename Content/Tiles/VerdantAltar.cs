using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using ShatteredRealm.Content.Items.Consumables.BossSpawns;

namespace ShatteredRealm.Content.Tiles
{
	public class VerdantAltar : ModTile
	{
		
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.CoordinateHeights = new int[3] { 16, 16, 16 };
			MineResist = 500000000f;
			MinPick = 500000000;
			// Placement
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3); // this style already takes care of direction for us
			TileObjectData.newTile.CoordinateHeights = new[] {16, 16, 16};
			TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
			TileObjectData.addTile(Type);

			// Etc
			AddMapEntry(new Color(191, 142, 111), Language.GetText("Verdant Altar"));
		}

		public override bool RightClick(int i, int j)
		{


			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Main.LocalPlayer.cursorItemIconEnabled = true;
			Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<VerdantTulip>();
		}
		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}
	}
}
