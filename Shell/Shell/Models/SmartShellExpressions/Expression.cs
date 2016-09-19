using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models.SmartShellExpressions
{
    public abstract class Expression
    {
        protected ObservableCollection<IShellItem> _itemsToShow = new ObservableCollection<IShellItem>();

        public abstract void Interpret(InterpreterContext context);

        protected IShellItem GetItem(string fullPath)
        {
            int pathLength = fullPath.ToCharArray().Length;
            string temp = fullPath.Substring(pathLength-2);
            if (temp == ":\\")
            {
                foreach (var disk in Disk.AllDisks)
                {
                    if (disk.Name == fullPath)
                        return disk;
                }
                return null;
            }
            for (int i = pathLength - 1; i > -1; i--)
            {
                if (fullPath[i] == '.')
                    return new File(fullPath);
                else if (fullPath[i] == '\\')
                    return new Folder(fullPath);
            }
            return null;
        }

        public ObservableCollection<IShellItem> GetItemsToShow()
        {
            return _itemsToShow;
        }
    }
}
