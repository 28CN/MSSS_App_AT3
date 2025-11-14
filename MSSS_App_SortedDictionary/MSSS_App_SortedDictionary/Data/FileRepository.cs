using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MSSS_App_SortedDictionary.Data
{
    public class FileRepository
    {
        private const string FileName = "MalinStaffNamesV3.csv";
        private readonly string _filePath;

        // Initializes the repository and sets the file path.
        public FileRepository()
        {
            _filePath = Path.Combine(AppContext.BaseDirectory, "Data", FileName);
        }

        // Q4.2 Load the data from the .csv file into the SortedDictionary data structure
        public SortedDictionary<int, string> LoadStaff()
        {
            var data = new SortedDictionary<int, string>();

            try
            {
                if (!File.Exists(_filePath))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Error: Data file not found at path:\n{_filePath}", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                    return data;
                }

                using (var reader = new StreamReader(_filePath))
                {
                    string headerLine = reader.ReadLine();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] parts = line.Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[0].Trim(), out int id))
                        {
                            if (!data.ContainsKey(id))
                            {
                                data.Add(id, parts[1].Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An unknown error occurred while loading data:\n{ex.Message}", "Error Loading Data", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
            return data;
        }

        // Saves the staff data from a SortedDictionary to the CSV file.
        public void SaveStaff(SortedDictionary<int, string> staffData)
        {
            try
            {
                using (var writer = new StreamWriter(_filePath))
                {
                    writer.WriteLine("staff_id,staff_name");

                    foreach (var staffMember in staffData)
                    {
                        writer.WriteLine($"{staffMember.Key},{staffMember.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Failed to save data to file:\n{ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}
