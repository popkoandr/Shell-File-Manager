using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Shell.Models;
using Shell.Models.SmartShellExpressions;
using Shell.Properties;
using Shell.ViewModels.VMBase;
using Shell.Views;

namespace Shell.ViewModels
{
    public class ShellControlVM : ViewModelBase
    {
        #region Fields

        private Disk _selectedDisk;
        private Bookmark _selectedBookmark;
        private IShellItem _selectedFile;
        private ObservableCollection<IShellItem> _files;
        private ObservableCollection<Bookmark> _bookmarks = new ObservableCollection<Bookmark>();
        private ObservableCollection<IShellItem> _openedFolders = new ObservableCollection<IShellItem>();
        private int _inFolderIndex = -1;

        #endregion

        #region Properties

        public MainWindow MW { get; set; }

        public bool CurrentVM { get; set; }

        public ObservableCollection<Disk> LocalDisks { get { return Disk.AllDisks; } }

        public IShellItem SelectedFile
        {
            get { return _selectedFile; }
            set
            {

                if (MW.LeftShellVM == this)
                {
                    CurrentVM = true;
                    MW.RightShellVM.CurrentVM = false;
                }
                else 
                {
                    CurrentVM = true;
                    MW.LeftShellVM.CurrentVM = false;
                }
                _selectedFile = value;  OnPropertyChanged("Path"); OnPropertyChanged("FileName");
                OnPropertyChanged("FileSize"); OnPropertyChanged("FileInfo1"); OnPropertyChanged("FileInfo2");
            }
        }

        public ObservableCollection<IShellItem> Files
        {
            get 
            {
                return _files;
            }
            set 
            { 
                _files = value; OnPropertyChanged("Files"); 
            }
        }

        public Disk SelectedDisk { get { return _selectedDisk; } set { _selectedDisk = value; OnPropertyChanged("Files"); } }

        public string Path
        {
            get { return (InFolder != null) ? InFolder.GetNameWithLocation() : ""; }
            set { OnPropertyChanged("Path"); }
        }

        public ObservableCollection<Bookmark> Bookmarks { get { return _bookmarks; } set { _bookmarks = value; } }

        public Bookmark SelectedBookmark
        {
            get
            {
                return _selectedBookmark;
            }
            set
            {
                _selectedBookmark = value; OnPropertyChanged("SelectedBookmark");
            }
        }

        public string CommandText { get; set; }

        public IShellItem InFolder { get; set; }

        #region File preview

        public string FileName
        {
            get { return (SelectedFile != null) ? "Name: " + SelectedFile.ToString() : ""; }
        }

        public string FileSize
        {
            get { return (SelectedFile != null) ? SelectedFile.GetSize() : ""; }

        }

        public string FileInfo1
        {
            get { return (SelectedFile != null) ? SelectedFile.GetInfo1() : ""; }
        }

        public string FileInfo2
        {
            get { return (SelectedFile != null) ? SelectedFile.GetInfo2() : ""; }

        }

        #endregion

        #endregion

        #region Commands

         public ICommand OpenCommand
         { get { return Commands.GetOrCreateCommand(() => OpenCommand, Open, SelectedFileIsNotNull); } }

         public ICommand OpenDiskCommand
         { get { return Commands.GetOrCreateCommand(() => OpenDiskCommand, OpenDisk, SelectedDiskIsNotNull); } }

         public ICommand AddBookmarkCommand
         { get { return Commands.GetOrCreateCommand(() => AddBookmarkCommand, AddBookmark, PathIsNotEmpty); } }

         public ICommand GoBookmarkCommand
         { get { return Commands.GetOrCreateCommand(() => GoBookmarkCommand, GoBookmark, SelectedBookmarkIsNotEmpty); } }

         public ICommand CloneBookmarkCommand
         { get { return Commands.GetOrCreateCommand(() => CloneBookmarkCommand, CloneBookmark, SelectedBookmarkIsNotEmpty); } }
        
         public ICommand DeleteBookmarkCommand
         { get { return Commands.GetOrCreateCommand(() => DeleteBookmarkCommand, DeleteBookmark, SelectedBookmarkIsNotEmpty); } }

