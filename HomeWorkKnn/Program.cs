using HomeWorkKnn.Csv;
using HomeWorkKnn.MachineLearning.Helpers;
using HomeWorkKnn.MachineLearning.KNN;

Console.Write("Введите значение K: ");
var k = int.Parse(Console.ReadLine());
var knn = new Knn(k);

var data = CsvReader.LoadDataFromCsv("data.csv");
MachineLearningHelper.SplitData(data, out var trainData, out var testData, 0.8);
knn.AddDataPoints(trainData);

var accuracy = knn.CalculateAccuracy(testData);
Console.WriteLine($"Accuracy: {accuracy * 100}%");

knn.SaveModel("model.json");
Console.ReadLine();

