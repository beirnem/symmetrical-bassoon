using System;
using System.IO;
using System.Text;

namespace ParseEdifact
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Need to receive the message
            // Read through the message to locate the 'LOC' statements
            // Parse the 'LOC' statements
            // Output the parsed data into arrays
            string path = "D:\\Users\\marcus\\Projects\\Code\\Study\\ParseEdifact\\ParseEdifact\\ParseEdifact\\Data\\sample.edi";
            var ediFileString = ParseEdiFileToString(path);
            
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
    }
}
