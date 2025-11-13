using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Diagnostics;

namespace MSSS_App_Dictionary.Data
{
    public static class DataManager
    {
        // Q4.1 Use a Dictionary<TKey, TValue> data structure.
        public static Dictionary<int, string> MasterFile { get; private set; } = new Dictionary<int, string>();
        private static readonly string _fileName = "MalinStaffNamesV3.csv";

        // Q4.2 Read the data from the .csv file
        public static void LoadDataFromFile()
        {
            // question 8 stopwatch to measure load time
            var sw = new Stopwatch();
            sw.Start();

            MasterFile = new Dictionary<int, string>();
            string filePath = Path.Combine("Data", _fileName);

            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"Error: Data file not found at path:\n{filePath}", "File Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // switch to streamreader after Question 8 IO Optimisation
                using (var reader = new StreamReader(filePath))
                {
                    string headerLine = reader.ReadLine(); // jump header
                    string line;
                    while ((line = reader.ReadLine()) != null) // line by line
                    {
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
            }
            catch (Exception ex) // Q4.10 Error trapping
            {
                MessageBox.Show($"An unknown error occurred while loading data:\n{ex.Message}", "Error Loading Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // stop the stopwatch and log the time taken
            sw.Stop();
            Trace.WriteLine($"--- TEST (After): LoadDataFromFile took {sw.ElapsedMilliseconds} ms ---");
        }

        public static void SaveDataToFile()
        {
            // question 8 stopwatch to measure load time
            var sw = new Stopwatch();
            sw.Start();

            string filePath = Path.Combine("Data", _fileName);
            try
            {
                // sort the keys first.
                var sortedKeys = MasterFile.Keys.ToList();
                sortedKeys.Sort();

                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("staff_id,staff_name"); // header
                    foreach (var key in sortedKeys)
                    {
                        writer.WriteLine($"{key},{MasterFile[key]}"); // save line by line
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save data to file:\n{ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // stop the stopwatch and log the time taken
            sw.Stop();
            Trace.WriteLine($"--- TEST (After): SaveDataToFile took {sw.ElapsedMilliseconds} ms ---");
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
            // Keep generating until found a unique number starting with 77*******.
            do
            {
                potentialId = rand.Next(770000000, 779999999);
            }
            while (MasterFile.ContainsKey(potentialId));

            return potentialId;
        }
    }
}

