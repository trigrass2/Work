using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDesk
{
    public interface IFileWorker
    {
        /// <summary>
        /// проверка существования файла
        /// </summary>
        /// <param name="filename">проверяемый файл</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string filename);

        /// <summary>
        /// сохранение текста в файл
        /// </summary>
        /// <param name="filename">файл назначения</param>
        /// <param name="text">сохраняемый текст</param>
        /// <returns></returns>
        Task SaveTextAsync(string filename, string text);

        /// <summary>
        /// загрузка текста из файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<string> LoadTextAsync(string filename);

        /// <summary>
        /// получение файлов из определнного каталога
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetFilesAsync();

        /// <summary>
        /// удаление файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task DeleteAsync(string filename);
    }
}
