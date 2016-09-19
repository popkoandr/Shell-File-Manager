using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shell.ViewModels.VMBase
{
    public class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private CommandHolder _commands;

        public CommandHolder Commands
        {
            get { return _commands ?? (_commands = new CommandHolder()); }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo Members

        public virtual string Error
        {
            get { return string.Empty; }
        }

        public virtual string this[string columnName]
        {
            get { return string.Empty; }
        }

        #endregion
    }
}
