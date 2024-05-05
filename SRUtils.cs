using Microsoft.CodeAnalysis.Operations;
using Microsoft.Xna.Framework;
using ShatteredRealm.Content.Globals;
using ShatteredRealm.Content.Items.Accessories.Ardent;
using Steamworks;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria;

namespace ShatteredRealm
{
	public static class SRUtils
	{
        public static NPC GetClosestNPC(Vector2 position, float maxDetectDistance, params NPC[] ignoreNPCs)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy() && !ignoreNPCs.Contains(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, position);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }
        

        public static NPC GetStrongestNearbyNPC(Vector2 position, float maxDetectDistance, params NPC[] ignoreNPCs)
        {
            int MinimumHP = 6;
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
                if (target.CanBeChasedBy() && !ignoreNPCs.Contains(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, position);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance && target.lifeMax > MinimumHP)
                    {
                        MinimumHP = target.lifeMax; 
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }
        public static int ScaleDamage(int origDmg, Player player, DamageClass damageClass)
        {
            return (int)(origDmg * player.GetTotalDamage(damageClass).Additive * player.GetTotalDamage(damageClass).Multiplicative + player.GetTotalDamage(damageClass).Flat);
        }
        public static int ScaleShieldEffectPower(int stat, Player player, float effectiveness = 1)
        {
            return (int)(stat * ((player.GetModPlayer<ShatteredPlayer>().ShieldEffectPower - 1) * effectiveness + 1));
        }
        public static float ScaleShieldEffectPower(float stat, Player player, float effectiveness = 1)
        {
            return stat * ((player.GetModPlayer<ShatteredPlayer>().ShieldEffectPower - 1) * effectiveness + 1);
        }

        public static ShieldItem shieldItem(this Item item)
        {
            return item.GetGlobalItem<ShieldItem>();
        }
    }
    class NPCandValue : IComparable<NPCandValue>
    {
        public NPC npc { get; set; }
        public float value { get; set; }

        public int CompareTo(NPCandValue other)
        {
            return this.value.CompareTo(other.value);
        }
    }
}