using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest_Console_2048
{
    internal class Program
    {
        static int n = 4;
        static Random rnd = new Random();
        static int score = 0;

        static void Main(string[] args)
        {
            int[,] m = new int[n, n];

            Restart(ref m);

            /*for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    m[i, j] = Convert.ToInt32(Math.Pow(2, j + (i * n)));
            m[0, 0] = 0;*/

            Console.CursorVisible = false;

            Console.Title = "2048";
            Console.WindowWidth = 3 * 2 + 9 * n;
            Console.WindowHeight = 4 + 1 + 3 * n;

            Console.BackgroundColor = ConsoleColor.Black;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("   2048");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("   Текущий счёт:");
            
            while (true)
            {
                Print(ref m, 3, 4);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(20 + 9 * (n - 3), 2);
                Console.Write($"{score,10}");

                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                ConsoleKeyInfo c = Console.ReadKey();

                switch(c.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        Transpose(ref m);
                        Do_move(ref m);
                        Transpose(ref m);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        Transpose(ref m);
                        Reflect_vertically(ref m);
                        Do_move(ref m);
                        Reflect_vertically(ref m);
                        Transpose(ref m);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        Reflect_vertically(ref m);
                        Do_move(ref m);
                        Reflect_vertically(ref m);
                        break;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        Do_move(ref m);
                        break;
                    case ConsoleKey.R:
                        Restart(ref m);
                        break;
                    default:
                        break;
                }

                if (c.Key == ConsoleKey.Escape)
                    break;
            }
        }

        static void Restart(ref int[,] m)
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    m[i, j] = 0;
            New_random_number(ref m);
            New_random_number(ref m);
            score = 0;
        }

        static void Print(ref int[,] m, int left, int top)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    SetColor(m[i, j]);
                    for (int k = 0; k < 3; k++)
                    {
                        Console.SetCursorPosition(left + j * 9, top + i * 3 + k);
                        Console.Write("         ");
                    }
                    Console.SetCursorPosition(left + j * 9, top + i * 3 + 1);
                    Console.Write($"{m[i, j],6}");
                }
                //Console.WriteLine();
            }
        }

        static void SetColor(int x)
        {
            switch (x)
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
        }

        static int delta = 0;

        static void Do_move(ref int[,] m)
        {
            delta = 0;
            if (Can_move(ref m))
            {
                m = Move_left(ref m);
                m = Merge_left(ref m);
                m = Move_left(ref m);
                New_random_number(ref m);
                score += delta / 2;
            }
        }

        static bool Can_move(ref int[,] m)
        {
            int[,] t = m;
            t = Move_left(ref t);
            t = Merge_left(ref t);
            t = Move_left(ref t);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (!t[i, j].Equals(m[i, j]))
                        return true;

            return false;
        }

        static void New_random_number(ref int[,] m)
        {
            int cnt = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (m[i, j] == 0)
                        cnt++;
            
            int t = rnd.Next(0, cnt) + 1;
            cnt = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
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
            int[,] t = new int[n, n];
            int[] id = new int[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
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
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j + 1 < n; j++)
                {
                    if (t[i, j] == t[i, j + 1])
                    {
                        t[i, j] = t[i, j] + t[i, j];
                        t[i, j + 1] = 0;
                        delta += t[i, j];
                    }
                }
            }
            return t;
        }

        static void Reflect_vertically(ref int[,] m)
        {
            int[,] t = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    t[i, j] = m[i, n - 1 - j];
            m = t;
        }

        static void Transpose(ref int[,] m)
        {
            int[,] t = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    t[i, j] = m[j, i];
            m = t;
        }
    }
}
