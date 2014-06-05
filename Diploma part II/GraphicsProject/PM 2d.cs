﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

using GLcamera;
using MatrixLib;

namespace GraphicsProject
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();           
        }
        //переменные для одномерно-локального разностного метода
        float[,] diffSchemSolution;
        float[] alfa, beta;
        float diffSchemStep;
        int diffSchemNumOfDivs;

        //переменные для метода частиц
        int nOfDivs, Averaging = 10;
        float rib, BigRibofSquare = 20, deltatime = 0.001f, ParticleMass = 0.01f, timeSteps = 30;
        public List<PointF> Particles = new List<PointF>();
        float[,] HeightOfCubes, tmpHeightOfCubes;
        Random rnd = new Random();

        //переменные для камеры
        Camera cam = new Camera();
        bool mouseRotate = false, mouseMove = false;
        float myMouseYcoord, myMouseXcoord, myMouseXcoordVar, myMouseYcoordVar, rot_cam_X = 0;

        public struct Point3D
        {
            public float x1, x2, U;

            public Point3D(float x, float y, float z)
            {
                x1 = x;
                x2 = y;
                U = z;
            }
        }

        private void InitGL()
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Gl.glClearColor(255, 255, 255, 1);

            Gl.glViewport(0, 0, simpleOpenGlControl1.Width, simpleOpenGlControl1.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluPerspective(45, (float)simpleOpenGlControl1.Width / (float)simpleOpenGlControl1.Height, 0.1, 200);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);

            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glLineWidth(1.0f);
            cam.Position_Camera(0, 6, -15, 0, 3, 0, 0, 1, 0);
        }

        private void Initialization()
        {
            

            Particles = new List<PointF>();
            tmpHeightOfCubes = new float[nOfDivs, nOfDivs];
            HeightOfCubes = new float[nOfDivs, nOfDivs];            

            int numofParticlesInCell;
            Random PosInCell = new Random();
            for (int i = 0; i < nOfDivs; i++)
                for (int j = 0; j < nOfDivs; j++)
                {
                    numofParticlesInCell = (int)(zeroTimeCondition((i + 0.5f) * rib, (j + 0.5f) * rib) / ParticleMass);
                    for (int k = 0; k < numofParticlesInCell; k++)
                        Particles.Add(new PointF(rib * (i + (float)PosInCell.NextDouble()), rib * (j + (float)PosInCell.NextDouble())));
                    HeightOfCubes[i,j] = ParticleMass * numofParticlesInCell;               
                }
            textBox3.Text = Particles.Count.ToString();
        }

        private void DrawGrid(int x, float quad_size)
        {
            float[] MatrixColorOX = { 1, 0, 0, 1 };
            float[] MatrixColorOY = { 0, 1, 0, 1 };
            float[] MatrixColorOZ = { 0, 0, 1, 1 };
            float[] MatrixOXOYColor = { 0.1f, 0.2f, 0.7f, 1 };
            //x - количество или длина сетки, quad_size - размер клетки
            Gl.glPushMatrix(); //Рисуем оси координат, цвет объявлен в самом начале
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixColorOX);
            Gl.glTranslated((-x * 2) / 2, 0, 0);
            Gl.glRotated(90, 0, 1, 0);
            Glut.glutSolidCylinder(0.02, x * 2, 12, 12);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixColorOZ);
            Gl.glTranslated(0, 0, (-x * 2) / 2);
            Glut.glutSolidCylinder(0.02, x * 2, 12, 12);
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixColorOY);
            Gl.glTranslated(0, x / 2, 0);
            Gl.glRotated(90, 1, 0, 0);
            Glut.glutSolidCylinder(0.02, x, 12, 12);
            Gl.glPopMatrix();

            Gl.glBegin(Gl.GL_LINES);

            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, MatrixOXOYColor);

            // Рисуем сетку 1х1 вдоль осей
            for (float i = -x; i <= x; i += 1)
            {
                Gl.glBegin(Gl.GL_LINES);
                // Ось Х
                Gl.glVertex3f(-x * quad_size, 0, i * quad_size);
                Gl.glVertex3f(x * quad_size, 0, i * quad_size);

                // Ось Z
                Gl.glVertex3f(i * quad_size, 0, -x * quad_size);
                Gl.glVertex3f(i * quad_size, 0, x * quad_size);
                Gl.glEnd();
            }
        }

        private void ParticleMove()
        {
            float alfa1, alfa2;            
            
            if (wholeWayAveragin.Checked)
            {
                List<PointF> SaveList = new List<PointF>();
                for (int g = 0; g < Particles.Count; g++)
                {
                    SaveList.Add(Particles[g]);
                }
                Array.Clear(HeightOfCubes, 0, HeightOfCubes.Length);

                for (int cnt = 0; cnt < Averaging; cnt++)
                {                    
                    Particles.Clear();
                    for (int g = 0; g < SaveList.Count; g++)
                    {
                        Particles.Add(SaveList[g]);
                    }
                    for (int i = 0; i < nOfDivs; i++)
                        for (int j = 0; j < nOfDivs; j++)                                                   
                            tmpHeightOfCubes[i, j] = zeroTimeCondition((i + 0.5f) * rib, (j + 0.5f) * rib) / ParticleMass;                             
                                       
                    for (int timeBegin = 0; timeBegin < timeSteps; timeBegin++)
                    {
                        float curCubeHeight, tmpX, tmpY;
                        for (int i = 0; i < Particles.Count; i++)
                        {
                            alfa1 = (float)rnd.NextDouble();
                            alfa2 = (float)rnd.NextDouble();
                            while (alfa1 == 0)
                                alfa1 = (float)rnd.NextDouble();
                            while (alfa2 == 0)
                                alfa2 = (float)rnd.NextDouble();
                            curCubeHeight = tmpHeightOfCubes[(int)Math.Floor((Particles[i].X) / rib), (int)Math.Floor((Particles[i].Y) / rib)];

                            tmpX = Particles[i].X + (float)(Math.Sin(2 * Math.PI * alfa1) * Math.Sqrt(-2 * Math.Log(alfa2))) * (float)Math.Sqrt(deltatime) *
                                sigma(curCubeHeight);
                            tmpY = Particles[i].Y + (float)(Math.Cos(2 * Math.PI * alfa1) * Math.Sqrt(-2 * Math.Log(alfa2))) * (float)Math.Sqrt(deltatime) * 
                                sigma(curCubeHeight);

                            if (tmpX <= BigRibofSquare && tmpY <= BigRibofSquare && tmpX >= 0 && tmpY >= 0)
                                Particles[i] = new PointF(tmpX, tmpY); 
                            else
                            {
                                Particles.RemoveAt(i);
                                i--;
                            }
                        }
                        Array.Clear(tmpHeightOfCubes, 0, tmpHeightOfCubes.Length);
                        for (int j = 0; j < Particles.Count; j++)
                        {                            
                            tmpHeightOfCubes[(int)Math.Floor(Particles[j].X / rib), (int)Math.Floor(Particles[j].Y / rib)] += ParticleMass;
                        }
                    }

                    for (int i = 0; i < nOfDivs; i++)
                        for (int j = 0; j < nOfDivs; j++)
                            HeightOfCubes[i, j] += tmpHeightOfCubes[i,j];                     
                }

                for (int i = 0; i < nOfDivs; i++)
                    for (int j = 0; j < nOfDivs; j++)
                        HeightOfCubes[i,j] /= Averaging;

                //float ParticleErr = 0;
                //textBox3.Text = NofPoints.ToString();
                //for (int i = 0; i < (int)(BigRibofSquare / rib) + 1; i++)
                //    for (int j = 0; j < (int)(BigRibofSquare / rib) + 1; j++)
                //    {
                //        HeightOfCubes[i, j] /= Averaging;
                //        ParticleErr += Math.Abs(f((i + 0.5f) * rib, (j + 0.5f) * rib) / 2 - HeightOfCubes[i, j]);

                //    }
                //textBox7.Text = (ParticleErr / NofCubes).ToString();
            }
            else if (everyStepAveraging.Checked)
            {
                float err = 0;
                List<PointF> tmpParticles = new List<PointF>();
                List<Point> errList = new List<Point>();
                float curCubeHeight, tmpX, tmpY;
                for (int timeBegin = 0; timeBegin < timeSteps; timeBegin++)
                {
                    Array.Clear(tmpHeightOfCubes, 0, tmpHeightOfCubes.Length);
                    tmpParticles.Clear();

                    for (int cnt = 0; cnt < Averaging; cnt++)
                    {                                                
                        for (int i = 0; i < Particles.Count; i++)
                        {
                            alfa1 = (float)rnd.NextDouble();
                            alfa2 = (float)rnd.NextDouble();
                            while (alfa1 == 0)
                                alfa1 = (float)rnd.NextDouble();
                            while (alfa2 == 0)
                                alfa2 = (float)rnd.NextDouble();
                            curCubeHeight = HeightOfCubes[(int)Math.Floor((Particles[i].X) / rib), (int)Math.Floor((Particles[i].Y) / rib)];

                            tmpX = Particles[i].X + (float)(Math.Sin(2 * Math.PI * alfa1) * Math.Sqrt(-2 * Math.Log(alfa2))) * (float)Math.Sqrt(deltatime) *
                                sigma(curCubeHeight);
                            tmpY = Particles[i].Y + (float)(Math.Cos(2 * Math.PI * alfa1) * Math.Sqrt(-2 * Math.Log(alfa2))) * (float)Math.Sqrt(deltatime) *
                                sigma(curCubeHeight);

                            if (tmpX <= BigRibofSquare && tmpY <= BigRibofSquare && tmpX >= 0 && tmpY >= 0)
                                tmpParticles.Add(new PointF(tmpX, tmpY));                           
                        }

                        for (int j = 0; j < Particles.Count; j++)
                        {
                            tmpHeightOfCubes[(int)Math.Floor(tmpParticles[j].X / rib), (int)Math.Floor(tmpParticles[j].Y / rib)] += ParticleMass;
                        }
                    }

                    int particlesToAdd;
                    Random PosInCell = new Random();
                    Particles.Clear();
                   
                    for (int i = 0; i < nOfDivs; i++)
                        for (int j = 0; j < nOfDivs; j++)
                        {

                            HeightOfCubes[i, j] = tmpHeightOfCubes[i, j] / Averaging;
                            
                            particlesToAdd = (int)Math.Floor(HeightOfCubes[i, j] / ParticleMass);
                            err += HeightOfCubes[i, j] % ParticleMass;
                            if ((int)(err / ParticleMass) == 0)
                                errList.Add(new Point(i, j));
                            else
                            {
                                errList.Add(new Point(i, j));
                                float x = 0, y = 0;
                                foreach (PointF p in errList)
                                {
                                    x += p.X;
                                    y += p.Y;
                                }

                                Particles.Add(new PointF(rib * (x / errList.Count), rib * (y / errList.Count)));
                                err -= ParticleMass;
                                errList.Clear();
                            }

                            for (int k = 0; k < particlesToAdd; k++)
                                Particles.Add(new PointF(rib * (i + (float)PosInCell.NextDouble()), rib * (j + (float)PosInCell.NextDouble())));
                        }                    
                }
                Console.WriteLine(err);
            }
            else if (parzenMethod.Checked)
            {                
                float curCubeHeight, tmpX, tmpY;
                for (int timeBegin = 0; timeBegin < timeSteps; timeBegin++)
                {                   
                    for (int i = 0; i < Particles.Count; i++)
                    {
                        alfa1 = (float)rnd.NextDouble();
                        alfa2 = (float)rnd.NextDouble();
                        while (alfa1 == 0)
                            alfa1 = (float)rnd.NextDouble();
                        while (alfa2 == 0)
                            alfa2 = (float)rnd.NextDouble();
                        curCubeHeight = HeightOfCubes[(int)Math.Floor((Particles[i].X) / rib), (int)Math.Floor((Particles[i].Y) / rib)];

                        tmpX = Particles[i].X + (float)(Math.Sin(2 * Math.PI * alfa1) * Math.Sqrt(-2 * Math.Log(alfa2))) * (float)Math.Sqrt(deltatime) *
                            sigma(curCubeHeight);
                        tmpY = Particles[i].Y + (float)(Math.Cos(2 * Math.PI * alfa1) * Math.Sqrt(-2 * Math.Log(alfa2))) * (float)Math.Sqrt(deltatime) *
                            sigma(curCubeHeight);

                        if (tmpX <= BigRibofSquare && tmpY <= BigRibofSquare && tmpX >= 0 && tmpY >= 0)
                            Particles[i] = new PointF(tmpX, tmpY);
                        else
                        {
                            Particles.RemoveAt(i);
                            i--;
                        }
                    }

                    for (int i = 0; i < nOfDivs; i++)
                        for (int j = 0; j < nOfDivs; j++)
                        {
                            HeightOfCubes[i, j] = (float)(density(new PointF((i + 0.5f) * rib, (j + 0.5f) * rib)) * Particles.Count * ParticleMass);
                        }
                }                
            }
            textBox3.Text = Particles.Count.ToString();
        }
       
        private void ParticleAdd()
        {                       
            int NofParticlesinCell;
            Random PosInCell = new Random();
            for (int i = 0; i < (int)(BigRibofSquare / rib); i++)
                for (int j = 0; j < (int)(BigRibofSquare / rib); j++)
                {
                    if (!(i * rib >= BigRibofSquare && j * rib >= BigRibofSquare))
                    {
                        NofParticlesinCell = (int)((deltatime * f((i + 0.5f) * rib, (j + 0.5f) * rib)) / ParticleMass);                        
                        for (int k = 0; k < NofParticlesinCell; k++)
                            Particles.Add(new PointF(rib * (i + (float)PosInCell.NextDouble()), rib * (j + (float)PosInCell.NextDouble())));                       
                    }
                }

        }

        private double density(PointF param)
        {
            double windowWidth = rib;
            double sum = 0;           
            for (int i = 0; i < Particles.Count; i++)
            {
                sum += (Math.Sqrt(1 / (2 * Math.PI)) * Math.Exp(-Math.Pow((param.X - Particles[i].X) / windowWidth, 2) / 2) / windowWidth) *
                    (Math.Sqrt(1 / (2 * Math.PI)) * Math.Exp(-Math.Pow((param.Y - Particles[i].Y) / windowWidth, 2) / 2) / windowWidth);
            }
            
            return sum / Particles.Count;
        }

        private float zeroTimeCondition(float x1, float x2)
        {
            if (x1 >= 7.5 && x1 <= 12.5 && x2 >= 7.5 && x2 <= 12.5)
            {
                if (x1 < x2)
                    return (x2 + x1) / (2f * x2 - x1);
                else
                    return (x1 + x2) / (2f * x1 - x2);                
            }
            else
                return 0;                
        }

        private float sigma(float param)
        {
            return (float)Math.Sqrt(param);
        }        

        //правая часть уравнения для метода частиц специально
        private float f(float x1, float x2)
        {
            //return (float)(2 * Math.Sin(2 * x1) * Math.Sin(2 * x2));
            return (float)(Math.Sin(x1 * Math.PI / BigRibofSquare) * Math.Sin(x2 * Math.PI / BigRibofSquare)) * 2;            
        }


        //правая часть для прогонки и релаксации
        private float f1(float x1, float x2)
        {
            //return (float)(2 * Math.Sin(2 * x1) * Math.Sin(2 * x2));
            return (float)(Math.Sin(x1) * Math.Sin(x2)) * 2;      
        }        

        private void simpleOpenGlControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mouseRotate = true; //Если нажата левая кнопка мыши

            if (e.Button == MouseButtons.Right)
                mouseMove = true; //Если нажата средняя кнопка мыши

            myMouseYcoord = e.X; //Передаем в нашу глобальную переменную позицию мыши по Х
            myMouseXcoord = e.Y;
        }

        private void simpleOpenGlControl1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseRotate = false;
            mouseMove = false;
        }

        private void simpleOpenGlControl1_MouseMove(object sender, MouseEventArgs e)
        {
            myMouseXcoordVar = e.Y;
            myMouseYcoordVar = e.X;
        }

        private void mouse_Events()
        {
            if (mouseRotate == true) //Если нажата левая кнопка мыши
            {
                simpleOpenGlControl1.Cursor = System.Windows.Forms.Cursors.SizeAll; //меняем указатель

                cam.Rotate_Position((float)(myMouseYcoordVar - myMouseYcoord), 0, 1, 0); //крутим камеру, в моем случае это от 3го лица

                rot_cam_X = rot_cam_X + (myMouseXcoordVar - myMouseXcoord);
                if ((rot_cam_X > -40) && (rot_cam_X < 40))
                    cam.upDown(((float)(myMouseXcoordVar - myMouseXcoord)) / 10);

                myMouseYcoord = myMouseYcoordVar;
                myMouseXcoord = myMouseXcoordVar;
            }
            else
            {
                if (mouseMove == true)
                {
                    simpleOpenGlControl1.Cursor = System.Windows.Forms.Cursors.SizeAll;

                    cam.Move_Camera((float)(myMouseXcoordVar - myMouseXcoord) / 50);
                    cam.Strafe(-((float)(myMouseYcoordVar - myMouseYcoord) / 50));

                    myMouseYcoord = myMouseYcoordVar;
                    myMouseXcoord = myMouseXcoordVar;

                }
                else
                {
                    simpleOpenGlControl1.Cursor = System.Windows.Forms.Cursors.Default;//возвращаем курсор
                };
            };
        }

        private void Draw()
        {                        
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glLoadIdentity();
            Gl.glColor3f(1.0f, 0, 0);

            cam.Look();
            Gl.glPushMatrix();
            DrawGrid(20, 1);
            for (int i = 0; i < nOfDivs; i++)
            {
                for (int j = 0; j < nOfDivs; j++)
                {
                    Gl.glPushMatrix();
                    Gl.glRotated(-90, 1, 0, 0);
                    Gl.glRotated(90, 0, 0, 1);
                    Gl.glTranslated((i + 0.5)* rib, (j + 0.5) * rib, 0);
                    Glut.glutSolidCylinder(rib / 2, HeightOfCubes[i, j], 16, 16);
                    Gl.glPopMatrix();
                }
            }
            Gl.glPopMatrix();
            Gl.glFlush();
            simpleOpenGlControl1.Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                    deltatime = (float)Convert.ToDouble(textBox1.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверно введен Шаг По Времени!");
            }
        }        

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text != "")
                    timeSteps = Convert.ToInt32(textBox4.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверно введено Кол-во шагов по времени");
                textBox4.Text = "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mouse_Events();
            cam.update();
            Draw();                 
        }

        private void timer2_Tick(object sender, EventArgs e)
        {          
            ParticleAdd();            
            ParticleMove();          
        }

        private void Form3_Load(object sender, EventArgs e)
        {                                                        
            InitGL();
        }

        private void button1_Click(object sender, EventArgs e)
        {                    
            Initialization();          
            ParticleMove();
            //MatrixSweep();
            //UpperRelaxation();

            timer1.Start();
            //timer2.Start();       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            timer1.Stop();                       
            Particles.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            Close();            
        }    

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Averaging = (int)numericUpDown1.Value;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    nOfDivs = Convert.ToInt32(textBox2.Text);
                    rib = BigRibofSquare / nOfDivs;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверно введено разбиение!");
            }
        }       
    }
}
