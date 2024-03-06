using DynamicData;
using DynamicData.Kernel;

namespace ReactiveTodoList.Wpf.Domain
{
    public interface IItemManager
    {
        public IObservable<IChangeSet<ToDoItem, Guid>> TodoItemChanges { get; }

        //public IObservableCache<ToDoItem, Guid> Live { get; }

        void AddOrUpdate(ToDoItem item);
        Optional<ToDoItem> Get(Guid id);
        void Remove(ToDoItem item);
    }
}