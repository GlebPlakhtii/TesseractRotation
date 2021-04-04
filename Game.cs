using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using GL = OpenTK.Graphics.GL;

namespace _4DRotaionCube
{
    
    class Game
    {
        GameWindow window;
        Tesseract tes;
        double a =.3;
        double b = .3;
        double c = .3;
        double d = .3;

        Matrix4 rotorX;
        Matrix4 rotorZ;
        Matrix4 rotorW;
        Matrix4 rotorY;
        Vector4[] t2;
        
        

        public Game(GameWindow window)
        {   
            this.window = window;
            window.Load += winLoad;
            window.RenderFrame += winRenderFrame;
            window.UpdateFrame += winUpdateFrame;
            tes = new Tesseract();
            rotorX = new Matrix4(1, 0, 0, 0,
                                0,Convert.ToSingle(Math.Cos(a)), -Convert.ToSingle(Math.Sin(a)), 0,
                                0,Convert.ToSingle(Math.Sin(a)), Convert.ToSingle(Math.Cos(a)), 0,
                                 0, 0, 0, 1);
            rotorZ = new Matrix4(Convert.ToSingle(Math.Cos(b)), -Convert.ToSingle(Math.Sin(b)), 0, 0,
                                Convert.ToSingle(Math.Sin(b)), Convert.ToSingle(Math.Cos(b)), 0, 0,
                                 0, 0, 1, 0,
                                 0, 0, 0, 1);

            rotorW = new Matrix4(1, 0, 0, 0,
                                0, 1, 0, 0,
                               0,0, Convert.ToSingle(Math.Cos(c)), -Convert.ToSingle(Math.Sin(c)),
                               0,0,Convert.ToSingle(Math.Sin(c)), Convert.ToSingle(Math.Cos(c)));
                
            rotorY = new Matrix4(Convert.ToSingle(Math.Cos(d)),0, -Convert.ToSingle(Math.Sin(d)),0,
                                 0,1, 0, 0,
                                 Convert.ToSingle(Math.Sin(d)),0, Convert.ToSingle(Math.Cos(d)),0,
                                 0, 0, 0, 1);




            t2 = new Vector4[16];
        }

        private void winUpdateFrame(object sender, FrameEventArgs e)
        {
           
           
        }
        private Vector4 mult(Vector4 v, Matrix4 m)
        {
            return new Vector4(
                m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14 * v.W,
                m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24 * v.W,
                m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34 * v.W,
                m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44 * v.W);
        }

        private void winRenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.Aqua);
            GL.Clear(OpenTK.Graphics.ClearBufferMask.ColorBufferBit);

            
            for (int i = 0; i < tes.vertex.Length; i++)
            {
                var p1 = mult(tes.vertex[i], rotorX);
                var p2 = mult(p1, rotorY);
                var p3 = mult(p2, rotorZ);
                var p4 = mult(p3, rotorW);
                





                t2[i] = p4; 
            }
           
