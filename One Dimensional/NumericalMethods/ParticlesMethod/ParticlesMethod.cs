using System;
using System.Collections.Generic;
using System.Linq;

namespace Diploma.NumericalMethods.ParticlesMethod
{
	internal class ParticlesMethod
	{
		private readonly ViewModel _viewModel = ViewModel.GetInstance();

		public readonly List<Particle> Particles;
		public readonly float[] PartitionsHeights;

		private static readonly Random RandomNumber = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
		private const float ParticleWeight = 0.02f;

		public ParticlesMethod(float partitionWidth)
		{
			Particles = new List<Particle>();

			PartitionsHeights = new float[_viewModel.PartitionsCount];

			for (var i = 0; i < _viewModel.PartitionsCount; i++)
			{
				var numOfParticlesInCell = (int)(MethodsCommon.Solution((i + 0.5f) * partitionWidth + _viewModel.LeftBound, 0) / ParticleWeight);
				for (var k = 0; k < numOfParticlesInCell; k++)
				{
					Particles.Add(new Particle(partitionWidth * (i + (float)RandomNumber.NextDouble()) + _viewModel.LeftBound));
				}

				PartitionsHeights[i] += ParticleWeight * numOfParticlesInCell;
			}

		}

		public float GetPreferableTimeStep()
		{
			return 1f / (float)Math.Sqrt(Particles.Count);
		}

		public void SimulateWithEveryStepAveraging()
		{
			var averagingPartitionHeights = new float[PartitionsHeights.Length];
			var averagingPartices = new List<Particle>();

			AddParticlesByHeater(Particles, PartitionsHeights);
			Array.Clear(PartitionsHeights, 0, PartitionsHeights.Length);

			foreach (var p in Particles)
				if (p.XCoord < _viewModel.RightBound && p.XCoord > _viewModel.LeftBound)
					PartitionsHeights[(int) Math.Floor((p.XCoord + _viewModel.LeftBound) / _viewModel.PartitionWidth)] +=
						ParticleWeight;

			for (var j = 0; j < _viewModel.Averaging; j++)
			{
				averagingPartices.Clear();
				foreach (var t in Particles)
				{
					var randomNumber1 = RandomNumber.NextDouble();
					var randomNumber2 = RandomNumber.NextDouble();
					while (randomNumber1 == 0)
						randomNumber1 = RandomNumber.NextDouble();
					while (randomNumber2 == 0)
						randomNumber2 = RandomNumber.NextDouble();

					averagingPartices.Add( 
						new Particle(t.XCoord + 
						             (float)(BoxMullerTransform(randomNumber1, randomNumber2) 
					                 * Math.Sqrt(_viewModel.TimeStep)                                              
					                 * LocalDiffusionCoefficient(PartitionsHeights[(int)Math.Floor((t.XCoord + _viewModel.LeftBound) / _viewModel.PartitionWidth)]))));
				}

				foreach (var t in averagingPartices)
				{
					if (t.XCoord < _viewModel.RightBound && t.XCoord > _viewModel.LeftBound)
						averagingPartitionHeights[(int)Math.Floor((t.XCoord + _viewModel.LeftBound) / _viewModel.PartitionWidth)] += ParticleWeight;
				}
			}

			for (var i = 0; i < PartitionsHeights.Length; i++)
			{
				PartitionsHeights[i] = averagingPartitionHeights[i] / _viewModel.Averaging;
			}

			Particles.Clear();

			var errorsList = new List<DensityRecoveryError>();
			for (var i = 0; i < _viewModel.PartitionsCount; i++)
			{
				var numofParticlesInCell = (int)Math.Floor(PartitionsHeights[i] / ParticleWeight);
				if (PartitionsHeights[i] % ParticleWeight > 0)
					errorsList.Add(new DensityRecoveryError( PartitionsHeights[i] % ParticleWeight, i));

				for (var k = 0; k < numofParticlesInCell; k++)
					Particles.Add( new Particle(_viewModel.PartitionWidth * (i + (float)RandomNumber.NextDouble()) + _viewModel.LeftBound));
			}

			float error = 0, missedParticleCoordinate = 0;

			//Воостановление потерянных частиц, которые появляются в результате
			//восстановления плотности после усреднения. Координата вычисляется как
			//сумма координат столбиков гастограмм, на которых была обнаружена потеря,
			//умноженная на величину потери(своеобразное весовое значение)
			foreach (var t in errorsList)
			{
				error += t.Value;
				missedParticleCoordinate += t.Value * (_viewModel.PartitionWidth * t.PartitionIndex) / ParticleWeight;

				if (!(error >= ParticleWeight)) continue;

				//регулируем координату 
				missedParticleCoordinate -= (error - ParticleWeight) * (_viewModel.PartitionWidth * t.PartitionIndex) / ParticleWeight;
				Particles.Add( new Particle(missedParticleCoordinate + _viewModel.LeftBound));

				//координаты остатка
				missedParticleCoordinate = (error - ParticleWeight) * (_viewModel.PartitionWidth * t.PartitionIndex) / ParticleWeight;
				error -= ParticleWeight;
			}

			error = (float)Math.Round(error, 2);
			if (error >= ParticleWeight)
				Particles.Add( new Particle(missedParticleCoordinate));

			_viewModel.ParticlesCount = Particles.Count;

			//SpendedTime = TimeStep * (double)(steps);
			//TimeStep = 1f / (float)Math.Sqrt(Particles.Count);
			//steps = (int)(SpendedTime / TimeStep);
			//textBox3.Text = TimeStep.ToString();
		}

