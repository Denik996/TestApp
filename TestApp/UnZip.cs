using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SharpCompress.Reader;
using SharpCompress.Common;

namespace TestApp
{
    class UnZip
    {
        string archiveName; 
        string archivePath; //from
        string resultPath;  //where

        public UnZip(string archiveName, string archivePath, string resultPath)
        {
            this.archiveName = archiveName;
            this.archivePath = archivePath;
            this.resultPath = resultPath;
        }

        public bool extract()
        {
            try
            {
                using (Stream stream = File.OpenRead(archivePath + archiveName))
                {
                    var reader = ReaderFactory.Open(stream);
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            Console.WriteLine(reader.Entry.FilePath);
                            reader.WriteEntryToDirectory(resultPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                        }
                    }
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
