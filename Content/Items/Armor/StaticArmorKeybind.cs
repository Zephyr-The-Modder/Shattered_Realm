using Terraria;
using Terraria.ID;
using Terraria.GameInput;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using ShatteredRealm.Content.Globals;


namespace ShatteredRealm.Content.Items.Armor
{
    // See Common/Systems/KeybindSystem for keybind registration.
    // Acts as a container for keybinds registered by this mod.
    // See Common/Players/ExampleKeybindPlayer for usage.
    public class StaticKeybindSystem : ModSystem
    {
        public static ModKeybind StaticBonusKeybind { get; private set; }

        public override void Load()
        {
            // Registers a new keybind
            // We localize keybinds by adding a Mods.{ModName}.Keybind.{KeybindName} entry to our localization files. The actual text displayed to english users is in en-US.hjson
            StaticBonusKeybind = KeybindLoader.RegisterKeybind(Mod, "StaticSetBonus", "Z");
        }

        // Please see ExampleMod.cs' Unload() method for a detailed explanation of the unloading process.
        public override void Unload()
        {
            // Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
            StaticBonusKeybind = null;
        }
    }
    public class StaticArmorKeybind : ModPlayer
    {
        int Timer = 0;
        bool Buff;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (StaticKeybindSystem.StaticBonusKeybind.JustPressed && !Player.HasBuff(ModContent.BuffType<Drained>()) && Player.GetModPlayer<ShatteredPlayer>().StaticLegs)
            {
                if (Player.CheckMana(45, true))
                {
                    Player.Teleport(Main.MouseWorld, TeleportationStyleID.RecallPotion);
                    Player.AddBuff(ModContent.BuffType<Drained>(), Main.rand.Next(240, 361));
                }

            }
        }
    }
    public class Drained : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;  // Is it a debuff?
            Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
            Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
            BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
        }

        // Allows you to make this buff give certain effects to the given player
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) *= 0.85f;
            player.statDefense -= 20;
            player.moveSpeed *= 0.55f;
            player.lifeRegen = -14;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.defense -= 25;
            npc.velocity *= 0.7f;
            npc.lifeRegen = -10;
        }
    }

}