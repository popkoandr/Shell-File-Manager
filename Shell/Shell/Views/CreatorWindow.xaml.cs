using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Shell.Views
{
    /// <summary>
    /// Interaction logic for CreatorWindow.xaml
    /// </summary>
    public partial class CreatorWindow : Window
    {
        public bool Created = false;
        public string itemName = "";
        public CreatorWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
                itemName = tbName.Text;
                Created = true;
                this.Close();
        }
    }
}
