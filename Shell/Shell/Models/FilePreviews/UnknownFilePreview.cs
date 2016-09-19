using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models.FilePreviews
{
    public class UnknownFilePreview : FilePreview
    {

        public override void Preview()
        {
            _file.Info1 = "Creation date: " + System.IO.File.GetCreationTime(_file.GetNameWithLocation()).ToString();
            _file.Info2 = "Access date: " + System.IO.File.GetLastAccessTime(_file.GetNameWithLocation()).ToString();
        }

        public UnknownFilePreview()
        {
            IconPath = "../Icons/unknown-file-icon.png";
        }

    }
}
