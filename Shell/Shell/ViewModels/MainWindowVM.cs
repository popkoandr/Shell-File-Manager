using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Shell.ViewModels.VMBase;
using Shell.Models;
using System.Windows.Media.Imaging;
using Shell.Views;
using System.IO;

namespace Shell.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region Properties

        public ShellControlVM LeftShell { get; set; }
        public ShellControlVM RightShell { get; set; }

        public ShellControlVM SelectedShell
        {
            get
            {
                if (LeftShell.CurrentVM == true) return LeftShell;
                else return RightShell;
            }
        }

        public ShellControlVM UnselectedShell
        {
            get
            {
                if (LeftShell.CurrentVM == false) return LeftShell;
                else return RightShell;
            }
        }

        private IShellItem CopiedShellItem { get; set; }

        private bool IsCut { get; set; }

        #endregion

        #region Commands

        public ICommand CopyCommand
        { get { return Commands.GetOrCreateCommand(() => CopyCommand, Copy, IsFileSelected); } }

        public ICommand PasteCommand
        { get { return Commands.GetOrCreateCommand(() => PasteCommand, Paste, CanPasteFile); } }

        public ICommand CutCommand
        { get { return Commands.GetOrCreateCommand(() => CutCommand, Cut, IsFileSelected); } }

        public ICommand DeleteCommand
        { get { return Commands.GetOrCreateCommand(() => DeleteCommand, Delete, IsFileSelected); } }

        public ICommand CreateFolderCommand
        { get { return Commands.GetOrCreateCommand(() => CreateFolderCommand, CreateFolder, CanCreateFile); } }

        public ICommand CreateFileCommand
        { get { return Commands.GetOrCreateCommand(() => CreateFileCommand, CreateFile, CanCreateFile); } }

        public ICommand FindCommand
        { get { return Commands.GetOrCreateCommand(() => FindCommand, Find); } }
        #endregion

        #region Methods      

        private void Copy()
        {
            CopiedShellItem = SelectedShell.SelectedFile;
            IsCut = false;
        }

        private void Paste()
        {
            if(CopiedShellItem is Shell.Models.File)
            {
                Shell.Models.File copiedFile = CopiedShellItem as Shell.Models.File;
                foreach(var item in SelectedShell.Files)
                {
                    if (item is Shell.Models.File)
                    {
                        Shell.Models.File temp = item as Shell.Models.File;
                        if (temp.GetNameWithExtention() == copiedFile.GetNameWithExtention())
                        {
                            var result = MessageBox.Show("There is a file with such name. Do you want to overwrite it?", "Overwrite", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                CopiedShellItem.Paste(SelectedShell.InFolder, true);
                                if (IsCut) CopiedShellItem.DeleteItself();
                                Refresh();
                                return;
                            }
                        }
                    }
                }
            }
            else if (CopiedShellItem is Folder)
            {
                Folder copiedFile = CopiedShellItem as Folder;
                foreach (var item in SelectedShell.Files)
                {
                    if(item is Folder)
                        if (item.Name == copiedFile.Name)
                        {
                            var result = MessageBox.Show("There is a folder with such name. Do you want to overwrite it?", "Overwrite", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                CopiedShellItem.Paste(SelectedShell.InFolder, true);
                                if (IsCut) CopiedShellItem.DeleteItself();
                                Refresh();
                                return;
                            }
                        }
                }
            }
            CopiedShellItem.Paste(SelectedShell.InFolder, true);
            if (IsCut) CopiedShellItem.DeleteItself();
            Refresh();
        }

        private void Cut()
        {
            CopiedShellItem = SelectedShell.SelectedFile;
            IsCut = true;
        }

        private void Delete()
        {
            if (CopiedShellItem == SelectedShell.SelectedFile)
                CopiedShellItem = null;
            var result = MessageBox.Show("Are you sure you want delete it?", "Delete", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                SelectedShell.SelectedFile.DeleteItself();
                Refresh();
            }

        }

        private void CreateFolder()
        {
            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
            if (creatorWindow.Created)
            {
                string folderName = creatorWindow.itemName;
                Folder.CreateNewFolder(SelectedShell.Path, folderName);
                Refresh();
            }
        }

        private void CreateFile()
        {
            CreatorWindow creatorWindow = new CreatorWindow();
            creatorWindow.ShowDialog();
            if (creatorWindow.Created)
            {
                string fileName = creatorWindow.itemName;
                Shell.Models.File.CreateFile(fileName, SelectedShell.Path);
                Refresh();
            }
        }

        private void Find()
        {
            FindWindow findW = new FindWindow();
            findW.ShowDialog();
            if (findW.Founded)
            {
                string subStr = findW.itemName;
                ObservableCollection<IShellItem> foundedItems = new ObservableCollection<IShellItem>();
                IShellItem startDirectory = SelectedShell.InFolder;
                if (startDirectory != null)
                    startDirectory.GetFoundedSubitems(subStr, foundedItems);
                SelectedShell.InFolder = null;
                SelectedShell.Files = foundedItems;
            }

        }

        private bool IsFileSelected()
        {
            return SelectedShell.CurrentVM;
        }

        private bool CanPasteFile()
        {
            return (SelectedShell.InFolder!=null && CopiedShellItem!=null);
        }

        private bool CanCreateFile()
        {
            return SelectedShell.InFolder !=null;
        }

        private void Refresh()
        {
            if (SelectedShell.Files != null) SelectedShell.Refresh();
            if (UnselectedShell.Files != null) UnselectedShell.Refresh();
        }


        #endregion

    }
}
