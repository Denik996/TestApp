using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TestApp
{
    public class Search
    {
        string _filename;
        long _timeout;
        List<Thread> _threads;
        List<string> _foundResult;
        System.Timers.Timer _timer;
        bool _isSearching = false;


        public Search(string filename)
        {
            _filename = filename;
        }

        public void StartSearch(long timeout = 0)
        {
            _timeout = timeout;

            if (timeout != 0)
            {
                _timer = new System.Timers.Timer();
                _timer.Interval = _timeout;
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(_timer_Elapsed);
                _timer.Start();
            }

            _isSearching = true;

            _threads = new List<Thread>();
            _foundResult = new List<string>();

            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                Thread findThread = new Thread(new ParameterizedThreadStart(FindOnDrive));
                findThread.Name = "Thread " + drive.Name.Substring(0, 1);
                _threads.Add(findThread);
            }

            for (int i = 0; i < _threads.Count; i++)
            {
                _threads[i].Start(drives[i].Name);
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _isSearching = false;
            _timer.Stop();
        }

        private void FindOnDrive(object driveName)
        {
            if (_isSearching)
            {
                string drive = (string)driveName;
                try
                {
                    foreach (string d in Directory.GetDirectories(drive))
                    {
                        foreach (string f in Directory.GetFiles(d, _filename))
                        {
                            _foundResult.Add(f);
                            Console.WriteLine(f);
                            StopSearch();
                        }
                        FindOnDrive(d);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                foreach (var thread in _threads)
                {
                    thread.Abort();
                    thread.Join();
                }
            }
        }

        private void StopSearch()
        {
            _isSearching = false;
            _timer.Stop();
        }
    }
}
