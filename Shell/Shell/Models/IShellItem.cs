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
    public abstract class IShellItem
    {
        public string Name { get; set; }
        public string Location { get; set; }
        protected string _iconPath;
        public ImageSource IconIS { get; set; }
        public abstract void Review();
        public virtual ObservableCollection<IShellItem> GetFilesInside()
        {
                ObservableCollection<IShellItem> filesInside = new ObservableCollection<IShellItem>();

                var folders = Directory.GetDirectories(GetNameWithLocation());

                foreach (var folder in folders)
                    filesInside.Add(new Folder(Path.GetDirectoryName(folder), new DirectoryInfo(folder).Name));
                var files = Directory.GetFiles(GetNameWithLocation());

                foreach (var file in files)
                    filesInside.Add(new File(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file), Path.GetExtension(file)));
                return filesInside;
        }

        protected virtual void SetIcon()
        {
            BitmapImage Icon = new BitmapImage();
            Icon.BeginInit();
            Icon.UriSource = new Uri(@GetImagePath(),UriKind.Relative);
            Icon.EndInit();
            IconIS = Icon;
        }

        public virtual void GetFoundedSubitems(string subname, ObservableCollection<IShellItem> store)
        {
            foreach (var item in GetFilesInside())
                if (item.ToString().Contains(subname))
                    store.Add(item);
            foreach (var item in GetFilesInside())
                item.GetFoundedSubitems(subname, store);
        }

        protected abstract string GetImagePath();

        public virtual void Paste(IShellItem destinationFolder, bool overwrite) { }

        public virtual void Open() { }

        public abstract string GetNameWithLocation();
        public abstract string GetLocation();
        public abstract string GetSize();
        public abstract string GetInfo1();
        public abstract string GetInfo2();
        public abstract void DeleteItself();
    }
}
