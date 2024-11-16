using System;
using System.Collections.Generic;
using System.IO;

namespace ActivelyServer
{
    public class BazaDanych
    {
        string filePath = "bazadanych.txt";
        Dictionary<string, string> users = new Dictionary<string, string>();

        public BazaDanych()
        {
            users = LoadDictionaryFromFile(filePath);
        }

        public void AddUser(string login, string password)
        {
            if (!users.ContainsKey(login))
            {
                users.Add(login, password);
                Save();  
            }
        }

        public Dictionary<string, string> GetUsers()
        {
            return users;
        }

        private void Save()
        {
            SaveDictionaryToFile(users, filePath);
        }

        private void SaveDictionaryToFile(Dictionary<string, string> dictionary, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var kvp in dictionary)
                {
                    writer.WriteLine($"{kvp.Key}:{kvp.Value}");
                }
            }
        }

        public static Dictionary<string, string> LoadDictionaryFromFile(string filePath)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            if (File.Exists(filePath)) 
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            string key = parts[0];
                            string value = parts[1];
                            dictionary[key] = value;
                        }
                    }
                }
            }

            return dictionary;
        }
    }
}