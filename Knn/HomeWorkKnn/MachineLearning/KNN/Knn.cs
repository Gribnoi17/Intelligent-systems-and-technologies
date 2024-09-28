using System.Text.Json;

namespace HomeWorkKnn.MachineLearning.KNN
{
    /// <summary>
    /// Реализация алгоритма K-Nearest Neighbors (KNN).
    /// </summary>
    public class Knn
    {
        private readonly int _k;
        private double[] _minValues;
        private double[] _maxValues;
        private List<DataPoint> _dataPoints;

        /// <summary>
        /// Конструктор класса KNN.
        /// </summary>
        /// <param name="k">Число ближайших соседей для классификации.</param>
        /// <exception cref="ArgumentOutOfRangeException">Если значение k меньше или равно нулю.</exception>
        public Knn(int k)
        {
            if (k <= 0) throw new ArgumentOutOfRangeException(nameof(k));
            
            _k = k;
            _dataPoints = new List<DataPoint>();
        }
        
        /// <summary>
        /// Добавляет данные для обучения модели.
        /// </summary>
        /// <param name="data">Список объектов TrainingData для обучения.</param>
        public void AddDataPoints(List<DataPoint> data)
        {
            _dataPoints.AddRange(data);
            
            NormalizeDataPoints();
        }
        
        /// <summary>
        /// Классифицирует новую точку на основе ближайших соседей.
        /// </summary>
        /// <param name="newPoint">Признаки новой точки.</param>
        /// <returns>Наименование класса, к которому принадлежит точка.</returns>
        public int Predict(double[] newPoint)
        {
            var normalizedPoint = NormalizeDataPoint(newPoint);

            // Находим ближайших соседей с учетом расстояния
            var neighbors = _dataPoints
                .Select(data => (Distance: EuclideanDistance(data.Features, normalizedPoint), Label: data.Label))
                .OrderBy(d => d.Distance)
                .Take(_k)
                .ToList();

            // Группируем по классам, используя взвешенные суммы (с учетом расстояния)
            var weightedVotes = neighbors
                .GroupBy(d => d.Label)
                .Select(group => new
                {
                    Label = group.Key,
                    WeightedSum = group.Sum(x => 1.0 / x.Distance) // Взвешиваем по обратному расстоянию
                })
                .OrderByDescending(g => g.WeightedSum)
                .ToList();

            // Проверка на равенство голосов
            if (weightedVotes.Count > 1 && Math.Abs(weightedVotes[0].WeightedSum - weightedVotes[1].WeightedSum) < 0.0001)
            {
                var random = new Random();
                return weightedVotes[random.Next(weightedVotes.Count)].Label;
            }

            return weightedVotes.First().Label;
        }
        
        /// <summary>
        /// Вычисляет точность модели на тестовых данных.
        /// </summary>
        /// <param name="testData">Список тестовых данных (TrainingData) для оценки.</param>
        /// <returns>Метрика точности (accuracy) как дробное число от 0 до 1.</returns>
        public double CalculateAccuracy(List<DataPoint> testData)
        {
            int correct = 0;
            foreach (var data in testData)
            {
                var predicted = Predict(data.Features);
                if (predicted == data.Label)
                {
                    correct++;
                }
            }
            return (double)correct / testData.Count;
        }
        
        /// <summary>
        /// Сохраняет модель в файл JSON.
        /// </summary>
        /// <param name="filePath">Путь к файлу, где будет сохранена модель.</param>
        public void SaveModel(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonData = JsonSerializer.Serialize(_dataPoints, options);
                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ошибка сохранения модели: {exception.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Загружает модель из файла JSON.
        /// </summary>
        /// <param name="filePath">Путь к файлу с сохранённой моделью.</param>
        public void LoadModel(string filePath)
        {
            try
            {
                var jsonData = File.ReadAllText(filePath);
                _dataPoints = JsonSerializer.Deserialize<List<DataPoint>>(jsonData);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ошибка загрузки модели: {exception.Message}");
                throw;
            }
        }
        
        private void NormalizeDataPoints()
        {
            if (!_dataPoints.Any()) return;

            var featureCount = _dataPoints[0].Features.Length;
            
            _minValues = new double[featureCount];
            _maxValues = new double[featureCount];

            for (int i = 0; i < featureCount; i++)
            {
                _minValues[i] = _dataPoints.Min(d => d.Features[i]);
                _maxValues[i] = _dataPoints.Max(d => d.Features[i]);
            }
            
            foreach (var data in _dataPoints)
            {
                data.Features = NormalizeDataPoint(data.Features);
            }
        }
        
        private double[] NormalizeDataPoint(double[] point)
        {
            var normalizedPoint = new double[point.Length];

            for (int i = 0; i < point.Length; i++)
            {
                if (_maxValues[i] != _minValues[i])
                {
                    normalizedPoint[i] = (point[i] - _minValues[i]) / (_maxValues[i] - _minValues[i]);
                }
                else
                {
                    normalizedPoint[i] = 0;
                }
            }

            return normalizedPoint;
        }
        
        private static double EuclideanDistance(double[] point1, double[] point2)
        {
            if (point1.Length != point2.Length)
            {
                throw new ArgumentException("Количество признаков различно");
            }
            
            double sum = 0;
            for (int i = 0; i < point1.Length; i++)
            {
                sum += Math.Pow(point1[i] - point2[i], 2);
            }
            return Math.Sqrt(sum);
        }
    }
}
