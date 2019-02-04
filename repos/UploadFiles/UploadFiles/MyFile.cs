using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadFiles
{
    public class MyFile
    {
        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }

        public MyFile(byte[] filebytes, string filename)
        {
            FileBytes = filebytes;
            FileName = filename;
        }
    }
}
