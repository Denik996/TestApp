using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    public class DownloadManager
    {
        private Uri _gameUri;
        private Uri _updatesUri;
        private Uri _patchlistUri;
        private string _temporaryDirectory;


        public DownloadManager(string gameUri, string updatesUri, string patchlistUri, string temporaryDirectory)
        {
            _gameUri = new Uri(gameUri);
            _updatesUri = new Uri(updatesUri);
            _patchlistUri = new Uri(patchlistUri);
            _temporaryDirectory = temporaryDirectory;
        }

        public Dictionary<int, string> GetPatchList()
        {
            if (!Directory.Exists(_temporaryDirectory))
                return null;

            string path = _temporaryDirectory.TrimEnd('\\') + "\\patchlist.txt";

            IFileDownloader fileDownloader = new FileDownloader();
            fileDownloader.DownloadFileAsync(_patchlistUri, path);
            while (fileDownloader.BytesReceived < fileDownloader.TotalBytesToReceive || fileDownloader.BytesReceived == 0)
            {
                Thread.Sleep(100);
            }

            Dictionary<int, string> patchlist = new Dictionary<int, string>();

            using (StreamReader sr = new StreamReader(path))
            {
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null)
                        break;

                    line.Trim();
                    string[] temp = line.Split(' ');
                    patchlist.Add(Convert.ToInt32(temp[0]), temp[1]);
                }
            }

            File.Delete(path);
            return patchlist;
        }
    }
}
