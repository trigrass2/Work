using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfSeeker
{
    public class Logger : IDisposable
    {
        StreamWriter file;
        static object locker = new object();

        private string FileName { get; set; }

        public Logger()
        {
            //CreateFile();
        }

        ~Logger()
        {
            Dispose();
        }

        public void Dispose()
        {

            file?.Close();
            
        }

        private void CreateFile()
        {
            //FileName = $"{Path.GetTempPath()}UsedSchalerParserLog.log";
            //lock (locker)
            //{
            //    file = File.CreateText(FileName);
            //}
            using (file = new StreamWriter(File.Open($"{Path.GetTempPath()}Seeker.log", FileMode.Append, FileAccess.Write)))
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
            //file.WriteLine(text);
            using (file = new StreamWriter(File.Open($"{Path.GetTempPath()}Seeker.log", FileMode.Append, FileAccess.Write)))
            {

                file.WriteLine(text);


            }
        }
    }
}
