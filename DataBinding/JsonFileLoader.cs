using System;
using System.IO;
using System.Text;

namespace DataBinding
{
    public interface IJsonFileLoader
    {
        string GetJsonFrom(string filePath);
    }

    public class AppDomainJsonLoader : IJsonFileLoader
    {
        public string GetJsonFrom(string filePath)
        {
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = $"{Path.GetFullPath(Path.Combine(runningPath))}{filePath}";

            string text;
            using (var streamReader = new StreamReader(fileName, Encoding.UTF8))
                text = streamReader.ReadToEnd();

            return text;
        }
    }
}