		public void SimulateWholeWayAveraging()
		{
			var averagingPartitionHeights = new float[PartitionsHeights.Length];

			Array.Clear(PartitionsHeights, 0, PartitionsHeights.Length);

			for (var j = 0; j < _viewModel.Averaging; j++)
			{
				var averagingPartices = Particles.ConvertAll(p => new Particle(p.XCoord));

				Array.Clear(averagingPartitionHeights, 0, averagingPartitionHeights.Length);
				foreach (var p in averagingPartices)
					averagingPartitionHeights[(int)Math.Floor((p.XCoord + _viewModel.LeftBound) / _viewModel.PartitionWidth)] += ParticleWeight;

				for (var k = 0; k < _viewModel.CurrentStep; k++)
				{
					AddParticlesByHeater(averagingPartices, averagingPartitionHeights);

					foreach (var particle in averagingPartices)
					{
						if(particle.XCoord + _viewModel.LeftBound < 0) continue;

						var randomNumber1 = RandomNumber.NextDouble();
						var randomNumber2 = RandomNumber.NextDouble();
						while (randomNumber1 == 0)
							randomNumber1 = RandomNumber.NextDouble();
						while (randomNumber2 == 0)
							randomNumber2 = RandomNumber.NextDouble();

						particle.XCoord += (float)(BoxMullerTransform(randomNumber1, randomNumber2)
						                    * Math.Sqrt(_viewModel.TimeStep)
						                    * LocalDiffusionCoefficient(averagingPartitionHeights[(int)Math.Floor((particle.XCoord + _viewModel.LeftBound) / _viewModel.PartitionWidth)]));
					}

					Array.Clear(averagingPartitionHeights, 0, averagingPartitionHeights.Length);
					foreach (var ap in averagingPartices)
						if (ap.XCoord < _viewModel.RightBound && ap.XCoord > _viewModel.LeftBound)
							averagingPartitionHeights[(int)Math.Floor((ap.XCoord + _viewModel.LeftBound) / _viewModel.PartitionWidth)] += ParticleWeight;
				}

				_viewModel.ParticlesCount = averagingPartices.Count;

				for (var i = 0; i < PartitionsHeights.Length; i++)
					PartitionsHeights[i] += averagingPartitionHeights[i];
			}

			for (var j = 0; j < PartitionsHeights.Length; j++)
				PartitionsHeights[j] /= _viewModel.Averaging;

			//SpendedTime = TimeStep * (double)(steps);
			//TimeStep = 1f / (float)(NumOfParticles);
			//steps = (int)(SpendedTime / TimeStep);
			//textBox3.Text = TimeStep.ToString();  
		}

