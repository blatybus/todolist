using DynamicData;
using DynamicData.Kernel;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ReactiveTodoList.Wpf.Domain
{
    public class ItemManagerBackground : IItemManager
    {
        private IObservable<IChangeSet<ToDoItem, Guid>> _cache;

        public IObservable<IChangeSet<ToDoItem, Guid>> TodoItemChanges { get; }

        private ToDoItemGenerator _generator;

        public ItemManagerBackground()
        {
            _generator = new ToDoItemGenerator();
            _cache = GenerateTradesAndMaintainCache();
            TodoItemChanges = _cache.AsObservableCache().Connect().RefCount();
        }

        public Optional<ToDoItem> Get(Guid id) =>
            throw new NotImplementedException();

        public void AddOrUpdate(ToDoItem item) => throw new NotImplementedException();

        public void Remove(ToDoItem item) => throw new NotImplementedException();

        private IObservable<IChangeSet<ToDoItem, Guid>> GenerateTradesAndMaintainCache()
        {
            //construct an cache datasource specifying that the primary key is Trade.Id
            return ObservableChangeSet.Create<ToDoItem, Guid>(cache =>
            {
                /*
                    The following code emulates an external trade provider. 
                    Alternatively you can use "new SourceCacheTrade, long>(t=>t.Id)" and manually maintain the cache.

                    For examples of creating a observable change sets, see https://github.com/RolandPheasant/DynamicData.Snippets
                */

                //bit of code to generate trades
                var random = new Random();

                //initally load some trades 

                var dueDate = DateOnly.FromDateTime(DateTime.Now);
                cache.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Family vacation planning", dueDate));
                cache.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Buy Christmas Gifts", dueDate));
                cache.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Go to the Bank", dueDate));
                cache.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Buy Milk", dueDate));

                TimeSpan RandomInterval() => TimeSpan.FromMilliseconds(random.Next(2500, 5000));

                var tradeGenerator =
                    RxApp.TaskpoolScheduler
                        .ScheduleRecurringAction(RandomInterval, () =>
                        {
                            var number = random.Next(1, 5);
                            var trades = _generator.Generate(number);
                            cache.AddOrUpdate(trades);
                        });

                return new CompositeDisposable();
            }, trade => trade.Id);
        }
    }
}
