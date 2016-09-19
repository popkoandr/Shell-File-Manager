using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Shell.Models
{
    public class Bookmark : IPrototype<Bookmark> 
    {
        public IShellItem PathFolder 
        {
            get; 
            set; 
        }

        public string Name 
        {
            get
            {
                return PathFolder.GetNameWithLocation();
            }
        }


        public Bookmark(IShellItem shellItem)
        {
            PathFolder = shellItem;
        }

        public Bookmark Clone()
        {
            return (Bookmark)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
