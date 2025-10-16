using MSSS_App_Dictionary.Data;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input; // Add this line if it's missing // 如果这行不存在，请添加

namespace MSSS_App_Dictionary.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataManager.LoadDataFromFile();
            DisplayAllStaff();
        }

        private void DisplayAllStaff()
        {
            lstAllStaff.Items.Clear();
            // The filtered results list cleared.
            lstFilteredStaff.Items.Clear();

            if (DataManager.MasterFile == null) return;

            // clear the details section on refresh.
            txtSelectedId.Text = "";
            txtSelectedName.Text = "";


            foreach (var staffMember in DataManager.MasterFile)
            {
                lstAllStaff.Items.Add($"{staffMember.Key} - {staffMember.Value}");
            }

            lblStatus.Text = $"Loaded {DataManager.MasterFile.Count} staff records.";
        }

        private void txtNameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNameFilter.IsFocused)
            {
                txtIdFilter.Text = string.Empty;
            }

            string filterText = txtNameFilter.Text.Trim().ToLower();
            lstFilteredStaff.Items.Clear();

            if (string.IsNullOrEmpty(filterText)) return;

            var filteredList = DataManager.MasterFile
                                .Where(staff => staff.Value.ToLower().Contains(filterText))
                                .ToList();

            foreach (var staffMember in filteredList)
            {
                lstFilteredStaff.Items.Add($"{staffMember.Key} - {staffMember.Value}");
            }
        }

        private void txtIdFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtIdFilter.IsFocused)
            {
                txtNameFilter.Text = string.Empty;
            }

            string filterText = txtIdFilter.Text.Trim();
            lstFilteredStaff.Items.Clear();

            if (string.IsNullOrEmpty(filterText)) return;

            var filteredList = DataManager.MasterFile
                                .Where(staff => staff.Key.ToString().Contains(filterText))
                                .ToList();

            foreach (var staffMember in filteredList)
            {
                lstFilteredStaff.Items.Add($"{staffMember.Key} - {staffMember.Value}");
            }
        }

        private void lstFilteredStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFilteredStaff.SelectedItem == null)
            {
                txtSelectedId.Text = string.Empty;
                txtSelectedName.Text = string.Empty;
                return;
            }

            string selectedItem = lstFilteredStaff.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { " - " }, StringSplitOptions.None);

            if (parts.Length == 2)
            {
                txtSelectedId.Text = parts[0];
                txtSelectedName.Text = parts[1];
            }
        }

        // when Alt + A is pressed, open the AdminWindow
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // debug: 
            // MessageBox.Show($"PreviewKeyDown Fired! Key: {e.Key}, Modifiers: {Keyboard.Modifiers}");

            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.A)
            {
                OpenAdminWindow();
            }
        }


        private void OpenAdminWindow()
        {
            int selectedId = 0;
            string selectedName = "";

            // Checks if the user has a selected staff member in the details section, if so, open in "Update Mode".
            if (!string.IsNullOrEmpty(txtSelectedId.Text) && int.TryParse(txtSelectedId.Text, out selectedId))
            {
                selectedName = txtSelectedName.Text;
            }

            // Creates an instance of AdminWindow and passes in the data.
            AdminWindow adminWindow = new AdminWindow(selectedId, selectedName);
            adminWindow.ShowDialog();

            // refresh after AdminWindow closed
            DisplayAllStaff();
        }


    }
}

