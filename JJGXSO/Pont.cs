using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJGXSO
{
    internal class Pont
    {
        char nev;
        public int X;
        public int Y;
        public char Nev
        {
            get { return nev; }
            set { nev = value; }
        }
        Pont[] szomszedok;
        public Pont[] Szomszedok
        {
            get { return szomszedok; }
            set { szomszedok = value; }
        }
        public Pont()
        {
            szomszedok = new Pont[0];
        }
        public void PontHozzadasa(Pont pont)
        {
            Pont[] tomb2 = new Pont[Szomszedok.Length + 1];
            if (szomszedok.Length == 0)
            {
                tomb2[0] = pont;
            }
            else
            {
                for (int i = 0; i < tomb2.Length; i++)
                {

                    if (i != tomb2.Length - 1)
                    {
                        tomb2[i] = szomszedok[i];
                    }
                    else
                    {
                        tomb2[i] = pont;
                    }
                }
            }
            szomszedok = tomb2;
        }
        public bool OsszeVannakKapcsolva(Pont pont)
        {
            return szomszedok.Contains(pont);
        }
        public override string ToString()
        {
            return nev.ToString();
        }
    }
}
