using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ParseEdifact
{
    class Program
    {
        private static string ediFile;
        private static Dictionary<string, string> specialChars;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Need to receive the message
            // Read through the message to locate the 'LOC' statements
            // Parse the 'LOC' statements
            // Output the parsed data into arrays
            string path = "D:\\Users\\marcus\\Projects\\Code\\Study\\ParseEdifact\\ParseEdifact\\ParseEdifact\\Data\\sample.edi";
            ediFile = ParseEdiFileToString(path);
            InitialiseEDISpecialCharacters();


        }

        /// <summary>
        /// Read the input .edi file and output a string
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string ParseEdiFileToString(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File Not Found at the location: {path}");
            }

            StringBuilder stringBuilder = new StringBuilder();

            using (FileStream fileStream = File.OpenRead(path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fileStream.Read(b, 0, b.Length) > 0)
                {
                    stringBuilder.Append(temp.GetString(b));
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Checks if the EDI string contains the UNA declaration and parses out the special characters to a Dictionary
        /// </summary>
        private static void InitialiseEDISpecialCharacters()
        {
            specialChars = new Dictionary<string, string>();

            if (ediFile.Contains("UNA"))
            {
                var unaIndex = ediFile.IndexOf("UNA");
                var chars = ediFile.Substring(unaIndex + 3, 6);
                var i = 0;
                specialChars.Add("Component Separator", chars.Substring(i, 1));
                i++;
                specialChars.Add("Data Separator", chars.Substring(i, 1));
                i++;
                specialChars.Add("Decimal Mark", chars.Substring(i, 1));
                i++;
                specialChars.Add("Release", chars.Substring(i, 1));
                i = i + 2;
                specialChars.Add("Segment Terminator", chars.Substring(i, 1));
            } else
            {
                specialChars.Add("Component Separator", ":");
                specialChars.Add("Data Separator", "+");
                specialChars.Add("Decimal Mark", ".");
                specialChars.Add("Release", "?");
                specialChars.Add("Segment Terminator", "'");
            }
        }

        /// <summary>
        /// Parse an EDI formatted string to output the elements of the 
        /// </summary>
        /// <returns></returns>
        private static List<string> ParseSegmentToArray()
        {
            var locStatements = new List<string>();

            return locStatements;

        }
    }
}
