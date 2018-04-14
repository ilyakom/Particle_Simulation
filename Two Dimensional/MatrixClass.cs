using System;
using System.Collections.Generic;

namespace GraphicsProject
{
	internal class MatrixClass
    {        
        
        public static double[][] OnNumber(double a, double[][] matrix)
        {
            double[][] result = MatrixClass.Create(matrix.Length, matrix[0].Length);
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    result[i][j] = a * matrix[i][j];
            return result;
        }
        
      
        public static double[][] MAdd(double[][] a, double[][] b)
        {
            if (a.Length != b.Length || a[0].Length != b[0].Length)
                throw new Exception("Non-conformable matrices in MatrixAdd");
            double[][] result = MatrixClass.Create(a.Length, a[0].Length);

            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < a[i].Length; j++)
                    result[i][j] = a[i][j] + b[i][j];
            return result;

        }

       
        public static double[] MAdd(double[] a, double[] b)
        {
            if (a.Length != b.Length)
                throw new Exception("Non-comfortable matrixes in MatrixAdd");
            double[] result = new double[a.Length];

            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] + b[i];
            return result;
        }

     
        public static double[][] Sub(double[][] a, double[][] b)
        {
            if (a.Length != b.Length || a[0].Length != b[0].Length)
                throw new Exception("Non-conformable matrices in MatrixSub");

            double[][] result = MatrixClass.Create(a.Length, a[0].Length);
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < a[i].Length; j++)
                    result[i][j] = a[i][j] - b[i][j];
            return result;

        }

        public static double[][] Create(int rows, int cols)
        {            
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols]; // автоинициализация в 0.0
            return result;
        }        
    
        public static string AsString(double[][] matrix)
        {
            string s = "";
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                    s += matrix[i][j].ToString("F3").PadLeft(8) + " ";
                s += Environment.NewLine;
            }
            return s;
        }
        
        public static double[][] Product(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices in MatrixProduct");
            double[][] result = Create(aRows, bCols);
            for (int i = 0; i < aRows; ++i) // каждая строка A
                for (int j = 0; j < bCols; ++j) // каждый столбец B
                    for (int k = 0; k < aCols; ++k)
                        result[i][j] += matrixA[i][k] * matrixB[k][j];
            return result;
        }
       
        public static double[] Product(double[][] A, double[] B)
        {
            int aRows = A.Length; int aCols = A[0].Length;
            int bRows = B.Length;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices in MatrixProduct");
            double[] result = new double[aRows];

            for (int i = 0; i < aRows; ++i) // каждая строка A
                for (int k = 0; k < aCols; ++k)
                    result[i] += A[i][k] * B[k];
            return result;
        }
       
        public static double[][] Identity(int n)
        {
            double[][] result = Create(n, n);
            for (int i = 0; i < n; ++i)
                result[i][i] = 1.0;
            return result;
        }
        
        //Создание единичной прямоугольной матрицы, где первые ROWSxROWS или
        public static double[][] IdentitySpecial(int rows, int cols)
        {
            double[][] result = new double[rows][];
            
            for (int i = 0; i < rows; ++i)
            {
                result[i] = new double[cols];
                if (i < cols)
                {
                    result[i][i] = 1.0;
                }
            }
            return result;
        }

        //Создание трехдиагональной матрицы
        public static double[][] ThreeDiagonal(int n, double a, double c, double d)
        {
            var result = Create(n, n);
            for (var i = 0; i < n; i++)
            {
                result[i][i] = c;
                if (i != n - 1)
                    result[i][i + 1] = d;
                if (i != 0)
                    result[i][i - 1] = a;
            }
                return result;
        }

        //Создание копии матрицы
        public static double[][] Duplicate(double[][] matrix)
        {
            // Предполагается, что матрица не нулевая
            var result = Create(matrix.Length, matrix[0].Length);
            for (var i = 0; i < matrix.Length; ++i) // Копирование значений
                for (var j = 0; j < matrix[i].Length; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        static double[][] Decompose(double[][] matrix, out int[] perm, out int toggle)
        {
            // Разложение LUP Дулитла. Предполагается,
            // что матрица квадратная.
            var n = matrix.Length; // для удобства
            var result = Duplicate(matrix);
            perm = new int[n];
            for (var i = 0; i < n; ++i) { perm[i] = i; }
            toggle = 1;
            for (var j = 0; j < n - 1; ++j) // каждый столбец
            {
                var colMax = Math.Abs(result[j][j]); // Наибольшее значение в столбце j
                var pRow = j;
                for (var i = j + 1; i < n; ++i)
                {
	                if (!(result[i][j] > colMax)) continue;
	                colMax = result[i][j];
	                pRow = i;
                }
                if (pRow != j) // перестановка строк
                {
                    var rowPtr = result[pRow];
                    result[pRow] = result[j];
                    result[j] = rowPtr;
                    var tmp = perm[pRow]; // Меняем информацию о перестановке
                    perm[pRow] = perm[j];
                    perm[j] = tmp;
                    toggle = -toggle; // переключатель перестановки строк
                }
                if (Math.Abs(result[j][j]) < 1.0E-20)
                    return null;
                for (var i = j + 1; i < n; ++i)
                {
                    result[i][j] /= result[j][j];
                    for (var k = j + 1; k < n; ++k)
                        result[i][k] -= result[i][j] * result[j][k];
                }
            } // основной цикл по столбцу j
            return result;
        }

	    private static double[] HelperSolve(IReadOnlyList<double[]> luMatrix, double[] b)
        {
            // Решаем luMatrix * x = b
            var n = luMatrix.Count;
            var x = new double[n];
            b.CopyTo(x, 0);
            for (var i = 1; i < n; ++i)
            {
                var sum = x[i];
                for (var j = 0; j < i; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum;
            }
            x[n - 1] /= luMatrix[n - 1][n - 1];
            for (var i = n - 2; i >= 0; --i)
            {
                var sum = x[i];
                for (var j = i + 1; j < n; ++j)
                    sum -= luMatrix[i][j] * x[j];
                x[i] = sum / luMatrix[i][i];
            }
            return x;
        }

        public static double[][] Inverse(double[][] matrix)
        {
            var n = matrix.Length;
            var result = Duplicate(matrix);
	        var lum = Decompose(matrix, out var perm, out var toggle);
            if (lum == null)
                throw new Exception("Unable to compute inverse");
            var b = new double[n];
            for (var i = 0; i < n; ++i)
            {
                for (var j = 0; j < n; ++j)
                {
                    if (i == perm[j])
                        b[j] = 1.0;
                    else
                        b[j] = 0.0;
                }
                var x = HelperSolve(lum, b);
                for (var j = 0; j < n; ++j)
                    result[j][i] = x[j];
            }
            return result;
        }                     
    }
}
