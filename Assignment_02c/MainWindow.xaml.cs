using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WeaponLib;

namespace Assignment_02c
{
    public partial class MainWindow : Window
    {
        private WeaponCollection mWeaponCollection;
        private ICollectionView _weaponView;

        public MainWindow()
        {
            InitializeComponent();

            mWeaponCollection = new WeaponCollection();
            WeaponListBox.ItemsSource = mWeaponCollection;

            _weaponView = CollectionViewSource.GetDefaultView(mWeaponCollection);

            var types = new List<string> { "All" };
            types.AddRange(Enum.GetNames(typeof(Weapon.WeaponType)));

            FilterTypeComboBox.ItemsSource = types;
            FilterTypeComboBox.SelectedIndex = 0;
        }

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json|XML Files (*.xml)|*.xml";

            if (dlg.ShowDialog() != true)
                return;

            bool result = mWeaponCollection.Load(dlg.FileName);

            if (!result)
            {
                MessageBox.Show("Load failed. Check file format.");
                return;
            }

            _weaponView.Refresh();
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "CSV Files (*.csv)|*.csv|JSON Files (*.json)|*.json|XML Files (*.xml)|*.xml";

            if (dlg.ShowDialog() != true)
                return;

            if (!mWeaponCollection.Save(dlg.FileName))
            {
                MessageBox.Show("Save failed.");
            }
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            EditWeaponWindow win = new EditWeaponWindow();

            if (win.ShowDialog() == true)
            {
                mWeaponCollection.Add(win.TempWeapon);
            }
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            if (WeaponListBox.SelectedItem is not Weapon selected)
                return;

            EditWeaponWindow win = new EditWeaponWindow(selected);

            if (win.ShowDialog() == true)
            {
                _weaponView.Refresh();
            }
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            if (WeaponListBox.SelectedItem is not Weapon selected)
                return;

            mWeaponCollection.Remove(selected);
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            if (sender is not RadioButton rb)
                return;

            mWeaponCollection.SortBy(rb.Content.ToString());
        }

        private void FilterTypeOnlySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterNameTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            _weaponView.Filter = item =>
            {
                if (item is not Weapon weapon)
                    return false;

                // Type filter
                string selectedType = FilterTypeComboBox.SelectedItem?.ToString();
                bool typeMatch = selectedType == "All" ||
                                 weapon.Type.ToString() == selectedType;

                // Name filter
                string nameFilter = FilterNameTextBox.Text ?? "";
                bool nameMatch = weapon.Name.StartsWith(
                    nameFilter,
                    StringComparison.OrdinalIgnoreCase);

                return typeMatch && nameMatch;
            };

            _weaponView.Refresh();
        }
    }
}