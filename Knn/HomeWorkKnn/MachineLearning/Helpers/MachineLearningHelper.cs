using HomeWorkKnn.MachineLearning.KNN;

namespace HomeWorkKnn.MachineLearning.Helpers
{
    /// <summary>
    /// Хелпер для машинного обучения, предоставляющий вспомогательные методы.
    /// </summary>
    public static class MachineLearningHelper
    {
        /// <summary>
        /// Разделяет данные на обучающие и тестовые наборы.
        /// </summary>
        /// <param name="data">Полный набор данных для разделения.</param>
        /// <param name="trainData">Результат: список обучающих данных.</param>
        /// <param name="testData">Результат: список тестовых данных.</param>
        /// <param name="trainRatio">Доля данных, используемая для обучения (например, 0.8 для 80%).</param>
        public static void SplitData(List<DataPoint> data, out List<DataPoint> trainData, out List<DataPoint> testData, double trainRatio = 0.8)
        {
            var random = new Random(42);
            var shuffledData = data.OrderBy(x => random.Next()).ToList();

            int trainSize = (int)(shuffledData.Count * trainRatio);
            trainData = shuffledData.Take(trainSize).ToList();
            testData = shuffledData.Skip(trainSize).ToList();
        }
    }
}