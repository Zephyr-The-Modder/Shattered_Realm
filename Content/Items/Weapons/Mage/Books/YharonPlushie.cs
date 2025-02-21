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
    public class YharonPlushie : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Acorn Staff"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("Hitting an ememy restores all used mana");
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 2100000;
            Item.knockBack = 1f;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player do the proper arm motion
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item28;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.noMelee = true; // The projectile will do the damage and not the item
            Item.crit = 20;
            Item.ArmorPenetration = 5;
            Item.master = true;

            Item.rare = ItemRarityID.Master;
            Item.value = Item.sellPrice(999, 0, 0, 0);
            Item.UseSound = SoundID.Item16;

            Item.shoot = ModContent.ProjectileType<YharonPlushieProj>(); // The projectile is what makes a shortsword work
            Item.shootSpeed = 21f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values

        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int NumProjectiles = 3; // The number of projectiles that this gun will shoot.

            for (int i = 0; i < NumProjectiles; i++)
            {
                // Rotate the velocity randomly by 30 degrees at max.
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
            }

            return false; // Return false because we don't want tModLoader to shoot projectile
        }
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 12f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }

        }
    }
    public class YharonPlushieProj : ModProjectile
    {
        int StateTimer = 0;
        private NPC HomingTarget
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }
        public override void SetDefaults()
        {
            Projectile.width = 24; // The width of projectile hitbox
            Projectile.height = 26; // The height of projectile hitbox
            Projectile.light = 0.1f;

            // Ccopy the ai of any given projectile using AIType, since we want
            // the projectile to essentially behave the same way as the vanilla projectile.
            AIType = 0;

            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.DamageType = DamageClass.Magic; // Is the projectile shoot by a ranged weapon?
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false; // Can the projectile collide with tiles?
            Projectile.timeLeft = 6000; // Each update timeLeft is decreased by 1. Once timeLeft hits 0, the Projectile will naturally despawn. (60 ticks = 1 second)

            Projectile.penetrate = 1;
            Projectile.ArmorPenetration = 999999;
            Projectile.alpha = 255;
            // 1: Projectile.penetrate = 1; // Will hit even if npc is currently immune to player
            // 2a: Projectile.penetrate = -1; // Will hit and unless 3 is use, set 10 ticks of immunity
            // 2b: Projectile.penetrate = 3; // Same, but max 3 hits before dying
            // 5: Projectile.usesLocalNPCImmunity = true;
            // 5a: Projectile.localNPCHitCooldown = -1; // 1 hit per npc max
            // 5b: Projectile.localNPCHitCooldown = 20; // 20 ticks before the same npc can be hit again
        }
        public ref float DelayTimer => ref Projectile.ai[1];
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.alpha -= 25;
            float maxDetectRadius = 99999f; // The maximum radius at which a projectile can detect a target

            // A short delay to homing behavior after being fired
            if (DelayTimer < 15)
            {
                DelayTimer += 1;
                return;
            }

            // First, we find a homing target if we don't have one
            if (HomingTarget == null)
            {
                HomingTarget = FindClosestNPC(maxDetectRadius);
            }

            // If we have a homing target, make sure it is still valid. If the NPC dies or moves away, we'll want to find a new target
            if (HomingTarget != null && !IsValidTarget(HomingTarget))
            {
                HomingTarget = null;
            }

            // If we don't have a target, don't adjust trajectory
            if (HomingTarget == null)
                return;

            // If found, we rotate the projectile velocity in the direction of the target.
            // We only rotate by 3 degrees an update to give it a smooth trajectory. Increase the rotation speed here to make tighter turns
            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(15)).ToRotationVector2() * length;
        }

        // Finding the closest NPC to attack within maxDetectDistance range
        // If not found then returns null
        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
            int Maxhp = 0;

            // Loop through all NPCs
            foreach (var target in Main.ActiveNPCs)
            {
                // Check if NPC able to be targeted. 
                if (IsValidTarget(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        if (target.life > Maxhp)
                        {
                            closestNPC = target;
                            sqrMaxDetectDistance = sqrDistanceToTarget;
                        }
                    }
                }
            }

            return closestNPC;
        }
        public bool IsValidTarget(NPC target)
        {
            // This method checks that the NPC is:
            // 1. active (alive)
            // 2. chaseable (e.g. not a cultist archer)
            // 3. max life bigger than 5 (e.g. not a critter)
            // 4. can take damage (e.g. moonlord core after all it's parts are downed)
            // 5. hostile (!friendly)
            // 6. not immortal (e.g. not a target dummy)
            // 7. doesn't have solid tiles blocking a line of sight between the projectile and NPC
            return target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
        }

    }
}
