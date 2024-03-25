using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ShatteredRealm.Content.Items.Ammos;
using ShatteredRealm.Content.Globals;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace ShatteredRealm.Content.Items.Weapons.Mage.Books
{
    public class VerdantStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.knockBack = 1f;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player do the proper arm motion
            Item.useAnimation = 20;
            Item.useTime = 4;
            Item.reuseDelay = 32;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item28;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.noMelee = true; // The projectile will do the damage and not the item
            Item.crit = 30;
            Item.mana = 4;
            Item.ArmorPenetration = 5;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.UseSound = SoundID.Item126;

            Item.shoot = ModContent.ProjectileType<VerdantStalkThornBomb>(); // The projectile is what makes a shortsword work
            Item.shootSpeed = 7f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        int ProjectileSpread = 0;
        float ProjectileRotation;
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int NumProjectiles = 1;
            int ProjectileAI;
            SoundEngine.PlaySound(SoundID.Item118, player.position);
            if (player.altFunctionUse == 2)
            {
                ProjectileSpread = 18;
                ProjectileAI = 1;
            }
            else
            {
                ProjectileSpread = 9;
                ProjectileAI = 0;
            }

            for (int i = 0; i < NumProjectiles; i++)
            {
                Vector2 newVelocity;

                newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(ProjectileSpread));

                // Decrease velocity randomly for nicer visuals.
                if (player.altFunctionUse != 2)
                {
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<VerdantStalkThorn>(), damage, knockback, player.whoAmI, ProjectileAI);
                }
                else
                {
                    Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<VerdantStalkThorn>(), 6, knockback, player.whoAmI, ProjectileAI);
                }
               
            }
            return false; // Return false because we don't want tModLoader to shoot projectile
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 12f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
    }
    public class VerdantStalkThorn : ModProjectile
    {
        public override void OnSpawn(IEntitySource source)
        {


        }

        public override void SetDefaults()
        {
            Projectile.width = 4; // The width of projectile hitbox
            Projectile.height = 10; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Magic; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.scale = 2f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.penetrate = 2;
        }

        // Custom AI
        int Timer = 0;


        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 1)
            {
                if (Main.rand.NextBool(3))
                {
                    if (Main.rand.NextBool(4))
                    {
                        if (Main.rand.NextBool(5))
                        {
                            if (Main.rand.NextBool(6))
                            {
                                Main.player[Projectile.owner].Heal(9);
                            }
                            else
                            {
                                Main.player[Projectile.owner].Heal(7);
                            }
                        }
                        else
                        {
                            Main.player[Projectile.owner].Heal(5);
                        }
                    }
                    else
                    {
                        Main.player[Projectile.owner].Heal(3);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDustDirect(Main.player[Projectile.owner].Center, Projectile.width, Projectile.height, DustID.Clentaminator_Green, 0, 0, 128, default, Scale: 1.1f);
                    }
                }
            }


            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.GreenMoss, 0, 0, 128, default, Scale: 1.25f);
            }


            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.ai[0] == 1)
            {
                if (Main.rand.NextBool(6))
                {
                    if (Main.rand.NextBool(4))
                    {
                        if (Main.rand.NextBool(5))
                        {
                            if (Main.rand.NextBool(6))
                            {
                                Main.player[Projectile.owner].Heal(9);
                            }
                            else
                            {
                                Main.player[Projectile.owner].Heal(7);
                            }
                        }
                        else
                        {
                            Main.player[Projectile.owner].Heal(5);
                        }
                    }
                    else
                    {
                        Main.player[Projectile.owner].Heal(3);
                    }
                    for (int i = 0; i < 15; i++)
                    {
                        Dust.NewDustDirect(Main.player[Projectile.owner].Center, Projectile.width, Projectile.height, DustID.Clentaminator_Green, 0, 0, 128, default, Scale: 1.1f);
                    }
                }
            }


            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.GreenMoss, 0, 0, 128, default, Scale: 1.25f);
            }


            // Finding the closest NPC to attack within maxDetectDistance range
            // If not found then returns null

        }

    }
    public class VerdantStalkThornBomb : ModProjectile
    {
        public override void OnSpawn(IEntitySource source)
        {


        }

        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            Projectile.width = 16; // The width of projectile hitbox
            Projectile.height = 16; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.scale = 2f;

            Projectile.penetrate = 1;
            Projectile.alpha = 255;
        }

        // Custom AI
        int Timer = 0;


        public override void AI()
        {
            Projectile.velocity.Y += 0.065f;
            if (Main.rand.Next(1, 3) == 1)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.DungeonGreen, 0, 0, 128, default, Scale: 1.25f);
            }

        }

        public override void OnKill(int timeLeft)
        {
            const int NumProjectiles = 4; // The humber of projectiles that this gun will shoot.

            for (int i = 0; i < NumProjectiles; i++)
            {
                // Rotate the velocity randomly by 30 degrees at max.
                Vector2 newVelocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(90));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                // Create a projectile.
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, newVelocity * 3, ModContent.ProjectileType<VerdantStalkThorn>(), Projectile.damage / 3, 1, Main.myPlayer);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
            {
                if (Main.rand.NextBool(4))
                {
                    if (Main.rand.NextBool(5))
                    {
                        if (Main.rand.NextBool(6))
                        {
                            Main.player[Projectile.owner].Heal(7);
                        }
                        else
                        {
                            Main.player[Projectile.owner].Heal(5);
                        }
                    }
                    else
                    {
                        Main.player[Projectile.owner].Heal(3);
                    }
                }
                else
                {
                    Main.player[Projectile.owner].Heal(1);
                }
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustDirect(Main.player[Projectile.owner].Center, Projectile.width, Projectile.height, DustID.Clentaminator_Green, 0, 0, 128, default, Scale: 1.1f);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.GreenMoss, 0, 0, 128, default, Scale: 1.25f);
            }

            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(6))
            {
                if (Main.rand.NextBool(8))
                {
                    if (Main.rand.NextBool(10))
                    {
                        if (Main.rand.NextBool(12))
                        {
                            Main.player[Projectile.owner].Heal(7);
                        }
                        else
                        {
                            Main.player[Projectile.owner].Heal(5);
                        }
                    }
                    else
                    {
                        Main.player[Projectile.owner].Heal(3);
                    }
                }
                else
                {
                    Main.player[Projectile.owner].Heal(1);
                }
                for (int i = 0; i < 15; i++)
                {
                    Dust.NewDustDirect(Main.player[Projectile.owner].Center, Projectile.width, Projectile.height, DustID.Clentaminator_Green, 0, 0, 128, default, Scale: 1.1f);
                }
            }
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.GreenMoss, 0, 0, 128, default, Scale: 1.25f);
            }
            // Finding the closest NPC to attack within maxDetectDistance range
            // If not found then returns null

        }
    }
}
