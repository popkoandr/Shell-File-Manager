using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shell.Models.SmartShellExpressions
{
    public class CopyExpression : Expression
    {
        public override void Interpret(InterpreterContext context)
        {
            string filePath = "";
            string destinationPath = "";
            int toIndex = 0;
            for (int i = 1; i < context.PartsOfMessage().Count - 1; i++)
            {
                if (context.PartsOfMessage()[i].ToLower() == "to")
                {
                    toIndex = i;
                    break;
                }
            }
            List<string> temp = new List<string>();
            for (int i = 1; i < toIndex; i++)
                temp.Add(context.PartsOfMessage()[i]);
            filePath = String.Join(" ", temp);
            temp.Clear();
            for(int i=toIndex+1;i<context.PartsOfMessage().Count;i++)
                temp.Add(context.PartsOfMessage()[i]);
            destinationPath = String.Join(" ", temp);
            IShellItem item = GetItem(filePath);
            if (item != null)
                item.Paste(new Folder(destinationPath), false);
            else
                MessageBox.Show("Can not find the file to copy");

        }
    }
}
