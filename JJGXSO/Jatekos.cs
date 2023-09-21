using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace JJGXSO
{
    class Jatekos
    {
        int x=-1;
        int y=-1;
        public int X { get { return x; } set { value = x; } }
        public int Y { get { return y; } set { value = y; } }
        public void Lep(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
    }
}
