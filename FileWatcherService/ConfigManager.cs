using System.Collections.Generic;
using System.IO;

namespace FileWatcherService
{
    internal class ConfigManager
    {
        private JsonParser jsonObject;
        private XmlParser xmlObject;
        private List<Options> option;

        public ConfigManager()
        {
            jsonObject = new JsonParser();
            xmlObject = new XmlParser();
            option = new List<Options>();
        }

        internal List<Options> GetOptions()
        {
            string[] files = new string[50];
            files = Directory.GetFiles(@"G:\Ira\c#\FileWatcherService\FileWatcherService");
            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".json") option = jsonObject.Parse();
                else if (Path.GetExtension(file) == ".xml") option = xmlObject.Parse();
            }

            return option;
        }

    }
}
