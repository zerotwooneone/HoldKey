using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HoldKey
{
    public class MainWindowViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public ICommand Enabled { get; }
        /// <summary>
        /// default value must match that of UI
        /// </summary>
        private bool isEnabled = false;
        public ICommand Train { get; }
        public ICommand Closing { get; }

        /// <summary>
        /// F13
        /// </summary>
        /// <remarks>comes from https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes</remarks>
        private static readonly byte TargetKey = 0x7C;
        public MainWindowViewmodel()
        {
            Enabled = new RelayCommand(OnEnabled, o => true);
            Train = new RelayCommand(OnTrain, o=>true);
            Closing = new RelayCommand(OnClosing, o => true);
            
            //attempt to fix jams
            Keyboard.KeyUp(TargetKey);            
        }

        private void OnClosing(object obj)
        {
            Keyboard.KeyUp(TargetKey);
        }

        private void OnTrain(object obj)
        {
            System.Threading.Thread.Sleep(2000);
            foreach(var x in Enumerable.Range(1, 100))
            {
                Keyboard.KeyDown(TargetKey);
                System.Threading.Thread.Sleep(100);
                Keyboard.KeyUp(TargetKey);
            }
        }

        private void OnEnabled(object obj)
        {
            var name = (string)obj;
            var wasEnabled = isEnabled;
            isEnabled = name == "On";

            if (!wasEnabled && isEnabled)
            {
                Keyboard.KeyDown(TargetKey);
            } else if (wasEnabled && !isEnabled)
            {
                Keyboard.KeyUp(TargetKey);
            }
        }
    }
}
