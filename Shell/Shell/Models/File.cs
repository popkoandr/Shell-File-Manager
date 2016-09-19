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
using Shell.Models.FilePreviews;

namespace Shell.Models
{
    public class File : IShellItem
    {
        public string Info1 = "", Info2 = "";

        public string Extension { get; set; }
        public long Size { get; set; }
        public string SizeFormat { get { return GetSize().Substring(6); } }


        private FilePreview _filePreview 
        {
            get
            {
                FilePreview _filepreview = (_extensions.ContainsKey(Extension)) ? _extensions[Extension] : new UnknownFilePreview() { _file = this};
                _filepreview._file = this;
                return _filepreview;
            }
        }

        public File(string name, string location, string extension)
        {
            Name = name;
            Location = location;
            Extension = extension;
            Size = new FileInfo(GetNameWithLocation()).Length;
            SetIcon();
        }

        public File(string fullPath)
        {
            string name = "";
            string location = "";
            string extension = "";
            string temp = "";
            for (int i = fullPath.ToCharArray().Length - 1; i > -1; i--)
            {

                if (fullPath[i] == '.')
                {
                    extension = "." + temp;
                    temp = "";
                    i--;
                }
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

            Extension = extension;
            Name = name;
            Location = location;
            Size = new FileInfo(fullPath).Length;
            SetIcon();
        }

        public override ObservableCollection<IShellItem> GetFilesInside()
        {
            return null;
        }

        public override void GetFoundedSubitems(string subname, ObservableCollection<IShellItem> store)
        {
            
        }

        public override void Paste(IShellItem destinationFolder, bool overwrite)
        {
            System.IO.File.Copy(GetNameWithLocation(), destinationFolder.GetNameWithLocation() + "\\" + Name + Extension, overwrite);
        }

        protected override string GetImagePath()
        {
            return _filePreview.IconPath;
        }

        public static void CreateFile(string name, string location)
        {
            System.IO.File.Create(location + "\\" + name);
        }

        public override void Review()
        {
            if (_filePreview != null)_filePreview.Preview();
        }

        public override void DeleteItself()
        {
            System.IO.File.Delete(GetNameWithLocation());
        }

        public override void Open()
        {
            try
            {
                System.Diagnostics.Process.Start(@GetNameWithLocation());
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        #region string methods

        public override string GetNameWithLocation() 
        {
            if (Location.ToCharArray()[Location.ToCharArray().Length - 1] == '\\')
                return Location + Name + Extension;
            else
                return Location + "\\" + Name + Extension;
        }

        public string GetNameWithExtention()
        {
            return Name + Extension;
        }

        public override string GetLocation()
        {
            return Location;
        }

        public override string GetSize()
        {
            if (Size < 1000) return "Size: " + Size.ToString() + " B";
            else if (1000 <= Size && Size < 1000000) return "Size: " + (Size / 1000).ToString() + " KB";
            else if (1000000 <= Size && Size < 1000000000) return "Size: " + (Size / 1000000).ToString() + " MB";
            else return "Size: " + (Size / 1000000000).ToString() + " GB";
        }

        public override string GetInfo1()
        {
            Review();
            return Info1;
        }

        public override string GetInfo2()
        {
            Review();
            return Info2;
        }

        public override string ToString()
        {
            return Name+Extension;
        }

        #endregion

        private static readonly Dictionary<string, FilePreview> _extensions = new Dictionary<string, FilePreview>()
        {
            #region System extensions
            { ".sys", new SystemFilePreview() },
            { ".lib", new SystemFilePreview() },
            { ".log", new SystemFilePreview() },
            { ".BAK", new SystemFilePreview() },
            #endregion

            #region Executable extensions
            { ".exe", new ExecutableFilePreview() },
            { ".torrent", new ExecutableFilePreview() },
            { ".dll", new ExecutableFilePreview() },
            { ".BAT", new ExecutableFilePreview() },
            { ".bat", new ExecutableFilePreview() },
            { ".url", new ExecutableFilePreview() },
            { ".reg", new ExecutableFilePreview() },
            { ".msi", new ExecutableFilePreview() },
            { ".iso", new ExecutableFilePreview() },
            { ".pdf", new ExecutableFilePreview() },
            { ".7z", new ExecutableFilePreview() },
            { ".zip", new ExecutableFilePreview() },
            { ".rar", new ExecutableFilePreview() },
            { ".cab", new ExecutableFilePreview() },
            #endregion   
   
            #region Audio extensions
            { ".flac", new AudioFilePreview() },
            { ".mp3", new AudioFilePreview() },
            { ".aac", new AudioFilePreview() },
            { ".asf", new AudioFilePreview() },
            { ".orc", new AudioFilePreview() },
            { ".mp4a", new AudioFilePreview() },
            #endregion

            #region Image extensions
            { ".jpg", new ImageFilePreview() },
            { ".bmp", new ImageFilePreview() },
            { ".png", new ImageFilePreview() },
            { ".ico", new ImageFilePreview() },
            { ".JPG", new ImageFilePreview() },
            { ".PNG", new ImageFilePreview() },
            #endregion

            #region Text extensions
            { ".txt", new TextFilePreview() },
            { ".doc", new TextFilePreview() },
            { ".docx", new TextFilePreview() },
            { ".xml", new TextFilePreview() },
            { ".cvs", new TextFilePreview() },
            { ".text", new TextFilePreview() },
            { ".rtf", new TextFilePreview() },
            #endregion

            #region Video extensions

            { ".avi", new VideoFilePreview() },
            { ".mp4", new VideoFilePreview() },
            { ".mkv", new VideoFilePreview() },
            { ".flv", new VideoFilePreview() },
            { ".mpg", new VideoFilePreview() },
            { ".m4p", new VideoFilePreview() },
            { ".m4v", new VideoFilePreview() },
            { ".rm", new VideoFilePreview() },
            { ".rmvb", new VideoFilePreview() },
            { ".mpeg", new VideoFilePreview() },
            { ".wmv", new VideoFilePreview() },
            { ".3gp", new VideoFilePreview() },
            { ".mov", new VideoFilePreview() },
            { ".MP4", new VideoFilePreview() },
            { ".AVI", new VideoFilePreview() }
           
            #endregion

        };
    }
}
