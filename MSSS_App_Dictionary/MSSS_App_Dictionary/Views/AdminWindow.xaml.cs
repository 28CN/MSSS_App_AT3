using MSSS_App_Dictionary.Data;
using System;
using System.Windows;
using System.Windows.Input;

namespace MSSS_App_Dictionary.Views
{
    public partial class AdminWindow : Window
    {
        private readonly int _currentId;
        private readonly bool _isUpdateMode;

        public AdminWindow(int staffId, string staffName)
        {
            InitializeComponent();
            _currentId = staffId;
            _isUpdateMode = _currentId > 0;
            txtStaffId.Text = _currentId > 0 ? _currentId.ToString() : "(Will be auto-generated)";
            txtStaffName.Text = staffName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isUpdateMode)
            {
                lblInstructions.Text = "Edit the name below.\nPress [Ctrl+S] to Update, or [Ctrl+D] to Delete.\nPress [Alt+L] to Leave.";
                lblStatus.Text = $"Editing record for ID: {_currentId}";
            }
            else
            {
                lblInstructions.Text = "Enter a name for the new staff member.\nA unique ID will be generated automatically.\nPress [Ctrl+S] to Create.\nPress [Alt+L] to Leave.";
                lblStatus.Text = "Ready to create a new record.";
            }
            txtStaffName.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.L)
            {
                this.Close();
            }
            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (_isUpdateMode)
                    UpdateStaff();
                else
                    CreateStaff();
            }
            else if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control && _isUpdateMode)
            {
                DeleteStaff();
            }
        }

        private void CreateStaff()
        {
            string newName = txtStaffName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                lblStatus.Text = "Error: Staff name cannot be empty.";
                return;
            }

            int newId = DataManager.AddStaff(newName);
            lblStatus.Text = $"Successfully created new record. ID: {newId}";
            txtStaffName.Clear();
            txtStaffId.Text = "(Will be auto-generated)";
        }

        private void UpdateStaff()
        {
            string updatedName = txtStaffName.Text.Trim();
            if (string.IsNullOrEmpty(updatedName))
            {
                lblStatus.Text = "Error: Staff name cannot be empty.";
                return;
            }

            if (DataManager.UpdateStaff(_currentId, updatedName))
            {
                lblStatus.Text = $"Record {_currentId} updated successfully.";
            }
            else
            {
                lblStatus.Text = $"Error: Could not find record with ID {_currentId}.";
            }
        }

        private void DeleteStaff()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete the record for '{txtStaffName.Text}' (ID: {_currentId})?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (DataManager.DeleteStaff(_currentId))
                {
                    this.Close();
                }
                else
                {
                    lblStatus.Text = $"Error: Could not find record with ID {_currentId}.";
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DataManager.SaveDataToFile();
        }
    }
}

