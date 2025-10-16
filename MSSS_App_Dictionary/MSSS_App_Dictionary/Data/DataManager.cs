using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MSSS_App_Dictionary.Data
{
    public static class DataManager
    {
        public static Dictionary<int, string> MasterFile { get; private set; } = new Dictionary<int, string>();
        private static readonly string _fileName = "MalinStaffNamesV3.csv";

        private static string GetFilePath()
        {
            string? exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (string.IsNullOrEmpty(exePath))
            {
                MessageBox.Show("Cannot get application directory.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new DirectoryNotFoundException("Application execution path is null.");
            }
            return Path.Combine(exePath, "Data", _fileName);
        }

        public static void LoadDataFromFile()
        {
            MasterFile = new Dictionary<int, string>();
            string filePath = GetFilePath();

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
            catch (Exception ex)
            {
                MessageBox.Show($"An unknown error occurred while loading data:\n{ex.Message}", "Error Loading Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void SaveDataToFile()
        {
            string filePath = GetFilePath();
            try
            {
                var lines = new List<string> { "staff_id,staff_name" };

                // 为了确保保存顺序一致，可以先对键进行排序
                // To ensure a consistent save order, we can sort the keys first.
                var sortedKeys = MasterFile.Keys.ToList();
                sortedKeys.Sort();

                foreach (var key in sortedKeys)
                {
                    lines.Add($"{key},{MasterFile[key]}");
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
            do
            {
                potentialId = rand.Next(770000000, 779999999);
            }
            while (MasterFile.ContainsKey(potentialId));

            return potentialId;
        }
    }
}

