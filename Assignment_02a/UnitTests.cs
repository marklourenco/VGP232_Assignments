using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment1;
using NUnit.Framework;

namespace Assignment_02a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection WeaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            WeaponCollection = new WeaponCollection();

            // load the CSV so that GetAllWeaponsOfRarity has data
            bool loaded = WeaponCollection.Load(inputPath);
            if (!loaded)
                throw new Exception("Failed to load data for tests");
        }


        [TearDown]
        public void CleanUp()
        {
            string[] files =
                {
                    "weapons.csv",
                    "weapons.json",
                    "weapons.xml",
                    "empty.csv",
                    "empty.json",
                    "empty.xml"
                };

            foreach (string file in files)
            {
                string path = CombineToAppPath(file);
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            WeaponCollection.Load(inputPath);
            int highest = WeaponCollection.GetHighestBaseAttack();
            Assert.That(highest, Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            WeaponCollection.Load(inputPath);
            int lowest = WeaponCollection.GetLowestBaseAttack();
            Assert.That(lowest, Is.EqualTo(23));
        }

        [TestCase(Weapon.WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(
            Weapon.WeaponType type, int expectedValue)
        {
            WeaponCollection.Load(inputPath);
            var list = WeaponCollection.GetAllWeaponsOfType(type);
            Assert.That(list.Count, Is.EqualTo(expectedValue));
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(
            int stars, int expectedValue)
        {
            WeaponCollection.Load(inputPath);
            var list = WeaponCollection.GetAllWeaponsOfRarity(stars);
            Assert.That(list.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            bool result = WeaponCollection.Load(inputPath);
            Assert.That(result);
            Assert.That(WeaponCollection.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            bool result = WeaponCollection.Load("NonExistentFile.csv");
            Assert.That(!result);
            Assert.That(WeaponCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            WeaponCollection.Load(inputPath);
            Assert.That(WeaponCollection.Save(outputPath));
            WeaponCollection newCollection = new WeaponCollection();
            Assert.That(newCollection.Load(outputPath));
            Assert.That(newCollection.Count == WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            WeaponCollection.Clear();
            Assert.That(WeaponCollection.Save(outputPath));
            Assert.That(WeaponCollection.Load(outputPath));
            Assert.That(WeaponCollection.Count == 0);
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            // TODO: create a Weapon with the stats above set properly
            Weapon expected = null;
            // TODO: uncomment this once you added the Type1 and Type2
            expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = Weapon.WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented.
            Assert.That(Weapon.TryParse(line, out actual));
            Assert.That(expected.Name, Is.EqualTo(actual.Name));
            Assert.That(expected.Type, Is.EqualTo(actual.Type));
            Assert.That(expected.BaseAttack, Is.EqualTo(actual.BaseAttack));
            // TODO: check for the rest of the properties, Image,Rarity,SecondaryStat,Passive
            Assert.That(expected.Image, Is.EqualTo(actual.Image));
            Assert.That(expected.Rarity, Is.EqualTo(actual.Rarity));
            Assert.That(expected.SecondaryStat, Is.EqualTo(actual.SecondaryStat));
            Assert.That(expected.Passive, Is.EqualTo(actual.Passive));
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null.
            string line = "1,Bulbasaur,A,B,C,65,65";
            Weapon actual = null;
            Assert.That(!Weapon.TryParse(line, out actual));
        }

        // 2b TESTS

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.Save("weapons.json"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.Load("weapons.json"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.SaveAsJSON("weapons.json"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.Load("weapons.json"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.SaveAsJSON("weapons.json"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.LoadJSON("weapons.json"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.Save("weapons.json"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.LoadJSON("weapons.json"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.Save("weapons.csv"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.Load("weapons.csv"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.SaveAsCSV("weapons.csv"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.LoadCSV("weapons.csv"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.Save("weapons.xml"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.Load("weapons.xml"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.SaveAsXML("weapons.xml"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.LoadXML("weapons.xml"));
            Assert.That(wc.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
            Assert.That(WeaponCollection.SaveAsJSON("empty.json"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.Load("empty.json"));
            Assert.That(wc.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            Assert.That(WeaponCollection.SaveAsCSV("empty.csv"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.Load("empty.csv"));
            Assert.That(wc.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            Assert.That(WeaponCollection.SaveAsXML("empty.xml"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(wc.Load("empty.xml"));
            Assert.That(wc.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.SaveAsJSON("weapons.json"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(!wc.LoadXML("weapons.json"));
            Assert.That(wc.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            Assert.That(WeaponCollection.Load(inputPath));
            Assert.That(WeaponCollection.SaveAsXML("weapons.xml"));

            WeaponCollection wc = new WeaponCollection();
            Assert.That(!wc.LoadJSON("weapons.xml"));
            Assert.That(wc.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            Assert.That(!WeaponCollection.LoadXML(inputPath));
            Assert.That(WeaponCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            Assert.That(!WeaponCollection.LoadJSON(inputPath));
            Assert.That(WeaponCollection.Count, Is.EqualTo(0));
        }
    }
}
