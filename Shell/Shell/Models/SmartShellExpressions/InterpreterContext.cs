using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shell.ViewModels;

namespace Shell.Models.SmartShellExpressions
{
    public class InterpreterContext
    {
        private string _input;

        public ShellControlVM ViewModel { get; set; }

        public InterpreterContext(string message, ShellControlVM vm)
        {
            _input = message;
            ViewModel = vm;
        }

        public string GetInput()
        {
            return _input;
        }

        public List<string> PartsOfMessage()
        {
            string[] parts = GetInput().Split(' ');
            List<string> temp = new List<string>();
            foreach (var part in parts)
                if (part != "")
                    temp.Add(part);
            return temp;
        }

        public void SetInput(string input)
        {
            _input = input;
        }
    }
}
