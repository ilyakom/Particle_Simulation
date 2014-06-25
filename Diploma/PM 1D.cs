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
            rnd = new Random();           
        }
        Random rnd;       
        SystemModel model = SystemModel.getInstance();

        //число разбиений отрезка, число частиц
        int numOfDivs, Averaging = 1, steps = 1;
        //длина разбиения
        double LengthOfDiv, TimeStep, ParticleMass = 0.02f;
        //массив частиц, содержащий Х координату частицы
        List<double> Particles, TmpParticles;
        double[] HeightLines, tmpHeightLines;

        //метод прогонки
        double[] alfa, beta, sweepSolution;
        int sweepDemention;
        double sweepStep;

        // размеры окна 
        double ScreenW, ScreenH, HalfScreenW, HalfScreenH;        

        // отношения сторон окна визуализации
        // для корректного перевода координат мыши в координаты, 
        // принятые в программе 
        private float devX;
        private float devY;       

        // флаг, означающий, что массив с значениями координат графика пока еще не заполнен 
        private bool Initialized = false;

        public List<Error> errorList = new List<Error>();
        
        // вспомогательные переменные для построения линий от курсора мыши к координатным осям 
        float lineX, lineY;
        // текущение координаты курсора мыши 
        float Mcoord_X = 0, Mcoord_Y = 0;
        //длинна локализации
        double Localize = 2 * Math.PI * Math.Sqrt(2 * 10);
        float Colapse = 1;
        double windowWidth = 0.3;

        private double density(double param)
        {
            double sum = 0;
            for (int i = 0; i < Particles.Count; i++)
            {
                sum += Math.Sqrt(1 / (2 * Math.PI)) * Math.Exp(-Math.Pow((param - Particles[i]) / LengthOfDiv, 2) / 2);
            }
            return sum / (Particles.Count * LengthOfDiv);
        }

        private double func(double param)
        {
            return 0;
            return param;
            //return Math.Sin(Math.PI * x / (ScreenW - 1)) - Math.Pow(t * Math.PI / (ScreenW - 1), 2) * Math.Cos(2 * x * Math.PI / (ScreenW - 1));
        }

        private double Sigma(double param)
        {         
            return Math.Sqrt(param);
        }

        private double soluttion(double x, double t)
        {
            //if (Math.Abs(x) <= Localize / 2)
            //{
            //    return (4 * Math.Pow(Math.Cos(Math.PI * x / Localize), 2)) / (3 * (Colapse - t));
            //}
            //else
            //    return 0;

            if (x > -10 && x <= 0)
                return 10 + x;
            else if (x > 0 && x < 10)
                return 10 - x;
            else
                return 0;
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
            model.xOffset = HalfScreenW;
            model.yOffset = 1;            

            Glu.gluOrtho2D(0.0, ScreenW, 0.0, ScreenH);
            // сохранение коэфицентов, которые нам необходимы для перевода координат указателя в оконной системе, в координаты 
            // принятые в нашей OpenGL сцене 
            devX = (float)ScreenW / (float)graph1d.Width;
            devY = (float)ScreenH / (float)graph1d.Height;

            // установка объектно-видовой матрицы 
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            sweepDementionTextBox.Text = "100";                   
        }

        private double A (int i)
        {
            if (i == sweepDemention - 1)
                return 0;
            else
                return  (sweepSolution[i] + sweepSolution[i - 1]) / 2;
        }

        private double B(int i)
        {
            if (i == 0)
                return 0;
            else
                return (sweepSolution[i] + sweepSolution[i + 1]) / 2;
        }

        private double C(int i)
        {

            if (i == sweepDemention - 1 || i == 0)
                return 1;
            else
                return A(i) + B(i) + sweepStep * sweepStep / TimeStep;
        }

        private double F(int i)
        {
            return sweepStep * sweepStep * (sweepSolution[i] / TimeStep + func(sweepSolution[i])) ;
        }

        private void differenceScheme()
        {
            for (int i = 1; i < sweepDemention - 1; i++)
            {
                alfa[i + 1] =  B(i) / (C(i) - A(i) * alfa[i]);
                beta[i + 1] = (F(i) + A(i) * beta[i]) / (C(i) - A(i) * alfa[i]);
            }

            //sweepSolution[sweepDemention - 1] = (F(sweepDemention - 1) + A(sweepDemention - 1) * beta[sweepDemention - 1]) / (C(sweepDemention - 1) - A(sweepDemention - 1) * alfa[sweepDemention - 1]);           
            
            for (int i = sweepDemention - 2; i >= 0; i--)
            {
                sweepSolution[i] = alfa[i + 1] * sweepSolution[i + 1] + beta[i + 1];
            }
        }

        private void initialization()
        {                                  
            LengthOfDiv = ScreenW / numOfDivs;
            Particles = new List<double>();
            HeightLines = new double[numOfDivs];
            tmpHeightLines = new double[numOfDivs];
            TmpParticles = new List<double>();
            alfa = new double[sweepDemention];
            beta = new double[sweepDemention];
            sweepSolution = new double[sweepDemention + 1];            

            for (int i = 1; i < sweepDemention - 1; i++)
            {
                sweepSolution[i] = soluttion(i * sweepStep - model.xOffset, 0);
            }

            int numofParticlesInCell;
            Random PosInCell = new Random();
            for (int i = 0; i < numOfDivs; i++)
            {
                numofParticlesInCell = (int)(soluttion((i + 0.5) * LengthOfDiv - model.xOffset, 0) / ParticleMass);              
                for (int k = 0; k < numofParticlesInCell; k++)
                    Particles.Add(LengthOfDiv * (i + (float)PosInCell.NextDouble()) - model.xOffset);
                HeightLines[i] += ParticleMass * numofParticlesInCell;               
            }


            textBox2.Text = Particles.Count.ToString();
            TimeStep = 1f / (float)Math.Sqrt(Particles.Count);
            textBox3.Text = TimeStep.ToString();
            Initialized = true;            
        }

        private void simulation()
        {      
            double RandomNum1, RandomNum2;
            double error = 0;
            double missedParticleCoordinate = 0;
            steps++;
            Console.WriteLine("Step №" + steps.ToString());
            differenceScheme();
            if (everyStepAveraging.Checked)
            {
                particlesAdd();
                Array.Clear(HeightLines, 0, HeightLines.Length);
                for (int i = 0; i < Particles.Count; i++)
                    if (Particles[i] < HalfScreenW && Particles[i] > -HalfScreenW)
                        HeightLines[(int)Math.Floor((Particles[i] + HalfScreenW) / LengthOfDiv)] += ParticleMass;

                Array.Clear(tmpHeightLines, 0, tmpHeightLines.Length);
                for (int j = 0; j < Averaging; j++)
                {
                    TmpParticles.Clear(); 
                    for (int i = 0; i < Particles.Count; i++)
                    {
                        RandomNum1 = rnd.NextDouble();
                        RandomNum2 = rnd.NextDouble();
                        while (RandomNum1 == 0)
                            RandomNum1 = rnd.NextDouble();
                        while (RandomNum2 == 0)
                            RandomNum2 = rnd.NextDouble();  
                     
                        TmpParticles.Add(Particles[i] + Math.Cos(2 * Math.PI * RandomNum1) * Math.Sqrt(-2 * Math.Log(RandomNum2) * TimeStep)
                            * Sigma(HeightLines[(int)Math.Floor((Particles[i] + HalfScreenW) / LengthOfDiv)]));
                    }

                    for (int i = 0; i < TmpParticles.Count; i++)
                    {
                        if (TmpParticles[i] < HalfScreenW && TmpParticles[i] > -HalfScreenW)
                            tmpHeightLines[(int)Math.Floor((TmpParticles[i] + HalfScreenW) / LengthOfDiv)] += ParticleMass;                                                                
                    }                    
                }

                for (int i = 0; i < HeightLines.Length; i++)
                {                   
                        HeightLines[i] = tmpHeightLines[i] / Averaging;
                }

                Random PosInCell = new Random();
                Particles.Clear();
                
                errorList.Clear();
                for (int i = 0; i < numOfDivs; i++)
                {
                    int numofParticlesInCell = (int)Math.Floor(HeightLines[i] / ParticleMass);                                    
                    if (HeightLines[i] % ParticleMass > 0)
                    {                       
                        errorList.Add(new Error() {value=HeightLines[i] % ParticleMass, num = i});                      
                    }
                    for (int k = 0; k < numofParticlesInCell; k++)
                        Particles.Add(LengthOfDiv * (i + (float)PosInCell.NextDouble()) - model.xOffset);                    
                }
                                        
                //Воостановление потерянных частиц, которые появляются в результате
                //восстановления плотности после усреднения. Координата вычисляется как
                //сумма координат столбиков гастограмм, на которых была обнаружена потеря,
                //умноженная на величину потери(своеобразное весовое значение)
                for (int i = 0; i < errorList.Count; i++)
                {
                    error += errorList[i].value;
                    missedParticleCoordinate += errorList[i].value * (LengthOfDiv * errorList[i].num - model.xOffset);
                    if (error >= ParticleMass)
                    {
                        missedParticleCoordinate -= (error - ParticleMass) * (LengthOfDiv * errorList[i].num - model.xOffset);
                        Particles.Add(missedParticleCoordinate);
                        error -= ParticleMass;
                        missedParticleCoordinate = 0;
                    }
                }

                error = Math.Round(error, 2);
                if (error >= ParticleMass)
                    Particles.Add(missedParticleCoordinate);

                textBox2.Text = Particles.Count.ToString();

                //SpendedTime = TimeStep * (double)(steps);
                //TimeStep = 1f / (float)Math.Sqrt(Particles.Count);
                //steps = (int)(SpendedTime / TimeStep);
                //textBox3.Text = TimeStep.ToString();
            }
            else if (wholeWayAveraging.Checked)
            {
                Array.Clear(HeightLines, 0, HeightLines.Length);                
                for (int j = 0; j < Averaging; j++)
                {
                    TmpParticles.Clear();
                    for (int k = 0; k < Particles.Count; k++)
                    {
                        TmpParticles.Add(Particles[k]);
                    }
                   
                    for (int k = 0; k < steps; k++)
                    {
                        particlesAdd(TmpParticles);

                        Array.Clear(tmpHeightLines, 0, tmpHeightLines.Length);
                        for (int i = 0; i < TmpParticles.Count; i++)
                            if (TmpParticles[i] < HalfScreenW && TmpParticles[i] > -HalfScreenW)
                                tmpHeightLines[(int)Math.Floor((TmpParticles[i] + HalfScreenW) / LengthOfDiv)] += ParticleMass;

                        for (int i = 0; i < TmpParticles.Count; i++)
                        {
                            RandomNum1 = rnd.NextDouble();
                            RandomNum2 = rnd.NextDouble();
                            while (RandomNum1 == 0)
                                RandomNum1 = rnd.NextDouble();
                            while (RandomNum2 == 0)
                                RandomNum2 = rnd.NextDouble();
                            if (TmpParticles[i] < HalfScreenW && TmpParticles[i] > -HalfScreenW)
                                TmpParticles[i] += Math.Cos(2 * Math.PI * RandomNum1) * Math.Sqrt(-2 * Math.Log(RandomNum2) * TimeStep)
                                    * Sigma(tmpHeightLines[(int)Math.Floor((TmpParticles[i] + HalfScreenW) / LengthOfDiv)]);                            
                        }

                        Array.Clear(tmpHeightLines, 0, tmpHeightLines.Length);
                        for (int i = 0; i < TmpParticles.Count; i++)
                            if (TmpParticles[i] < HalfScreenW && TmpParticles[i] > -HalfScreenW)
                                tmpHeightLines[(int)Math.Floor((TmpParticles[i] + HalfScreenW) / LengthOfDiv)] += ParticleMass;                                   
                    }
                    textBox2.Text = TmpParticles.Count.ToString();
                    for (int i = 0; i < HeightLines.Length; i++)
                        HeightLines[i] += tmpHeightLines[i];
                }
                                                               
                for (int j = 0; j < HeightLines.Length; j++)
                {
                    HeightLines[j] /= Averaging;
                }
                                
                //SpendedTime = TimeStep * (double)(steps);
                //TimeStep = 1f / (float)(NumOfParticles);
                //steps = (int)(SpendedTime / TimeStep);
                //textBox3.Text = TimeStep.ToString();              
            }
            else if (parzenMethod.Checked)
            {
                particlesAdd();
                TmpParticles.Clear();
                for (int i = 0; i < Particles.Count; i++)
                {
                    RandomNum1 = rnd.NextDouble();
                    RandomNum2 = rnd.NextDouble();
                    while (RandomNum1 == 0)
                        RandomNum1 = rnd.NextDouble();
                    while (RandomNum2 == 0)
                        RandomNum2 = rnd.NextDouble();
                    TmpParticles.Add(Math.Cos(2 * Math.PI * RandomNum1) * Math.Sqrt(-2 * Math.Log(RandomNum2) * TimeStep)
                        * Sigma(density(Particles[i]) * Particles.Count * ParticleMass));
                }                            
                Array.Clear(HeightLines, 0, HeightLines.Length);
                for (int i = 0; i < Particles.Count; i++)
                {
                    Particles[i] += TmpParticles[i];
                    if (Particles[i] > HalfScreenW || Particles[i] < -HalfScreenW)
                    {
                        Particles.RemoveAt(i);
                        TmpParticles.RemoveAt(i);
                        i--;                        
                    }
                }
                for(int i = 0; i < HeightLines.Length; i++)
                {
                    HeightLines[i] = density((i + 0.5) * LengthOfDiv - HalfScreenW) * Particles.Count * ParticleMass;
                }
                                            
            }     
        }

        private void particlesAdd(List<double> arr = null)
        {
            int NumofParticlesInCell;
            Random PosInCell = new Random();
            if (arr == null)
                for (int i = 0; i < numOfDivs; i++)                                                
                {
                    NumofParticlesInCell = (int)(TimeStep * func(HeightLines[i]) / ParticleMass);
                
                    for (int k = 0; k < NumofParticlesInCell; k++)
                        Particles.Add(LengthOfDiv * (i + (float)PosInCell.NextDouble()) - model.xOffset);                               
                }                
            else
                for (int i = 0; i < numOfDivs; i++)
                {
                    NumofParticlesInCell = (int)(TimeStep * func(tmpHeightLines[i]) / ParticleMass);

                    for (int k = 0; k < NumofParticlesInCell; k++)
                        arr.Add(LengthOfDiv * (i + (float)PosInCell.NextDouble()) - model.xOffset);
                }              
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            simulateTimer.Start();            
            button1.Enabled = false;
        }

        private void simulateTimer_Tick(object sender, EventArgs e)
        {
            Draw();
            simulation();           
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
                Gl.glVertex2d(ax * LengthOfDiv - model.xOffset, HeightLines[ax]);
                Gl.glVertex2d((ax + 1) * LengthOfDiv - model.xOffset, HeightLines[ax]);
            }
            
            Gl.glEnd();                                    
        }

        private void DrawSolution()
        {
            double SollutionDrawStep = 0.5;
            int ax = (int)0;

            Gl.glBegin(Gl.GL_LINE_STRIP);
            while (ax * SollutionDrawStep < ScreenW)
            {
                // передаем в OpenGL информацию о вершине, участвующей в построении линий 
                Gl.glVertex2d(ax * SollutionDrawStep - model.xOffset, 
                    soluttion(ax * SollutionDrawStep - model.xOffset, TimeStep * steps));
                Gl.glVertex2d((ax + 1) * SollutionDrawStep - model.xOffset, 
                    soluttion((ax + 1) * SollutionDrawStep - model.xOffset, TimeStep * steps));
                ax ++;
            }

            Gl.glEnd();
        }

        private void DrawDensity()
        {
            int ax = 0;
            Gl.glColor3f(0, 0, 1);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            while (ax * 0.5 < ScreenW)
            {
                Gl.glVertex2d(ax * 0.5 - model.xOffset, density(ax * 0.5 - model.xOffset) * Particles.Count * ParticleMass);
                Gl.glVertex2d((ax + 1) * 0.5 - model.xOffset, density((ax + 1) * 0.5 - model.xOffset) * Particles.Count * ParticleMass);
                ax++;
            }
            Gl.glEnd();
            Gl.glColor3f(0, 0, 0);
        }

        private void DrawDifferenceScheme()
        {
            int ax = 0;
            Gl.glColor3f(1, 0, 0);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            while (ax < sweepDemention - 1)
            {
                Gl.glVertex2d(ax * sweepStep - model.xOffset, sweepSolution[ax]);
                Gl.glVertex2d((ax + 1) * sweepStep - model.xOffset, sweepSolution[ax + 1]);
                ax++;
            }
            Gl.glEnd();
            Gl.glColor3f(0, 0, 0);
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
            Gl.glTranslated(model.xOffset, model.yOffset, 0);

            // активируем рижим рисования (Указанные далее точки будут выводиться как точки GL_POINTS) 
            Gl.glBegin(Gl.GL_POINTS);

            // с помощью прохода вдумя циклами, создаем сетку из точек 
            for (int ax = -(int)model.xOffset; ax < (int)model.xOffset + 1; ax++)
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
            Gl.glVertex2d(0, -model.yOffset);
            Gl.glVertex2d(0, ScreenH);

            Gl.glVertex2d(-model.xOffset, 0);
            Gl.glVertex2d(model.xOffset, 0);

            // вертикальная стрелка 
            Gl.glVertex2d(0, ScreenH - model.yOffset);
            Gl.glVertex2d(0.5, ScreenH - 0.5 - model.yOffset);
            Gl.glVertex2d(0, ScreenH - model.yOffset);
            Gl.glVertex2d(-0.5, ScreenH - 0.5 - model.yOffset);

            // горизонтальная трелка 
            Gl.glVertex2d(model.xOffset, 0);
            Gl.glVertex2d(model.xOffset - 0.5, 0.5);
            Gl.glVertex2d(model.xOffset, 0);
            Gl.glVertex2d(model.xOffset - 0.5, -0.5);

            // завершаем режим рисования 
            Gl.glEnd();

            // выводим подписи осей "x" и "y" 
            PrintText2D((float)(ScreenW - model.xOffset - 1.3f), -1f, "x");
            PrintText2D(1f, (float)(ScreenH - 0.5 - model.yOffset), "y");

            if (Initialized)
            {
                // вызываем функцию рисования графика 
                DrawDiagram();
                //рисование кривой метода парзена-розенблатта                
                //DrawDensity();
                //DrawSolution();
                DrawDifferenceScheme();
            }

            // возвращаем матрицу из стека 
            Gl.glPopMatrix();

            // выводим текст со значением координат возле курсора 
            //PrintText2D(devX * Mcoord_X + 0.2f, (float)ScreenH - devY * Mcoord_Y + 0.4f,
            //    "[ x: " + (devX * Mcoord_X - model.xOffset).ToString() + " ; y: " + ((float)ScreenH - devY * Mcoord_Y - model.yOffset).ToString() + "]");

            // устанавливаем красный цвет 
            Gl.glColor3f(255, 0, 0);

            // включаем режим рисования линий, для того чтобы нарисовать 
            // линии от курсора мыши к координатным осям 
            //Gl.glBegin(Gl.GL_LINES);

            //Gl.glVertex2d(lineX, model.yOffset);
            //Gl.glVertex2d(lineX, lineY);
            //Gl.glVertex2d(model.xOffset, lineY);
            //Gl.glVertex2d(lineX, lineY);

            //Gl.glEnd();

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
                    numOfDivs = (int)Convert.ToInt32(textBox1.Text);
                    if (numOfDivs <= 0)
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
            button1.Enabled = true;
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
                MessageBox.Show("Неверно введено колличество испытаний!");
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != "")
                {
                    TimeStep = Convert.ToDouble(textBox3.Text);
                    if (TimeStep <= 0)
                        throw (new FormatException());
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверно введен шажок по времени!");
                textBox3.Text = "";
            }            
        }       

        private void sweepDemention_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sweepDementionTextBox.Text != "")
                {
                    sweepDemention = Convert.ToInt32(sweepDementionTextBox.Text);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Неверная размерность сетки для метода прогонки!");
                sweepDementionTextBox.Text = "";
            }
            finally
            {
                if (sweepDementionTextBox.Text != "")
                {
                    sweepStep = ScreenW / sweepDemention;
                    sweepStepTextBox.Text = sweepStep.ToString();
                }
                else
                {
                    sweepStepTextBox.Text = "";
                }
            }
        }               
    }

    public class Error
    {
        public double value { get; set; }
        public int num { get; set; }
    }
}
