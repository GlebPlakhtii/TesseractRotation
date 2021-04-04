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
    class Tesseract
    {
        public Vector4[] vertex;
        public Tesseract()
        {
            int i = 0;
            vertex = new Vector4[16];
            for (int a = 0; a < 2; a++)
            {
                for (int b = 0; b < 2; b++)
                {
                    for (int c = 0; c < 2; c++)
                    {
                        for (int d = 0; d < 2; d++)
                        {
                            vertex[i] = (new Vector4(.5f-a,.5f-b,.5f-c,.5f-d))/3.5f;
                            i += 1;
                            

                        }

                    }

                }

            }

        }
    }
}
