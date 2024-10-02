namespace GraphCenterVertices.Helpers
{
   /// <summary>
   /// Класс с вспомогательными методами для работы с графами.
   /// </summary>
   public static class GraphHelper
   {
       /// <summary>
       /// Подсчитывает количество уникальных связей в графе.
       /// </summary>
       /// <param name="graph">Граф, представленный в виде словаря, где ключ — ID пользователя, а значение — список его друзей.</param>
       /// <returns>Количество уникальных связей в графе.</returns>
       public static int CountUniqueConnections(Dictionary<long, List<long>> graph)
       {
           var uniqueConnections = new HashSet<(long, long)>();
   
           foreach (var kv in graph)
           {
               var user = kv.Key;
               foreach (var friend in kv.Value)
               {
                   var connection = user < friend ? (user, friend) : (friend, user);
                   uniqueConnections.Add(connection);
               }
           }
   
           return uniqueConnections.Count;
       }
       
       /// <summary>
       /// Преобразует граф, представленный в виде словаря, в матрицу смежности.
       /// </summary>
       /// <param name="graph">Граф, где ключ — ID пользователя, а значение — список его друзей.</param>
       /// <returns>Матрица смежности, представляющая связи между пользователями.</returns>
       public static int[,] ConvertGraphToAdjacencyMatrix(Dictionary<long, List<long>> graph)
       {
           var userIds = graph.Keys.ToList();
           var n = userIds.Count;
           
           var adjacencyMatrix = new int[n, n];
           
           for (int i = 0; i < n; i++)
           {
               var userId = userIds[i];
               var friends = graph[userId];

               for (int j = 0; j < n; j++)
               {
                   var friendId = userIds[j];
                   adjacencyMatrix[i, j] = friends.Contains(friendId) ? 1 : 0;
               }
           }

           return adjacencyMatrix;
       }
       
       /// <summary>
       /// Находит цепочку друзей от исходного пользователя до конечного.
       /// </summary>
       /// <param name="graph">Граф, представленный в виде словаря, где ключ — ID пользователя, а значение — список его друзей.</param>
       /// <param name="startUserId">ID исходного пользователя.</param>
       /// <param name="endUserId">ID конечного пользователя.</param>
       /// <returns>Цепочка друзей от исходного пользователя до конечного или null, если путь не найден.</returns>
       public static List<long> FindFriendChain(Dictionary<long, List<long>> graph, long startUserId, long endUserId)
       {
           if (!graph.ContainsKey(startUserId))
           {
               Console.WriteLine($"ID исходного пользователя {startUserId} отсутствует в графе.");
               return null;
           }

           if (!graph.ContainsKey(endUserId))
           {
               Console.WriteLine($"ID конечного пользователя {endUserId} отсутствует в графе.");
               return null;
           }

           var queue = new Queue<List<long>>();
           queue.Enqueue(new List<long> { startUserId });
           var visited = new HashSet<long> { startUserId };

           while (queue.Count > 0)
           {
               var path = queue.Dequeue();
               long currentUserId = path[^1];

               // Обрабатываем всех друзей текущего пользователя
               foreach (var friendId in graph[currentUserId])
               {
                   if (!visited.Contains(friendId))
                   {
                       visited.Add(friendId);
                       var newPath = new List<long>(path) { friendId };

                       if (friendId == endUserId)
                       {
                           return newPath;
                       }

                       queue.Enqueue(newPath);
                   }
               }
           }

           Console.WriteLine("Цепочка друзей не найдена.");
           return null;
       }

   }
}