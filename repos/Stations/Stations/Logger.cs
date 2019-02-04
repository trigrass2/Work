using System;
using System.IO;

namespace Stations
{
    public class Logger : IDisposable
    {
        StreamWriter file;
        
        private string FileName { get; set; }

        public Logger()
        {
            
        }

        ~Logger()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (file != null)
            {
                file.Close();
            }
        }

        private void CreateFile()
        {
            using (file = new StreamWriter(File.Open($"{Path.GetTempPath()}StationsLog.log", FileMode.Append, FileAccess.Write)))
            {
                file.WriteLine(FileName);
            }
        }

        /// <summary>
        /// Запись в файл событий
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            using (file = new StreamWriter(File.Open($"{Path.GetTempPath()}StationsLog.log", FileMode.Append, FileAccess.Write)))
            {
                file.WriteLine(text);
            }
        }
    }
}
