namespace ReactiveTodoList.Wpf.Domain
{
    public class ToDoItem
    {
        public ToDoItem(Guid id, string title, DateOnly dueDate, bool isCompleted = false)
        {
            Id = id;
            Title = title;
            DueDate = dueDate;
            IsCompleted = isCompleted;
        }

        public Guid Id { get; }

        public string Title { get; }

        public DateOnly DueDate { get; }

        public bool IsCompleted { get; set; }
    }
}
