using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using ShatteredRealm.Content.Buffs;
using ShatteredRealm.Content.Items.Accessories.Lush;
using ShatteredRealm.Content.Items.Accessories.AccessoryProjectiles;

namespace ShatteredRealm.Content.Globals
{
    public class ShatteredPlayer : ModPlayer
    {
    
        public bool verdantSetBonus; //Verdant Armor

        public int plantburnerConsume = 1; //Used to consume ammo on the Spore Spewer
        
        public int CoordinatedAttacksLushOrbs; //This is stored here so all orbs have the same variable.
        public int CoordinatedDashOrbRand; //Lush

        public bool ConsumedForestHeart; //Forest Heart Perm Buff

        public bool verdantBoosterShot;

        public bool rampantBoosterShot;

        public bool PetalTrinket;

        public bool ArdentShieldStat;

        public override void SaveData(TagCompound tag)
        {
            tag["ForestHeartSave"] = ConsumedForestHeart;
        }

        public override void LoadData(TagCompound tag)
        {
            ConsumedForestHeart = tag.GetBool("ForestHeartSave"); // load

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (verdantSetBonus && Main.rand.Next(1, 4) == 1)
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
            if (ArdentShieldStat)
            {
                target.AddBuff(BuffID.OnFire3, Main.rand.Next(150, 300));
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (ConsumedForestHeart)
            {
                if (Player.lifeRegen < 0)
                {
                    Player.lifeRegen -= Player.lifeRegen / 3;
                }
            }
        }
        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (PetalTrinket)
            {
                healValue += 20;
            }
            base.GetHealLife(item, quickHeal, ref healValue);
        }

        public override void PostUpdate()
        {
            AutoConsumePotion();
 
        }

        public override void ResetEffects()
        {
            verdantSetBonus = false;

            PetalTrinket = false;

            verdantBoosterShot = false;
            rampantBoosterShot = false;

            ArdentShieldStat = false;

            base.ResetEffects();
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (ArdentShieldStat)
            {
                
                for (int i = 0; i < 10; i++)
                {
                    Projectile.NewProjectileDirect(Player.GetSource_OnHit(npc), Player.Center, Vector2.One, ModContent.ProjectileType<ArdentSparks>(), hurtInfo.SourceDamage + 9, 4);
                }
               
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (ArdentShieldStat)
            {

                for (int i = 0; i < 6; i++)
                {
                    Projectile.NewProjectileDirect(Player.GetSource_OnHit(proj), Player.Center, Vector2.One, ModContent.ProjectileType<ArdentSparks>(), hurtInfo.SourceDamage + 9, 4);
                }

            }
        }
        public void AutoConsumePotion()
        {
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<SwiftTrinketofPetals>()))
            {
                if (!Player.HasBuff(BuffID.PotionSickness))
                {
                    if (Player.statLife <= Player.statLifeMax2 / 10)
                    {
                        Player.QuickHeal();
                    }
                }
            }
        }
    }
}
