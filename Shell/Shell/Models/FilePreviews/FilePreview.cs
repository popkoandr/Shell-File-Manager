using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Models.FilePreviews
{
    public abstract class FilePreview
    {
        public File _file;

        public abstract void Preview();

        public string IconPath;
    }
}
