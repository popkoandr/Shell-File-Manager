using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Shell.Models.FilePreviews
{
    public class VideoFilePreview : FilePreview
    {
        public override void Preview()
        {
        }

        public VideoFilePreview()
        {
            IconPath = "../Icons/video_icon.png";
        }
    }
}
