using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static int CompareByName(Weapon left, Weapon right)
        {
            return left.Name.CompareTo(right.Name);
        }
        // TODO: add sort for each property:
        // CompareByType
        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }
        // CompareByRarity
        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }
        // CompareByBaseAttack
        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            // TODO: construct a comma seperated value string
            // Name,Type,Rarity,BaseAttack
            string result = $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
            return result;
        }

        public static bool TryParse(string rawData, out Weapon weapon)
        {
            string[] values = rawData.Split(',');

            weapon = new Weapon();

            // use tryparse for each property, if any fails return false
            weapon.Name = values[0];
            if (Enum.TryParse<WeaponType>(values[1], out WeaponType type))
            {
                weapon.Type = type;
            }
            else
            {
                Console.Write($"Type {values[1]} is invalid format");
                return false;
            }
            weapon.Image = values[2];
            if (int.TryParse(values[3], out int rarity))
            {
                weapon.Rarity = rarity;
            }
            else
            {
                Console.Write($"Rarity {values[3]} is invalid format");
                return false;
            }
            if (int.TryParse(values[4], out int baseAttack))
            {
                weapon.BaseAttack = baseAttack;
            }
            else
            {
                Console.Write($"Base Attack {values[4]} is invalid format");
                return false;
            }
            weapon.SecondaryStat = values[5];
            weapon.Passive = values[6];

            weapon = new Weapon()
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
