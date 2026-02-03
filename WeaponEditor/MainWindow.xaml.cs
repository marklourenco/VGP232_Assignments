using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeaponLib;

namespace WeaponEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WeaponCollection mWeaponCollection;

        public MainWindow()
        {
            InitializeComponent();

            mWeaponCollection = new WeaponCollection();
            WeaponListBox.ItemsSource = mWeaponCollection;

            // Populate WeaponType combo box
            FilterTypeComboBox.ItemsSource =
                Enum.GetNames(typeof(Weapon.WeaponType));
        }

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Supported|*.csv;*.json;*.xml";

            if (dlg.ShowDialog() == true)
            {
                mWeaponCollection.Load(dlg.FileName);
                WeaponListBox.Items.Refresh();
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CSV|*.csv|JSON|*.json|XML|*.xml";

            if (dlg.ShowDialog() == true)
            {
                mWeaponCollection.Save(dlg.FileName);
            }
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            EditWeaponWindow win = new EditWeaponWindow();
            win.Setup(null); // ADD MODE

            if (win.ShowDialog() == true)
            {
                mWeaponCollection.Add(win.TempWeapon);
                WeaponListBox.Items.Refresh();
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            Weapon selected = WeaponListBox.SelectedItem as Weapon;
            if (selected == null) return;

            EditWeaponWindow win = new EditWeaponWindow();
            win.Setup(selected);

            if (win.ShowDialog() == true)
            {
                WeaponListBox.Items.Refresh();
            }
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            Weapon selected = WeaponListBox.SelectedItem as Weapon;
            if (selected == null) return;

            mWeaponCollection.Remove(selected);
            WeaponListBox.Items.Refresh();
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            mWeaponCollection.SortBy(rb.Content.ToString());
            WeaponListBox.Items.Refresh();
        }

        private void FilterTypeOnlySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterTypeComboBox.SelectedItem == null) return;

            Weapon.WeaponType type =
                (Weapon.WeaponType)Enum.Parse(
                    typeof(Weapon.WeaponType),
                    FilterTypeComboBox.SelectedItem.ToString());

            WeaponListBox.ItemsSource =
                mWeaponCollection.GetAllWeaponsOfType(type);
        }

        private void FilterNameTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = FilterNameTextBox.Text;

            WeaponListBox.ItemsSource =
                mWeaponCollection
                    .Where(w => w.Name.StartsWith(text,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
        }
    }
}