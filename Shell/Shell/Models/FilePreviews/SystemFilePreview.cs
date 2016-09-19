using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models.FilePreviews
{
    public class SystemFilePreview : FilePreview
    {
        public override void Preview()
        {
        }

        public SystemFilePreview()
        {
            IconPath = "../Icons/sys_icon.png";
        }



    }
}
