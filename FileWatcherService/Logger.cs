using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace FileWatcherService
{
    public class Logger
    {
        private ConfigManager configManager;
        private List<Options> Options = new List<Options>();
        private string targDirPath;
        private string scrDirPath;
        private bool enabled = true;

        private FileSystemWatcher scrWatcher;

        object obj = new object();

        public Logger()
        {
            configManager = new ConfigManager();
            Options = configManager.GetOptions();

            if (Options.Count >= 0)
            {
                scrDirPath = Options[0].Source;
                targDirPath = Options[0].Target;
                scrWatcher = new FileSystemWatcher(scrDirPath);
                scrWatcher.Deleted += Watcher_Deleted;
                scrWatcher.Created += Watcher_Created;
                scrWatcher.Changed += Watcher_Changed;
                scrWatcher.Renamed += Watcher_Renamed;
            }
        }

        public void Start()
        {
            scrWatcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }
        public void Stop()
        {
            scrWatcher.EnableRaisingEvents = false;
            enabled = false;
        }
        // переименование файлов
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string fileEvent = "переименован в " + e.FullPath;
            string filePath = e.OldFullPath;
            RecordEntry(fileEvent, filePath);
        }
        // изменение файлов
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "изменен";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        // создание файлов
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "создан";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }
        // удаление файлов
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string fileEvent = "удален";
            string filePath = e.FullPath;
            RecordEntry(fileEvent, filePath);
        }

        private void RecordEntry(string fileEvent, string filePath)
        {
            lock (obj)
            {
                using (StreamWriter writer = new StreamWriter("G:\\templog.txt", true))
                {
                    writer.WriteLine(String.Format("{0} файл {1} был {2}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), filePath, fileEvent));
                    writer.Flush();
                }
                if (fileEvent == "изменен" && filePath.EndsWith(".txt"))
                {
                    string name = filePath.Split((char)92)[filePath.Split((char)92).Length - 1];
                    Copier exp = new Copier(scrDirPath, targDirPath, name);
                    exp.AddFile();
                }
            }
        }
    }
}
