using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MSSS_App_SortedDictionary.Data
{
    public static class DataManager
    {
        // Q4.1 Use a SortedDictionary<TKey, TValue> data structure.
        public static SortedDictionary<int, string> MasterFile { get; private set; } = new SortedDictionary<int, string>();
        private static readonly string _fileName = "MalinStaffNamesV3.csv";

        // Q4.2 Read the data from the .csv file
        public static void LoadDataFromFile()
        {
            MasterFile = new SortedDictionary<int, string>();
            string filePath = Path.Combine("Data", _fileName);

            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Error: Data file not found at path:\n{filePath}", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string[] lines = File.ReadAllLines(filePath);
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[0].Trim(), out int id))
                    {
                        if (!MasterFile.ContainsKey(id))
                        {
                            MasterFile.Add(id, parts[1].Trim());
                        }
                    }
                }
            }
            catch (Exception ex) // Q4.10 Error trapping
            {
                MessageBox.Show($"An unknown error occurred while loading data:\n{ex.Message}", "Error Loading Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void SaveDataToFile()
        {
            string filePath = Path.Combine("Data", _fileName);
            try
            {
                var lines = new List<string> { "staff_id,staff_name" };

                // sort is not needed in SortedDictionary.
                //var sortedKeys = MasterFile.Keys.ToList();
                //sortedKeys.Sort();

                foreach (var staff in MasterFile)
                {
                    lines.Add($"{staff.Key},{staff.Value}");
                }

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save data to file:\n{ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static int AddStaff(string name)
        {
            int newId = GenerateNewUniqueId();
            MasterFile.Add(newId, name);
            return newId;
        }

        public static bool UpdateStaff(int id, string newName)
        {
            if (MasterFile.ContainsKey(id))
            {
                MasterFile[id] = newName;
                return true;
            }
            return false;
        }

        public static bool DeleteStaff(int id)
        {
            return MasterFile.Remove(id);
        }

        private static int GenerateNewUniqueId()
        {
            Random rand = new Random();
            int potentialId;
            // Keep generating until found a unique number.
            do
            {
                potentialId = rand.Next(770000000, 779999999);
            }
            while (MasterFile.ContainsKey(potentialId));

            return potentialId;
        }
    }
}

