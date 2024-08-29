using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurrowsWheeler
{
    public class SaveToBinaryFile
    {
        public static void SaveToBinary(string text, string filePath)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                File.WriteAllBytes(filePath, bytes);
                Console.WriteLine("Saved binary data to file: " + filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving binary file: " + ex.Message);
            }
        }
    }
}

