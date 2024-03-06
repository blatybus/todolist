using ReactiveTodoList.Wpf.Domain;
using ReactiveTodoList.Wpf.ViewModel;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace WpfApp
{

    // MainWindow class derives off ReactiveWindow which implements the IViewFor<TViewModel>
    // interface using a WPF DependencyProperty. We need this to use WhenActivated extension
    // method that helps us handling View and ViewModel activation and deactivation.
    public partial class MainWindow : Window, IViewFor<MainWindowViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(MainWindowViewModel), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel(new ItemManager());

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Items, x => x.lvToDo.ItemsSource)
                    .DisposeWith(disposable);

                this.Bind(this.ViewModel, x => x.NewItemTitle, x => x.txtItemDesc.Text)
                    .DisposeWith(disposable);

                this.BindCommand(ViewModel, x => x.AddCommand, x => x.btnAdd)
                    .DisposeWith(disposable);

                this.Bind(this.ViewModel, x => x.ShowCompletedItems, x => x.chkShowDone.IsChecked)
                    .DisposeWith(disposable);
            });
        }

        public MainWindowViewModel? ViewModel
        {
            get => (MainWindowViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainWindowViewModel)value!;
        }
    }
}
