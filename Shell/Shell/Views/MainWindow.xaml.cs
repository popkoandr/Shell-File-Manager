using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shell.Models;
using Shell.ViewModels;

namespace Shell.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowVM vm;
        public ShellControlVM LeftShellVM, RightShellVM;
        public MainWindow()
        {
            Disk.GetLocalDisks();
            InitializeComponent();
            vm = new MainWindowVM();
            LeftShellVM = new ShellControlVM();
            RightShellVM = new ShellControlVM();
            this.DataContext = vm;
            LeftShellVM.MW = this;
            RightShellVM.MW = this;
            vm.LeftShell = LeftShellVM;
            vm.RightShell = RightShellVM;

            LeftShellControl.DataContext = LeftShellVM;
            RightShellControl.DataContext = RightShellVM;
        }

    }
}
