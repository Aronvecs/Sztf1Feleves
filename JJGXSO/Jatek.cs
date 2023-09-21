using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJGXSO
{
    class Jatek
    {
        TerkepMegrajzolas terkep;
        Jatekos[] jatekosok;
        Jatekos drX;
        public bool DrxLephet;
        int korokSzama;
        public Jatekos Drx
        {
            get { return drX; }
        } 
        public Jatekos[] Jatekosok
        {
            get { return jatekosok; }
        }
        public int KorokSzama
        {
            get { return korokSzama; }
        }
        public Jatek(int jatekosokSzama, TerkepMegrajzolas terkep)
        {
            jatekosok = new Jatekos[jatekosokSzama];
            for (int i = 0; i < jatekosok.Length; i++)
            {
                jatekosok[i] = new Jatekos();
            }
            drX = new Jatekos();
            DrxLephet = true;
            this.terkep = terkep;
        }
        public bool JatekosLephetOda(int x, int y, Jatekos jatekos)
        {
            if (terkep.Map[x,y] != null &&
                terkep.Map[jatekos.X, jatekos.Y] != null &&
                terkep.Map[jatekos.X, jatekos.Y].OsszeVannakKapcsolva(terkep.Map[x,y]))
            {
                return true;
            }
            return false;
        }
        public Pont[] JatekosSzomszedai(Jatekos jatekos)
        {
            return terkep.Map[jatekos.X, jatekos.Y].Szomszedok;
        }
        public bool AmeddigTartAJatek()
        {
            korokSzama++;
            if (!DrxLephet)
            {
                return false;
            }
            else if (korokSzama <= terkep.CsomopontokSzama / 2) 
            {
                return true;
            }
            return false;
        }
        public bool Contains(char elem, ref int x, ref int y)
        {
            int i = 0;
            while (i<terkep.Pontok.Length && terkep.Pontok[i].Nev != elem)
            {
                i++;
            }
            if (i<terkep.Pontok.Length && terkep.Pontok[i].Nev == elem)
            {
                x = terkep.Pontok[i].X;
                y = terkep.Pontok[i].Y;
                return true;
            }
            return false;
        }
        public bool Szomszedok(Pont pont1, Pont pont2)
        {
            return pont1.OsszeVannakKapcsolva(pont2) || pont1.Nev == pont2.Nev;
        }
        private void PontTorles(Pont pont, ref Pont[] tomb)
        {
            Pont[] seged = new Pont[tomb.Length - 1];
            int j = 0;
            for (int i = 0; i < tomb.Length; i++)
            {
                if (tomb[i].Nev != pont.Nev)
                {
                    seged[j] = tomb[i];
                    j++;
                }
            }
            tomb = seged;
        }
        public bool EgyeznekAKordinatak(int x, int x2, int y, int y2)
        {
            return x == x2 && y == y2;
        }
        public bool FoglaltPont(Pont pont, Jatekos jatekos)
        {
            int i = 0;
            while (
                i<jatekosok.Length && 
                (!EgyeznekAKordinatak(jatekosok[i].X, pont.X, jatekosok[i].Y, pont.Y) ||jatekos == jatekosok[i])
                )
            {
                i++;
            }
            if (i < jatekosok.Length)
            {
                return true;
            }
            return false;
        }
        public void DrXLep()
        {
            Random r = new Random();
            int j;
            Pont[] tomb;
            if (drX.X == -1 && drX.Y == -1)
            {
                tomb = terkep.Pontok;
            }
            else
            {
                tomb = terkep.Map[drX.X, drX.Y].Szomszedok;
            }
            for (int i = 0; i < jatekosok.Length; i++)
            {
                j = 0;
                while (j < tomb.Length && !EgyeznekAKordinatak(jatekosok[i].X, tomb[j].X, jatekosok[i].Y, tomb[j].Y))
                {
                    j++;
                }
                if (j < tomb.Length)
                {
                    PontTorles(tomb[j], ref tomb);
                }
            }
            if (tomb.Length != 0)
            {
                int index = r.Next(0, tomb.Length-1);
                drX.Lep(tomb[index].X, tomb[index].Y);
            }
        }
    }
}
