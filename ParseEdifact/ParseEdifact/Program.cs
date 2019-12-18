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
            // I am assuming that the input EDIFACT data can be received as a file because I'm not familiar with how EDIFACT data are typically handled.
            // If the EDIFACT data is received in a different format then a different function could be implemented.
            string path = @"..\..\..\Data\sample.edi";
            ediFile = ParseEdiFileToString(path);
            InitialiseEDISpecialCharacters();

            var locArray = ParseSegmentToArray("LOC");
            Console.WriteLine("Complete");
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
                // This would be better handled using an error messaging service to advise the user of the invalid file loc.
                // I viewed implementing this messaging service as out of the scope of this test.
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
        /// Parse an EDI formatted string to output the elements of the specified segments as a list of arrays
        /// </summary>
        /// <returns></returns>
        private static string[,] ParseSegmentToArray(string segmentCode)
        {
            var segmentStartIndex = ediFile.IndexOf(segmentCode);
            var temp = ediFile.Substring(segmentStartIndex).Replace("\r\n","");
            // specialChars.GetValueOrDefault("Segment Terminator");
            
            // Split the EDI file into an array of segments using the provided code as the separator
            var segmentArray = temp.Split(segmentCode, StringSplitOptions.RemoveEmptyEntries);

            // Trim the last element in the array so that it does not contain unwanted segments
            var lastSegmentIndex = segmentArray.Length - 1;
            var terminatorIndex = segmentArray[lastSegmentIndex].IndexOf(specialChars.GetValueOrDefault("Segment Terminator"));
            segmentArray[lastSegmentIndex] = segmentArray[lastSegmentIndex].Remove(terminatorIndex);
            
            // Populate the final array with the elements of each segment
            var elementsArray = new string[segmentArray.Length, 2];
            for (var i = 0; i < segmentArray.Length; i++)
            {
                // Remove the terminator characters, we don't need them anymore, then split the segment on the data separator
                var tempElementArray = segmentArray[i].Replace(specialChars.GetValueOrDefault("Segment Terminator"), "")
                    .Split(specialChars.GetValueOrDefault("Data Separator"), StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < tempElementArray.Length; j++)
                {
                    elementsArray[i, j] = tempElementArray[j];
                }
            }

            return elementsArray;
        }
    }
}
