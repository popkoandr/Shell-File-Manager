using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Models.SmartShellExpressions
{
    public class FindExpression : Expression
    {
        public override void Interpret(InterpreterContext context)
        {
            string subName = "";
            string startDirectory = "";
            int inIndex = 0;
            
            for (int i = 1; i < context.PartsOfMessage().Count; i++)
            {
                if (context.PartsOfMessage()[i].ToLower() == "in")
                {
                    inIndex = i;
                    break;
                }
            }

            ////// Determinating of subname 
            List<string> temp = new List<string>();
            for (int i = 1; i < inIndex; i++)
                temp.Add(context.PartsOfMessage()[i]);
            subName = String.Join(" ", temp);
            temp.Clear();

            ////// Determinating of directory  
            for (int i = inIndex +1; i < context.PartsOfMessage().Count; i++)
                temp.Add(context.PartsOfMessage()[i]);
            startDirectory = String.Join(" ", temp);
            _itemsToShow.Clear();
            new Folder(startDirectory).GetFoundedSubitems(subName, _itemsToShow);
        }
    }
}
