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
using System.ComponentModel;

namespace Assignment_02c
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WeaponCollection mWeaponCollection;
        private ICollectionView _weaponView;

        public MainWindow()
        {
            InitializeComponent();

            mWeaponCollection = new WeaponCollection();
            WeaponListBox.ItemsSource = mWeaponCollection;

            _weaponView = CollectionViewSource.GetDefaultView(WeaponListBox.ItemsSource);

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
            EditWeaponWindow win = new EditWeaponWindow(null);
            if (win.ShowDialog() == true)
            {
                mWeaponCollection.Add(win.TempWeapon);
                _weaponView.Refresh();
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            Weapon selected = WeaponListBox.SelectedItem as Weapon;
            if (selected == null) return;

            EditWeaponWindow win = new EditWeaponWindow(selected);
            if (win.ShowDialog() == true)
            {
                _weaponView.Refresh();
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

            Weapon.WeaponType type = (Weapon.WeaponType)Enum.Parse(
                typeof(Weapon.WeaponType),
                FilterTypeComboBox.SelectedItem.ToString());

            _weaponView.Filter = w => ((Weapon)w).Type == type;
            _weaponView.Refresh();
        }

        private void FilterNameTextChanged(object sender, TextChangedEventArgs e)
        {
            string text = FilterNameTextBox.Text;

            _weaponView.Filter = w =>
            {
                Weapon weapon = (Weapon)w;
                return weapon.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase);
            };
            _weaponView.Refresh();
        }

    }

}