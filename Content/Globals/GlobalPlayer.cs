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
using ShatteredRealm.Content.Items.Accessories.Ardent;
using Terraria.Localization;
using Terraria.UI;
using ShatteredRealm.Content.Items.Accessories.Combos;

namespace ShatteredRealm.Content.Globals
{
    public class ShatteredPlayer : ModPlayer
    {
        //Armors
        public bool verdantSetBonus; //Verdant Armor

       
        //Permanent buffs
        public bool ConsumedForestHeart; //Forest Heart Perm Buff

        //Accessories
        public bool verdantBoosterShot;
        public bool rampantBoosterShot;
        public bool PetalTrinket;
        public bool ArdentShieldStat;
        public bool PetalTrinketUpgrade;

        //Shield Stats
        public int shieldDurability; //Do not set this value.
        public int shieldMaxDurability; //Set this to a damage value
        public bool shieldEquipped = false; //Set this in all shields to have this stat
        public int shieldCooldown; //Do not set this
        public int shieldMaxCooldown; //Set this to the cooldown length
        public bool firstComing = true;
        public bool secondComing = false; //This is only used in the code below
        public float ShieldDR = 0;
        public int shieldMaxCooldownPrevious;
        public string shieldType;

        //Misc
        public int plantburnerConsume = 1; //Used to consume ammo on the Spore Spewer

        public int CoordinatedAttacksLushOrbs; //This is stored here so all orbs have the same variable.
        public int CoordinatedDashOrbRand; //Lush


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

        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            if (PetalTrinketUpgrade)
            {
                healValue += 20;
            }
            base.GetHealMana(item, quickHeal, ref healValue);
        }
        public override void PreUpdate()
        {
            if (!shieldEquipped && shieldMaxCooldownPrevious > 0)
            {
                shieldMaxCooldownPrevious = 0;
                shieldCooldown = 0;
            }
            if (shieldEquipped && shieldMaxCooldownPrevious == 0)
            {
                shieldCooldown = shieldMaxCooldown;
            }
            if (shieldEquipped)
            {
                if (shieldCooldown == 0)
                {
                    shieldDurability = shieldMaxDurability;
                }
            }
            shieldCooldown--;
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
            PetalTrinketUpgrade = false;
            ArdentShieldStat = false;
            shieldEquipped = false;
            shieldMaxDurability = 0;
            shieldMaxCooldownPrevious = shieldMaxCooldown;
            shieldMaxCooldown = 0;
            shieldType = "";
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


        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (shieldEquipped && shieldDurability > 0)
            {
                modifiers.FinalDamage *= ShieldDR;
            }
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (shieldEquipped && shieldDurability > 0)
            {
                int preShieldDmg = (int)(info.Damage / ShieldDR);
                int shieldDmg = preShieldDmg - info.Damage;
                shieldDurability -= shieldDmg;

                CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.DarkCyan, shieldDmg);

                if (shieldDurability <= 0)
                {
                    ShieldBreak(shieldType);
                }
            }
        }
        public void ShieldBreak(string type)
        {
            shieldDurability = 0;
            shieldCooldown = shieldMaxCooldown;

            switch (type)
            {
                case "ArdentShield":
                    break;

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
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<EnrichedFlower>()))
            {
                 if (Player.statMana <= Player.statManaMax2 / 20)
                 {
                     Player.QuickMana();
                 }
            }
        }
    }
}
