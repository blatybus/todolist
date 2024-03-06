using DynamicData;
using DynamicData.Kernel;
using System.Reactive.Concurrency;

namespace ReactiveTodoList.Wpf.Domain
{
    public class ItemManager : IItemManager
    {
        private SourceCache<ToDoItem, Guid> _itemsCache = new(item => item.Id);

        public IObservable<IChangeSet<ToDoItem, Guid>> TodoItemChanges { get; }

        public ItemManager()
        {
            TodoItemChanges = _itemsCache.Connect().RefCount();
        }

        public Optional<ToDoItem> Get(Guid id) => _itemsCache.Lookup(id);

        public void AddOrUpdate(ToDoItem item)
        {
            _itemsCache.AddOrUpdate(item);
        }

        private IDisposable AddItemm(IScheduler scheduler, ToDoItem item)
        {
            throw new NotImplementedException();
        }

        public void Remove(ToDoItem item)
        {
            _itemsCache?.Remove(item);
        }
    }
}
