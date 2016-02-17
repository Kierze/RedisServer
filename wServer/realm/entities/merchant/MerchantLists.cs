using common;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace wServer.realm.entities
{
    internal class MerchantLists
    {
        public static int[] AccessoryClothList;
        public static int[] AccessoryDyeList;
        public static int[] ClothingClothList;
        public static int[] ClothingDyeList;

        // Normal price, testing price, simplified price. Normal currency, testing currency,
        // simplified currency.
        public static Dictionary<int, Tuple<int, CurrencyType>> MerchantPrices = new Dictionary<int, Tuple<int, CurrencyType>>
        {
            {0xa07, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) }, // Wand of Death
            {0xa85, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Wand of Deep Sorcery
            {0xa86, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Wand of Shadow
            {0xa87, new Tuple<int, CurrencyType>(450, CurrencyType.Gold) }, // Wand of Ancient Warning
            {0xaf6, new Tuple<int, CurrencyType>(550, CurrencyType.Gold) }, // Wand of Recompense
            {0xa1e, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) },  // Golden Bow
            {0xa8b, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Verdant Bow
            {0xa8c, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Bow of Fey Magic
            {0xa8d, new Tuple<int, CurrencyType>(450, CurrencyType.Gold) }, // Bow of Innocent Blood
            {0xb02, new Tuple<int, CurrencyType>(600, CurrencyType.Gold) }, // Bow of Covert Havens
            {0xa19, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) },  // Fire Dagger
            {0xa88, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Ragetalon Dagger
            {0xa89, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Emeraldshard Dagger
            {0xa8a, new Tuple<int, CurrencyType>(450, CurrencyType.Gold) }, // Agateclaw Dagger
            {0xaff, new Tuple<int, CurrencyType>(650, CurrencyType.Gold) }, // Dagger of Foul Malevolence
            {0xa82, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) },  // Ravenheart Sword
            {0xa83, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Dragonsoul Sword
            {0xa84, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Archon Sword
            {0xa47, new Tuple<int, CurrencyType>(450, CurrencyType.Gold) }, // Skysplitter Sword
            {0xb0b, new Tuple<int, CurrencyType>(900, CurrencyType.Gold) }, // Sword of Acclaim
            {0xa9f, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) },  // Staff of Horror
            {0xaa0, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Staff of Necrotic Arcana
            {0xaa1, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Staff of Diabolic Secrets
            {0xaa2, new Tuple<int, CurrencyType>(450, CurrencyType.Gold) }, // Staff of Astral Knowledge
            {0xb08, new Tuple<int, CurrencyType>(900, CurrencyType.Gold) }, // Staff of the Cosmic Whole
            {0xc4c, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) },  // Demon Edge
            {0xc4d, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Jewel Eye Katana
            {0xc4e, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Ichimonji
            {0xc4f, new Tuple<int, CurrencyType>(450, CurrencyType.Gold) }, // Muramasa
            {0xc50, new Tuple<int, CurrencyType>(700, CurrencyType.Gold) }, // Masamune

            {0xabf, new Tuple<int, CurrencyType>(90, CurrencyType.Gold) },  // Ring of Paramount Attack
            {0xac0, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Ring of Paramount Defense
            {0xac1, new Tuple<int, CurrencyType>(90, CurrencyType.Gold) },  // Ring of Paramount Speed
            {0xac2, new Tuple<int, CurrencyType>(90, CurrencyType.Gold) },  // Ring of Paramount Vitality
            {0xac3, new Tuple<int, CurrencyType>(90, CurrencyType.Gold) },  // Ring of Paramount Wisdom
            {0xac4, new Tuple<int, CurrencyType>(90, CurrencyType.Gold) },  // Ring of Paramount Dexterity
            {0xac5, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Ring of Paramount Health
            {0xac6, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Ring of Paramount Magic
            {0xac7, new Tuple<int, CurrencyType>(180, CurrencyType.Gold) }, // Ring of Exalted Attack
            {0xac8, new Tuple<int, CurrencyType>(360, CurrencyType.Gold) }, // Ring of Exalted Defense
            {0xac9, new Tuple<int, CurrencyType>(180, CurrencyType.Gold) }, // Ring of Exalted Speed
            {0xaca, new Tuple<int, CurrencyType>(180, CurrencyType.Gold) }, // Ring of Exalted Vitality
            {0xacb, new Tuple<int, CurrencyType>(180, CurrencyType.Gold) }, // Ring of Exalted Wisdom
            {0xacc, new Tuple<int, CurrencyType>(180, CurrencyType.Gold) }, // Ring of Exalted Dexterity
            {0xacd, new Tuple<int, CurrencyType>(360, CurrencyType.Gold) }, // Ring of Exalted Health
            {0xace, new Tuple<int, CurrencyType>(360, CurrencyType.Gold) }, // Ring of Exalted Magic

            {0xa13, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) }, // Dragonscale Armor
            {0xa60, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) }, // Robe of the Master
            {0xa8e, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Roc Leather Armor
            {0xa8f, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Hippogriff Hide Armor
            {0xa90, new Tuple<int, CurrencyType>(425, CurrencyType.Gold) }, // Griffon Hide Armor
            {0xa91, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Desolation Armor
            {0xa92, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Vengeance Armor
            {0xa93, new Tuple<int, CurrencyType>(425, CurrencyType.Gold) }, // Abyssal Armor
            {0xa94, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Robe of the Shadow Magus
            {0xa95, new Tuple<int, CurrencyType>(225, CurrencyType.Gold) }, // Robe of the Moon Wizard
            {0xa96, new Tuple<int, CurrencyType>(425, CurrencyType.Gold) }, // Robe of the Elder Warlock
            {0xad3, new Tuple<int, CurrencyType>(51, CurrencyType.Gold) }, // Drake Hide Armor
            {0xaf9, new Tuple<int, CurrencyType>(800, CurrencyType.Gold) }, // Hydra Skin Armor
            {0xafc, new Tuple<int, CurrencyType>(850, CurrencyType.Gold) }, // Acropolis Armor
            {0xb05, new Tuple<int, CurrencyType>(850, CurrencyType.Gold) }, // Robe of the Grand Sorcerer

            {0xa0c, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Mithril Shield
            {0xa30, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Magic Nova Spell
            {0xa46, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Banishment Orb
            {0xa55, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Seal of the Holy Warrior
            {0xa5b, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Tome of Divine Favor
            {0xa65, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Golden Quiver
            {0xa6b, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Golden Helm
            {0xaa8, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Nightwing Venom
            {0xaaf, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Lifedrinker Skull
            {0xab6, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Dragonstalker Trap
            {0xae1, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Cloak of Endless Twilight
            {0xb20, new Tuple<int, CurrencyType>(175, CurrencyType.Gold) }, // Prism of Phantoms
            {0xb22, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Colossus Shield
            {0xb23, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Prism of Apparitions
            {0xb24, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Elemental Detonation Spell
            {0xb25, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Tome of Holy Guidance
            {0xb26, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Seal of the Blessed Champion
            {0xb27, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Cloak of Ghostly Concealment
            {0xb28, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Quiver of Elvish Mastery
            {0xb29, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Helm of the Great General
            {0xb2a, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Baneserpent Poison
            {0xb2b, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Bloodsucker Skull
            {0xb2c, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Giantcatcher Trap
            {0xb2d, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Planefetter Orb
            {0xb32, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Scepter of Skybolts
            {0xb33, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Scepter of Storms
            {0xc58, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Ice Star
            {0xc59, new Tuple<int, CurrencyType>(400, CurrencyType.Gold) }, // Doom Circle

            {0xcc4, new Tuple<int, CurrencyType>(240, CurrencyType.Gold) }, // Chocolate Cream Sandwich Cookie
            {0xcc5, new Tuple<int, CurrencyType>(180, CurrencyType.Gold) }, // Power Pizza
            {0xcc6, new Tuple<int, CurrencyType>(120, CurrencyType.Gold) }, // Great Taco
            {0xcc7, new Tuple<int, CurrencyType>(720, CurrencyType.Gold) }, // Double Cheeseburger Deluxe
            {0xcc8, new Tuple<int, CurrencyType>(480, CurrencyType.Gold) }, // Superburger
            {0xcc9, new Tuple<int, CurrencyType>(20, CurrencyType.Gold) }, // Soft Drink
            {0xcca, new Tuple<int, CurrencyType>(360, CurrencyType.Gold) }, // Grapes of Wrath
            {0xccb, new Tuple<int, CurrencyType>(60, CurrencyType.Gold) }, // Fries
            {0xccc, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Ambrosia

            {0xc86, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Feline Egg
            {0xc87, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Feline Egg
            {0xc8a, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Canine Egg
            {0xc8b, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Canine Egg
            {0xc8e, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Avian Egg
            {0xc8f, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Avian Egg
            {0xc92, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Exotic Egg
            {0xc93, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Exotic Egg
            {0xc96, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Farm Egg
            {0xc97, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Farm Egg
            {0xc9a, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Woodland Egg
            {0xc9b, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Woodland Egg
            {0xc9e, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Reptile Egg
            {0xc9f, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Reptile Egg
            {0xca2, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Insect Egg
            {0xca3, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Insect Egg
            {0xca6, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Penguin Egg
            {0xca7, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Penguin Egg
            {0xcaa, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Aquatic Egg
            {0xcab, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Aquatic Egg
            {0xcae, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Spooky Egg
            {0xcaf, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Spooky Egg
            {0xcb2, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Humanoid Egg
            {0xcb3, new Tuple<int, CurrencyType>(2000, CurrencyType.Gold) }, // Rare Humanoid Egg
            {0xcb6, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon ???? Egg
            {0xcb7, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare ???? Egg
            {0xcba, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Automaton Egg
            {0xcbb, new Tuple<int, CurrencyType>(1200, CurrencyType.Gold) }, // Rare Automaton Egg
            {0xcbe, new Tuple<int, CurrencyType>(300, CurrencyType.Gold) }, // Uncommon Mystery Egg
            {0xcbf, new Tuple<int, CurrencyType>(1000, CurrencyType.Gold) }, // Rare Mystery Egg

            {0x2290, new Tuple<int, CurrencyType>(150, CurrencyType.Gold) }, // Bella's Key
            {0x701, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Undead Lair Key
            {0x705, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Pirate Cave Key
            {0x70a, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Abyss of Demons Key
            {0x70b, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Snake Pit key
            {0x710, new Tuple<int, CurrencyType>(250, CurrencyType.Gold) }, // Tomb of the Ancients Key
            {0x71f, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Sprite World Key
            {0xc11, new Tuple<int, CurrencyType>(250, CurrencyType.Gold) }, // Ocean Trench Key
            {0xc19, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Totem Key
            {0xc23, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Manor Key
            {0xc2e, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Davy's Key
            {0xc2f, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Lab Key
            {0xcce, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Deadwater Docks Key
            {0xccf, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Woodland Labyrinth Key
            {0xcda, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // The Crawling Depths Key
            {0xcdd, new Tuple<int, CurrencyType>(250, CurrencyType.Gold) }, // Shatters Key
            {0xcd4, new Tuple<int, CurrencyType>(250, CurrencyType.Gold) }, // Draconis Key
            {0x2294, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) }, // Shaitan's Key

            {0xc6c, new Tuple<int, CurrencyType>(100, CurrencyType.Gold) } // Backpack
        };

        // Keys
        public static int[] StoreList1 =
        {
            0x2290, 0x701, 0x705, 0x70a, 0x70b, 0x710, 0x71f,
            0xc11, 0xc19, 0xc23, 0xc2e, 0xc2f, 0xcce, 0xccf,
            0xcda, 0xcdd, 0xcd4, 0x2294
        };

        public static int[] StoreList10 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList11 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList12 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList13 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList14 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList15 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList16 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList17 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList18 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        public static int[] StoreList19 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        // Rare Eggs & Backpack
        public static int[] StoreList2 =
        {
            0xc6c, 0xc87, 0xc8b, 0xc8f, 0xc93, 0xc97, 0xc9b,
            0xc9f, 0xca3, 0xca7, 0xcab, 0xcaf, 0xcb3, 0xcb7,
            0xcbb, 0xcbf
        };

        public static int[] StoreList20 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        // Uncommon Eggs & Food
        public static int[] StoreList3 =
        {
            0xccc, 0xccb, 0xcca, 0xcc9, 0xcc8, 0xcc7, 0xcc6,
            0xcc5, 0xcc4
        };

        // Abilities
        public static int[] StoreList4 =
        {
            0xb25, 0xa5b, 0xb22, 0xa0c, 0xb24, 0xa30, 0xb26,
            0xa55, 0xb27, 0xae1, 0xb28, 0xa65, 0xb29, 0xa6b,
            0xb2a, 0xaa8, 0xb2b, 0xaaf, 0xb2c, 0xab6, 0xb2d,
            0xa46, 0xb23, 0xb20, 0xb33, 0xb32, 0xc59, 0xc58
        };

        // Armor
        public static int[] StoreList5 =
        {
            0xb05, 0xa96, 0xa95, 0xa94, 0xa60, 0xafc, 0xa93,
            0xa92, 0xa91, 0xa13, 0xaf9, 0xa90, 0xa8f, 0xa8e,
            0xad3
        };

        // Weapons 1
        public static int[] StoreList6 =
        {
            0xaf6, 0xa87, 0xa86, 0xa85, 0xa07, 0xb02, 0xa8d,
            0xa8c, 0xa8b, 0xa1e, 0xb08, 0xaa2, 0xaa1, 0xaa0,
            0xa9f
        };

        // Weapons 2
        public static int[] StoreList7 =
        {
            0xb0b, 0xa47, 0xa84, 0xa83, 0xa82, 0xaff, 0xa8a,
            0xa89, 0xa88, 0xa19, 0xc50, 0xc4f, 0xc4e, 0xc4d,
            0xc4c
        };

        // Rings
        public static int[] StoreList8 =
        {
            0xabf, 0xac0, 0xac1, 0xac2, 0xac3, 0xac4, 0xac5,
            0xac6, 0xac7, 0xac8, 0xac9, 0xaca, 0xacb, 0xacc,
            0xacd, 0xace
        };

        // Shatters
        public static int[] StoreList9 =
        {
            0xb41, 0xbab, 0xbad, 0xbac
        };

        private static readonly string[] ExcludedCloths =
        {
            "Large Ivory Dragon Scale Cloth", "Small Ivory Dragon Scale Cloth",
            "Large Green Dragon Scale Cloth", "Small Green Dragon Scale Cloth",
            "Large Midnight Dragon Scale Cloth", "Small Midnight Dragon Scale Cloth",
            "Large Blue Dragon Scale Cloth", "Small Blue Dragon Scale Cloth",
            "Large Red Dragon Scale Cloth", "Small Red Dragon Scale Cloth",
            "Large Jester Argyle Cloth", "Small Jester Argyle Cloth",
            "Large Alchemist Cloth", "Small Alchemist Cloth",
            "Large Mosaic Cloth", "Small Mosaic Cloth",
            "Large Spooky Cloth", "Small Spooky Cloth",
            "Large Flame Cloth", "Small Flame Cloth",
            "Large Heavy Chainmail Cloth", "Small Heavy Chainmail Cloth",
            "Large Blue Camo Cloth", "Small Blue Camo Cloth"
        };

        private static readonly ILog logger = LogManager.GetLogger(typeof(MerchantLists));

        public static void InitMerchantLists(XmlData data)
        {
            logger.Info("Loading MerchantLists");
            List<int> accessoryDyeList = new List<int>();
            List<int> clothingDyeList = new List<int>();
            List<int> accessoryClothList = new List<int>();
            List<int> clothingClothList = new List<int>();

            foreach (KeyValuePair<ushort, Item> item in data.Items.Where(_ => ExcludedCloths.All(i => i != _.Value.ObjectId)))
            {
                if (item.Value.Texture1 != 0 && item.Value.ObjectId.Contains("Clothing") && item.Value.ObjectId.Contains("Dye"))
                {
                    MerchantPrices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(51, CurrencyType.Gold));
                    clothingDyeList.Add(item.Value.ObjectType);
                }

                if (item.Value.Texture2 != 0 && item.Value.ObjectId.Contains("Accessory") && item.Value.ObjectId.Contains("Dye"))
                {
                    MerchantPrices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(51, CurrencyType.Gold));
                    accessoryDyeList.Add(item.Value.ObjectType);
                }

                if (item.Value.Texture1 != 0 && item.Value.ObjectId.Contains("Cloth") && item.Value.ObjectId.Contains("Large"))
                {
                    MerchantPrices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(160, CurrencyType.Gold));
                    clothingClothList.Add(item.Value.ObjectType);
                }

                if (item.Value.Texture2 != 0 && item.Value.ObjectId.Contains("Cloth") && item.Value.ObjectId.Contains("Small"))
                {
                    MerchantPrices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(160, CurrencyType.Gold));
                    accessoryClothList.Add(item.Value.ObjectType);
                }
            }

            ClothingDyeList = clothingDyeList.ToArray();
            ClothingClothList = clothingClothList.ToArray();
            AccessoryClothList = accessoryClothList.ToArray();
            AccessoryDyeList = accessoryDyeList.ToArray();
            logger.Info("MerchantLists added");
        }
    }
}