            for (int i = 0; i < tes.vertex.Length; i++)
            {
                for (int j = 0; j < tes.vertex.Length; j++)
                {
                    if (i != j)
                    {
                        int a4 = i % 2;
                        int a3 = i / 2 % 2;
                        int a2 = i / 4 % 2;
                        int a1 = i / 8 % 2;

                        int b4 = j % 2;  
                        int b3 = j / 2 % 2;
                        int b2 = j / 4 % 2;
                        int b1 = j / 8 % 2;

                        if ((a1 == b1 && a2 == b2 && a3 == b3 && a4 != b4) ||
                            (a1 == b1 && a2 == b2 && a3 != b3 && a4 == b4) ||
                            (a1 == b1 && a2 != b2 && a3 == b3 && a4 == b4) ||
                            (a1 != b1 && a2 == b2 && a3 == b3 && a4 == b4))
                             
                        {

                            
                            GL.Begin(OpenTK.Graphics.BeginMode.Lines);
                            
                            GL.Color3(1f, 0, 0);
                            GL.LineWidth(5);
                            GL.Vertex2(t2[i].X/(.2*t2[i].Z+.2)*(.2 * t2[i].W + .2) - .7, t2[i].Y/(.2*t2[i].Z+.2)*(.2 * t2[i].W + .2) + .4);
                            GL.Vertex2(t2[j].X /(.2*t2[j].Z +.2)* (.2 * t2[j].W + .2) - .7, t2[j].Y / (.2 *t2[j].Z +.2)* (.2 * t2[j].W + .2) + .4);

                            GL.Vertex2(t2[i].X / (.2 * t2[i].Y + .2) * (.2 * t2[i].W + .2), t2[i].Z / (.2 * t2[i].Y + .2) * (.2 * t2[i].W + .2) + .4);
                            GL.Vertex2(t2[j].X / (.2 * t2[j].Y + .2) * (.2 * t2[j].W + .2), t2[j].Z / (.2 * t2[j].Y + .2) * (.2 * t2[j].W + .2) + .4);

                            GL.Vertex2(t2[i].X / (.2 * t2[i].Z + .2)* (.2 * t2[i].Y + .2) + .7, t2[i].W / (.2 * t2[i].Z + .2) * (.2 * t2[i].Y + .2) + .4);
                            GL.Vertex2(t2[j].X / (.2 * t2[j].Z + .2) * (.2 * t2[j].Y + .2) + .7, t2[j].W / (.2 * t2[j].Z + .2) * (.2 * t2[j].Y + .2)+ .4);

                            GL.Vertex2(t2[i].Y / (.2 * t2[i].X + .2) * (.2 * t2[i].W + .2) - .7, t2[i].Z / (.2 * t2[i].X + .2) * (.2 * t2[i].W + .2) - .4);
                            GL.Vertex2(t2[j].Y / (.2 * t2[j].X + .2) * (.2 * t2[j].W + .2) - .7, t2[j].Z / (.2 * t2[j].X + .2) * (.2 * t2[j].W + .2) - .4);

                            GL.Vertex2(t2[i].Y / (.2 * t2[i].Z + .2) * (.2 * t2[i].X + .2), t2[i].W / (.2 * t2[i].Z + .2) * (.2 * t2[i].X + .2) - .4);
                            GL.Vertex2(t2[j].Y / (.2 * t2[j].Z + .2) * (.2 * t2[j].X + .2), t2[j].W / (.2 * t2[j].Z + .2) * (.2 * t2[j].X + .2) - .4);

                            GL.Vertex2(t2[i].Z / (.2 * t2[i].Y + .2) * (.2 * t2[i].X + .2) + .7, t2[i].W / (.2 * t2[i].Y + .2) * (.2 * t2[i].X + .2) - .4);
                            GL.Vertex2(t2[j].Z / (.2 * t2[j].Y + .2) * (.2 * t2[j].X + .2) + .7, t2[j].W / (.2 * t2[j].Y + .2) * (.2 * t2[j].X + .2) - .4);
                            GL.End();
                        }
                    }


                }
            }

            a += .004;
            b += .005;
            c += .006;
            d += .007;
            rotorX = new Matrix4(1, 0, 0, 0,
                                 0, Convert.ToSingle(Math.Cos(a)), -Convert.ToSingle(Math.Sin(a)), 0,
                                 0, Convert.ToSingle(Math.Sin(a)), Convert.ToSingle(Math.Cos(a)), 0,
                                  0, 0, 0, 1);
            rotorZ = new Matrix4(Convert.ToSingle(Math.Cos(b)), -Convert.ToSingle(Math.Sin(b)), 0, 0,
                                Convert.ToSingle(Math.Sin(b)), Convert.ToSingle(Math.Cos(b)), 0, 0,
                                 0, 0, 1, 0,
                                 0, 0, 0, 1);

            rotorW = new Matrix4(1, 0, 0, 0,
                                0, 1, 0, 0,
                               0, 0, Convert.ToSingle(Math.Cos(c)), -Convert.ToSingle(Math.Sin(c)),
                               0, 0, Convert.ToSingle(Math.Sin(c)), Convert.ToSingle(Math.Cos(c)));

            rotorY = new Matrix4(Convert.ToSingle(Math.Cos(d)), 0, -Convert.ToSingle(Math.Sin(d)), 0,
                                 0, 1, 0, 0,
                                 Convert.ToSingle(Math.Sin(d)), 0, Convert.ToSingle(Math.Cos(d)), 0,
                                 0, 0, 0, 1);



            window.SwapBuffers();

           
        }

        private void winLoad(object sender, EventArgs e)
        {
            GL.Enable(OpenTK.Graphics.EnableCap.LineSmooth);
           
        }
    }
}
