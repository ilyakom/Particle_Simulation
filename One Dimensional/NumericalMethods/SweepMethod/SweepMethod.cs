using System;

namespace Diploma.NumericalMethods.SweepMethod
{
	internal class SweepMethod
	{
		public float[] Solution;

		private float[] Alfa, Betta;

		private readonly ViewModel _viewModel = ViewModel.GetInstance();

        public SweepMethod(int dimension)
        {
            Alfa = new float[dimension];
            Betta = new float[dimension];
            Solution = new float[dimension + 1];

            for (var i = 1; i < dimension - 1; i++)
                Solution[i] = MethodsCommon.Solution(i * _viewModel.SweepDimensionStep + _viewModel.LeftBound, 0);
        }

        public void DifferenceScheme()
        {
            for (var i = 1; i < _viewModel.SweepDimension - 1; i++)
            {
                Alfa[i + 1] = B(i) / (C(i) - A(i) * Alfa[i]);
                Betta[i + 1] = (F(i) + A(i) * Betta[i]) / (C(i) - A(i) * Alfa[i]);
            }

            //sweepSolution[sweepDemention - 1] = (F(sweepDemention - 1) + A(sweepDemention - 1) * beta[sweepDemention - 1]) / (C(sweepDemention - 1) - A(sweepDemention - 1) * alfa[sweepDemention - 1]);           

            for (var i = _viewModel.SweepDimension - 2; i >= 0; i--)
            {
                Solution[i] = Alfa[i + 1] * Solution[i + 1] + Betta[i + 1];
            }
        }

        private float A(int i)
        {
            return i == _viewModel.SweepDimension - 1 ? 0 : (Solution[i] + Solution[i - 1]) / 2;
        }

        private float B(int i)
        {
            return i == 0 ? 0 : (Solution[i] + Solution[i + 1]) / 2;
        }

        private float C(int i)
        {
            return (float)(i == _viewModel.SweepDimension - 1 || i == 0 
				? 1 
				: A(i) + B(i) + Math.Pow(_viewModel.SweepDimensionStep, 2) / _viewModel.TimeStep);
        }

        private float F(int i)
        {
            return (float)Math.Pow(_viewModel.SweepDimensionStep, 2) * (float)(Solution[i] / _viewModel.TimeStep + MethodsCommon.HeatGenerator(Solution[i]));
        }
    }
}
