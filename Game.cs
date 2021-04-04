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
        double XY, XZ, XW, YZ, YW, ZW = .3;

        Matrix4 rotorXY;
        Matrix4 rotorXZ;
        Matrix4 rotorXW;
        Matrix4 rotorYZ;
        Matrix4 rotorYW;
        Matrix4 rotorZW;
       
        Vector4[] t2;
        
        

        public Game(GameWindow window)
        {   
            this.window = window;
            window.Load += winLoad;
            window.RenderFrame += winRenderFrame;
            window.UpdateFrame += winUpdateFrame;
            tes = new Tesseract();
            rotorXY = new Matrix4(1, 0, 0, 0,
                                0, 1, 0, 0,
                               0, 0, Convert.ToSingle(Math.Cos(XY)), -Convert.ToSingle(Math.Sin(XY)),
                               0, 0, Convert.ToSingle(Math.Sin(XY)), Convert.ToSingle(Math.Cos(XY)));

            rotorXZ = new Matrix4(1, 0, 0, 0,
                              0, Convert.ToSingle(Math.Cos(XZ)), 0, -Convert.ToSingle(Math.Sin(XZ)),
                              0, 0, 1, 0,
                              0, Convert.ToSingle(Math.Sin(XZ)), 0, Convert.ToSingle(Math.Cos(XZ)));


            rotorXW = new Matrix4(1, 0, 0, 0,
                                0, Convert.ToSingle(Math.Cos(XW)), -Convert.ToSingle(Math.Sin(XW)), 0,
                                0, Convert.ToSingle(Math.Sin(XW)), Convert.ToSingle(Math.Cos(XW)), 0,
                                 0, 0, 0, 1);

            rotorYZ = new Matrix4
                                 (Convert.ToSingle(Math.Cos(YZ)), 0, 0, -Convert.ToSingle(Math.Sin(YZ)),
                                 0, 1, 0, 0,
                                 0, 0, 1, 0,
                                 Convert.ToSingle(Math.Sin(YZ)), 0, 0, Convert.ToSingle(Math.Cos(YZ))
                                 );

            rotorYW = new Matrix4(Convert.ToSingle(Math.Cos(YW)), 0, -Convert.ToSingle(Math.Sin(YW)), 0,
                                 0, 1, 0, 0,
                                 Convert.ToSingle(Math.Sin(YW)), 0, Convert.ToSingle(Math.Cos(YW)), 0,
                                 0, 0, 0, 1);


            rotorZW = new Matrix4(Convert.ToSingle(Math.Cos(ZW)), -Convert.ToSingle(Math.Sin(ZW)), 0, 0,
                                Convert.ToSingle(Math.Sin(ZW)), Convert.ToSingle(Math.Cos(ZW)), 0, 0,
                                 0, 0, 1, 0,
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
                var rxy = mult(tes.vertex[i], rotorXY);
                var rxz = mult(rxy, rotorXZ);
                var rxw = mult(rxz, rotorXW);
                var ryz = mult(rxw, rotorYZ);
                var ryw = mult(ryz, rotorYW);
                var rzw = mult(ryw, rotorZW);



                t2[i] = rzw; 
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

            XY += .01;
            XZ += .01;
            XW += .01;
            YZ += .01;
            YW += .01;
            ZW += .01;


            rotorXY = new Matrix4(1, 0, 0, 0,
                                0, 1, 0, 0,
                               0, 0, Convert.ToSingle(Math.Cos(XY)), -Convert.ToSingle(Math.Sin(XY)),
                               0, 0, Convert.ToSingle(Math.Sin(XY)), Convert.ToSingle(Math.Cos(XY)));

            rotorXZ = new Matrix4(1, 0, 0, 0,
                              0, Convert.ToSingle(Math.Cos(XZ)), 0, -Convert.ToSingle(Math.Sin(XZ)),
                              0, 0, 1, 0,
                              0, Convert.ToSingle(Math.Sin(XZ)), 0, Convert.ToSingle(Math.Cos(XZ)));


            rotorXW = new Matrix4(1, 0, 0, 0,
                                0, Convert.ToSingle(Math.Cos(XW)), -Convert.ToSingle(Math.Sin(XW)), 0,
                                0, Convert.ToSingle(Math.Sin(XW)), Convert.ToSingle(Math.Cos(XW)), 0,
                                 0, 0, 0, 1);

            rotorYZ = new Matrix4
                                 (Convert.ToSingle(Math.Cos(YZ)), 0, 0, -Convert.ToSingle(Math.Sin(YZ)),
                                 0, 1, 0, 0,
                                 0, 0, 1, 0,
                                 Convert.ToSingle(Math.Sin(YZ)), 0, 0, Convert.ToSingle(Math.Cos(YZ))
                                 );

            rotorYW = new Matrix4(Convert.ToSingle(Math.Cos(YW)), 0, -Convert.ToSingle(Math.Sin(YW)), 0,
                                 0, 1, 0, 0,
                                 Convert.ToSingle(Math.Sin(YW)), 0, Convert.ToSingle(Math.Cos(YW)), 0,
                                 0, 0, 0, 1);


            rotorZW = new Matrix4(Convert.ToSingle(Math.Cos(ZW)), -Convert.ToSingle(Math.Sin(ZW)), 0, 0,
                                Convert.ToSingle(Math.Sin(ZW)), Convert.ToSingle(Math.Cos(ZW)), 0, 0,
                                 0, 0, 1, 0,
                                 0, 0, 0, 1);




            window.SwapBuffers();

           
        }

        private void winLoad(object sender, EventArgs e)
        {
            GL.Enable(OpenTK.Graphics.EnableCap.LineSmooth);
           
        }
    }
}
