using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace ShatteredRealm.Content.Items.Consumables.BossSpawns
{
    public class VerdantTulip : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 50;
            Item.value = 3500;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 999;
        }

    }
}