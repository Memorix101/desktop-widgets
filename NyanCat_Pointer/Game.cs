using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using System.Windows.Interop;
using System.Windows;

namespace NyanCat_Pointer
{
    public class Game
    {

        internal static class CursorPosition
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct PointInter
            {
                public int X;
                public int Y;
                public static explicit operator Point(PointInter point) => new Point(point.X, point.Y);
            }

            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out PointInter lpPoint);

            // For your convenience
            public static Point GetCursorPosition()
            {
                PointInter lpPoint;
                GetCursorPos(out lpPoint);
                return (Point)lpPoint;
            }
        }


        internal static class Window
        {

            [DllImport("user32.dll")]
            public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

            [DllImport("user32.dll")]
            static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

            public const int GWL_EXSTYLE = -20;
            public const int WS_EX_LAYERED = 0x80000;
            public const int LWA_ALPHA = 0x2;
            public const int LWA_COLORKEY = 0x1;

            [DllImport("user32.dll")]
            private static extern long GetWindowLong(IntPtr hWnd, int nIndex);

            // Makes a window transparent by setting a transparency color.
            public static bool MakeWindowTransparent(IntPtr sdl_window, uint colorKey)
            {
                // Get window handle (https://stackoverflow.com/a/24118145/3357935)
                //https://stackoverflow.com/questions/10675305/how-to-get-the-hwnd-of-window-instance
                // Get the Win32 HWND from the FNA window
                SDL.SDL_SysWMinfo info = new SDL.SDL_SysWMinfo();
                SDL.SDL_GetWindowWMInfo(sdl_window, ref info);
                IntPtr winHandle = info.info.win.window;

                // Change window type to layered (https://stackoverflow.com/a/3970218/3357935)
                SetWindowLong(winHandle = info.info.win.window, GWL_EXSTYLE, GetWindowLong(winHandle, GWL_EXSTYLE) | WS_EX_LAYERED);

                // Set transparency color
                return SetLayeredWindowAttributes(winHandle, colorKey, 0, LWA_COLORKEY);
            }
        }

        static SDL.SDL_Rect guyPos;
        static SDL.SDL_Rect Message_rect; //create a rect

        public float x = 100;
        public float y = 100;

        private static float lastTick;
        private static float deltaTime;

        static float speed = 10f;
        static float rot;
        static float rot1;
        static float rot2;
        static float rot3;
        static float temp;

        public void Run()
        {
            int keys;

            //Screen dimension constants
            const int SCREEN_WIDTH = 640;
            const int SCREEN_HEIGHT = 480;

            IntPtr windows_body = IntPtr.Zero;
            IntPtr window_head = IntPtr.Zero;
            IntPtr window_tail = IntPtr.Zero;

            IntPtr window_feet0 = IntPtr.Zero;
            IntPtr window_feet1 = IntPtr.Zero;
            IntPtr window_feet2 = IntPtr.Zero;
            IntPtr window_feet3 = IntPtr.Zero;

            IntPtr window_rainbow0 = IntPtr.Zero;
            IntPtr window_rainbow1 = IntPtr.Zero;
            IntPtr window_rainbow2 = IntPtr.Zero;
            IntPtr window_rainbow3 = IntPtr.Zero;

            IntPtr window_star0 = IntPtr.Zero;
            IntPtr window_star1 = IntPtr.Zero;
            IntPtr window_star2 = IntPtr.Zero;
            IntPtr window_star3 = IntPtr.Zero;

            //The surface contained by the window
            IntPtr batch_body = IntPtr.Zero;
            IntPtr batch_head = IntPtr.Zero;
            IntPtr batch_tail = IntPtr.Zero;

            IntPtr batch_feet0 = IntPtr.Zero;
            IntPtr batch_feet1 = IntPtr.Zero;
            IntPtr batch_feet2 = IntPtr.Zero;
            IntPtr batch_feet3 = IntPtr.Zero;

            IntPtr batch_rainbow0 = IntPtr.Zero;
            IntPtr batch_rainbow1 = IntPtr.Zero;
            IntPtr batch_rainbow2 = IntPtr.Zero;
            IntPtr batch_rainbow3 = IntPtr.Zero;

            //The image we will load and show on the screen
            IntPtr gfx_head = IntPtr.Zero;
            IntPtr gfx_tail = IntPtr.Zero;
            IntPtr gfx_body = IntPtr.Zero;
            IntPtr gfx_feet = IntPtr.Zero;
            IntPtr gfx_rainbow = IntPtr.Zero;

            Star star_0 = new Star();

            //The image we will load and show on the screen
            IntPtr blueGuy = IntPtr.Zero;

            //Event handler
            SDL.SDL_Event e;

            //Music
            IntPtr music = IntPtr.Zero;

            //Sound
            IntPtr snd_powerup = IntPtr.Zero;

            //Initialization flag
            bool success = true;

            //Initialize SDL
            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine("SDL could not initialize! SDL_Error: %s\n", SDL.SDL_GetError());
                success = false;
            }

            //Initialize SDL_mixer
            if (SDL_mixer.Mix_OpenAudio(22050, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 4096) == -1)
            {
                Console.WriteLine("SDL could not initialize mixer! SDL_Error: %s\n", SDL.SDL_GetError());
            }

            //Create window
            window_rainbow0 = SDL.SDL_CreateWindow("window_rainbow0", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);
            window_rainbow1 = SDL.SDL_CreateWindow("window_rainbow1", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);
            window_rainbow2 = SDL.SDL_CreateWindow("window_rainbow2", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);
            window_rainbow3 = SDL.SDL_CreateWindow("window_rainbow3", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 84, 149, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);

            window_feet0 = SDL.SDL_CreateWindow("window_feet0", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);
            window_feet2 = SDL.SDL_CreateWindow("window_feet2", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);

            window_tail = SDL.SDL_CreateWindow("window_tail", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);
            windows_body = SDL.SDL_CreateWindow("windows_body", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 192, 150, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);
            window_head = SDL.SDL_CreateWindow("window_head", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 134, 109, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);

            window_feet1 = SDL.SDL_CreateWindow("window_feet1", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);
            window_feet3 = SDL.SDL_CreateWindow("window_feet3", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 35, 35, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS | SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR);

            //window_star0 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
            //window_star1 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
            //window_star2 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
            //window_star3 = SDL.SDL_CreateWindow("SDL Tutorial", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, 50, 50, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);

            //Get window surface
            batch_body = SDL.SDL_GetWindowSurface(windows_body);
            batch_tail = SDL.SDL_GetWindowSurface(window_tail);
            batch_head = SDL.SDL_GetWindowSurface(window_head);

            batch_feet0 = SDL.SDL_GetWindowSurface(window_feet0);
            batch_feet1 = SDL.SDL_GetWindowSurface(window_feet1);
            batch_feet2 = SDL.SDL_GetWindowSurface(window_feet2);
            batch_feet3 = SDL.SDL_GetWindowSurface(window_feet3);

            star_0.batch_star = SDL.SDL_GetWindowSurface(window_star0);
            batch_rainbow1 = SDL.SDL_GetWindowSurface(window_rainbow1);
            batch_rainbow2 = SDL.SDL_GetWindowSurface(window_rainbow2);
            batch_rainbow3 = SDL.SDL_GetWindowSurface(window_rainbow3);

            batch_rainbow0 = SDL.SDL_GetWindowSurface(window_rainbow0);
            batch_rainbow1 = SDL.SDL_GetWindowSurface(window_rainbow1);
            batch_rainbow2 = SDL.SDL_GetWindowSurface(window_rainbow2);
            batch_rainbow3 = SDL.SDL_GetWindowSurface(window_rainbow3);

            //Main loop flag
            bool quit = false;

            SDL_ttf.TTF_Init();

            //string t = "nalacat.jpg";
            gfx_head = SDL_image.IMG_Load("head.png");
            gfx_body = SDL_image.IMG_Load("body.png");
            gfx_feet = SDL_image.IMG_Load("feet.png");
            gfx_tail = SDL_image.IMG_Load("tail.png");
            gfx_rainbow = SDL_image.IMG_Load("rainbow.jpg");

            //blueGuy = SDL_image.IMG_Load("BlueGuy.png");

            //IntPtr Sans = SDL_ttf.TTF_OpenFont("TilWeb.ttf", 24);
            SDL.SDL_Color c = new SDL.SDL_Color();
            c.r = 255;
            //IntPtr surfaceMessage = SDL_ttf.TTF_RenderText_Solid(Sans, "put your text here", c);

            SDL.SDL_Surface sur;
            //sur = (SDL.SDL_Surface)Marshal.PtrToStructure(surfaceMessage, typeof(SDL.SDL_Surface));

            //Message_rect.x = SCREEN_WIDTH / 2 - sur.w / 2;  //controls the rect's x coordinate 
            //Message_rect.y = SCREEN_HEIGHT / 2 - sur.h / 2; // controls the rect's y coordinte

            SDL.SDL_Rect blueGuyRect;
            blueGuyRect.w = 32;
            blueGuyRect.h = 32;
            blueGuyRect.x = 0;
            blueGuyRect.y = 0;

            guyPos.x = 100;
            guyPos.y = 50;

            music = SDL_mixer.Mix_LoadMUS("nyan.ogg");
            SDL_mixer.Mix_VolumeMusic(10);
            //snd_powerup = SDL_mixer.Mix_LoadWAV("powerup_get.wav");

            //Play the music
            if (SDL_mixer.Mix_PlayMusic(music, -1) == -1)
            {
                Console.WriteLine("Unable to play music");
            }

            // Add window transparency (Magenta will be see-through)
            Window.MakeWindowTransparent(windows_body, 0x000000);
            Window.MakeWindowTransparent(window_head, 0x000000);
            Window.MakeWindowTransparent(window_tail, 0x000000);
            Window.MakeWindowTransparent(window_feet0, 0x000000);
            Window.MakeWindowTransparent(window_feet1, 0x000000);
            Window.MakeWindowTransparent(window_feet2, 0x000000);
            Window.MakeWindowTransparent(window_feet3, 0x000000);

            while (!quit)
            {
                deltaTime = (SDL.SDL_GetTicks() - lastTick) / 1000;

                //Handle events on queue
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    /* if (e.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                         SDL.SDL_GetGlobalMouseState(out x, out y);*/

                    //User requests quit
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        quit = true;
                    }
                }

                IntPtr currentKeyStates = SDL.SDL_GetKeyboardState(out keys);

                SDL.SDL_Keysym key;
                key = (SDL.SDL_Keysym)Marshal.PtrToStructure(currentKeyStates, typeof(SDL.SDL_Keysym));


                if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
                {
                    //Free resources and close SDL
                    quit = true;
                }


                /* if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_SPACE)
                 {
                     //Play the high hit effect
                     SDL_mixer.Mix_PlayChannel(-1, snd_powerup, 0);
                 }*/

                rot += speed * deltaTime; //calculate angle in relatime * speed
                rot1 += speed * 1.25f * deltaTime;
                rot2 += speed * 1.5f * deltaTime;
                rot3 += speed * 1.75f * deltaTime;
                double cos = 10f * Math.Cos(rot); //sinus from your maths class
                double cos1 = 10f * Math.Cos(rot1); //sinus from your maths class
                double cos2 = 10f * Math.Cos(rot2); //sinus from your maths class
                double cos3 = 10f * Math.Cos(rot3); //sinus from your maths class

                //x += 100 * deltaTime; //100 pixels in a second

                var p = CursorPosition.GetCursorPosition();
                //x = p.X;
                //y = p.Y;
                int vX = 100 + ((int)x - p.X) / 5;
                int vY = ((int)y - p.Y) / 5;
                x -= vX;
                y -= vY;

                //star_0.x += 10 + p.X * deltaTime; //100 pixels in a second
                //star_0.x = p.X;

                SDL.SDL_SetWindowPosition(windows_body, (int)x + 200, (int)y + (int)cos1);
                SDL.SDL_SetWindowPosition(window_head, (int)x + 325, (int)y + (int)cos + 15);
                SDL.SDL_SetWindowPosition(window_tail, (int)x + 160, (int)y + 50 + (int)cos + 15);

                SDL.SDL_SetWindowPosition(window_feet0, (int)x + 350, (int)y + 120 + (int)cos2 + 15);
                SDL.SDL_SetWindowPosition(window_feet1, (int)x + 310, (int)y + 120 + (int)cos3 + 15);
                SDL.SDL_SetWindowPosition(window_feet2, (int)x + 200, (int)y + 120 + (int)cos3 + 15);
                SDL.SDL_SetWindowPosition(window_feet3, (int)x + 240, (int)y + 120 + (int)cos2 + 15);

                SDL.SDL_SetWindowPosition(window_rainbow0, (int)x + 150, (int)y + (int)cos2 + 15);
                SDL.SDL_SetWindowPosition(window_rainbow1, (int)x + 100, (int)y + (int)cos3 + 15);
                SDL.SDL_SetWindowPosition(window_rainbow2, (int)x + 50, (int)y + (int)cos3 + 15);
                SDL.SDL_SetWindowPosition(window_rainbow3, (int)x, (int)y + (int)cos2 + 15);

                SDL.SDL_SetWindowPosition(window_star0, (int)star_0.x, (int)y + (int)cos2 + 15);

                //Apply the image
                SDL.SDL_BlitSurface(gfx_head, IntPtr.Zero, batch_head, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_body, IntPtr.Zero, batch_body, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_tail, IntPtr.Zero, batch_tail, IntPtr.Zero);

                SDL.SDL_BlitSurface(gfx_feet, IntPtr.Zero, batch_feet0, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_feet, IntPtr.Zero, batch_feet1, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_feet, IntPtr.Zero, batch_feet2, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_feet, IntPtr.Zero, batch_feet3, IntPtr.Zero);

                SDL.SDL_BlitSurface(gfx_rainbow, IntPtr.Zero, batch_rainbow0, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_rainbow, IntPtr.Zero, batch_rainbow1, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_rainbow, IntPtr.Zero, batch_rainbow2, IntPtr.Zero);
                SDL.SDL_BlitSurface(gfx_rainbow, IntPtr.Zero, batch_rainbow3, IntPtr.Zero);

                SDL.SDL_BlitSurface(star_0.gfx_star, IntPtr.Zero, star_0.batch_star, IntPtr.Zero);

                /*SDL.SDL_BlitSurface(surfaceMessage, IntPtr.Zero, gScreenSurface, ref Message_rect);
                SDL.SDL_BlitSurface(blueGuy, ref blueGuyRect, gScreenSurface, ref guyPos);*/

                //Update the surface
                SDL.SDL_UpdateWindowSurface(windows_body);
                SDL.SDL_UpdateWindowSurface(window_head);
                SDL.SDL_UpdateWindowSurface(window_tail);

                SDL.SDL_UpdateWindowSurface(window_feet0);
                SDL.SDL_UpdateWindowSurface(window_feet1);
                SDL.SDL_UpdateWindowSurface(window_feet2);
                SDL.SDL_UpdateWindowSurface(window_feet3);

                SDL.SDL_UpdateWindowSurface(window_rainbow0);
                SDL.SDL_UpdateWindowSurface(window_rainbow1);
                SDL.SDL_UpdateWindowSurface(window_rainbow2);
                SDL.SDL_UpdateWindowSurface(window_rainbow3);

                SDL.SDL_UpdateWindowSurface(window_star0);

                lastTick = (int)(SDL.SDL_GetTicks());
                Console.WriteLine(deltaTime + " - " + cos);
                SDL.SDL_Delay(30);

                //Wait two seconds
                //SDL.SDL_Delay(2000);
            }
        }
    }
}
