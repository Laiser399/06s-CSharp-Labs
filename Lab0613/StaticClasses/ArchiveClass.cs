using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab0613
{
    static class ArchiveClass
    {
        public static bool Compress(string sourceFilename, string compressFilename)
        {
            try
            {
                using (FileStream inputStream = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read))
                using (GZipStream compressStream = new GZipStream(new FileStream(compressFilename,
                    FileMode.OpenOrCreate), CompressionMode.Compress))
                {
                    inputStream.CopyTo(compressStream);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
