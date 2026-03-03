using System;
using System.Windows;
using System.Windows.Controls;
using WeaponLib;

namespace Assignment_02c
{
    public partial class EditWeaponWindow : Window
    {
        public Weapon TempWeapon { get; private set; }
        private Random random = new Random();

        public EditWeaponWindow(Weapon weapon = null)
        {
            InitializeComponent();
            Setup(weapon);
        }

        private void Setup(Weapon weapon)
        {
            if (weapon == null)
            {
                TempWeapon = new Weapon();
                Title = "Add Weapon";
                SubmitButton.Content = "Add";
            }
            else
            {
                TempWeapon = weapon;
                Title = "Edit Weapon";
                SubmitButton.Content = "Save";

                NameTextBox.Text = weapon.Name;
                BaseAttackTextBox.Text = weapon.BaseAttack.ToString();
                SecondaryStatTextBox.Text = weapon.SecondaryStat;
                PassiveTextBox.Text = weapon.Passive;
                ImageTextBox.Text = weapon.Image;

                TypeComboBox.SelectedIndex = (int)weapon.Type;
                RarityComboBox.SelectedIndex = weapon.Rarity - 1;
            }
        }

        private void SubmitClicked(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Name must not be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                NameTextBox.Focus();
                return;
            }

            if (TypeComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a weapon type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                TypeComboBox.Focus();
                return;
            }

            string url = ImageTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(url) ||
                !(url.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || url.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Image URL must end with .png or .jpg.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                ImageTextBox.Focus();
                return;
            }

            if (RarityComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a rarity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                RarityComboBox.Focus();
                return;
            }

            if (!int.TryParse(BaseAttackTextBox.Text, out int baseAttack))
            {
                MessageBox.Show("Base Attack must be a number and not empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                BaseAttackTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(SecondaryStatTextBox.Text))
            {
                MessageBox.Show("Secondary Stat must not be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                SecondaryStatTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PassiveTextBox.Text))
            {
                MessageBox.Show("Passive must not be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                PassiveTextBox.Focus();
                return;
            }

            TempWeapon.Name = NameTextBox.Text.Trim();
            TempWeapon.Image = url;
            TempWeapon.BaseAttack = baseAttack;
            TempWeapon.SecondaryStat = SecondaryStatTextBox.Text.Trim();
            TempWeapon.Passive = PassiveTextBox.Text.Trim();
            TempWeapon.Type = (Weapon.WeaponType)TypeComboBox.SelectedIndex;
            TempWeapon.Rarity = RarityComboBox.SelectedIndex + 1;

            DialogResult = true;
            Close();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GenerateClicked(object sender, RoutedEventArgs e)
        {
            BaseAttackTextBox.Text = random.Next(20, 51).ToString();

            TypeComboBox.SelectedIndex = random.Next(TypeComboBox.Items.Count);

            RarityComboBox.SelectedIndex = random.Next(5);

            NameTextBox.Text = "";
            ImageTextBox.Text = "";
            SecondaryStatTextBox.Text = "";
            PassiveTextBox.Text = "";
        }
    }
}