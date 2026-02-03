using System;

namespace Assignment1
{
    public class Weapon
    {
        public enum WeaponType
        {
            Sword,
            Polearm,
            Claymore,
            Catalyst,
            Bow,
            None
        }

        // Name,Type,Rarity,BaseAttack,Image,SecondaryStat
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string Image { get; set; }
        public string SecondaryStat { get; set; }
        public string Passive { get; set; }

        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right) => left.Name.CompareTo(right.Name);

        // TODO: add sort for each property:
        // CompareByType
        public static int CompareByType(Weapon left, Weapon right) => left.Type.CompareTo(right.Type);

        // CompareByRarity
        public static int CompareByRarity(Weapon left, Weapon right) => left.Rarity.CompareTo(right.Rarity);

        // CompareByBaseAttack
        public static int CompareByBaseAttack(Weapon left, Weapon right) => left.BaseAttack.CompareTo(right.BaseAttack);

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formatted string</returns>
        public override string ToString()
        {
            // TODO: construct a comma separated value string
            // Name,Type,Rarity,BaseAttack
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        /// <summary>
        /// Tries to parse a comma-separated string into a Weapon object
        /// </summary>
        /// <param name="rawData">CSV string of the weapon</param>
        /// <param name="weapon">Output weapon object</param>
        /// <returns>True if parsing succeeded, false otherwise</returns>
        public static bool TryParse(string rawData, out Weapon weapon)
        {
            weapon = null;

            if (string.IsNullOrWhiteSpace(rawData))
                return false;

            string[] values = rawData.Split(',');
            if (values.Length < 7)
                return false;

            // Parse each property safely
            if (!Enum.TryParse(values[1], out WeaponType type))
                return false;

            if (!int.TryParse(values[3], out int rarity))
                return false;

            if (!int.TryParse(values[4], out int baseAttack))
                return false;

            // Assign parsed values
            weapon = new Weapon
            {
                Name = values[0],
                Type = type,
                Image = values[2],
                Rarity = rarity,
                BaseAttack = baseAttack,
                SecondaryStat = values[5],
                Passive = values[6]
            };

            return true;
        }
    }
}