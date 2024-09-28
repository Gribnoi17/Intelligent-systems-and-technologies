using GraphCenterVertices;
using GraphCenterVertices.FileHelpers;
using GraphCenterVertices.Helpers;

var graph = JsonHelper.LoadGraphFromJson(Configs.GraphJsonName);
var matrix = GraphHelper.ConvertGraphToAdjacencyMatrix(graph);

var totalConnections = GraphHelper.CountUniqueConnections(graph);
Console.WriteLine($"Общее количество вершин: {totalConnections}");

var friendChain = GraphHelper.FindFriendChain(graph, 321819438, 202635044);

Console.WriteLine("Цепочка друзей:");
Console.WriteLine(string.Join(" -> ", friendChain));