		public void SimulateParzenMethod()
		{
			AddParticlesByHeater(Particles, PartitionsHeights);

			for (var i = 0; i < Particles.Count; i++)
			{
				var randomNumber1 = RandomNumber.NextDouble();
				var randomNumber2 = RandomNumber.NextDouble();
				while (randomNumber1 == 0)
					randomNumber1 = RandomNumber.NextDouble();
				while (randomNumber2 == 0)
					randomNumber2 = RandomNumber.NextDouble();

				Particles[i].XCoord += (float)BoxMullerTransform(randomNumber1, randomNumber2) 
				                       * (float)Math.Sqrt(_viewModel.TimeStep) 
				                       * (float)LocalDiffusionCoefficient(PartitionsHeights[(int)Math.Floor((Particles[i].XCoord + _viewModel.LeftBound) / _viewModel.PartitionWidth)]);

				if (Particles[i].XCoord > _viewModel.LeftBound && Particles[i].XCoord < _viewModel.RightBound) continue;

				Particles.RemoveAt(i);
				i--;
			}

			// оценка ширины окна по правилу Сильвермана
			var mean = Particles.Average(p => p.XCoord);
			var sumOfSquaresOfDifferences = Particles.Select(val => (val.XCoord - mean) * (val.XCoord - mean)).Sum();
			var std = Math.Sqrt(sumOfSquaresOfDifferences / Particles.Count);

			_viewModel.ParzenWindowWidth = (float)(std * Math.Pow(4.0 / 3 / Particles.Count, 1.0 / 5));
			_viewModel.ParticlesCount = Particles.Count;

			for (var i = 0; i < PartitionsHeights.Length; i++)
			{
				PartitionsHeights[i] = (int)(Density((i + 0.5f) * _viewModel.PartitionWidth + _viewModel.LeftBound) * Particles.Count) * ParticleWeight;
			}
		}

		/// <summary>
		/// Функция Гаусса для ядерной оценки плотности
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public float Density(float x)
		{
			var sum = Math.Sqrt(0.5 / Math.PI) * 
			          Particles.Sum(p => Math.Exp(-Math.Pow((x - p.XCoord) / _viewModel.ParzenWindowWidth, 2) / 2));
			return (float)sum / (Particles.Count * _viewModel.ParzenWindowWidth);
		}

		/// <summary>
		/// Преобразование Бокса Мюллера для получения стандартного нормльноко распределения
		/// </summary>
		/// <param name="randomNumber1">Равномерно распределенная случайная величина</param>
		/// <param name="randomNumber2">Равномерно распределенная случайная величина</param>
		/// <returns></returns>
		private static double BoxMullerTransform(double randomNumber1, double randomNumber2)
		{
			return Math.Cos(2 * Math.PI * randomNumber1) * Math.Sqrt(-2 * Math.Log(randomNumber2));
		}

		/// <summary>
		/// Коэфицент локальной диффузии
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		private static double LocalDiffusionCoefficient(double x)
		{
			return Math.Sqrt(x);
		}


		/// <summary>
		/// Инициализирует частицы по высотам разбиений
		/// </summary>
		/// <param name="particles"> Коллекция которую инициализируем</param>
		/// <param name="partitionHeights"> Коллекция по которой инициализируем</param>
		private void AddParticlesByHeater(ICollection<Particle> particles, IList<float> partitionHeights)
		{
			for (var i = 0; i < partitionHeights.Count; i++)
			{
				var numOfParticlesInCell = (int)(_viewModel.TimeStep * MethodsCommon.HeatGenerator(partitionHeights[i]) / ParticleWeight);

				for (var k = 0; k < numOfParticlesInCell; k++)
					particles.Add( new Particle(_viewModel.PartitionWidth * (i + (float)RandomNumber.NextDouble()) + _viewModel.LeftBound));
			}
		}
	}
}
