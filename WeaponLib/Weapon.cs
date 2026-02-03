using System.Collections.ObjectModel;
using System.Text.Json;
using System.Xml.Serialization;

namespace WeaponLib
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

    public class WeaponCollection : ObservableCollection<Weapon>, IPeristence, ICsvSerializable, IJsonSerializable, IXmlSerializable
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
                foreach (var w in weapons)
                    this.Add(w);

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
                if (weapons != null)
                {
                    foreach (var w in weapons)
                        this.Add(w);

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
            List<Weapon> sorted = columnName switch
            {
                "Name" => this.OrderBy(w => w.Name).ToList(),
                "Type" => this.OrderBy(w => w.Type).ToList(),
                "Rarity" => this.OrderBy(w => w.Rarity).ToList(),
                "BaseAttack" => this.OrderBy(w => w.BaseAttack).ToList(),
                _ => this.ToList()
            };

            this.Clear();
            foreach (var w in sorted)
                this.Add(w);
        }
    }

    public interface IPeristence
    {
        bool Load(string filename);
        bool Save(string filename);
    }

    public interface ICsvSerializable
    {
        bool LoadCSV(string path);
        bool SaveAsCSV(string path);
    }

    public interface IJsonSerializable
    {
        bool LoadJSON(string path);
        bool SaveAsJSON(string path);
    }

    public interface IXmlSerializable
    {
        bool LoadXML(string path);
        bool SaveAsXML(string path);
    }
}