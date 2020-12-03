using System;
using System.IO;

namespace FileWatcherService
{
    class Copier
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string FileName { get; set; }
        public FileInfo Info { get; set; }

        public Copier(string source, string target, string fileName)
        {
            Source = source;
            Target = target;
            FileName = fileName;
            Info = new FileInfo(source + fileName);
        }
        public void AddFile()
        {
            try
            {
                string cryptPath = Cryptograph.CryptFile(Source + FileName);
                string zipPath = Compressor.Compress(cryptPath);
                File.Delete(cryptPath);
                string newFileName = $"Sales_{Info.CreationTime.Year}_{Info.CreationTime.Month}_{Info.CreationTime.Day}" +
                    $"_{Info.CreationTime.Hour}_{Info.CreationTime.Minute}_{Info.CreationTime.Second}";
                string filialPath = CreateFolders();
                File.Copy(zipPath, Target + filialPath + newFileName + ".gz");
                string achivePath = Target + "Archive\\" + filialPath + newFileName;   // нет расширения файла
                File.Move(zipPath, achivePath + ".gz");
                Compressor.Decompress(achivePath + ".gz");
                Cryptograph.DecryptFile(achivePath + ".crypt");
                File.Delete(achivePath + ".gz");
                File.Delete(achivePath + ".crypt");
            }
            catch (Exception ex)
            {
                File.WriteAllText(Source+"\\Error.txt", ex.Message);
                File.WriteAllText(Source+"\\EStackTrace.txt", ex.StackTrace);
            }
        }
        public string CreateFolders()
        {
            string year = Info.CreationTime.Year.ToString();
            string month = Info.CreationTime.Month.ToString();
            string day = Info.CreationTime.Day.ToString();
            string path = $"{year}\\{month}\\{day}\\";
            Directory.CreateDirectory(Target + path);
            Directory.CreateDirectory(Target + "Archive\\" + path);
            return path;
        }
    }
}
