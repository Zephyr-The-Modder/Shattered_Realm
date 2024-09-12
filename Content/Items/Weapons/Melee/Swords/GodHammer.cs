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
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using ShatteredRealm.Content.Globals;
using System.Linq;
using Terraria.Localization;

namespace ShatteredRealm.Content.Items.Weapons.Melee.Swords
{
    public class GodHammer : ModItem
    {
        public static LocalizedText TooltipWithVar { get; private set; }
        public override void SetStaticDefaults()
        {
            TooltipWithVar = this.GetLocalization(nameof(TooltipWithVar));
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            TooltipLine tooltip;
            tooltip = new TooltipLine(Mod, "tooltipWithVar", TooltipWithVar.Format("XP: " + player.GetModPlayer<HammerPlayer>().XP, "Level: " +  player.GetModPlayer<HammerPlayer>().Level, "iFrames: " + player.GetModPlayer<HammerPlayer>().iFrames / 60, "Use Time: " + Item.useAnimation / 60, "Max Hammers: " + player.GetModPlayer<HammerPlayer>().MaxCount));

            TooltipLine line = tooltips.FirstOrDefault((TooltipLine x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            if (line != null)
            {
                line.Text += tooltip.Text;
            }
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.damage = 22;
            Item.knockBack = 4.5f;
            Item.width = 34;
            Item.height = 58;
            Item.scale = 1f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1, silver: 50); // Sell price is 5 times less than the buy price.
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
            Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GodHammerProj>();
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true;


        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<HammerPlayer>().MaxLVL)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ShatteredRealm/Content/Items/Weapons/Melee/Swords/GodHammerUpgrade");
                spriteBatch.Draw(texture, position, default, drawColor, default, origin, scale, default, default);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }
        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
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
    public class GodHammer2 : ModItem
    {
        public static LocalizedText TooltipWithVar { get; private set; }
        public override void SetStaticDefaults()
        {
            TooltipWithVar = this.GetLocalization(nameof(TooltipWithVar));
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            TooltipLine tooltip;
            tooltip = new TooltipLine(Mod, "tooltipWithVar", TooltipWithVar.Format("XP: " + player.GetModPlayer<HammerPlayer>().XP, "Level: " + player.GetModPlayer<HammerPlayer>().Level, "iFrames: " + player.GetModPlayer<HammerPlayer>().iFrames / 60, "Use Time: " + Item.useAnimation / 60, "Max Hammers: " + player.GetModPlayer<HammerPlayer>().MaxCount));

            TooltipLine line = tooltips.FirstOrDefault((TooltipLine x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            if (line != null)
            {
                line.Text += tooltip.Text;
            }
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.damage = 22;
            Item.knockBack = 4.5f;
            Item.width = 34;
            Item.height = 58;
            Item.scale = 1f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1, silver: 50); // Sell price is 5 times less than the buy price.
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
            Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GodHammerProj>();
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true;


        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<HammerPlayer>().MaxLVL)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ShatteredRealm/Content/Items/Weapons/Melee/Swords/GodHammerUpgrade_Red");
                spriteBatch.Draw(texture, position, default, drawColor, default, origin, scale, default, default);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }
        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
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
    public class GodHammer3 : ModItem
    {
        public static LocalizedText TooltipWithVar { get; private set; }
        public override void SetStaticDefaults()
        {
            TooltipWithVar = this.GetLocalization(nameof(TooltipWithVar));
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            TooltipLine tooltip;
            tooltip = new TooltipLine(Mod, "tooltipWithVar", TooltipWithVar.Format("XP: " + player.GetModPlayer<HammerPlayer>().XP, "Level: " + player.GetModPlayer<HammerPlayer>().Level));

            TooltipLine line = tooltips.FirstOrDefault((TooltipLine x) => x.Name == "Tooltip0" && x.Mod == "Terraria");
            if (line != null)
            {
                line.Text += tooltip.Text;
            }
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 18;
            Item.useTime = 18;
            Item.damage = 22;
            Item.knockBack = 4.5f;
            Item.width = 34;
            Item.height = 58;
            Item.scale = 1f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1, silver: 50); // Sell price is 5 times less than the buy price.
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
            Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GodHammerProj>();
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true;


        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<HammerPlayer>().MaxLVL)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("ShatteredRealm/Content/Items/Weapons/Melee/Swords/GodHammerUpgrade_Green");
                spriteBatch.Draw(texture, position, default, drawColor, default, origin, scale, default, default);
                return false;
            }
            else
            {
                return true;
            }
        }
        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }
        public override void UseAnimation(Player player)
        {
            base.UseAnimation(player);
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
    public class GodHammerProj : ModProjectile
    {
        int StateTimer = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 31; // The width of projectile hitbox
            Projectile.height = 31; // The height of projectile hitbox
            Projectile.scale = 1.55f;

            // Ccopy the ai of any given projectile using AIType, since we want
            // the projectile to essentially behave the same way as the vanilla projectile.
            AIType = ProjectileID.Bullet;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 35;
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.DamageType = DamageClass.Melee; // Is the projectile shoot by a ranged weapon?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 1000000; // Each update timeLeft is decreased by 1. Once timeLeft hits 0, the Projectile will naturally despawn. (60 ticks = 1 second)
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;

            Projectile.penetrate = -1;
            // 1: Projectile.penetrate = 1; // Will hit even if npc is currently immune to player
            // 2a: Projectile.penetrate = -1; // Will hit and unless 3 is use, set 10 ticks of immunity
            // 2b: Projectile.penetrate = 3; // Same, but max 3 hits before dying
            // 5: Projectile.usesLocalNPCImmunity = true;
            // 5a: Projectile.localNPCHitCooldown = -1; // 1 hit per npc max
            // 5b: Projectile.localNPCHitCooldown = 20; // 20 ticks before the same npc can be hit again

        }
        int alp = 0;
        bool returning = false;
        bool collided = false;
        int Timer = 0;
        public override void OnSpawn(IEntitySource source)
        {
            Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().Count++;
            Projectile.localNPCHitCooldown = Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().iFrames;
            switch (Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().Type)
            {
                case "red":
                    Projectile.frame = 1;
                    break;
                case "blue":
                    Projectile.frame = 0;
                    break;
                case "green":
                    Projectile.frame = 2;
                    break;
            }
            Projectile.width = 17; // The width of projectile hitbox
            Projectile.height = 17; // The height of projectile hitbox
        }
        public override void AI()
        {
            if (Projectile.Colliding(Projectile.getRect(), Main.player[Main.myPlayer].getRect()) && returning)
            {
                Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().Count--;
                Projectile.Kill();
            }
            if (returning)
            {
                collided = false;
                Projectile.velocity = Projectile.DirectionTo(Main.player[Main.myPlayer].Center) * 13;
            }
            if (!collided == true)
            {
                if (!returning)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
                }
                else
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(225);
                }
            }
            Projectile.velocity.Y += 0.25f;
            if (Main.mouseRight && collided)
            {
                returning = true;
                Projectile.tileCollide = false;
            }
            Timer++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;
            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);

            Texture2D projectileTexture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = origin;
            spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length && k < StateTimer; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(Projectile.width / 2, Projectile.height / 2); ;
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                color.A = (byte)(color.A * 0.75f);
                Main.spriteBatch.Draw(projectileTexture, drawPos, sourceRectangle, color, Projectile.oldRot[k], drawOrigin, Projectile.scale - k * 0.02f, spriteEffects, 0f);
            }

            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;

        }
        float GainXP = 1;
        List<int> enemiesHit = new List<int>();
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!enemiesHit.Contains(target.whoAmI))
            {
                float randAdd = Main.rand.NextFloat(GainXP / 2, GainXP * 2);
                if (target.type == NPCID.TargetDummy)
                {
                    Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().XP += randAdd / 3;
                }
                else
                {
                    
                }
                
                GainXP *= 0.5f;
                enemiesHit.Add(target.whoAmI);
            }

            if (Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().MaxLVL)
            {
                switch (Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().Type)
                {
                    case "red":
                        target.AddBuff(BuffID.OnFire3, 180);
                        break;
                    case "blue":
                        target.AddBuff(BuffID.Electrified, 30);
                        break;
                    case "green":
                        if (target.type != NPCID.TargetDummy)
                        {
                            Main.player[Projectile.owner].Heal(2);
                        }
                        target.AddBuff(BuffID.Poisoned, 120);
                        break;
                }
            }

            if (target.life - hit.Damage <= 0)
            {
                if (!target.boss)
                {
                    Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().XP += 3;
                }
                else
                {
                    Main.player[Main.myPlayer].GetModPlayer<HammerPlayer>().XP += 23.25f;
                }
            }
            
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            collided = true;
            return false;
        }

    }
    public class HammerPlayer : ModPlayer
    {
        public int iFrames = 30;
        public int Count = 0;
        public int MaxCount = 1;
        public float XP;
        float XPMax = 5000;
        public bool MaxLVL = false;

        public override bool CanUseItem(Item item)
        {
            if (item.type == ModContent.ItemType<GodHammer>() && Count >= MaxCount)
            {
                return false;
            }
            if (item.type == ModContent.ItemType<GodHammer2>() && Count >= MaxCount)
            {
                return false;
            }
            if (item.type == ModContent.ItemType<GodHammer3>() && Count >= MaxCount)
            {
                return false;
            }
            return base.CanUseItem(item);
        }
        public string Type;
        public string Level;
        public override void PreUpdate()
        {
            MathHelper.Clamp(XP, 0, XPMax);
            if (Player.HeldItem.type == ModContent.ItemType<GodHammer>())
            {
                Type = "blue";
                Blue();
            }
            if (Player.HeldItem.type == ModContent.ItemType<GodHammer2>())
            {
                Type = "red";
                Red();
            }
            if (Player.HeldItem.type == ModContent.ItemType<GodHammer3>())
            {
                Type = "green";
                Green();
            }

        }
        public void Red()
        {
            if (XP >= 1000)
            {
                Level = "One; This level gives off of the previous: +6 damage";
                MaxLVL = false;
            }
            else
            {
                Level = "Zero";
                MaxLVL = false;
            }
            if (XP >= 2000)
            {
                Level = "Two; This level gives off of the previous: +11 damage, +2 iFrames";
                MaxLVL = false;
            }
            if (XP >= 3000)
            {
                Level = "Three; This level gives off of the previous: +12 damage, +3 iFrames";
                MaxLVL = false;
            }
            if (XP >= 4000)
            {
                Level = "Four; This level gives off of the previous: +13 damage, +5 iFrames";
                MaxLVL = false;
            }
            if (XP >= 5000)
            {
                Level = "Max; This level gives off of the previous: +15 damage, +20 iFrames";
                MaxLVL = true;
            }
            if (XP > 1000)
            {
                iFrames = 30;
                MaxCount = 1;
                Player.HeldItem.damage = 28;
            }
            else
            {
                iFrames = 30;
                MaxCount = 1;
                Player.HeldItem.damage = 22;
                Player.HeldItem.useAnimation = 18;
                Player.HeldItem.useTime = 18;
            }
            if (XP > 2000)
            {
                iFrames = 32;
                MaxCount = 1;
                Player.HeldItem.damage = 39;
            }
            if (XP > 3000)
            {
                iFrames = 35;
                MaxCount = 1;
                Player.HeldItem.damage = 51;
            }
            if (XP > 4000)
            {
                iFrames = 40;
                MaxCount = 1;
                Player.HeldItem.damage = 64;
            }
            if (XP > 5000)
            {
                iFrames = 60;
                MaxCount = 1;
                Player.HeldItem.damage = 79;
            }
        }
        public void Blue()
        {
            if (XP >= 1000)
            {
                Level = "One; This level gives off of the previous: +1 Max Count, Weapon attacks faster";
                MaxLVL = false;
            }
            else
            {
                Level = "Zero";
                MaxLVL = false;
            }
            if (XP >= 2000)
            {
                Level = "Two; This level gives off of the previous: +1 Max Count, Weapon attacks slightly faster";
                MaxLVL = false;
            }
            if (XP >= 3000)
            {
                Level = "Three; This level gives off of the previous: +5 Max Count, Weapon attacks slightly faster";
                MaxLVL = false;
            }
            if (XP >= 4000)
            {
                Level = "Four; This level gives off of the previous: +4 Max Count, Weapon attacks faster";
                MaxLVL = false;
            }
            if (XP >= 5000)
            {
                Level = "Max; This level gives off of the previous: +13 Max Count, Weapon attacks faster";
                MaxLVL = true;
            }
            if (XP > 1000)
            {
                iFrames = 30;
                MaxCount = 2;
                Player.HeldItem.useAnimation = 16;
                Player.HeldItem.useTime = 16;
                Player.HeldItem.damage = 22;
            }
            else
            {
                iFrames = 30;
                MaxCount = 1;
                Player.HeldItem.damage = 22;
                Player.HeldItem.useAnimation = 18;
                Player.HeldItem.useTime = 18;

            }
            if (XP > 2000)
            {
                iFrames = 30;
                MaxCount = 3;
                Player.HeldItem.useAnimation = 15;
                Player.HeldItem.useTime = 15;
                Player.HeldItem.damage = 22;
            }
            if (XP > 3000)
            {
                iFrames = 30;
                MaxCount = 8;
                Player.HeldItem.useAnimation = 14;
                Player.HeldItem.useTime = 14;
                Player.HeldItem.damage = 22;
            }
            if (XP > 4000)
            {
                iFrames = 25;
                MaxCount = 12;
                Player.HeldItem.useAnimation = 12;
                Player.HeldItem.useTime = 12;
                Player.HeldItem.damage = 22;
            }
            if (XP > 5000)
            {
                iFrames = 20;
                MaxCount = 25;
                Player.HeldItem.useAnimation = 10;
                Player.HeldItem.useTime = 10;
                Player.HeldItem.damage = 22;
            }
        }
        public void Green()
        {
            if (XP >= 1000)
            {
                Level = "One; This level gives off of the previous: -5 iFrames, +1 damage";
                MaxLVL = false;
            }
            else
            {
                Level = "Zero";
                MaxLVL = false;
            }
            if (XP >= 2000)
            {
                Level = "Two; This level gives off of the previous: -5 iFrames, +1 damage";
                MaxLVL = false;
            }
            if (XP >= 3000)
            {
                Level = "Three; This level gives off of the previous: -3 iFrames, +1 damage";
                MaxLVL = false;
            }
            if (XP >= 4000)
            {
                Level = "Four; This level gives off of the previous: -2 iFrames, +1 damage, +1 Max Count";
                MaxLVL = false;
            }
            if (XP >= 5000)
            {
                Level = "Max; This level gives off of the previous: -3 iFrames, +1 damage, +1 Max Count";
                MaxLVL = true;
            }
            if (XP > 1000)
            {
                MaxCount = 1;
                iFrames = 25;
                Player.HeldItem.damage = 23;
            }
            else
            {
                iFrames = 30;
                MaxCount = 1;
                Player.HeldItem.damage = 22;
                Player.HeldItem.useAnimation = 18;
                Player.HeldItem.useTime = 18;

            }
            if (XP > 2000)
            {
                MaxCount = 1;
                iFrames = 20;
                Player.HeldItem.damage = 24;
            }
            if (XP > 3000)
            {
                MaxCount = 1;
                iFrames = 17;
                Player.HeldItem.damage = 25;
            }
            if (XP > 4000)
            {
                MaxCount = 2;
                iFrames = 15;
                Player.HeldItem.damage = 26;
            }
            if (XP > 5000)
            {
                MaxCount = 3;
                iFrames = 12;
                Player.HeldItem.damage = 27;
            }
        }
        public override void SaveData(TagCompound tag)
        {
            tag["XP"] = XP;
        }

        public override void LoadData(TagCompound tag)
        {
            XP = tag.GetFloat("XP"); // load
        }

    }

}
