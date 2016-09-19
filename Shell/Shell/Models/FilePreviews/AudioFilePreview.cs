using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models.FilePreviews
{
    public class AudioFilePreview : FilePreview
    {
        public override void Preview()
        {

        }

        public AudioFilePreview()
        {
            IconPath = "../Icons/audio_icon.png";
        }

    }
}
