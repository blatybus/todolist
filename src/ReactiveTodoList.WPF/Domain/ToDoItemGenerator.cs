using System.Reactive.Linq;

namespace ReactiveTodoList.Wpf.Domain
{
    public class ToDoItemGenerator : IDisposable
    {
        private readonly Random _random = new Random();
        private readonly IDisposable _cleanUp;
        private readonly object _locker = new object();
        private int _counter = 0;

        public IEnumerable<ToDoItem> Generate(int numberToGenerate, bool initialLoad = false)
        {
            ToDoItem NewTrade()
            {
                var id = _counter++;

                if (initialLoad)
                {
                    var dueDateOffset = _random.Next(-10, 2);

                    return new ToDoItem(
                        Guid.NewGuid(),
                        "todo " + id,
                        DateOnly.FromDateTime(DateTime.Now.Date.AddDays(dueDateOffset)),
                        true);
                }

                return new ToDoItem(
                    Guid.NewGuid(),
                    "todo " + id,
                    DateOnly.FromDateTime(DateTime.Now.Date.AddDays(3)));
            }


            IEnumerable<ToDoItem> result;
            lock (_locker)
            {
                result = Enumerable.Range(1, numberToGenerate).Select(_ => NewTrade()).ToArray();
            }
            return result;
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}