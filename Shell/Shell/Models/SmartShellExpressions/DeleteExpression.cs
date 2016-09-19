using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models.SmartShellExpressions
{
    public class DeleteExpression : Expression
    {
        public override void Interpret(InterpreterContext context)
        {
            String path = context.GetInput().Substring(7);
            IShellItem item = GetItem(path);
            if (item != null)
            {
                item.DeleteItself();
                MessageBox.Show("File "+item.Name+" has been deleted");
            }
            else
                MessageBox.Show("Can not find the file");
        }
    }
}
