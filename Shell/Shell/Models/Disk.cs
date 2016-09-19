using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models
{
    public class Disk : IShellItem
    {
        public static ObservableCollection<Disk> AllDisks = new ObservableCollection<Disk>();

        public static void GetLocalDisks()
        {
            var disks = Directory.GetLogicalDrives();
            if (disks.Length > AllDisks.Count)
            {
                var disk = new Disk(disks[AllDisks.Count]);
                AllDisks.Add(disk);
                GetLocalDisks();
            }
        }

        public override void Review() { }

        public Disk(string name)
        {
            Name = name;
        }

        protected override string GetImagePath()
        {
            throw new NotImplementedException();
        }

        public override void DeleteItself()
        { }

        #region string methods

        public override string ToString()
        {
            return Name;
        }

        public override string GetNameWithLocation()
        {
            return Name;
        }

        public override string GetLocation()
        {
            return "";
        }
        public override string GetSize()
        {
            return new FileInfo(GetNameWithLocation()).Length.ToString();
        }
        public override string GetInfo1()
        {
            return "";
        }
        public override string GetInfo2()
        {
            return "";
        }

        #endregion

    }
}
