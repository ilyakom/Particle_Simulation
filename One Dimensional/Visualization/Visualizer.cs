using System;
using System.Collections.Generic;
using Diploma.NumericalMethods;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace Diploma.Visualization
{
	internal static class Visualizer
	{
		private static readonly VisualizationModel Model = VisualizationModel.GetInstance();
		private static readonly ViewModel ViewModel = ViewModel.GetInstance();
		private static bool _initialized;
		private static SimpleOpenGlControl _openGlControl;

		/// <summary>
		/// Конструктор
		/// </summary>
		public static void Init(SimpleOpenGlControl control)
		{
			_openGlControl = control ?? throw new ArgumentNullException(nameof(control));
 
			Glut.glutInit();
			Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

			// установка цвета очистки экрана (RGBA) 
			Gl.glClearColor(255, 255, 255, 1);

			// установка порта вывода 
			Gl.glViewport(0, 0, control.Width, control.Height);

			// активация проекционной матрицы 
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			// очистка матрицы 
			Gl.glLoadIdentity();

			// определение параметров настройки проекции, в зависимости от размеров сторон элемента graph1d. 
			if (control.Width <= control.Height)
			{
				Model.ProjectionWidth = 30;
				Model.ProjectionHeight = 30 * (float)control.Height / control.Width;
			}
			else
			{
				Model.ProjectionWidth = 30 * (float)control.Width / control.Height;
				Model.ProjectionHeight = 30;
			}

			Model.XOffset = 1;
			Model.YOffset = 1;

			Glu.gluOrtho2D(0.0, Model.ProjectionWidth, 0.0, Model.ProjectionHeight);

			// установка объектно-видовой матрицы 
			Gl.glMatrixMode(Gl.GL_MODELVIEW);

			_initialized = true;
		}

		/// <summary>
		/// Функция, управляющая визуализацией сцены 
		/// </summary>
		public static void Draw( float[] histHeights, Func<float, float> densityFunction, float[] sweepSolution)
		{
			if (!_initialized) throw new Exception("Not initialized vizulizer");

			// очистка буфера цвета и буфера глубины 
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

			// очищение текущей матрицы 
			Gl.glLoadIdentity();

			// утснаовка черного цвета 
			Gl.glColor3f(0, 0, 0);

			// помещаем состояние матрицы в стек матриц 
			Gl.glPushMatrix();

			// выполняем перемещение в прострастве по осям X и Y 
			Gl.glTranslated(Model.XOffset, Model.YOffset, 0);

			// активируем рижим рисования (Указанные далее точки будут выводиться как точки GL_POINTS) 
			Gl.glBegin(Gl.GL_POINTS);

			// с помощью прохода вдумя циклами, создаем сетку из точек 
			for (var ax = -(int)Model.XOffset; ax < (int)Model.ProjectionWidth - Model.XOffset; ax++)
				for (var bx = -(int)Model.YOffset; bx < (int)Model.ProjectionHeight - Model.YOffset; bx++)
					Gl.glVertex2d(ax, bx);

			// завершение редима рисования примитивов 
			Gl.glEnd();

			// активируем режим рисования, каждые 2 последовательно вызванные комманды glVertex объединяются в линии 
			Gl.glBegin(Gl.GL_LINES);

			// рисуем координатные оси и стрекли на их концах 
			Gl.glVertex2d(0, -Model.YOffset);
			Gl.glVertex2d(0, Model.ProjectionHeight);

			Gl.glVertex2d( -Model.XOffset, 0);
			Gl.glVertex2d( Model.ProjectionWidth - Model.XOffset, 0);

			// вертикальная стрелка 
			Gl.glVertex2d(0, Model.ProjectionHeight - Model.YOffset);
			Gl.glVertex2d(0.5, Model.ProjectionHeight - 0.5 - Model.YOffset);
			Gl.glVertex2d(0, Model.ProjectionHeight - Model.YOffset);
			Gl.glVertex2d(-0.5, Model.ProjectionHeight - 0.5 - Model.YOffset);

			// горизонтальная трелка 
			Gl.glVertex2d(Model.ProjectionWidth - Model.XOffset, 0);
			Gl.glVertex2d(Model.ProjectionWidth - Model.XOffset - 0.5, 0.5);
			Gl.glVertex2d(Model.ProjectionWidth - Model.XOffset, 0);
			Gl.glVertex2d(Model.ProjectionWidth - Model.XOffset - 0.5, -0.5);

			// завершаем режим рисования 
			Gl.glEnd();

			// выводим подписи осей "x" и "y" 
			PrintText2D(Model.ProjectionWidth - Model.XOffset - 1.3f, -1f, "x");
			PrintText2D(1f, (float)(Model.ProjectionHeight - 0.5 - Model.YOffset), "y");


			// вызываем функцию рисования графика
			if (histHeights != null)
				DrawDiagram(ViewModel.PartitionWidth, histHeights);
			//рисование кривой метода парзена-розенблатта

			DrawDensity(0.5f, densityFunction, ViewModel.ParticlesCount * 0.02f);
			DrawSolution(0.5f, ViewModel.TimeStep * ViewModel.CurrentStep, MethodsCommon.Solution);
			DrawDifferenceScheme(sweepSolution, ViewModel.SweepDimensionStep, ViewModel.SweepDimension);

			// возвращаем матрицу из стека 
			Gl.glPopMatrix();

			// устанавливаем красный цвет 
			Gl.glColor3f(255, 0, 0);

			// дожидаемся завершения визуализации кадра 
			Gl.glFlush();

			// сигнал для обновление элемента реализующего визуализацию. 
			_openGlControl.Invalidate();
		}

		private static void PrintText2D(float x, float y, string text)
		{
			// устанавливаем позицию вывода растровых символов в переданных координатах x и y. 
			Gl.glRasterPos2f(x, y);
			foreach (var charForDraw in text)
			{
				// визуализируем символ используя шрифт GLUT_BITMAP_9_BY_15. 
				Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, charForDraw);
			}
		}

		/// <summary>
		/// Draw Density
		/// </summary>
		/// <param name="xStep"> Density draw step</param>
		/// <param name="dencity"></param>
		/// <param name="fullWeight"> Overall particle weight (particle.count * particleMass) </param>
		private static void DrawDensity(float xStep, Func<float, float> dencity, float fullWeight)
		{
			Gl.glColor3f(0, 0, 1);
			Gl.glBegin(Gl.GL_LINE_STRIP);

			for (var ax = 0; ax * xStep < Model.ProjectionWidth; ax++)
			{
				Gl.glVertex2d(ax * xStep, dencity(ax * xStep) * fullWeight);
				Gl.glVertex2d((ax + 1) * xStep, dencity((ax + 1) * xStep) * fullWeight);
			}

			Gl.glEnd();
			Gl.glColor3f(0, 0, 0);
		}

		/// <summary>
		/// Draw solution
		/// </summary>
		/// <param name="xStep"> Solution draw step </param>
		/// <param name="y"> Function to calculate solution in (x,t) coords </param>
		/// <param name="time"> Time value (timeStep * steps)</param>
		private static void DrawSolution( float xStep, float time, Func<float, float, float> y)
		{

			Gl.glBegin(Gl.GL_LINE_STRIP);
			for (var ax = 0; ax * xStep < Model.ProjectionWidth; ax++)
			{
				Gl.glVertex2d(ax * xStep, y(ax * xStep, time));
				Gl.glVertex2d((ax + 1) * xStep, y((ax + 1) * xStep, time));
			}

			Gl.glEnd();
		}

		/// <summary>
		/// Drawing Sweep Solution
		/// </summary>
		/// <param name="y"> Sweep Solution</param>
		/// <param name="xStep"> Sweep Step</param>
		/// <param name="dimention">Sweep Dimention</param>
		private static void DrawDifferenceScheme(IList<float> y, double xStep, int dimention)
		{
			Gl.glColor3f(1, 0, 0);
			Gl.glBegin(Gl.GL_LINE_STRIP);
			for (var ax = 0; ax < dimention - 1; ax++)
			{
				Gl.glVertex2d(ax * xStep, y[ax]);
				Gl.glVertex2d((ax + 1) * xStep, y[ax + 1]);
			}
			Gl.glEnd();
			Gl.glColor3f(0, 0, 0);
		}

		/// <summary>
		/// Draw Histogram
		/// </summary>
		/// <param name="xStep"> Length of partitioning</param>
		/// <param name="y"> Heights of histogram columns </param>
		private static void DrawDiagram(double xStep, IList<float> y)
		{
			Gl.glColor3f(0, 1, 0);
			Gl.glBegin(Gl.GL_LINE_STRIP);
			for (var ax = 0; ax < y.Count; ax++)
			{
				Gl.glVertex2d(ax * xStep, y[ax]);
				Gl.glVertex2d((ax + 1) * xStep, y[ax]);
			}
			Gl.glEnd();
			Gl.glColor3f(0, 0, 0);
		}
	}
}
