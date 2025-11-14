using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MSSS_App_Dictionary.Data
{
    // Code Review: Single Responsibility Principle
    // Separate file IO responsibilities to another class. for example, fileio.OPenFIle("filename.csv"); return

    public static class DataManager
    {
        // Main data store
        public static Dictionary<int, string> MasterFile { get; private set; } = new Dictionary<int, string>();

        // Create repository instance
        private static readonly FileRepository _repository = new FileRepository();


        public static void LoadDataFromFile()
        {
            // question 8 stopwatch to measure load time
            //var sw = new Stopwatch();
            //sw.Start();

            MasterFile = _repository.LoadStaff();

            // stop the stopwatch and log the time taken
            //sw.Stop();
            //Trace.WriteLine($"--- TEST (After): LoadDataFromFile took {sw.ElapsedMilliseconds} ms ---");
        }

        public static void SaveDataToFile()
        {
            // question 8 stopwatch to measure load time
            //var sw = new Stopwatch();
            //sw.Start();

            _repository.SaveStaff(MasterFile);

            // stop the stopwatch and log the time taken
            //sw.Stop();
            //Trace.WriteLine($"--- TEST (After): SaveDataToFile took {sw.ElapsedMilliseconds} ms ---");
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

