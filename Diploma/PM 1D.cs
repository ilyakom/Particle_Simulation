using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// для работы с библиотекой OpenGL
using Tao.OpenGl;
// для работы с библиотекой FreeGLUT
using Tao.FreeGlut;
// для работы с элементом управления SimpleOpenGLControl
using Tao.Platform.Windows;


namespace Diploma
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
            graph1d.InitializeContexts();
        }
        Random rnd;
        //число разбиений отрезка, число частиц
        int NumOfDivs, NumOfParticles, Averaging, steps = 0;
        //длина разбиения
        double LengthOfDiv, TimeStep, ParticleMass = 0.04f, SpendedTime;
        //массив частиц, содержащий Х координату частицы
        List<double> Particles;
        double[] HeightLines, TmpParticles;

        // размеры окна 
        double ScreenW, ScreenH, HalfScreenW, HalfScreenH;        

        // отношения сторон окна визуализации
        // для корректного перевода координат мыши в координаты, 
        // принятые в программе 
        private float devX;
        private float devY;       

        // флаг, означающий, что массив с значениями координат графика пока еще не заполнен 
        private bool Initialized = false;
        
        // вспомогательные переменные для построения линий от курсора мыши к координатным осям 
        float lineX, lineY;
        // текущение координаты курсора мыши 
        float Mcoord_X = 0, Mcoord_Y = 0;
        //длинна локализации
        double Localize = 2 * Math.PI * Math.Sqrt(2 * 10);
        float Colapse = 1;

        private double func(double param)
        {
            return 0;
            //return Math.Sin(Math.PI * x / (ScreenW - 1)) - Math.Pow(t * Math.PI / (ScreenW - 1), 2) * Math.Cos(2 * x * Math.PI / (ScreenW - 1));
        }

        private double Sigma(double param)
        {

            return Math.Sqrt(param);
        }

        private double soluttion(double x, double t)
        {
           // return t * Math.Sin(x * Math.PI / (ScreenW - 1));
            if (Math.Abs(x) <= Localize / 2)
            {
                return (4 * Math.Pow(Math.Cos(Math.PI * x / Localize), 2)) / (3 * (Colapse - t));
            }
            else
                return 0;
        }

        private void differenceScheme()
        {

        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            // инициализация бибилиотеки glut 
            Glut.glutInit();
            // инициализация режима экрана 
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            // установка цвета очистки экрана (RGBA) 
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода 
            Gl.glViewport(0, 0, graph1d.Width, graph1d.Height);

            // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матрицы 
            Gl.glLoadIdentity();

            // определение параметров настройки проекции, в зависимости от размеров сторон элемента graph1d. 
            if ((float)graph1d.Width <= (float)graph1d.Height)
            {
                ScreenW = 30.0;
                ScreenH = 30.0 * (float)graph1d.Height / (float)graph1d.Width;                    
            }
            else
            {
                ScreenW = 30.0 * (float)graph1d.Width / (float)graph1d.Height;
                ScreenH = 30.0;              
            }
            HalfScreenH = ScreenH / 2;
            HalfScreenW = ScreenW / 2;
            Glu.gluOrtho2D(0.0, ScreenW, 0.0, ScreenH);
            // сохранение коэфицентов, которые нам необходимы для перевода координат указателя в оконной системе, в координаты 
            // принятые в нашей OpenGL сцене 
            devX = (float)ScreenW / (float)graph1d.Width;
            devY = (float)ScreenH / (float)graph1d.Height;

            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            // старт щетчика, отвечающего за выхов функции визуализации сцены 
            DrawTimer.Start();
        }

        private void initialization()
        {
            NumOfParticles = 0;
            rnd = new Random();            
            LengthOfDiv = (ScreenW - 1.0f) / NumOfDivs;
            Particles = new List<double>();
            HeightLines = new double[NumOfDivs];

            //double offset = 0.5f * (ScreenW - 1);
            //double StartLeight = 0.1f * (ScreenW - 1);
            //for (int i = 0; i < NumOfParticles; i++)
            //{
            //    RandomNum1 = rnd.NextDouble();
            //    RandomNum2 = rnd.NextDouble();
            //    while (RandomNum1 == 0)
            //        RandomNum1 = rnd.NextDouble();
            //    while (RandomNum2 == 0)
            //        RandomNum2 = rnd.NextDouble();               
            //    Particles.Add(offset + StartLeight * Math.Cos(2 * Math.PI * RandomNum1) * Math.Sqrt(-2 * Math.Log(RandomNum2)));
            //    HeightLines[(int)(Particles[i] / LengthOfDiv)] += ParticleMass;
            //}

            int NumofParticlesInCell;
            Random PosInCell = new Random();
            for (int i = 0; i < NumOfDivs; i++)
            {
                NumofParticlesInCell = (int)(soluttion((i - (int)(NumOfDivs / 2) + 0.5) * LengthOfDiv, 0) / ParticleMass);

                for (int k = 0; k < NumofParticlesInCell; k++)
                    Particles.Add(LengthOfDiv * (i + (float)PosInCell.NextDouble()));
                HeightLines[i] += ParticleMass * NumofParticlesInCell;
                NumOfParticles += NumofParticlesInCell;
            }
            textBox2.Text = NumOfParticles.ToString();
            TimeStep = 1f / (float)(NumOfParticles);
            textBox3.Text = TimeStep.ToString();
            Initialized = true;
        }

        private void simulation()
        {
            steps++;
            particlesAdd();
            double RandomNum1, RandomNum2;            
            TmpParticles = new double[Particles.Count];                         
            for (int j = 0; j < Averaging; j++)
            {                
                for (int i = 0; i < Particles.Count; i++)
                {
                    RandomNum1 = rnd.NextDouble();
                    RandomNum2 = rnd.NextDouble();
                    while (RandomNum1 == 0)
                        RandomNum1 = rnd.NextDouble();
                    while (RandomNum2 == 0)
                        RandomNum2 = rnd.NextDouble();
                    TmpParticles[i] += Math.Cos(2 * Math.PI * RandomNum1) * Math.Sqrt(-2 * Math.Log(RandomNum2) * TimeStep) 
                        * Sigma(HeightLines[(int)((Particles[i])/ LengthOfDiv)]);                                              
                }
            }

            Array.Clear(HeightLines, 0, HeightLines.Length);
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i] += TmpParticles[i] / Averaging;
                if (Particles[i] > ScreenW - 1 || Particles[i] < 1)
                {
                    Particles.RemoveAt(i);
                    i--;
                    continue;
                }
                else
                {
                    HeightLines[(int)((Particles[i]) / LengthOfDiv)] += ParticleMass;
                }
            }

            SpendedTime = TimeStep * (double)(steps);
            TimeStep = 1f / Math.Sqrt(NumOfParticles);
            steps = (int)(SpendedTime / TimeStep);
            textBox3.Text = TimeStep.ToString();
        }

        private void particlesAdd()
        {
            int NumofParticlesInCell;
            Random PosInCell = new Random();
            for (int i = 0; i < NumOfDivs; i++)                                                
            {
                NumofParticlesInCell = (int)(TimeStep * func(HeightLines[i]) / ParticleMass);
                
                for (int k = 0; k < NumofParticlesInCell; k++)
                    Particles.Add(LengthOfDiv * (i + (float)PosInCell.NextDouble()));
                NumOfParticles += NumofParticlesInCell;                
            }                
        }

        private void button1_Click(object sender, EventArgs e)
        {
            simulateTimer.Start();
            textBox2.Enabled = false;
        }

        private void PointInGrap_Tick(object sender, EventArgs e)
        {            
            // функция визуализации 
            Draw();           
        }

        private void simulateTimer_Tick(object sender, EventArgs e)
        {            
            simulation();
            textBox2.Text = NumOfParticles.ToString();
        }

        private void graph1d_MouseMove(object sender, MouseEventArgs e)
        {
            // созраняем координаты мыши 
            Mcoord_X = e.X;
            Mcoord_Y = e.Y;

            // вычисляем параметры для будующей дорисовке линий от указателя мыши к координатным осям. 
            lineX = devX * e.X;
            lineY = (float)(ScreenH - devY * e.Y);
        }

        private void PrintText2D(float x, float y, string text)
        {
            // устанавливаем позицию вывода растровых символов 
            // в переданных координатах x и y. 
            Gl.glRasterPos2f(x, y);

            // в цикле foreach перебираем значения из массива text, 
            // который содержит значение строки для визуализации 
            foreach (char char_for_draw in text)
            {
                // визуализируем символ c, с помощью функции glutBitmapCharacter, используя шрифт GLUT_BITMAP_9_BY_15. 
                Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
            }
        }        

        // визуализация графика 
        private void DrawDiagram()
        {            
            // стартуем отрисовку в режиме визуализации точек 
            // объединяемых в линии (GL_LINE_STRIP) 
            Gl.glBegin(Gl.GL_LINE_STRIP);            
           
            for (int ax = 0; ax < HeightLines.Length; ax += 1)
            {
                // передаем в OpenGL информацию о вершине, участвующей в построении линий 
                Gl.glVertex2d(ax * LengthOfDiv, HeightLines[ax]);
                Gl.glVertex2d((ax + 1) * LengthOfDiv, HeightLines[ax]);
            }
            
            Gl.glEnd();                                    
        }

        private void DrawSolution()
        {
            double SollutionDrawStep = 0.5;
            int ax = (int)(-HalfScreenW);

            Gl.glBegin(Gl.GL_LINE_STRIP);

            while (ax * SollutionDrawStep < ScreenW - 1)
            {
                // передаем в OpenGL информацию о вершине, участвующей в построении линий 
                Gl.glVertex2d(ax * SollutionDrawStep + HalfScreenW, soluttion(ax * SollutionDrawStep, TimeStep * steps));
                Gl.glVertex2d((ax + 1) * SollutionDrawStep + HalfScreenW, soluttion((ax + 1) * SollutionDrawStep, TimeStep * steps));
                ax ++;
            }

            Gl.glEnd();
        }

        // функция, управляющая визуализацией сцены 
        private void Draw()
        {
            // очистка буфера цвета и буфера глубины 
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            // очищение текущей матрицы 
            Gl.glLoadIdentity();

            // утснаовка черного цвета 
            Gl.glColor3f(0, 0, 0);

            // помещаем состояние матрицы в стек матриц 
            Gl.glPushMatrix();

            // выполняем перемещение в прострастве по осям X и Y 
            Gl.glTranslated(1, 1, 0);

            // активируем рижим рисования (Указанные далее точки будут выводиться как точки GL_POINTS) 
            Gl.glBegin(Gl.GL_POINTS);

            // с помощью прохода вдумя циклами, создаем сетку из точек 
            for (int ax = 0; ax < (int)ScreenW; ax++)
            {
                for (int bx = 0; bx < (int)ScreenH; bx++)
                {
                    // вывод точки 
                    Gl.glVertex2d(ax, bx);
                }
            }

            // завершение редима рисования примитивов 
            Gl.glEnd();


            // активируем режим рисования, каждые 2 последовательно вызванные комманды glVertex 
            // объединяются в линии 
            Gl.glBegin(Gl.GL_LINES);

            // далее мы рисуем координатные оси и стрекли на их концах 
            Gl.glVertex2d(0, -1);
            Gl.glVertex2d(0, ScreenH);

            Gl.glVertex2d(-1, 0);
            Gl.glVertex2d(ScreenW, 0);

            // вертикальная стрелка 
            Gl.glVertex2d(0, ScreenH - 1);
            Gl.glVertex2d(0.5, ScreenH - 1.5);
            Gl.glVertex2d(0, ScreenH - 1);
            Gl.glVertex2d(-0.5, ScreenH - 1.5);

            // горизонтальная трелка 
            Gl.glVertex2d(ScreenW - 1, 0);
            Gl.glVertex2d(ScreenW - 1.5, 0.5);
            Gl.glVertex2d(ScreenW - 1, 0);
            Gl.glVertex2d(ScreenW -1.5, -0.5);

            // завершаем режим рисования 
            Gl.glEnd();

            // выводим подписи осей "x" и "y" 
            PrintText2D((float)ScreenW - 1.5f, 1f, "x");
            PrintText2D(1f, (float)(ScreenH - 1.5), "y");

            if (Initialized)
            {
                // вызываем функцию рисования графика 
                DrawDiagram();
                DrawSolution();
            }

            // возвращаем матрицу из стека 
            Gl.glPopMatrix();

            // выводим текст со значением координат возле курсора 
            PrintText2D(devX * Mcoord_X + 0.2f, (float)ScreenH - devY * Mcoord_Y + 0.4f, "[ x: " + (devX * Mcoord_X - 1).ToString() + " ; y: " + ((float)ScreenH - devY * Mcoord_Y - 1).ToString() + "]");

            // устанавливаем красный цвет 
            Gl.glColor3f(255, 0, 0);

            // включаем режим рисования линий, для того чтобы нарисовать 
            // линии от курсора мыши к координатным осям 
            Gl.glBegin(Gl.GL_LINES);

            Gl.glVertex2d(lineX, 1);
            Gl.glVertex2d(lineX, lineY);
            Gl.glVertex2d(1, lineY);
            Gl.glVertex2d(lineX, lineY);

            Gl.glEnd();

            // дожидаемся завершения визуализации кадра 
            Gl.glFlush();

            // сигнал для обновление элемента реализующего визуализацию. 
            graph1d.Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    NumOfDivs = (int)Convert.ToInt32(textBox1.Text);
                    if (NumOfDivs <= 0)
                        throw (new FormatException());
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверно введено колличество разбиений!");
                textBox1.Text = "";
            }
            finally
            {
                if (textBox1.Text != "" || textBox2.Text != "" || textBox3.Text != "" || textBox4.Text != "")
                {
                    initialization();
                    if (textBox3.Text != "")
                        button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    Initialized = false;
                }
            }
        }             

        private void button2_Click(object sender, EventArgs e)
        {
            simulateTimer.Stop();
            textBox2.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text != "")
                {
                    Averaging = Convert.ToInt32(textBox4.Text);
                    if (Averaging <= 0)
                        throw (new FormatException());
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверно введен шаг по времени!");
                textBox4.Text = "";
            }
            finally
            {
                if (textBox1.Text != "" || textBox2.Text != "" || textBox3.Text != "" || textBox4.Text != "")
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    Initialized = false;
                }
            }
        }
        
    }
}
