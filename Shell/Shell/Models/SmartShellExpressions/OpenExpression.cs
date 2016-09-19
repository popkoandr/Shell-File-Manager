using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models.SmartShellExpressions
{
    public class OpenExpression : Expression
    {
        public override void Interpret(InterpreterContext context)
        {
            String path = context.GetInput().Substring(5);
            IShellItem item = GetItem(path);
            if (item != null)
                item.Open();
            else
                MessageBox.Show("Can not find file");
        }
    }
}
