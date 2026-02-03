using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeaponLib;

namespace Assignment_02c
{
    /// <summary>
    /// Interaction logic for EditWeaponWindow.xaml
    /// </summary>
    public partial class EditWeaponWindow : Window
    {
        public Weapon TempWeapon { get; private set; }

        public void Setup(Weapon weapon)
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
            }
        }

        private void SubmitClicked(object sender, RoutedEventArgs e)
        {
            TempWeapon.Name = NameTextBox.Text;
            TempWeapon.BaseAttack = int.Parse(BaseAttackTextBox.Text);

            DialogResult = true;
            Close();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void GenerateClicked(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            BaseAttackTextBox.Text = r.Next(20, 51).ToString();
            RarityComboBox.SelectedIndex = r.Next(1, 6);
            TypeComboBox.SelectedIndex = r.Next(TypeComboBox.Items.Count);
        }
    }
}
