using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace FileHelper
{
    class Program
    {
        public static string PathToTransferredObject { get; private set; }
        public static string CurrentDirectory { get; private set; }

        static void Main(string[] args)
        {
            CreateShortCut();
            if (args.Length > 0)
            {
                PathToTransferredObject = args[0];
                CurrentDirectory = args[0].Substring(0, args[0].LastIndexOf('\\'));

                IMenu mainMenu = new MainMenu();
                mainMenu.Show();
            }
        }

        private static void CreateShortCut()
        {
            string pathToSendTo = Environment.GetFolderPath(Environment.SpecialFolder.SendTo);
            IWshShortcut link = (IWshShortcut)new WshShellClass().CreateShortcut(Path.Combine(pathToSendTo, "FileHelper.lnk"));
            link.WindowStyle = 1;
            link.TargetPath = Path.Combine(Environment.CurrentDirectory, "FileHelper.exe");
            link.WorkingDirectory = pathToSendTo;
            link.Save();
        }
    }
}
