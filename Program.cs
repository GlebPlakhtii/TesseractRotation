using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;



namespace _4DRotaionCube
{
    class Program
    {   
        
        static void Main(string[] args)
        {
            GameWindow win = new GameWindow(800, 600,GraphicsMode.Default,"Tesseract",GameWindowFlags.Default);
            Game game = new Game(win);
            win.Run(60);
        }
    }
}
