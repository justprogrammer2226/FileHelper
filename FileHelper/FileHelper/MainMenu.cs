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
                new Option("Сделать все файлы и папки видимыми", () => { SetDirectoriesAttribute(Program.CurrentDirectory, FileAttributes.Normal); SetFilesAttribute(Program.CurrentDirectory, FileAttributes.Normal); }),
                new Option("Сделать все файлы и папки видимыми (Также для вложенных файлов и папок)", () => { SetDirectoriesAttribute(Program.CurrentDirectory, FileAttributes.Normal, true); SetFilesAttribute(Program.CurrentDirectory, FileAttributes.Normal); }),
                new Option("Сделать все папки видимыми", () => SetDirectoriesAttribute(Program.CurrentDirectory, FileAttributes.Normal)),
                new Option("Сделать все файлы видимыми", () => SetFilesAttribute(Program.CurrentDirectory, FileAttributes.Normal)),
                new Option("Скрыть перенесённый объект", () => SetAttributeForTransferredObject(Program.PathToTransferredObject, FileAttributes.Hidden))
            };
        }

        /// <summary> Показывает меню. </summary>
        public void Show()
        {
            while(true)
            {
                Console.Clear();

                if (Title != null) Console.WriteLine(Title);

                for (int i = 0; i < Options.Count; i++)
                    Console.WriteLine($"{i + 1}. {Options[i].Name}");

                if (int.TryParse(Console.ReadLine(), out int indexSelectedOption) && indexSelectedOption >= 1 && indexSelectedOption <= Options.Count)
                {
                    Options[indexSelectedOption - 1].Action();
                    break;
                }
                else
                {
                    Console.WriteLine("Данной опции не существует.");
                    Console.ReadKey();
                }
            }
        }

        /// <summary> Применяет атрибуты к директориям в директории path. </summary>
        /// <param name="path"> Директория, в которой к другим директориям нужно применить атрибуты attributess. </param>
        /// <param name="attributes"> Атрибуты, которые нужно применить к папкам в папке по пути path. </param>
        /// <param name="forNested"> Применить ли атрибут attributes к вложеным директориям и файлам. </param>
        private void SetDirectoriesAttribute(string path, FileAttributes attributes, bool forNested = false)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            DirectoryInfo[] directories = directoryInfo.GetDirectories();

            for (int i = 0; i < directories.Length; i++)
            {
                directories[i].Attributes = attributes;
                if (forNested)
                {
                    SetFilesAttribute(directories[i].FullName, attributes);
                    SetDirectoriesAttribute(directories[i].FullName, attributes, true);
                }
            }
        }

        /// <summary> Применяет атрибуты к файлам в директории path. </summary>
        /// <param name="path"> Директория, в которой к файлам нужно применить атрибуты attributes. </param>
        /// <param name="attributes"> Атрибуты, которые нужно применить к файлам в папке по пути path. </param>
        private void SetFilesAttribute(string path, FileAttributes attributes)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();

            for (int i = 0; i < files.Length; i++)
                files[i].Attributes = attributes;
        }

        /// <summary> Применяет атрибуты к переданому объекту по пути path. </summary>
        /// <param name="path"> Путь к объекту, к которому нужно применить атрибуты attributes. </param>
        /// <param name="attributes"> Атрибуты, которые нужно применить к объекту по пути path. </param>
        private void SetAttributeForTransferredObject(string path, FileAttributes attributes)
        {
            if(File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                fileInfo.Attributes = attributes;
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                directoryInfo.Attributes = attributes;
            }
        }
    }
}
