using System.Globalization;
using HomeWorkKnn.MachineLearning.KNN;

namespace HomeWorkKnn.Csv
{
    /// <summary>
    /// Класс для работы с cvs файлами.
    /// </summary>
    public static class CsvReader
    {
        /// <summary>
        /// Загрузить данные из CSV файла.
        /// </summary>
        /// <param name="filePath">Путь к CSV файлу.</param>
        /// <returns>Список объектов <see cref="DataPoint"/>.</returns>
        public static List<DataPoint> LoadDataFromCsv(string filePath)
        {
            var data = new List<DataPoint>();
            using (var reader = new StreamReader(filePath))
            {
                var headerLine = reader.ReadLine();

                if (headerLine == null)
                {
                    throw new ArgumentException(nameof(headerLine));
                }
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line == null)
                    {
                        throw new ArgumentException(nameof(line));
                    }
                    
                    var values = line.Split(';');
                    
                    var features = values
                        .Take(values.Length - 1)
                        .Select(x => double.Parse(x, new CultureInfo("ru-RU"))).ToArray();
                    var label = int.Parse(values.Last(), CultureInfo.InvariantCulture);
                    
                    data.Add(new DataPoint { Features = features, Label = label });
                }
            }

            return data;
        }
    }
}