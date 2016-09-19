using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Shell.Models.FilePreviews
{
    public class ImageFilePreview : FilePreview
    {
        private int _width;
        private int _height;

        public override void Preview()
        {
            var bitmapFrame = BitmapFrame.Create(new Uri(_file.GetNameWithLocation()));
            _width = bitmapFrame.PixelWidth;
            _height = bitmapFrame.PixelHeight;
            _file.Info1 = "Resolution: "+_width.ToString()+"x"+_height.ToString();
            _file.Info2 = "Creation date: " + System.IO.File.GetCreationTime(_file.GetNameWithLocation()).ToString();
        }

        public ImageFilePreview()
        {
            IconPath = "../Icons/image_icon.png";
        }
    }
}