         public ICommand UndoCommand
         { get { return Commands.GetOrCreateCommand(() => UndoCommand, Undo, CanUndo); } }

         public ICommand RedoCommand
         { get { return Commands.GetOrCreateCommand(() => RedoCommand, Redo, CanRedo); } }

         public ICommand SmartSayCommand
         { get { return Commands.GetOrCreateCommand(() => SmartSayCommand, SmartSay, CanSay); } }

        #endregion

        #region Methods

        private void Open()
        {
            if (SelectedFile.GetType() == typeof(Folder))
            {
                InFolder = SelectedFile;
                _openedFolders.Add(InFolder);
                _inFolderIndex++;
                Refresh();
                OnPropertyChanged("Path");
            }
            else if (SelectedFile.GetType() == typeof(Shell.Models.File))
            {
                SelectedFile.Open();
            }
        }

        public void Refresh()
        {
            Files = InFolder.GetFilesInside();
            OnPropertyChanged("Files");
        }

        private bool SelectedFileIsNotNull() { return (SelectedFile != null) ? true : false; }

        private void OpenDisk()
        {
            InFolder = SelectedDisk;
            _openedFolders.Add(InFolder);
            _inFolderIndex++;
            Refresh();
            OnPropertyChanged("InFolder");
            OnPropertyChanged("Path");
        }

        private bool SelectedDiskIsNotNull() { return (SelectedDisk != null) ? true : false; }
        
        private void AddBookmark()
        {
            if (Bookmarks == null) Bookmarks = new ObservableCollection<Bookmark>();
            Bookmarks.Add(new Bookmark(InFolder));
            OnPropertyChanged("Bookmarks");
            MessageBox.Show("New Bookmark at path = "+Path);
        }

        private bool PathIsNotEmpty() { return (Path != null) ? true : false; }

        private void GoBookmark()
        {
            Files = SelectedBookmark.PathFolder.GetFilesInside();
            _openedFolders.Add(SelectedBookmark.PathFolder);
            OnPropertyChanged("Path");
        }

        private void CloneBookmark()
        {
            if (MW.RightShellVM == this) MW.LeftShellVM.Bookmarks.Add(SelectedBookmark.Clone());
            else if (MW.LeftShellVM == this) MW.RightShellVM.Bookmarks.Add(SelectedBookmark.Clone());
        }

        private void DeleteBookmark()
        {
            Bookmarks.Remove(SelectedBookmark);
        }

        private bool SelectedBookmarkIsNotEmpty() { return (SelectedBookmark != null) ? true : false; }

        private void Undo()
        {
            _inFolderIndex--;
            InFolder = _openedFolders[_inFolderIndex];
            Files = InFolder.GetFilesInside();
            OnPropertyChanged("Path");
        }

        private bool CanUndo()
        {
            if (_openedFolders.Count > 0)
                if (_openedFolders[0] != InFolder)
                    return true;
            return false;
        }

        private void Redo()
        {
            _inFolderIndex++;
            InFolder = _openedFolders[_inFolderIndex];
            Files = InFolder.GetFilesInside();
            OnPropertyChanged("Path");
        }

        private bool CanRedo()
        {
            if (_openedFolders.Count > 0)
                if (_openedFolders[_openedFolders.Count-1] != InFolder)
                    return true;
            return false;
        }

        private void SmartSay()
        {
            InterpreterContext context = new InterpreterContext(CommandText, this);
            string start = context.PartsOfMessage()[0];
            Shell.Models.SmartShellExpressions.Expression expression = null;

            if (start.ToLower() == "open")
                expression = new OpenExpression();
            else if (start.ToLower() == "copy")
                expression = new CopyExpression();
            else if (start.ToLower() == "delete")
                expression = new DeleteExpression();
            else if (start.ToLower() == "find")
                expression = new FindExpression();

            if (expression != null)
            {
                try
                { expression.Interpret(context); }
                catch (Exception e) { MessageBox.Show(e.ToString()); }
                if (expression.GetItemsToShow() != null) Files = expression.GetItemsToShow();
            }
            else
                MessageBox.Show("Invalid command");
            try
            {
                Refresh();
            }
            catch  { }
        }

        private bool CanSay()
        {
            return (CommandText != null) ? true : false ;
        }

         #endregion

    }
}
