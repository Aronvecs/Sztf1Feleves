using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace JJGXSO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TerkepMegrajzolas terkep;
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine("nyomd meg a 2-est ha szeretnél új pályát és 1-est ha már meglévőt");
                key = Console.ReadKey();
                Console.Clear();
            } while (key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D1);
            if (key.Key == ConsoleKey.D2 || !File.Exists("fajnevek.txt"))
            {
                terkep = new TerkepMegrajzolas();
            }
            else
            {
                string fajl;
                string[] fajlnevek;
                do
                {
                    fajlnevek = File.ReadAllLines("fajnevek.txt");
                    for (int i = 0; i < fajlnevek.Length; i++)
                    {
                        terkep = new TerkepMegrajzolas(fajlnevek[i]);
                        Console.WriteLine();
                        Console.WriteLine(fajlnevek[i]);
                        terkep.KiRajzolas(-1, -1);
                    }
                    Console.Write("Válasz a lehetőségek közül: ");
                    fajl = Console.ReadLine();
                } while (!fajlnevek.Contains(fajl));
                terkep = new TerkepMegrajzolas(fajl);
                Console.Clear();
            }
            Console.Clear();
            string jatekosokSzama;
            int szam;
            do
            {
                Console.Write($"Add meg a játékosok számát.(maximum {terkep.CsomopontokSzama/4} fő lehet): ");
                jatekosokSzama = Console.ReadLine();
            } while (!int.TryParse(jatekosokSzama, out szam) || szam <= 0 || szam > (terkep.CsomopontokSzama / 4));

            Jatek jatek = new Jatek(szam, terkep);
            int j;
            do
            {
                j = 0;
                MindenkiLep(jatek, terkep, ref j);
            } while (jatek.AmeddigTartAJatek());
            if (jatek.DrxLephet)
            {
                Console.WriteLine("\nDrX nyert");
            }
            else
            {
                Console.WriteLine($"\nJatekos{j} nyert");
            }
            Console.ReadKey();
        }
        static void MindenkiLep(Jatek jatek, TerkepMegrajzolas terkep, ref int i)
        {
            char pont;
            int x = 0, y = 0;
            while (i < jatek.Jatekosok.Length && jatek.DrxLephet)
            {
                if (jatek.Jatekosok[i].X == -1 && jatek.Jatekosok[i].Y == -1)
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Jatekos{i + 1} lép.");
                        terkep.KiRajzolas(jatek.Jatekosok[i].X, jatek.Jatekosok[i].Y);
                        Console.Write("Adj meg egy pontot: ");
                        pont = Console.ReadKey().KeyChar;
                    } while (!jatek.Contains(pont, ref x, ref y) || jatek.FoglaltPont(terkep.Map[x, y], jatek.Jatekosok[i]));
                }
                else
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Jatekos{i + 1} lép.");
                        terkep.KiRajzolas(jatek.Jatekosok[i].X, jatek.Jatekosok[i].Y);
                        Console.Write("jatekos szomszedai: ");
                        Pont[] szomszedok = jatek.JatekosSzomszedai(jatek.Jatekosok[i]);
                        for (int j = 0; j < szomszedok.Length; j++)
                        {
                            if (!jatek.FoglaltPont(szomszedok[j], null))
                            {
                                if (j != szomszedok.Length - 1)
                                {
                                    Console.Write(szomszedok[j] + ",");
                                }
                                else
                                {
                                    Console.Write(szomszedok[j]);
                                }
                            }
                        }
                        Console.WriteLine();
                        Console.Write("Adj meg egy pontot: ");
                        pont = Console.ReadKey().KeyChar;
                    } while (!jatek.Contains(pont, ref x, ref y) || jatek.FoglaltPont(terkep.Map[x, y], jatek.Jatekosok[i]) ||
                    !jatek.Szomszedok(terkep.Map[jatek.Jatekosok[i].X, jatek.Jatekosok[i].Y], terkep.Map[x, y])
                    );
                }
                jatek.Jatekosok[i].Lep(x, y);
                if (jatek.EgyeznekAKordinatak(jatek.Jatekosok[i].X, jatek.Drx.X, jatek.Jatekosok[i].Y, jatek.Drx.Y))
                {
                    jatek.DrxLephet = false;
                }
                i++;
            }
            if (jatek.DrxLephet)
            {
                jatek.DrXLep();

                if (jatek.KorokSzama % 4 == 0 && jatek.KorokSzama != 0)
                {
                    Console.Write($"\nDrX a {terkep.Map[jatek.Drx.X, jatek.Drx.Y].Nev} ponton van");
                    Console.ReadKey();
                }
            }
        }
        
    }
}
