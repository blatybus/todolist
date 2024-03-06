using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveTodoList.Wpf.Domain;
using ReactiveUI;

namespace ReactiveTodoList.Wpf.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _newItemTitle = "";

        public string NewItemTitle
        {
            get => _newItemTitle;
            set => this.RaiseAndSetIfChanged(ref _newItemTitle, value);
        }

        private bool _showDoneItems = true;

        public bool ShowCompletedItems
        {
            get => _showDoneItems;
            set => this.RaiseAndSetIfChanged(ref _showDoneItems, value);
        }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReadOnlyObservableCollection<ToDoItem> Items => _toDoItems;

        public override string Id => "Reactive ToDo";

        private readonly ReadOnlyObservableCollection<ToDoItem> _toDoItems;
        private IItemManager _itemManager;

        public MainWindowViewModel(IItemManager itemManager)
        {
            _itemManager = itemManager;

            AddCommand =
                ReactiveCommand.Create(
                    ExecuteAdd,
                    canExecute: this.WhenAnyValue(x => x.NewItemTitle, (title) => !string.IsNullOrEmpty(title)));

            ShowCompletedItems = true;

            var showCompleted = this.WhenValueChanged(x => x.ShowCompletedItems);

            var displayFilter = showCompleted.Select(BuildDisplayFilter);

            Func<ToDoItem, bool> BuildDisplayFilter(bool showCompleted)
            {
                return
                    showCompleted ?
                        static todoItem => true :
                        static todoItem => !todoItem.IsCompleted;
            }

            itemManager
                 .TodoItemChanges
                 .Filter(displayFilter)
                 .Bind(out _toDoItems)
                 .DisposeMany()
                 .Subscribe()
                 .DisposeWith(Subscriptions);

            //itemManager
            //    .Live.Connect()
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Bind(out _items)
            //    .DisposeMany()
            //    .Subscribe()
            //    .DisposeWith(Subscriptions);

            var dueDate = DateOnly.FromDateTime(DateTime.Now);

            itemManager.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Family vacation planning", dueDate));
            itemManager.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Buy Christmas Gifts", dueDate, true));
            itemManager.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Go to the Bank", dueDate));
            itemManager.AddOrUpdate(new ToDoItem(Guid.NewGuid(), "Buy Milk", dueDate, true));

            //this.WhenAnyValue(x => x.SelectedItem)
            //    .Where(x => x != null)
            //    .InvokeCommand(ViewCommand)
            //    .DisposeWith(Subscriptions);
        }

        private void ExecuteAdd()
        {
            _itemManager
                .AddOrUpdate(new ToDoItem(Guid.NewGuid(), NewItemTitle, DateOnly.FromDateTime(DateTime.Now)));

            NewItemTitle = string.Empty;
        }
    }
}