using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.GameContent.UI;
using Terraria.ModLoader.IO;
using ShatteredRealm.Content.Globals;
using ShatteredRealm.Content.Items.Accessories.Shield;
using ShatteredRealm.Content.Items.Accessories.ShieldModifiers;
using System.Diagnostics.Contracts;

namespace ShatteredRealm.Content.Prefixes.Shield
{
    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    public class PrefixIncorrigible : ModPrefix
    {
        // We declare a custom *virtual* property here, so that another type, ExampleDerivedPrefix, could override it and change the effective power for itself.

        // Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
        public override PrefixCategory Category => PrefixCategory.Accessory;

        // See documentation for vanilla weights and more information.
        // In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
        // Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
        // Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
        public override float RollChance(Item item)
        {
            return 4f;
        }

        // Determines if it can roll at all.
        // Use this to control if a prefix can be rolled or not.
        public override bool CanRoll(Item item)
        {
            return item.shieldItem().shield;
        }


        // This is used to modify most other stats of items which have this modifier.
        public override void Apply(Item item)
        {
            item.shieldItem().cooldown = (int)(item.shieldItem().cooldown * 1.2f);
        }
        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 0.8f;
        }

        // This prefix doesn't affect any non-standard stats, so these additional tooltiplines aren't actually necessary, but this pattern can be followed for a prefix that does affect other stats.
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            yield return new TooltipLine(Mod, "PrefixWeaponAwesomeDescription", NegativeTooltip.Value)
            {
                IsModifier = true,

                IsModifierBad = true,
            };
            // If possible and suitable, try to reuse the name identifier and translation value of Terraria prefixes. For example, this code uses the vanilla translation for the word defense, resulting in "-5 defense". Note that IsModifierBad is used for this bad modifier.
            /*yield return new TooltipLine(Mod, "PrefixAccDefense", "-5" + Lang.tip[25].Value) {
				IsModifier = true,
				IsModifierBad = true,
			};*/
        }

        // AdditionalTooltip shows off how to do the inheritable localized properties approach. This is necessary this this example uses inheritance and we want different translations for each inheriting class. https://github.com/tModLoader/tModLoader/wiki/Localization#inheritable-localized-properties
        public LocalizedText NegativeTooltip => this.GetLocalization(nameof(NegativeTooltip));


        public override void SetStaticDefaults()
        {
            // this.GetLocalization is not used here because we want to use a shared key
            // This seemingly useless code is required to properly register the key for AdditionalTooltip
            _ = NegativeTooltip;

        }
    }
}