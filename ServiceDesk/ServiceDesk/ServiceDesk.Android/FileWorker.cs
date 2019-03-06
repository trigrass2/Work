using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

[assembly: Dependency(typeof(ServiceDesk.Droid.FileWorker))]
namespace ServiceDesk.Droid
{
    public class FileWorker : IFileWorker
    {
        /// <summary>
        /// Удаляет файл
        /// </summary>
        /// <param name="filename">удаляемый файл</param>
        /// <returns></returns>
        public Task DeleteAsync(string filename)
        {            
            File.Delete(GetFilePath(filename));
            return Task.FromResult(true);
        }

        /// <summary>
        /// Проверяет файл на существование
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string filename)
        {            
            string filepath = GetFilePath(filename);
            
            bool exists = File.Exists(filepath);
            return Task<bool>.FromResult(exists);
        }

        /// <summary>
        /// Возвращает все файлы
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<string>> GetFilesAsync()
        {
            
            IEnumerable<string> filenames = from filepath in Directory.EnumerateFiles(GetDocsPath())
                                            select Path.GetFileName(filepath);
            return Task<IEnumerable<string>>.FromResult(filenames);
        }

        /// <summary>
        /// Загружает текст из файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<string> LoadTextAsync(string filename)
        {
            string filepath = GetFilePath(filename);
            using (StreamReader reader = File.OpenText(filepath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Записывает текст в файл
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task SaveTextAsync(string filename, string text)
        {
            string filepath = GetFilePath(filename);
            using (StreamWriter writer = File.CreateText(filepath))
            {
                await writer.WriteAsync(text);
            }
        }
        /// <summary>
        /// вспомогательный метод для построения пути к файлу
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetFilePath(string filename)
        {
            return Path.Combine(GetDocsPath(), filename);
        }
        /// <summary>
        /// получаем путь к папке MyDocuments
        /// </summary>
        /// <returns></returns>
        string GetDocsPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}