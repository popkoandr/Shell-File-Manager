using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shell.Models
{
    public class Folder : IShellItem
    {
        public override void Review() { }

        public Folder(string location, string name)
        {
            Location = location;
            Name = name;
            SetIcon();
        }

        public Folder(string fullPath)
        {
            string name = "";
            string location = "";
            string temp = "";
            for (int i = fullPath.ToCharArray().Length - 1; i > -1; i--)
            {
                if (fullPath[i] == '\\')
                {
                    name = temp;
                    temp = "";
                    i--;
                    location = fullPath.Substring(0, i + 1);
                    break;
                }
                temp = fullPath[i] + temp;
            }

            Name = name;
            Location = location;
            SetIcon();
        }

        public static void CreateNewFolder(string path, string name)
        {
            Directory.CreateDirectory(path + "\\" + name);
        }

        public override void Paste(IShellItem destinationFolder, bool overwrite)
        {
            Folder copiedFolder = new Folder(destinationFolder.GetNameWithLocation(), Name);
            Folder.CreateNewFolder(copiedFolder.Location, copiedFolder.Name);
            foreach (var shellItem in GetFilesInside())
                shellItem.Paste(copiedFolder, overwrite);
        }

        protected override string GetImagePath()
        {
            return "../Icons/folder-icon.png";
        }

        public override void DeleteItself()
        {
            foreach (var shellItem in GetFilesInside())
                shellItem.DeleteItself();
            Directory.Delete(GetNameWithLocation());
        }

        #region string methods

        public override string GetNameWithLocation()
        {
            if (Location.ToCharArray()[Location.ToCharArray().Length-1] == '\\')
                return Location + Name;
            else
                return Location + "\\"+Name;
        }

        public override string GetLocation()
        {
            return Location;
        }

        public override string GetSize() 
        { 
            return ""; 
        }

        public override string GetInfo1()
        {         
            return "";
        }

        public override string GetInfo2()
        {
            return "";
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
