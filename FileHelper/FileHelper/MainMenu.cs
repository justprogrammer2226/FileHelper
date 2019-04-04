using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHelper
{
    internal class MainMenu : IMenu
    {
        /// <summary> Заголовок меню. </summary>
        /// <remarks> Заголовок меню будет отображаться при показе меню. </remarks>
        public string Title { get; }

        /// <summary> Список опций меню. </summary>
        public List<Option> Options { get; }

        public MainMenu(string title = null)
        {
            Title = title;

            Options = new List<Option>()
            {
                new Option("Сделать все файлы и папки видимыми", () => { MakeAllFoldersVisible(Program.CurrentDirectory); MakeAllFilesVisible(Program.CurrentDirectory); }),
                new Option("Сделать все папки видимыми", () => MakeAllFoldersVisible(Program.CurrentDirectory)),
                new Option("Сделать все файлы видимыми", () => MakeAllFilesVisible(Program.CurrentDirectory)),
                new Option("Скрыть перенесённый объект", () => HideTransferredObject(Program.PathToTransferredObject))
            };
        }

        /// <summary> Показывает меню. </summary>
        public void Show()
        {
            Console.Clear();

            if (Title != null) Console.WriteLine(Title);

            for (int i = 0; i < Options.Count; i++)
                Console.WriteLine($"{i + 1}. {Options[i].Name}");

            if (int.TryParse(Console.ReadLine(), out int indexSelectedOption) && indexSelectedOption >= 1 && indexSelectedOption <= Options.Count)
            {
                Options[indexSelectedOption - 1].Action();
            }
            else
            {
                Console.WriteLine("Данной опции не существует.");
                Console.ReadKey();
            }
        }

        /// <summary> Делает все папки видимыми в папке directory. </summary>
        /// <param name="directory"> Папка в которой нужно сделать все папки видимыми. </param>
        private void MakeAllFoldersVisible(string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            DirectoryInfo[] directories = directoryInfo.GetDirectories();

            for (int i = 0; i < directories.Length; i++)
                directories[i].Attributes = FileAttributes.Normal; 
        }

        /// <summary> Делает все файлы видимыми в папке directory. </summary>
        /// <param name="directory"> Папка в которой нужно сделать все файлы видимыми. </param>
        private void MakeAllFilesVisible(string directory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] files = directoryInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
                files[i].Attributes = FileAttributes.Normal;
        }

        private void HideTransferredObject(string path)
        {
            if(File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Attributes = FileAttributes.Hidden;
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                directoryInfo.Attributes = FileAttributes.Hidden;
            }
        }
    }
}
