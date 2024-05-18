using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ShatteredRealm.Content.Items.Weapons.Ranged.Flamethrowers;
using ShatteredRealm.Content.Buffs;
using ShatteredRealm.Content.Items.Accessories.Ardent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Threading;

namespace ShatteredRealm.Content.Globals
{
    public class BlunderbussItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public int reloadTime = 5; //Whether the item is a shield or not
        public int reloadShots = 10;
        public bool isShotgun = false;

        public override void HoldItem(Item item, Player player)
        {
            if (!item.blunderbussItem().isShotgun)
            {
                return;
            }
            player.GetModPlayer<BlunderbussPlayer>().reloadTime = item.blunderbussItem().reloadTime;
            player.GetModPlayer<BlunderbussPlayer>().reloadShotsMax = item.blunderbussItem().reloadShots;
            player.GetModPlayer<BlunderbussPlayer>().isShotgun = item.blunderbussItem().isShotgun;
        }
        
    }
    public class BlunderbussPlayer : ModPlayer
    {
        public int reloadTime = 5;
        public int reloadShotsMax = 10;
        public int reloadTimer = 0;
        public bool isShotgun = false;
        public bool reloading = false;
        public int reloadShots = 1;

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.blunderbussItem().isShotgun)
            {
                reloadShots--;
                CombatText.NewText(Player.getRect(), Color.WhiteSmoke, reloadShots + "/" + reloadShotsMax);
                if (reloadShots <= 0)
                {
                    reloading = true;
                    reloadTimer = reloadTime * 60;
                    CombatText.NewText(Player.getRect(), Color.WhiteSmoke, "Reloading!", true);
                }
            }
            
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        public override bool CanUseItem(Item item)
        {
            if (reloading)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public override void PreUpdate()
        {
            if (reloading)
            {
                reloadTimer--;
                if (reloadTimer <= 0)
                {
                    reloading = false;
                    reloadShots = reloadShotsMax;
                    CombatText.NewText(Player.getRect(), Color.WhiteSmoke, reloadShotsMax + "/" + reloadShotsMax);
                }
            }
        }


    }

}
