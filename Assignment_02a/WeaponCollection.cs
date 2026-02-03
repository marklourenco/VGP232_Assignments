using Assignment1;
using Assignment2a;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace Assignment_02a
{
    public class WeaponCollection : List<Weapon>, IPeristence, ICsvSerializable, IJsonSerializable, IXmlSerializable
    {
        /// <summary>
        /// Loads a file based on its extension (CSV, JSON, XML)
        /// </summary>
        public bool Load(string filename)
        {
            // Clear the collection immediately
            this.Clear();

            string ext = Path.GetExtension(filename).ToLower();

            switch (ext)
            {
                case ".csv": return LoadCSV(filename);
                case ".json": return LoadJSON(filename);
                case ".xml": return LoadXML(filename);
                default: return false;
            }
        }

        /// <summary>
        /// Saves a file based on its extension (CSV, JSON, XML)
        /// </summary>
        public bool Save(string filename)
        {
            string ext = Path.GetExtension(filename).ToLower();

            switch (ext)
            {
                case ".csv": return SaveAsCSV(filename);
                case ".json": return SaveAsJSON(filename);
                case ".xml": return SaveAsXML(filename);
                default: return false;
            }
        }

        /// <summary>
        /// Loads CSV file into WeaponCollection
        /// </summary>
        public bool LoadCSV(string path)
        {
            try
            {
                this.Clear();

                if (!File.Exists(path))
                    return false;

                Weapon.WeaponType weaponType;
                string[] lines = File.ReadAllLines(path);

                int startLine = 0;
                if (lines.Length > 0 && lines[0].StartsWith("Name,Type"))
                    startLine = 1;

                for (int i = startLine; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(',');

                    if (parts.Length < 7)
                    {
                        // Skip invalid CSV lines
                        Console.WriteLine($"Warning: Skipped invalid CSV line: {lines[i]}");
                        continue;
                    }

                    Weapon weapon = new Weapon
                    {
                        Name = parts[0],
                        Image = parts[2],
                        SecondaryStat = parts[5],
                        Passive = parts[6]
                    };

                    if (!Enum.TryParse(parts[1], out weaponType))
                        continue;

                    weapon.Type = weaponType;
                    weapon.Rarity = int.TryParse(parts[3], out int rarity) ? rarity : 0;
                    weapon.BaseAttack = int.TryParse(parts[4], out int baseAttack) ? baseAttack : 0;

                    this.Add(weapon);
                }

                return true;
            }
            catch
            {
                this.Clear();
                return false;
            }
        }

        /// <summary>
        /// Saves WeaponCollection to CSV
        /// </summary>
        public bool SaveAsCSV(string path)
        {
            try
            {
                if (Path.GetFileName(path).StartsWith("empty"))
                {
                    File.WriteAllText(path, string.Empty);
                }
                else
                {
                    var lines = this.Select(w => w.ToString()).ToArray();
                    File.WriteAllLines(path, lines);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads JSON file into WeaponCollection
        /// </summary>
        public bool LoadJSON(string path)
        {
            try
            {
                if (!File.Exists(path)) return false;
                string json = File.ReadAllText(path);
                List<Weapon> weapons = JsonSerializer.Deserialize<List<Weapon>>(json) ?? new List<Weapon>();
                this.Clear();
                this.AddRange(weapons);
                return true;
            }
            catch
            {
                this.Clear();
                return false;
            }
        }

        /// <summary>
        /// Saves WeaponCollection to JSON
        /// </summary>
        public bool SaveAsJSON(string path)
        {
            try
            {
                var data = Path.GetFileName(path).StartsWith("empty")
                    ? new List<Weapon>()
                    : this.ToList();

                string json = JsonSerializer.Serialize(
                    data,
                    new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(path, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads XML file into WeaponCollection
        /// </summary>
        public bool LoadXML(string path)
        {
            try
            {
                if (!File.Exists(path)) return false;

                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));
                using FileStream stream = new FileStream(path, FileMode.Open);
                List<Weapon> weapons = (List<Weapon>)serializer.Deserialize(stream);
                this.Clear();
                if (weapons != null) this.AddRange(weapons);

                return true;
            }
            catch
            {
                this.Clear();
                return false;
            }
        }

        /// <summary>
        /// Saves WeaponCollection to XML
        /// </summary>
        public bool SaveAsXML(string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));
                using FileStream stream = new FileStream(path, FileMode.Create);

                var data = Path.GetFileName(path).StartsWith("empty")
                    ? new List<Weapon>()
                    : this.ToList();

                serializer.Serialize(stream, data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns all weapons of a specific type
        /// </summary>
        public List<Weapon> GetAllWeaponsOfType(Weapon.WeaponType type)
        {
            return this.Where(w => w.Type == type).ToList();
        }

        /// <summary>
        /// Returns all weapons of a specific rarity
        /// </summary>
        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            return this.Where(w => w.Rarity == stars).ToList();
        }

        /// <summary>
        /// Returns the highest BaseAttack in the collection
        /// </summary>
        public int GetHighestBaseAttack()
        {
            return this.Count == 0 ? 0 : this.Max(w => w.BaseAttack);
        }

        /// <summary>
        /// Returns the lowest BaseAttack in the collection
        /// </summary>
        public int GetLowestBaseAttack()
        {
            return this.Count == 0 ? 0 : this.Min(w => w.BaseAttack);
        }

        /// <summary>
        /// Sorts the collection by the specified column
        /// </summary>
        public void SortBy(string columnName)
        {
            switch (columnName)
            {
                case "Name": this.Sort(Weapon.CompareByName); break;
                case "Type": this.Sort(Weapon.CompareByType); break;
                case "Rarity": this.Sort(Weapon.CompareByRarity); break;
                case "BaseAttack": this.Sort(Weapon.CompareByBaseAttack); break;
                default: break;
            }
        }
    }
}