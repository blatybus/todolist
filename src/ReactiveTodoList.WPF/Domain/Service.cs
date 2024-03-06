//using System;
//using System.Linq;
//using System.Reactive.Concurrency;
//using System.Reactive.Disposables;
//using System.Reactive.Linq;
//using DynamicData;
//using DynamicData.Kernel;
//using ReactiveUI;
//using WpfApp.Domain;

//namespace Trader.Domain.Services
//{
//    public class DataServiceGenerator : IDisposable
//    {
//        private readonly ToDoItemGenerator _tradeGenerator;

//        private readonly IDisposable _cleanup;

//        public DataServiceGenerator()
//        {
//            _tradeGenerator = new ToDoItemGenerator();

//            //emulate a trade service which asynchronously 
//            var todoData = GenerateTradesAndMaintainCache().Publish();

//            //call AsObservableCache() so the cache can be directly exposed
//            All = todoData.AsObservableCache();

//            _cleanup = new CompositeDisposable(All, todoData.Connect());
//        }

//        private IObservable<IChangeSet<ToDoItem, Guid>> GenerateTradesAndMaintainCache()
//        {
//            //construct an cache datasource specifying that the primary key is Trade.Id
//            return ObservableChangeSet.Create<ToDoItem, Guid>(cache =>
//            {
//                /*
//                    The following code emulates an external trade provider. 
//                    Alternatively you can use "new SourceCacheTrade, long>(t=>t.Id)" and manually maintain the cache.

//                    For examples of creating a observable change sets, see https://github.com/RolandPheasant/DynamicData.Snippets
//                */

//                //bit of code to generate trades
//                var random = new Random();

//                //initally load some trades 
//                cache.AddOrUpdate(_tradeGenerator.Generate(5_000, true));

//                TimeSpan RandomInterval() => TimeSpan.FromMilliseconds(random.Next(2500, 5000));


//                // create a random number of trades at a random interval
//                var tradeGenerator = RxApp.TaskpoolScheduler
//                    .ScheduleRecurringAction(RandomInterval, () =>
//                    {
//                        var number = random.Next(1, 5);
//                        var trades = _tradeGenerator.Generate(number);
//                        cache.AddOrUpdate(trades);
//                    });

//                return new CompositeDisposable(tradeGenerator);
//            }, trade => trade.Id);
//        }

//        public IObservableCache<ToDoItem, string> All { get; }

//        public void Dispose()
//        {
//            _cleanup.Dispose();
//        }
//    }
//}