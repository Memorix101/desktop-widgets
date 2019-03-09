using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

namespace NyanCat_Pointer
{
    class Program
    {

        [STAThread]
        static void Main()
        {
            Thread thread1 = new Thread(new ThreadStart(A));
            Thread thread2 = new Thread(new ThreadStart(B));
            thread1.Start();
           // thread2.Start();
            thread1.Join();
            //thread2.Join();
        }

        static void A()
        {
            Thread.Sleep(100);
            int x = 100;
            int y = 100;
            var game = new Game();
            game.x = x;
            game.y = y;
            game.Run();
        }

        static void B()
        {
            Thread.Sleep(1000);
            int x = 300;
            int y = 300;
            var game1 = new Game();
            game1.x = x;
            game1.y = y;
            game1.Run();
        }
    }
}
