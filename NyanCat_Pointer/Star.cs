using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace NyanCat_Pointer
{
    class Star
    {
       public IntPtr gfx_star = IntPtr.Zero;
       public IntPtr batch_star = IntPtr.Zero;

       public float x;
       //public IntPtr window_star = IntPtr.Zero;

       public Star()
       {
           gfx_star = SDL_image.IMG_Load("star.png");
        }
    }
}
