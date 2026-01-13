using Assignment1;
using Assignment2a;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_02a
{
    public class WeaponCollection : List<Weapon>, IPeristence
    {
        public bool Load(string filename)
        {
            // TODO: implement this method to load weapons from a file
            Weapon.WeaponType weaponType;
            if (File.Exists(filename)) {
                string[] lines = File.ReadAllLines(filename);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 7)
                    {
                        Weapon weapon = new Weapon();
                        weapon.Name = parts[0];
                        if (Enum.TryParse(parts[1], out weaponType))
                        {
                            weapon.Type = weaponType;
                        }
                        weapon.Image = parts[2];
                        weapon.Rarity = int.TryParse(parts[3], out int rarity) ? rarity : 0;
                        weapon.BaseAttack = int.TryParse(parts[4], out int baseAttack) ? baseAttack : 0;
                        weapon.SecondaryStat = parts[5];
                        weapon.Passive = parts[6];
                        this.Add(weapon);
                    }
                }
                return true;
            }
            return false;
        }

        public bool Save(string filename)
        {
            // TODO: implement this method to save weapons to a file
            Weapon weapon;
            List<string> lines = new List<string>();
            foreach (var item in this)
            {
                weapon = item;
                string line = $"{weapon.Name},{weapon.Type},{weapon.Rarity},{weapon.BaseAttack},{weapon.Image},{weapon.SecondaryStat},{weapon.Passive}";
                lines.Add(line);
            }
            File.WriteAllLines(filename, lines);
            return true;
        }

        public int GetHighestBaseAttack()
        {
            // TODO: implement this method to return the highest BaseAttack value from the collection
            int highestBaseAttack = 0;
            foreach (var item in this) {
                if (item.BaseAttack > highestBaseAttack) {
                    highestBaseAttack = item.BaseAttack;
                }
            }
            return highestBaseAttack;
        }

        public int GetLowestBaseAttack()
        {
            // TODO: implement this method to return the lowest BaseAttack value from the collection
            int lowestBaseAttack = 0;
            foreach (var item in this)
            {
                if (lowestBaseAttack == 0 || item.BaseAttack < lowestBaseAttack)
                {
                    lowestBaseAttack = item.BaseAttack;
                }
            }
            return lowestBaseAttack;
        }

        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType type)
        {
            // TODO: implement this method to return a list of all weapons of the specified type
            Weapon weapon;
            List<Weapon> weaponsOfType = new List<Weapon>();
            foreach (var item in this)
            {
                weapon = item;
                if (weapon.Type == type)
                {
                    weaponsOfType.Add(weapon);
                }
            }
            return weaponsOfType;
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            // TODO: implement this method to return a list of all weapons of the specified rarity
            Weapon weapon;
            List<Weapon> weaponsOfRarity = new List<Weapon>();
            foreach (var item in this)
            {
                weapon = item;
                if (weapon.Rarity == stars)
                {
                    weaponsOfRarity.Add(weapon);
                }
            }
            return weaponsOfRarity;
        }

        public void SortBy(string columnName)
        {
            // TODO: implement this method to sort the collection by the specified column name
            switch (columnName)
            {
                case "Name":
                    this.Sort(Weapon.CompareByName);
                    break;
                case "Type":
                    this.Sort(Weapon.CompareByType);
                    break;
                case "Rarity":
                    this.Sort(Weapon.CompareByRarity);
                    break;
                case "BaseAttack":
                    this.Sort(Weapon.CompareByBaseAttack);
                    break;
                default:
                    break;
            }
        }
    }
}
