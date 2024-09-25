namespace HomeWorkKnn.MachineLearning.KNN
{
    /// <summary>
    /// Представляет тренировочные данные для алгоритма машинного обучения.
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// Массив признаков, описывающих объект.
        /// </summary>
        public double[] Features { get; set; }

        /// <summary>
        /// Метка класса, к которому принадлежит объект.
        /// </summary>
        public int Label { get; set; }
    }
}