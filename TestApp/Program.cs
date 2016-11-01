using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //IFileDownloader fileDownloader = new FileDownloader();
            //fileDownloader.DownloadProgressChanged += OnDownloadProgressChanged;
            //fileDownloader.DownloadFileCompleted += DownloadFileCompleted;
            //fileDownloader.DownloadFileAsync(new Uri("http://kubix-service.info/download"), "D:\\TestRar.rar");
            //Console.ReadLine();

            DownloadManager dm = new DownloadManager("http://kubix-service.info/patchlist", "http://kubix-service.info/patchlist", "http://kubix-service.info/patchlist", "D:\\");
            dm.GetPatchList();
        }


        static void DownloadFileCompleted(object sender, DownloadFileCompletedArgs eventArgs)
        {
            if (eventArgs.State == CompletedState.Succeeded)
            {
                //download completed
            }
            else if (eventArgs.State == CompletedState.Failed)
            {
                //download failed
            }
        }

        static void OnDownloadProgressChanged(object sender, DownloadFileProgressChangedArgs args)
        {
            Console.Clear();
            Console.WriteLine(args.ProgressPercentage.ToString() + "%");
            //Console.WriteLine("Download {0} bytes from {1} bytes", args.BytesReceived, args.TotalBytesToReceive);
        }
    }
}
