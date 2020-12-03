using System.IO;

namespace FileWatcherService
{
    class Cryptograph
    {
        public static string CryptFile(string path)
        {
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] fileBytes = new byte[fstream.Length];
                fstream.Read(fileBytes, 0, fileBytes.Length);
                for (int i = 0; i < fileBytes.Length; i++)
                {
                    fileBytes[i] ^= 1;
                }
                string newPath = path.Remove(path.LastIndexOf(".txt")) + ".crypt";
                using (FileStream wrStream = new FileStream(newPath, FileMode.OpenOrCreate))
                {
                    wrStream.Write(fileBytes, 0, fileBytes.Length);
                }
                return newPath;
            }
        }
        public static void DecryptFile(string path)
        {
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] fileBytes = new byte[fstream.Length];
                fstream.Read(fileBytes, 0, fileBytes.Length);
                for (int i = 0; i < fileBytes.Length; i++)
                {
                    fileBytes[i] ^= 1;
                }
                string newPath = path.Remove(path.LastIndexOf(".crypt")) + ".txt";
                using (FileStream wrStream = new FileStream(newPath, FileMode.OpenOrCreate))
                {
                    wrStream.Write(fileBytes, 0, fileBytes.Length);
                }
            }
        }
    }
}
