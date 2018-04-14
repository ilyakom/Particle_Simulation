namespace Diploma.NumericalMethods.ParticlesMethod
{
	public class DensityRecoveryError
	{
		/// <summary>
		/// Масса ошибки при восстановлении протности
		/// </summary>
		public float Value { get; set; }

		/// <summary>
		/// Индекс разбиения
		/// </summary>
		public int PartitionIndex { get; set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="value"></param>
		/// <param name="partitionIndex"></param>
		public DensityRecoveryError(float value, int partitionIndex )
		{
			Value = value;
			PartitionIndex = partitionIndex;
		}
	}
}
