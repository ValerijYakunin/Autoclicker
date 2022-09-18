using System;
using System.Threading;
using Spectre.Console;
using AutoIt;
using System.Runtime.InteropServices;

namespace Autoclicker
{
    internal class Program
    {
        public static string version = "v 0.0.1";
        public static bool check = true;
        public static int startDelay = 5;
        public static Mutex mutexObj = new Mutex();
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags
            );
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetAsyncKeyState(int vkey);
        public static void Main(string[] args)
        {
            AnsiConsole.Write(new Markup("[underline green]PeepoSoft [/]"));
            AnsiConsole.Write(new Markup("[underline]AutoClicker " + version + "[/]"));
            AnsiConsole.WriteLine();
            try
            {
                AnsiConsole.Write(new Markup("Press [underline]Space[/] to start clicker"));
                AnsiConsole.WriteLine();
                if (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
                {
                    Console.WriteLine("Program closing...");
                    Thread.Sleep(1000);
                    return;
                }
                Console.WriteLine("Starting...");
                Thread.Sleep(1000);
                for (int i = startDelay; i > 0; i--)
                {
                    Console.WriteLine(i + "...");
                    Thread.Sleep(1000);
                }
                Thread keyHook = new Thread(() =>
                {
                    while (true)
                    {
                        if (GetAsyncKeyState(0x20) == -32768)
                        {
                            check = false;
                            break;
                        }
                    }
                });
                keyHook.IsBackground = true;
                keyHook.Start();
                AnsiConsole.Write(new Markup("[underline green]Clicker enabled![/]"));
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("Press [underline]Space[/] to stop"));
                AnsiConsole.WriteLine();
                Clicker();
                AnsiConsole.Write(new Markup("[underline green]Clicker disabled![/]"));
                AnsiConsole.WriteLine();
                AnsiConsole.Write(new Markup("Press [underline]Escape[/] to close"));
                AnsiConsole.WriteLine();
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Program closing...");
                    Thread.Sleep(1000);
                    return;
                }
                else 
                {
                    key = Console.ReadKey(true).Key;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup("[underline red]Ошибка работы: " + ex.ToString() + "[/]"));
                AnsiConsole.WriteLine();
            }
        }

        public static void Clicker()
        {
            while (check)
            {
                if (!SetWindowPos(GetConsoleWindow(), (IntPtr)(-1), 0, 0, 0, 0, 0x1 | 0x2))
                    throw new Exception();
                AutoItX.MouseClick();
                Thread.Sleep(100);
            }
        }
    }
}