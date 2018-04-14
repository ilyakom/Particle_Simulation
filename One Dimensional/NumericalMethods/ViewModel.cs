using System.ComponentModel;
using System.Windows.Forms;
using Diploma.Annotations;

namespace Diploma.NumericalMethods
{
	[Bindable(BindableSupport.Yes)]
	public class ViewModel : INotifyPropertyChanged
	{
		private ViewModel()
		{
			Averaging = 1;
			CurrentStep = 1;
			SweepDimension = 100;

			LeftBound = 0;
		}

		private static ViewModel _model;

		public static ViewModel GetInstance()
		{
			return _model ?? (_model = new ViewModel());
		}

		private float _parzenWindowWidth;
		/// <summary>
		/// Ширина парзеновского окна
		/// </summary>
		public float ParzenWindowWidth
		{
			get => _parzenWindowWidth;
			set
			{
				_parzenWindowWidth = value;
				OnPropertyChanged(nameof(ParzenWindowWidth));
			}
		}

		private int _particlesCount;
		/// <summary>
		/// Кол-во частиц
		/// </summary>
		public int ParticlesCount
		{
			get => _particlesCount;
			set
			{
				_particlesCount = value;
				OnPropertyChanged(nameof(ParticlesCount));
			}
		}

		private int _sweepDimension;
		/// <summary>
		/// Размерность сетки для метода прогонки
		/// </summary>
		public int SweepDimension
		{
			get => _sweepDimension;
			set
			{
				_sweepDimension = value;
				OnPropertyChanged(nameof(SweepDimension));
				SweepDimensionStep = (RightBound - LeftBound) / SweepDimension;
			}
		}

		private float _sweepDimensionStep;
		/// <summary>
		/// Шаг сетки для метода прогонки
		/// </summary>
		public float SweepDimensionStep
		{
			get => _sweepDimensionStep;
			set
			{
				_sweepDimensionStep = value;
				OnPropertyChanged(nameof(SweepDimensionStep));
			}
		}

		private float _timeStep;
		/// <summary>
		/// Размер временного шага
		/// </summary>
		public float TimeStep
		{
			get => _timeStep;
			set
			{
				_timeStep = value;
				OnPropertyChanged(nameof(TimeStep));
			}
		}

		private int _averaging;
		/// <summary>
		/// Количества усреднения 
		/// </summary>
		public int Averaging
		{
			get => _averaging;
			set
			{
				_averaging = value;
				OnPropertyChanged(nameof(Averaging));
			}
		}

		private int _partitionsCount;
		/// <summary>
		/// Кол-во разбиений для Метода Частиц
		/// </summary>
		public int PartitionsCount
		{
			get => _partitionsCount;
			set
			{
				_partitionsCount = value;
				OnPropertyChanged(nameof(PartitionsCount));
			}
		}

		private float _partitionWidth;
		/// <summary>
		/// Длинна единичного разбиеная
		/// </summary>
		public float PartitionWidth
		{
			get => _partitionWidth;
			set
			{
				_partitionWidth = value;
				OnPropertyChanged(nameof(PartitionWidth));
			}
		}

		private float _leftBound;
		/// <summary>
		/// Левая граница
		/// </summary>
		public float LeftBound
		{
			get => _leftBound;
			set
			{
				_leftBound = value;
				OnPropertyChanged(nameof(LeftBound));
			}
		}

		private float _rightBound;
		/// <summary>
		/// Правая граница
		/// </summary>
		public float RightBound
		{
			get => _rightBound;
			set
			{
				_rightBound = value;
				OnPropertyChanged(nameof(RightBound));
			}
		}

		private int _currebtStep;
		/// <summary>
		/// Текущий шаг по временм 
		/// </summary>
		public int CurrentStep
		{
			get => _currebtStep;
			set
			{
				_currebtStep = value;
				OnPropertyChanged(nameof(CurrentStep));
			}
		}

			

		public static void SetSweepDimension(object sender, ConvertEventArgs e)
		{
			e.Value = int.Parse((string)e.Value);
		}

		public static void SetTimeStep(object sender, ConvertEventArgs e)
		{
			e.Value = float.Parse((string)e.Value);
		}

		public static void SetAveraging(object sender, ConvertEventArgs e)
		{
			e.Value = int.Parse((string) e.Value);
		}

		public static void SetPartitionsCount(object sender, ConvertEventArgs e)
		{
			e.Value = int.Parse((string) e.Value);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
