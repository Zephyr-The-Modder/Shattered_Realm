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
    public class ManyThorns : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.knockBack = 1f;
            Item.useStyle = ItemUseStyleID.Shoot; // Makes the player do the proper arm motion
            Item.useAnimation = 6;
            Item.useTime = 6;
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item28;
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;
            Item.noMelee = true; // The projectile will do the damage and not the item
            Item.crit = 20;
            Item.mana = 1;
            Item.ArmorPenetration = 5;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 1, 50, 0);
            Item.UseSound = SoundID.Item126;

            Item.shoot = ModContent.ProjectileType<CactusThorn>(); // The projectile is what makes a shortsword work
            Item.shootSpeed = 7f; // This value bleeds into the behavior of the projectile as velocity, keep that in mind when tweaking values

        }

        int ProjectileSpread = 0;
        float ProjectileRotation;
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int NumProjectiles = 2;
            for (int i = 0; i < NumProjectiles; i++)
            {
                Vector2 newVelocity;

                newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(25));

                Projectile.NewProjectile(source, position, newVelocity, type, damage, knockback, player.whoAmI);

                // Decrease velocity randomly for nicer visuals.
               
            }
            return false; // Return false because we don't want tModLoader to shoot projectile
        }
    }
    public class CactusThorn : ModProjectile
    {
        public override void OnSpawn(IEntitySource source)
        {


        }

        public override void SetDefaults()
        {
            Projectile.width = 4; // The width of projectile hitbox
            Projectile.height = 1; // The height of projectile hitbox

            Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
            Projectile.DamageType = DamageClass.Magic; // What type of damage does this projectile affect?
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.ignoreWater = false; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.timeLeft = 600; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.scale = 2f;

            Projectile.penetrate = 2;
        }

        // Custom AI
        int Timer = 0;

        public override void AI()
        {
            Projectile.velocity.Y += 0.125f;
            Projectile.rotation = Projectile.velocity.ToRotation();

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }
    }
}
