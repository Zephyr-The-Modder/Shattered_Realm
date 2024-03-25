using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using ShatteredRealm.Content.Items.Ammos;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace ShatteredRealm.Content.Items.Weapons.Melee.Swords
{
	// This Example show how to implement simple homing projectile
	// Can be tested with ExampleCustomAmmoGun
	public class StratosBaneProjectile : ModProjectile
	{
		int StateTimer = 0;
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		public override void OnSpawn(IEntitySource source)
        {
			Projectile.scale = Main.rand.NextFloat(0.7f, 1.4f);
		}
		public override void SetDefaults()
		{
			Projectile.width = 17; // The width of projectile hitbox
			Projectile.height = 32; // The height of projectile hitbox

			Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
			Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false; // Can the projectile collide with tiles?
			Projectile.timeLeft = 60; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)

			Projectile.ArmorPenetration = 25;
			Projectile.penetrate = 3;
		}

		// Custom AI
		int Timer = 0;
	
		
		public override void AI()
		{
			StateTimer++;
			Lighting.AddLight(Projectile.Center, Color.DeepSkyBlue.ToVector3());
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity *= 0.93f;
			Projectile.scale *= 0.98f;
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

		// Finding the closest NPC to attack within maxDetectDistance range
		// If not found then returns null

	}

	public class StratosFireball : ModProjectile
	{
		public override void OnSpawn(IEntitySource source)
		{
			Projectile.scale = Main.rand.NextFloat(0.7f, 1.4f);
		}

        public override void SetStaticDefaults()
        {
			Main.projFrames[Type] = 4;

		}
        public override void SetDefaults()
		{
			Projectile.width = 34; // The width of projectile hitbox
			Projectile.height = 58; // The height of projectile hitbox

			Projectile.aiStyle = 0; // The ai style of the projectile (0 means custom AI). For more please reference the source code of Terraria
			Projectile.DamageType = DamageClass.Melee; // What type of damage does this projectile affect?
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false; // Can the projectile collide with tiles?
			Projectile.timeLeft = 135; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)

			Projectile.ArmorPenetration = 3;
			Projectile.penetrate = 6;
			Projectile.alpha = 255;
		}

		// Custom AI
		int Timer = 0;


		public override void AI()
		{
			Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFlare, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, default, default, 2f);
			Lighting.AddLight(Projectile.Center, Color.DeepSkyBlue.ToVector3());
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Timer == 0)
			{
				Projectile.frame = 0;
			}
			if (Timer == 10)
            {
				Projectile.frame = 1;
            }
			if (Timer == 20)
			{
				Projectile.frame = 2;
			}
			if (Timer == 30)
			{
				Projectile.frame = 3;
				Timer = 0;
			}
			Timer++;
		}



		// Finding the closest NPC to attack within maxDetectDistance range
		// If not found then returns null

	}

}