using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;

namespace FileWatcherService
{
    public interface IParser // интерфейс содержит метод Parse(), который содержит свою реализацию в двух классах - JsonParser и XmlParser
    {
        List<Options> Parse();
    }



    internal class XmlParser : IParser// используем интерфейс IParser, в котором определен метод, который необходимо реализовать.
    {
        public virtual List<Options> Parse()
        {
            string Xmlpath = @"G:\Ira\c#\FileWatcherService\FileWatcherService\config.xml";
            List<Options> fullXMLOptions = new List<Options>(); // cписок для хранения опций.
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Options));// создаем сериализатор для типа Options
            Options XmlOptions = new Options();

            using (var xmlRead = new FileStream(Xmlpath, FileMode.OpenOrCreate))
            {
                XmlOptions = (Options)xmlSerializer.Deserialize(xmlRead);
            }

            if (XmlOptions != null) // если содержимое есть - добавляем в список опций.
                fullXMLOptions.Add(XmlOptions);
            else
                throw new NullReferenceException();


            return fullXMLOptions;
        }

    }


    internal class JsonParser : IParser
    {
        private List<Options> optionsJson = new List<Options>(); // опции, предоставляемые  в json файле
        private string jsonString; // преобразуем содержимое json файла в строку        
        public virtual List<Options> Parse()
        {
            using (var jsonStream = new StreamReader(@"G:\Ira\c#\FileWatcherService\FileWatcherService\appsettings.json"))
            {
                jsonString = jsonStream.ReadToEnd();
            }
            Options optionJson = JsonSerializer.Deserialize<Options>(jsonString);

            if (optionJson != null) optionsJson.Add(optionJson);
            else throw new NullReferenceException();

            return optionsJson;
        }
    }
}
