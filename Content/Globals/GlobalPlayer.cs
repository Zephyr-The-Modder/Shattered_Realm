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
using System.Security.Permissions;
using Microsoft.CodeAnalysis;
using ShatteredRealm.Content.Items.Weapons.Mage.Staffs;
using ShatteredRealm.Content.Items.Armor;
using Terraria.DataStructures;

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
        public bool plantyShieldCoating;

        //Shield Stats
        public int shieldDurability; //Do not set this value.
        public int shieldMaxDurability; //Set this to a damage value
        public bool shieldEquipped = false; //Set this in all shields to have this stat
        public float shieldCooldown; //Do not set this
        public int shieldMaxCooldown; //Set this to the cooldown length
        public bool firstComing = true;
        public bool secondComing = false; //This is only used in the code below
        public float ShieldDR = 0;
        public int shieldMaxCooldownPrevious;
        public string shieldType;
        public float shieldDurabilityMult;
        public float shieldCooldownMult;
        public bool overrideShieldBreak;
        public bool overridePlayerDamage;
        public Color shieldBreakColor;

        public bool CrystalCoating = false;
        public bool ReflexiveCharm = false;

        public bool shieldsActive;

        public int MinimumShieldDamage = 10;
        public int MaximumShieldDamage = 70;
        public bool StaticLegs;
        public bool ShockBonus1;
        public bool StaticSet;
        public int TimeBetweenShots;
        public bool GoblinShield = false;


        //Misc
        public int plantburnerConsume = 1; //Used to consume ammo on the Spore Spewer

        public int CoordinatedAttacksLushOrbs; //This is stored here so all orbs have the same variable.
        public int CoordinatedDashOrbRand; //Lush

        int plantyShieldCoatingTimer;


        public override void SaveData(TagCompound tag)
        {
            tag["ForestHeartSave"] = ConsumedForestHeart;
        }

        public override void LoadData(TagCompound tag)
        {
            ConsumedForestHeart = tag.GetBool("ForestHeartSave"); // load
        }

        public override void PostUpdateEquips()
        {
            if (plantyShieldCoating)
            {
                if (shieldEquipped)
                {
                    if (shieldDurability < shieldMaxDurability)
                    {
                        if (shieldDurability > 0)
                        {
                            if (plantyShieldCoatingTimer >= 89)
                            {
                               shieldDurability += 1;
                            }

                        }
                    }
                }
                if (plantyShieldCoatingTimer >= 89)
                {
                    plantyShieldCoatingTimer = 0;
                }
                plantyShieldCoatingTimer++;
            }
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
            int maxShieldDurability = (int)(shieldMaxDurability * shieldDurabilityMult);
            MathHelper.Clamp(maxShieldDurability, 1, 9999);
            if (!shieldEquipped && shieldMaxCooldownPrevious > 0)
            {
                shieldsActive = false;
                shieldMaxCooldownPrevious = 0;
                shieldCooldown = 0;
            }
            if (shieldEquipped && shieldMaxCooldownPrevious == 0)
            {
                shieldCooldown = shieldMaxCooldown;
            }
            if (shieldEquipped)
            {
                if (shieldCooldown <= 0 && !shieldsActive)
                {
                    shieldDurability = maxShieldDurability;
                    shieldsActive = true;
                }
                if (shieldDurability > maxShieldDurability)
                {
                    shieldDurability = maxShieldDurability;
                }
            }
            shieldCooldown -= shieldCooldownMult;
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
            shieldCooldownMult = 1;
            shieldDurabilityMult = 1;
            shieldBreakColor = Color.LightGray;
            overridePlayerDamage = false;
            overrideShieldBreak = false;
            CrystalCoating = false;
            StaticLegs = false;
            ShockBonus1 = false;
            StaticSet = false;
            GoblinShield = false;

            base.ResetEffects();
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (ArdentShieldStat)
            {
                for (int i = 0; i < 5; i++)
                {
                    int damage = hurtInfo.SourceDamage;
                    MathHelper.Clamp(damage, 25, 70);
                    Projectile.NewProjectileDirect(Player.GetSource_OnHit(npc), Player.Center, Vector2.One, ModContent.ProjectileType<ArdentSparks>(), damage, 4);
                }
               
            }  
            if (ShockBonus1)
            {
                npc.AddBuff(ModContent.BuffType<Drained>(), 600);
            }
            if (GoblinShield)
            {
                npc.AddBuff(BuffID.ShadowFlame, 300);
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (CrystalCoating && proj.hostile && proj.active && shieldsActive)
            {
                NPC closestNPC = SRUtils.GetClosestNPC(Player.Center, 1500);
                if (closestNPC == null)
                {
                    proj.velocity *= -1f;
                }
                else
                {
                    float magnitude = Vector2.Distance(proj.velocity, Vector2.Zero);
                    Vector2 vel = Vector2.Normalize(closestNPC.Center - Player.Center) * magnitude;
                    proj.velocity = vel;
                }

                proj.hostile = false;
                proj.friendly = true;
                proj.penetrate = 1;
                proj.damage = (int)(proj.damage * 2f);


                SoundEngine.PlaySound(SoundID.Item154, Player.Center);
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (ArdentShieldStat)
            {

                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectileDirect(Player.GetSource_OnHit(proj), Player.Center, Vector2.One, ModContent.ProjectileType<ArdentSparks>(), hurtInfo.SourceDamage + 9, 4);
                }

            }

            
        }


        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (!overridePlayerDamage)
            {
                if (shieldEquipped && shieldDurability > 0)
                {
                    modifiers.FinalDamage *= ShieldDR;
                }
            }
            else
            {
                if (shieldEquipped && shieldDurability > 0)
                {
                    modifiers.FinalDamage *= ChangeHurt(shieldType);
                }
            }

        }

        public float ChangeHurt(string type)
        {
            float HurtModifiers = 1;
            switch (type)
            {
                case "TurtleShield":
                    HurtModifiers = 0.00000001f;
                    break;
            }
            return HurtModifiers;
        }

        public override void OnHurt(Player.HurtInfo info)
        {

            if (shieldEquipped && shieldDurability > 0)
            {
                int shieldDmg;
                if (!overrideShieldBreak)
                {
                    shieldDmg = (int)(info.SourceDamage * (1 - ShieldDR));
                }
                else
                {
                    shieldDmg = OverrideShieldDamage(shieldType, info);
                }

                DamageShield(shieldDmg, 1);

                if (shieldDurability <= 0)
                {
                    ShieldBreak(shieldType);
                }
            }
        }

        public void DamageShield(int dmg, float damageModifier)
        {
            shieldDurability -= (int)(dmg * damageModifier);
            CombatText.NewText(Player.getRect(), Color.DarkCyan, dmg);
        }
        public int OverrideShieldDamage(string type, Player.HurtInfo info)
        {
            int modifiedDmg = (int)(info.SourceDamage * (1 - ShieldDR));
            float ModifiedDR = 0f;

            switch (type)
            {
                case "SporeShield":
                    ModifiedDR = 0.125f;
                    modifiedDmg = info.SourceDamage / 8;
                    break;
                case "StoneShield":
                    ModifiedDR = 0.15f;
                    modifiedDmg = info.SourceDamage / 7;
                    break;
                case "TurtleShield":
                    modifiedDmg = info.SourceDamage;
                    break;
            }
            return modifiedDmg;
        }
        public void ShieldBreak(string type)
        {
            CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), shieldBreakColor, "Shield Break!", dramatic: true);

            shieldsActive = false;
            shieldDurability = 0;
            shieldCooldown = shieldMaxCooldown;

            switch (type)
            {
                case "ArdentShield":
                    break;
                case "SporeShield":
                    break;
                case "EnchantedShield":
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 newPos = Player.Center;
                        newPos.Y -= 700;
                        Vector2 newVel = newPos.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.ToRadians(35)) * 14.5f;
                        newVel *= 1f - Main.rand.NextFloat(0.4f);

                        Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), newPos, newVel, ProjectileID.HallowStar, 14, 0, Player.whoAmI);
                    }
                    break;
                case "HellstoneShield":
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 newVel = Player.DirectionTo(Main.MouseWorld).RotatedByRandom(MathHelper.ToRadians(35)) * 6;
                        
                        Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), Player.Center, newVel, ModContent.ProjectileType<MagmaStaffProj>(), 17, 0, Player.whoAmI);
                    }
                    break;
                case "BeeShield":
                    for (int i = 0; i < 60; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), Player.Center, Main.rand.NextVector2Circular(5, 5) * 2.5f + new Vector2(0, -6.5f), ProjectileID.Bee, 12, 0, Player.whoAmI);
                    }
                    break;
                case "TeleportingShield":
                    Player.Teleport(Main.MouseWorld, TeleportationStyleID.RodOfDiscord);
                    break;
                case "WoodShield":
                    break;
                case "GoldShield":
                    float distanceFromTarget = 225;

                    // This code is required either way, used for finding a target
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];

                        if (npc.CanBeChasedBy())
                        {
                            float between = Vector2.Distance(npc.Center, this.Player.Center);
                            bool inRange = between < distanceFromTarget;

                            if (inRange)
                            {
                                npc.AddBuff(BuffID.Midas, 900);
                            }
                        }
                    }
                    break;
                case "PlatinumShield":
                    Player.AddBuff(ModContent.BuffType<PlatinumShieldLuck>(), 600);
                    break;
                case "StoneShield":
                    break;
                case "ArachnidAegis":
                    for (int i = 0; i < 12; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_NaturalSpawn(), Player.Center, Main.rand.NextVector2Circular(5, 5) + new Vector2(0, -6.5f), ProjectileID.BabySpider, 45, 0, Player.whoAmI);
                    }
                    break;
                case "TurtleShield":
                    Player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason("Was poked to death by their shield"), shieldDurability / 2, 0);
                    break;
                case "GolemShield":
                    break;
                case "GoblinShield":
                    float distanceFromTarget2 = 650;

                    // This code is required either way, used for finding a target
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];

                        if (npc.CanBeChasedBy())
                        {
                            float between = Vector2.Distance(npc.Center, this.Player.Center);
                            bool inRange = between < distanceFromTarget2;

                            if (inRange)
                            {
                                npc.AddBuff(BuffID.ShadowFlame, 660);
                            }
                        }
                    }
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
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (TimeBetweenShots >= 240)
            {
                damage *= 3;
            }
            if (StaticSet)
            {
                TimeBetweenShots = 0;
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
        
    }
}
