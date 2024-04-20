using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShatteredRealm.Content.Items.Weapons.Mage.Staffs
{

    public class ArdentMagicTrident : ModItem
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Acorn Staff"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("Hitting an ememy restores all used mana");
            Item.staff[Type] = true;
		}

        public override void SetDefaults()
        {
            Item.damage = 157;
            Item.mana = 14;
            Item.DamageType = DamageClass.Magic;
            Item.width = 42;
            Item.height = 46;
            Item.useTime = 33;
            Item.useAnimation = 33;
            Item.useStyle = 5;
            Item.knockBack = 0.5f;
            Item.value = 10000;
            Item.rare = 6;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ArdentMagicTridentProj>();
            Item.shootSpeed = 8f;
            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            List<NPCandValue> npcDistances = new List<NPCandValue> { };


            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = 640 * 640;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target2 = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target2.CanBeChasedBy())
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target2.Center, Main.MouseWorld);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        npcDistances.Add(
                            new NPCandValue { npc = target2, value = sqrDistanceToTarget }
                            );
                    }
                }
            }

            npcDistances.Sort();

            int npcs = npcDistances.Count;
            if (npcs > 3)
            {
                npcs = 3;
            }
            if (npcs > 0)
            {
                for (int i = 0; i < npcs; i++)
                {
                    for (int j = 0; j < 3 - i; j++)
                    {
                        Projectile.NewProjectile(source, npcDistances[i].npc.Center - new Vector2(npcDistances[i].npc.width + 16, 0), Vector2.Zero, ModContent.ProjectileType<ArdentMagicTridentProj>(), damage, 2, player.whoAmI, npcDistances[i].npc.whoAmI);
                    } 
                }
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<ArdentMagicTridentProj>(), damage, 2, player.whoAmI, -1);
                }
            }
            return false;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 94f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
    }

    public class ArdentMagicTridentProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wind"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 60;
            Projectile.light = 1f;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.alpha = 255;
        }
        Vector2 EnemyPos;
        bool didit = false;
        float dir;
        int StateTimer = 0;

        public override bool? CanDamage()
        {
            if (didit)
            {
                return base.CanDamage();
            }
            return false;
        }

        public override void AI()
        {
            StateTimer++;

            if (!didit)
            {
                dir = Main.rand.NextFloat(0, MathHelper.TwoPi);
                if (Projectile.ai[0] == -1)
                {
                    EnemyPos = Main.MouseWorld;
                }

                didit = true;
            }

            if (Projectile.ai[0] > -1) {
                NPC npc = Main.npc[(int)Projectile.ai[0]];
                if (npc.active)
                {
                    EnemyPos = npc.Center;
                }
            }
            Projectile.Center = new Vector2(MathHelper.Lerp(EnemyPos.X - (float)(192 * Math.Cos(dir)), EnemyPos.X + (float)(192 * Math.Cos(dir)), Projectile.timeLeft / 60f), MathHelper.Lerp(EnemyPos.Y - (float)(192 * Math.Sin(dir)), EnemyPos.Y + (float)(192 * Math.Sin(dir)), Projectile.timeLeft / 60f));

            if (Projectile.timeLeft >= 45)
            {
                Projectile.alpha -= 17;
            }
            if (Projectile.timeLeft < 15)
            {
                Projectile.alpha += 17;
            }
            Projectile.rotation = dir + MathHelper.ToRadians(225);

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
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(4f, 4f);
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
    }

}