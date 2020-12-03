using System.IO;
using System.IO.Compression;

namespace FileWatcherService
{
    class Compressor
    {
        public static string Compress(string FileSource)
        {
            using (FileStream sourceStream = new FileStream(FileSource, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(FileSource.Remove(FileSource.LastIndexOf(".crypt")) + ".gz"))
                {
                    // поток архивации.
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой.
                    }
                }
                return FileSource.Remove(FileSource.LastIndexOf(".crypt")) + ".gz";
            }
        }
        public static void Decompress(string ComprFilePath)
        {
            FileInfo fileInfo = new FileInfo(ComprFilePath);
            using (FileStream sourceStream = new FileStream(ComprFilePath, FileMode.Open))
            {
                string DecomprFilePath = ComprFilePath.Remove(ComprFilePath.LastIndexOf(".gz")) + ".crypt";
                using (FileStream targetStream = File.Create(DecomprFilePath))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
        }
    }
}
