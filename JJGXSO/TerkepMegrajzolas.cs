using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJGXSO
{
    class TerkepMegrajzolas
    {
        Pont[,] map = new Pont[10, 10];
        Pont[] pontok;
        public Pont[] Pontok
        {
            get { return pontok; }
        }
        int csomopontokSzama;
        public int CsomopontokSzama
        {
            get { return csomopontokSzama; }
        }
        public Pont[,] Map
        {
            get { return map; }
        }
        public TerkepMegrajzolas()
        {
            pontok = new Pont[0];
            Osszekapcsolasok();
            FajlbaIras();
        }
        public TerkepMegrajzolas(string fajlneve)
        {
            pontok = new Pont[0];
            FajlBeolvas(fajlneve);
        }
        private void FajlBeolvas(string fajlnev)
        {
            string[] pontok = File.ReadAllLines(fajlnev + "pontok.txt");
            string[] kordinatak = File.ReadAllLines(fajlnev + "kordinatak.txt");
            csomopontokSzama = pontok.Length;
            for (int i = 0; i < pontok.Length; i++)
            {
                string[] tobbKordinata = kordinatak[i].Split(',');
                string[] kordinata = tobbKordinata[0].Split(';');
                map[Convert.ToInt32(kordinata[0]), Convert.ToInt32(kordinata[1])] = new Pont
                {
                    Nev = pontok[i][0],
                    X = Convert.ToInt32(kordinata[0]),
                    Y = Convert.ToInt32(kordinata[1])
                };
                UjPontHozzaAdas(map[Convert.ToInt32(kordinata[0]), Convert.ToInt32(kordinata[1])]);
            }
            for (int i = 0; i < pontok.Length; i++)
            {
                string[] tobbKordinata = kordinatak[i].Split(',');
                string[] fopont = tobbKordinata[0].Split(';');
                for (int j = 1; j < tobbKordinata.Length; j++)
                {
                    string[] kordinata = tobbKordinata[j].Split(';');
                    map[Convert.ToInt32(fopont[0]), Convert.ToInt32(fopont[1])].
                        PontHozzadasa(map[Convert.ToInt32(kordinata[0]), Convert.ToInt32(kordinata[1])]);
                }
            }
        }
        private void FajlbaIras()
        {
            Console.Write("Kérlek adj nevet a térkének: ");
            string TXTneve = Console.ReadLine();
            string Spontok = "";
            string kordinatak = "";
            for (int i = 0; i < pontok.Length; i++)
            {
                Spontok += pontok[i];
                kordinatak += $"{pontok[i].X};{pontok[i].Y},";
                for (int j = 0; j < pontok[i].Szomszedok.Length; j++)
                {
                    Spontok += pontok[i].Szomszedok[j].Nev;
                    if (j == pontok[i].Szomszedok.Length - 1)
                    {
                        kordinatak += $"{pontok[i].Szomszedok[j].X};{pontok[i].Szomszedok[j].Y}";
                    }
                    else
                    {
                        kordinatak += $"{pontok[i].Szomszedok[j].X};{pontok[i].Szomszedok[j].Y},";
                    }
                }
                Spontok += "\n";
                kordinatak += "\n";
            }
            File.WriteAllText(TXTneve + "pontok.txt", Spontok);
            File.WriteAllText(TXTneve + "kordinatak.txt", kordinatak);
            File.AppendAllText("fajnevek.txt", $"{TXTneve}\n");
        }
        private void Osszekapcsolasok()
        {
            string szx;
            string szy;
            int x;
            int y;
            do
            {
                Console.Clear();
                KiRajzolas(-1, -1);
                Console.WriteLine("Válazd ki egy csomopontot");
                Console.Write("A csomopont X kordinátája: ");
                szx = Console.ReadLine();
                Console.Write("A csomopont Y kordinátája: ");
                szy = Console.ReadLine();
            } while (!int.TryParse(szx, out x) || !int.TryParse(szy, out y) ||
                    x - 1 < 0 || y - 1 < 0 || x - 1 >= map.GetLength(0) || y - 1 >= map.GetLength(1));
            Pont csomopont = new Pont();
            do
            {
                Console.Write("Kérlek nevezd el a csomopontot: ");
                csomopont.Nev = Console.ReadKey().KeyChar;
                Console.WriteLine();
            } while (!ErvenyesCsomopontNev(csomopont.Nev) || Contains(csomopont.Nev));
            x -= 1;
            y -= 1;
            map[x, y] = csomopont;
            map[x, y].X = x;
            map[x, y].Y = y;
            UjPontHozzaAdas(map[x, y]);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            csomopontokSzama++;
            while (key.Key != ConsoleKey.Escape)
            {
                KiRajzolas(x, y);
                Szomszedok(ref x, ref y, ref map[x, y]);
                if (csomopontokSzama > 10) 
                {
                    Console.WriteLine("Nyomj egy Escapet ha abba szeretnéd hagyni és bármi mást ha folytatni szeretnéd");
                    key = Console.ReadKey();
                }
                if (key.Key != ConsoleKey.Escape)
                {
                    string szomszedx;
                    string szomszedy;
                    int x2;
                    int y2;
                    do
                    {
                        Console.WriteLine("Válassz egy szomszédos pontot");
                        Console.Write("A csomopont sora: ");
                        szomszedx = Console.ReadLine();
                        Console.Write("A csomopont oszlopa: ");
                        szomszedy = Console.ReadLine();
                    } while (!int.TryParse(szomszedx, out x2) || !int.TryParse(szomszedy, out y2) ||
                    !Szomszedok(x, y, x2 - 1, y2 - 1) ||
                    map[x2-1, y2-1] == null ||
                    !map[x, y].OsszeVannakKapcsolva(map[x2-1, y2-1]) || (x == x2-1 && y == y2-1)
                    );
                    x = x2-1;
                    y = y2-1;
                } 
                KiRajzolas(x, y);
                Console.Clear();
            }
        }
        void UjPontHozzaAdas(Pont pont)
        {
            Pont[] tomb = new Pont[pontok.Length + 1];
            for (int i = 0; i < tomb.Length; i++)
            {
                if (i != tomb.Length-1)
                {
                    tomb[i] = pontok[i];
                }
                else
                {
                    tomb[i] = pont;
                }
            }
            pontok = tomb;
        }
        public bool Contains(char elem)
        {
            int i = 0;
            while (i < pontok.Length && pontok[i].Nev != elem)
            {
                i++;
            }
            if (i < pontok.Length)
            {
                return true;
            }
            return false;
        }
        bool ErvenyesCsomopontNev(char csomopont)
        {
            if (((csomopont >= 65 && csomopont <= 90) || (csomopont >= 97 && csomopont <= 122)) && csomopont != 88 && csomopont != 120)
            {
                return true;
            }
            return false;
        }
        int SzomszedokDarabSzama(int x, int y)
        {
            int db = 0;
            if (x - 1 >= 0 && y - 1 >= 0 && Szomszedok(x, y, x - 1, y - 1)) 
            {
                db++;
            }
            if (x + 1 < map.GetLength(0) && y + 1 < map.GetLength(1) && Szomszedok(x, y, x + 1, y + 1)) 
            {
                db++;
            }
            if (x + 1 < map.GetLength(0) && y - 1 >= 0 && Szomszedok(x, y, x + 1, y - 1)) 
            {
                db++;
            }
            if (y + 1 < map.GetLength(1) && x - 1 >= 0 && Szomszedok(x, y, x - 1, y + 1)) 
            {
                db++;
            }
            if (x + 1 < map.GetLength(0) && Szomszedok(x, y, x + 1, y))
            {
                db++;
            }
            if (y + 1 < map.GetLength(1) && Szomszedok(x, y, x, y + 1))
            {
                db++;
            }
            if (y - 1 >= 0 && Szomszedok(x, y, x, y - 1)) 
            {
                db++;
            }
            if (x - 1 >= 0 && Szomszedok(x, y, x - 1, y)) 
            {
                db++;
            }
            return db;
        }
        void Szomszedok(ref int x, ref int y, ref Pont pont)
        {
            int i = 0;
            int szomszedokDarabszama = SzomszedokDarabSzama(x, y) - pont.Szomszedok.Length;
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            while (key.Key != ConsoleKey.Escape && i < szomszedokDarabszama)
            {
                Console.Clear();
                string Szx;
                string Szy;
                int szx;
                int szy;
                KiRajzolas(x, y);
                do
                {
                    Console.WriteLine("Válazd ki a szomszéd kordinátáit akikkel szeretnéd hogy kapcsolatban legyen");
                    Console.Write("A csomopont sora: ");
                    Szx = Console.ReadLine();

                    Console.Write("A csomopont oszlopa: ");
                    Szy = Console.ReadLine();
                } while (!int.TryParse(Szx, out szx) || !int.TryParse(Szy, out szy) ||
                !Szomszedok(x, y, szx - 1, szy - 1) || pont.OsszeVannakKapcsolva(map[szx - 1, szy - 1]));
                szx -= 1;
                szy -= 1;

                Pont csomopont = new Pont();
                if (map[szx, szy] == null)
                {
                    do
                    {
                        Console.Write("Kérlek nevezd el a csomopontot: ");
                        csomopont.Nev = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                    } while (!ErvenyesCsomopontNev(csomopont.Nev) || Contains(csomopont.Nev));
                    map[szx, szy] = csomopont;
                    map[szx, szy].PontHozzadasa(pont);
                    map[szx, szy].X = szx;
                    map[szx, szy].Y = szy;

                    pont.PontHozzadasa(map[szx, szy]);
                    UjPontHozzaAdas(map[szx, szy]);
                    csomopontokSzama++;
                }
                else
                {
                    pont.PontHozzadasa(map[szx, szy]);
                    map[szx, szy].PontHozzadasa(pont);
                }
                Console.Clear();
                i++;
                KiRajzolas(x, y);
                if (i == szomszedokDarabszama) 
                {
                    key = new ConsoleKeyInfo();
                }
                else
                {
                    Console.WriteLine("Elég ennyi szomszéd?(nyomj escape-t ha igen és bármi mást ha nem)");
                    key = Console.ReadKey();
                }
            }
        }
        bool Szomszedok(int X, int Y, int szomszedX, int szomszedY)
        {
            if (szomszedX >= 0 && szomszedX < map.GetLength(0) && szomszedY >= 0 && szomszedY < map.GetLength(1)) 
            {
                if ((X - 1 == szomszedX || X + 1 == szomszedX) && (Y - 1 == szomszedY || Y + 1 == szomszedY))
                {
                    return true;
                }
                else if (X == szomszedX && (Y + 1 == szomszedY || Y - 1 == szomszedY))
                {
                    return true;
                }
                else if (Y == szomszedY && (X + 1 == szomszedX || X - 1 == szomszedX))
                {
                    return true;
                }
            }
            return false;
        }
        public void KiRajzolas(int i, int j)
        {
            Console.WriteLine();
            int sorok = map.GetLength(0) * 2;
            int oszlopok = map.GetLength(1) * 2;
            for (int x = 0; x < sorok; x++)
            {
                if (x==0)
                {
                    Console.Write(" ");
                }
                for (int y = 0; y < oszlopok; y++)
                {
                    if (x == 0 && y % 2 != 0)
                    {
                        Console.Write((y / 2) + 1);
                    }
                    else if (x == 0 && y % 2 == 0) 
                    {
                        Console.Write(" ");
                    }
                    if (x > 0) 
                    {
                        
                        if (y == 0 && x % 2 == 0)
                        {
                            Console.Write("  ");
                        }
                        else if (y == 0 && x == (map.GetLength(0) * 2) - 1)
                        {
                            Console.Write($"{(x / 2) + 1}");
                        }
                        else if (y == 0 && x % 2 != 0)
                        {
                            Console.Write($"{(x / 2) + 1} ");
                        }
                        else if (y % 2 != 0 && x % 2 != 0)
                        {
                            if (map[x / 2, y / 2] == null)
                            {
                                Console.Write("*");
                            }
                            else if (x / 2 == i && y / 2 == j)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(map[x / 2, y / 2]);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.Write(map[x / 2, y / 2]);
                            }
                        }
                        else if (x % 2 != 0 && y % 2 == 0 &&
                            y < oszlopok - 1 &&
                            map[x / 2, (y / 2)-1] != null && map[x / 2, (y + 1) / 2] != null &&
                            map[x / 2, (y / 2)-1].OsszeVannakKapcsolva(map[x / 2, (y + 1) / 2])
                            )
                        {
                            Console.Write("-");
                        }
                        else if (x % 2 == 0 && y % 2 == 0 &&
                            y > 0 && y < oszlopok - 1 &&
                            x > 0 && x < sorok - 1 &&
                            map[(x - 1) / 2, (y - 1) / 2] != null && map[(x + 1) / 2, (y + 1) / 2] != null &&
                            map[(x + 1) / 2, (y - 1) / 2] != null && map[(x - 1) / 2, (y + 1) / 2] != null &&
                            map[(x - 1) / 2, (y - 1) / 2].OsszeVannakKapcsolva(map[(x + 1) / 2, (y + 1) / 2]) &&
                            map[(x + 1) / 2, (y - 1) / 2].OsszeVannakKapcsolva(map[(x - 1) / 2, (y + 1) / 2])
                            )
                        {
                            Console.Write("X");
                        }
                        else if (x % 2 == 0 && y % 2 == 0 &&
                            y > 0 && y < oszlopok - 1 &&
                             x > 0 && x < sorok - 1 &&
                            map[(x - 1) / 2, (y - 1) / 2] != null && map[(x + 1) / 2, (y + 1) / 2] != null &&
                            map[(x - 1) / 2, (y - 1) / 2].OsszeVannakKapcsolva(map[(x + 1) / 2, (y + 1) / 2]))
                        {
                            Console.Write("\\");
                        }
                        else if (x % 2 == 0 && y % 2 == 0 &&
                            y > 0 && y < oszlopok - 1 &&
                             x > 0 && x < sorok - 1 &&
                            map[(x + 1) / 2, (y - 1) / 2] != null && map[(x - 1) / 2, (y + 1) / 2] != null &&
                            map[(x + 1) / 2, (y - 1) / 2].OsszeVannakKapcsolva(map[(x - 1) / 2, (y + 1) / 2]))
                        {
                            Console.Write("/");
                        }
                        else if (x % 2 == 0 && y % 2 != 0 &&
                            map[(x - 1) / 2, y / 2] != null && map[(x + 1) / 2, y / 2] != null &&
                            map[(x - 1) / 2, y / 2].OsszeVannakKapcsolva(map[(x + 1) / 2, y / 2]))
                        {
                            Console.Write("|");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
