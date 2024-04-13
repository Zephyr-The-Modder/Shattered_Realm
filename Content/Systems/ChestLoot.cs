using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using ShatteredRealm.Content.Items.Accessories.Shield;


namespace ShatteredRealm.Content.Systems
{
    public class ChestSpawning : ModSystem
    {
        //Important Chest IDs, such as wooden, gold, golem, etc. feel free to reference these in other scripts.
        public static readonly int WoodenChest = 0;

        public static readonly int GoldChest = 1;

        public static readonly int IceChest = 11;

        public static readonly int DungeonChest = 2;

        public static readonly int HellChest = 4;

        public static readonly int LivingChest = 13;

        public static readonly int SkyChest = 14;

        public static readonly int WebChest = 16;

        public static readonly int GolemChest = 17;

        public static readonly int WaterChest = 18;

        public static readonly int JungleBiomeChest = 24;

        public static readonly int CorruptBiomeChest = 25;

        public static readonly int CrimsonBiomeChest = 26;

        public static readonly int DesertBiomeChest = 27;

        public static readonly int IceBiomeChest = 28;

        public static readonly int HoneyChest = 30;

        public static readonly int MangroveChest = 9;
        public static readonly int IvyChest = 11;

        public static readonly int MushroomChest = 33;

        public override void PostWorldGen()
        {
            ChestLoot();
        }

        public void ChestLoot()
        {
            GenerateChestLoot(WoodenChest, 4, ModContent.ItemType<WoodShield>(), 1);
            //Wood shield natural gen in WoodChest

            GenerateChestLoot(HellChest, 10, ModContent.ItemType<HellstoneShield>(), 1);
            //Hellstone shield natural gen in HellChest
        }

        public void GenerateChestLoot(int chestID, int chanceDenom, int itemType, int stack)
        {
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                int itemsToPlaceInChests4 = itemType;

                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == chestID * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (Main.rand.NextBool(chanceDenom))
                            {
                                chest.item[inventoryIndex].SetDefaults(itemsToPlaceInChests4);
                                chest.item[inventoryIndex].stack = stack;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}