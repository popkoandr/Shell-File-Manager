using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shell.Models
{
    public interface  IPrototype<T> where T: class
    {

        T Clone();

    }
}
