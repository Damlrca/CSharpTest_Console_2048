using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest_Console_2048
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[,] m = new int[4,4];

            New_random_number(ref m);
            New_random_number(ref m);

            /*for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    m[i, j] = Convert.ToInt32(Math.Pow(2, j + (i * 4)));
            m[0, 0] = 0;*/

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.CursorVisible = false;

            while (true)
            {
                //Console.Clear();
                Console.SetCursorPosition(0, 0);

                Print(ref m);

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Black;
                ConsoleKeyInfo c = Console.ReadKey();
                
                if (c.Key == ConsoleKey.UpArrow)
                {
                    Transpose(ref m);
                    Do_move(ref m);
                    Transpose(ref m);
                }
                else if (c.Key == ConsoleKey.DownArrow)
                {
                    Transpose(ref m);
                    Reflect_vertically(ref m);
                    Do_move(ref m);
                    Reflect_vertically(ref m);
                    Transpose(ref m);
                }
                else if (c.Key == ConsoleKey.RightArrow)
                {
                    Reflect_vertically(ref m);
                    Do_move(ref m);
                    Reflect_vertically(ref m);
                }
                else if (c.Key == ConsoleKey.LeftArrow)
                {
                    Do_move(ref m);
                }
            }
        }

        static Random rnd = new Random();

        static void Print(ref int[,] m)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    switch (m[i,j])
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case 2:
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 4:
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 8:
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 16:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 32:
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 64:
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 128:
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 256:
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 512:
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case 1024:
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        case 2048:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                        default:
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.Write($"{m[i, j],5}");
                }
                Console.WriteLine();
            }
        }

        static void Do_move(ref int[,] m)
        {
            if (Can_move(ref m))
            {
                m = Move_left(ref m);
                m = Merge_left(ref m);
                m = Move_left(ref m);
                New_random_number(ref m);
            }
        }

        static bool Can_move(ref int[,] m)
        {
            int[,] t = m;
            t = Move_left(ref t);
            t = Merge_left(ref t);
            t = Move_left(ref t);

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (!t[i, j].Equals(m[i, j]))
                        return true;

            return false;
        }

        static void New_random_number(ref int[,] m)
        {
            int cnt = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (m[i, j] == 0)
                        cnt++;
            
            int t = rnd.Next(0, cnt) + 1;
            cnt = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (m[i, j] == 0)
                        cnt++;

                    if (t == cnt)
                    {
                        if (rnd.Next(0, 10) == 0)
                        {
                            m[i, j] = 4;
                        }
                        else
                        {
                            m[i, j] = 2;
                        }
                        return;
                    }
                }
            }
        }

        static int[,] Move_left(ref int[,] m)
        {
            int[,] t = new int[4, 4];
            int[] id = new int[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (m[i, j] != 0)
                    {
                        t[i, id[i]] = m[i, j];
                        id[i]++;
                    }
                }
            }
            return t;
        }

        static int[,] Merge_left(ref int[,] m)
        {
            int[,] t = m;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j + 1 < 4; j++)
                {
                    if (t[i, j] == t[i, j + 1])
                    {
                        t[i, j] = t[i, j] + t[i, j];
                        t[i, j + 1] = 0;
                    }
                }
            }
            return t;
        }

        static void Reflect_vertically(ref int[,] m)
        {
            int[,] t = new int[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    t[i, j] = m[i, 3 - j];
            m = t;
        }

        static void Transpose(ref int[,] m)
        {
            int[,] t = new int[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    t[i, j] = m[j, i];
            m = t;
        }
    }
}
