using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRX {
    #region "Rx is single-threaded by default"
    //class Program {
    //    static void Main() {
    //        Console.WriteLine($"Starting on threadId:{Thread.CurrentThread.ManagedThreadId}");
    //        var subject = new Subject<Object>();
    //        subject.Subscribe(
    //            o => Console.WriteLine($"Received {o} on threadId:{Thread.CurrentThread.ManagedThreadId} "));

    //        ParameterizedThreadStart notify = obj => {
    //            Console.WriteLine($"OnNext({obj}) on threadId:{Thread.CurrentThread.ManagedThreadId}");
    //            subject.OnNext(obj);
    //        };
    //        notify(1);
    //        new Thread(notify).Start(2);
    //        new Thread(notify).Start(3);
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "SubscribeOn and ObserveOn"
    //class Program {
    //    static void Main() {
    //        Console.WriteLine($"Starting on threadId:{Thread.CurrentThread.ManagedThreadId}");
    //        var source = Observable.Create<int>(
    //            o => {
    //                Console.WriteLine($"Invoked on threadId:{Thread.CurrentThread.ManagedThreadId}");
    //                o.OnNext(1);
    //                o.OnNext(2);
    //                o.OnNext(3);
    //                o.OnCompleted();
    //                Console.WriteLine($"Finished on threadId:{Thread.CurrentThread.ManagedThreadId}");
    //                return Disposable.Empty;
    //            });
    //        source
    //            //.SubscribeOn(Scheduler.ThreadPool)
    //            .Subscribe(
    //            o => Console.WriteLine($"Received {o} on threadId:{Thread.CurrentThread.ManagedThreadId}"),
    //            () => Console.WriteLine($"OnCompleted on threadId:{Thread.CurrentThread.ManagedThreadId}"));
    //        Console.WriteLine("Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Lock-ups"
    //class Program {
    //    static void Main() {
    //        var sequence = new Subject<int>();
    //        Console.WriteLine("Next line should lock the system.");
    //        var value =sequence.First();
    //        sequence.OnNext(1);
    //        Console.WriteLine("I can never execute...");
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Advanced features of schedulers"
    #region "Passing state"
    //class Program {
    //    static void Main() {
    //        //var myName = "Lee";
    //        //Scheduler.NewThread.Schedule(() => Console.WriteLine(myName));
    //        //Console.ReadLine();

    //        //var myName = "Lee";
    //        //IScheduler scheduler = Scheduler.Immediate;
    //        //scheduler.Schedule(
    //        //() => Console.WriteLine("myName = {0} in thread {1}", myName, Thread.CurrentThread.ManagedThreadId));
    //        //myName = "John";//What will get written to the console?
    //        //Console.WriteLine("myName = {0} in thread {1}", myName, Thread.CurrentThread.ManagedThreadId);
    //        //Console.ReadLine();

    //        //var myName = "Lee";
    //        //IScheduler scheduler = Scheduler.Immediate;
    //        //scheduler.Schedule(
    //        //    myName, (_, state) => {
    //        //        Console.WriteLine(state);
    //        //        return Disposable.Empty;
    //        //    });
    //        //myName = "John";
    //        //Console.ReadLine();

    //            var list = new List<int>();
    //            IScheduler scheduler = Scheduler.CurrentThread;
    //            scheduler.Schedule(list,
    //                (innerScheduler, state) => {
    //                    Console.WriteLine(state.Count);
    //                    return Disposable.Empty;
    //                });
    //            list.Add(1);          
    //        Console.Read();
    //    }
    //}
    #endregion

    #region "Future scheduling"
    //class Program {
    //    static void Main() {
    //        var delay = TimeSpan.FromSeconds(1);
    //        Console.WriteLine($"Before schedule at {DateTime.Now:o}");
    //        IScheduler scheduler = Scheduler.NewThread;
    //        scheduler.Schedule(delay,
    //            ()=> Console.WriteLine($"Inside schedule at { DateTime.Now:o}"));
    //        Console.WriteLine($"After schedule at {DateTime.Now:o}");
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Cancelation"
    //class Program {
    //    static void Main() {
    //        var delay = TimeSpan.FromSeconds(1);
    //        Console.WriteLine($"Before schedule at {DateTime.Now:o}");
    //        var scheduler = Scheduler.NewThread;
    //        var token = scheduler.Schedule(
    //            delay,
    //            () => Console.WriteLine($"Inside schedule at {DateTime.Now:o}"));
    //        Console.WriteLine($"After schedule at  {DateTime.Now:o}");
    //        token.Dispose();
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Cancelation"
    //class Program {
    //    static void Main() {
    //        var list = new List<int>();
    //        Console.WriteLine("Enter to quit:");
    //        IScheduler scheduler = Scheduler.NewThread;
    //        var token = scheduler.Schedule(list, Work);
    //        Console.ReadLine();
    //        Console.WriteLine("Cancelling...");
    //        token.Dispose();
    //        Console.WriteLine("Cancelled");
    //        Console.ReadLine();
    //    }

    //    public static IDisposable Work(IScheduler scheduler, List<int> list) {
    //        var tokenSource = new CancellationTokenSource();
    //        var cancelToken = tokenSource.Token;
    //        var task = new Task(() => {
    //            Console.WriteLine();
    //            for (int i = 0; i < 1000; i++) {
    //                var sw = new SpinWait();
    //                for (int j = 0; j < 3000; j++) sw.SpinOnce();
    //                Console.Write(".");
    //                list.Add(i);
    //                if (cancelToken.IsCancellationRequested) {
    //                    Console.WriteLine("Cancelation requested");
    //                    //cancelToken.ThrowIfCancellationRequested();
    //                    return;
    //                }
    //            }
    //        }, cancelToken);
    //        task.Start();
    //        return Disposable.Create(tokenSource.Cancel);
    //    }
    //}
    #endregion

    #region "Recursion"
    //class Program {
    //    static void Main() {
    //        Action<Action> work = (Action self) => {
    //            Console.WriteLine("Running");
    //            self();
    //        };
    //        var token = Scheduler.NewThread.Schedule(work);
    //        Console.ReadLine();
    //        Console.WriteLine("Cancelling");
    //        token.Dispose();
    //        Console.WriteLine("Cancelled");
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Creating your own iterator"
    //static class Program {
    //    static void Main() {
    //        var source = new FileStream(@"D:\Somefile.txt", FileMode.Open, FileAccess.Read);
    //        //var factory = Observable.FromAsyncPattern<byte[], int, int, int>(source.BeginRead, source.EndRead);
    //        //var buffer = new byte[source.Length];
    //        //IObservable<int> reader = factory(buffer, 0, (int)source.Length);
    //        //reader.Subscribe(
    //        //bytesRead =>
    //        //Console.WriteLine("Read {0} bytes from file into buffer", bytesRead));


    //        //var sourceSequence = source.ToObservable((int)source.Length, Scheduler.CurrentThread);
    //        //var sourceSequence = source.ToObservable_((int)source.Length, Scheduler.CurrentThread);
    //        var sourceSequence = source.ToObservable__((int)source.Length, Scheduler.CurrentThread);
    //        sourceSequence.Subscribe(s => Console.WriteLine(s));
    //        Console.ReadLine();
    //    }

    //    public static IObservable<byte> ToObservable__(this FileStream source, int bufferSize, IScheduler scheduler) {
    //        var bytes = Observable.Create<byte>(o => {
    //            var initialState = new StreamReaderState(source, bufferSize);
    //            var currentStateSubscription = new SerialDisposable();
    //            Action<StreamReaderState, Action<StreamReaderState>> iterator =
    //            (state, self) =>
    //            currentStateSubscription.Disposable = state.ReadNext().Subscribe(
    //                bytesRead => {
    //                    for (int i = 0; i < bytesRead; i++) {
    //                        o.OnNext(state.Buffer[i]);
    //                    }
    //                    if (bytesRead > 0)
    //                        self(state);
    //                    else
    //                        o.OnCompleted();
    //                }, o.OnError);
    //            var scheduledWork = scheduler.Schedule(initialState, iterator);
    //            return new CompositeDisposable(currentStateSubscription, scheduledWork);
    //        });
    //        return Observable.Using(() => source, _ => bytes);
    //    }

    //    public static IObservable<byte> ToObservable(this FileStream source, int bufferSize, IScheduler scheduler) {
    //        var bytes = Observable.Create<byte>(o => {
    //            var initialState = new StreamReaderState(source, bufferSize);
    //            initialState.ReadNext().Subscribe(
    //                    bytesRead => {
    //                        for (int i = 0; i < bytesRead; i++) {
    //                            o.OnNext(initialState.Buffer[i]);
    //                        }
    //                    });
    //            return Disposable.Empty;
    //        });
    //        return Observable.Using(() => source, _ => bytes);
    //    }

    //    public static IObservable<byte> ToObservable_(this FileStream source, int bufferSize, IScheduler scheduler) {
    //        var bytes = Observable.Create<byte>(o => {
    //            var initialState = new StreamReaderState(source, bufferSize);
    //            Action<StreamReaderState, Action<StreamReaderState>> iterator;
    //            iterator = (state, self) => {
    //                state.ReadNext().Subscribe(
    //                    bytesRead => {
    //                        for (int i = 0; i < bytesRead; i++) {
    //                            o.OnNext(state.Buffer[i]);
    //                        }
    //                        self(state);
    //                    });
    //            };
    //            return scheduler.Schedule(initialState, iterator);
    //        });
    //        return Observable.Using(() => source, _ => bytes);
    //    }
    //}

    //public sealed class StreamReaderState {
    //    private readonly int _bufferSize;
    //    private readonly Func<byte[], int, int, IObservable<int>> _factory;
    //    public byte[] Buffer { get; set; }
    //    public StreamReaderState(FileStream source, int bufferSize) {
    //        _bufferSize = bufferSize;
    //        _factory = Observable.FromAsyncPattern<byte[], int, int, int>(
    //            source.BeginRead,
    //            source.EndRead);
    //        Buffer = new byte[bufferSize];
    //    }

    //    public IObservable<int> ReadNext() {
    //        return _factory(Buffer, 0, _bufferSize);
    //    }
    //}
    #endregion

    #region "Schedulers in-depth"
    //class Program {
    //    static void Main() {
    //        CurrentThreadExample();
    //        ImmediateExample();
    //        Console.ReadLine();
    //    }
    //    private static void ScheduleTasks(IScheduler scheduler) {
    //        Action leafAction = () => Console.WriteLine("----leafAction.");
    //        Action innerAction = () => {
    //            Console.WriteLine("--innerAction start.");
    //            scheduler.Schedule(leafAction);
    //            Console.WriteLine("--innerAction end.");
    //        };
    //        Action outerAction = () => {
    //            Console.WriteLine("outer start.");
    //            scheduler.Schedule(innerAction);
    //            Console.WriteLine("outer end.");
    //        };
    //        scheduler.Schedule(outerAction);
    //    }

    //    public static void CurrentThreadExample() {
    //        ScheduleTasks(Scheduler.CurrentThread);
    //        /*Output:
    //        outer start.
    //        outer end.
    //        --innerAction start.
    //        --innerAction end.
    //        ----leafAction.
    //        */
    //    }

    //    public  static void ImmediateExample() {
    //        ScheduleTasks(Scheduler.Immediate);
    //        /*Output:
    //        outer start.
    //        --innerAction start.
    //        ----leafAction.
    //        --innerAction end.
    //        outer end.
    //        */
    //    }
    //}
    #endregion

    #region "DispatcherScheduler"
    //class Program {
    //    static void Main() {
    //        var Lines = new List<string>();
    //        var filePath = @"D:\Somefile.txt";
    //        var fileLines = Observable.Create<string>(
    //            o => {
    //                var dScheduler = DispatcherScheduler.Instance;
    //                var lines = File.ReadAllLines(filePath);
    //                foreach (var line in lines) {
    //                    var localLine = line;
    //                    dScheduler.Schedule(
    //                        () => o.OnNext(localLine));
    //                }
    //                return Disposable.Empty;
    //            });
    //        fileLines
    //            .SubscribeOn(Scheduler.ThreadPool)
    //            .Subscribe(line => Lines.Add(line));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "New Thread"
    //class Program {
    //    static void Main() {
    //        Console.WriteLine($"Starting on thread :{Thread.CurrentThread.ManagedThreadId}");
    //        Scheduler.TaskPool.Schedule("A", OuterAction);
    //        Scheduler.TaskPool.Schedule("B", OuterAction);
    //        Console.ReadLine();
    //    }

    //    private static IDisposable OuterAction(IScheduler scheduler, string state) {
    //        Console.WriteLine($"{state} start. ThreadId:{Thread.CurrentThread.ManagedThreadId}");
    //        scheduler.Schedule(state + ".inner", InnerAction);
    //        Console.WriteLine($"{state} end. ThreadId:{Thread.CurrentThread.ManagedThreadId}");
    //        return Disposable.Empty;
    //    }

    //    private static IDisposable InnerAction(IScheduler scheduler, string state) {
    //        Console.WriteLine($"{state} start. ThreadId:{Thread.CurrentThread.ManagedThreadId}");
    //        scheduler.Schedule(state + ".Leaf", LeafAction);
    //        Console.WriteLine("{0} end. ThreadId:{1}", state, Thread.CurrentThread.ManagedThreadId);
    //        return Disposable.Empty;
    //    }

    //    private static IDisposable LeafAction(IScheduler scheduler, string state) {
    //        Console.WriteLine($"{state}. ThreadId:{Thread.CurrentThread.ManagedThreadId}");
    //        return Disposable.Empty;
    //    }
    //}
    #endregion
    #endregion
}
