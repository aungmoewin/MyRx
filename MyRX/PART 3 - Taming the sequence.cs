using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRX {
    #region "Side effects"
    #region "Issues with side effects"
    //class Program {
    //    static void Main() {
    //        var letters = Observable.Range(0, 3)
    //            .Select(i => (char)(i + 65));
    //        var index = -1;
    //        var result = letters.Select(
    //            c => {
    //                index++;
    //                return c;
    //            });
    //        result.Subscribe(
    //            c => Console.WriteLine($"Received {c} at index {index}"),
    //            () => Console.WriteLine("completed"));
    //        //Side effect
    //        result.Subscribe(
    //            c => Console.WriteLine($"Also received {c} at index {index}"),
    //            () => Console.WriteLine("2nd completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Composing data in a pipeline"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Range(0, 3);

    //        var result = source.Select(
    //            (idx, value) => new {
    //                Index = idx,
    //                Letter = (char)(value + 65)
    //            });

    //        ////Equivalent to
    //        //var result = source.Scan(
    //        //    new {
    //        //        Index = -1,
    //        //        Letter = new char()
    //        //    },
    //        //    (acc, value) => new {
    //        //        Index = acc.Index + 1,
    //        //        Letter = (char)(value + 65)
    //        //    });


    //        result.Subscribe(
    //            x => Console.WriteLine($"Received {x.Letter} at index {x.Index}"),
    //            () => Console.WriteLine("completed"));
    //        result.Subscribe(
    //            x => Console.WriteLine($"Also received {x.Letter} at index {x.Index}"),
    //            () => Console.WriteLine("2nd completed"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Do"
    ////class Program {
    ////    static void Main() {
    ////        var source = Observable
    ////            .Interval(TimeSpan.FromSeconds(1))
    ////            .Take(3);
    ////        var result = source.Do(
    ////            i => Log(i),
    ////            ex => Log(ex),
    ////            () => Log());
    ////        result.Subscribe(
    ////            Console.WriteLine,
    ////            () => Console.WriteLine("completed"));
    ////        Console.ReadKey();
    ////    }

    ////    private static void Log(object onNextValue) {
    ////        Console.WriteLine($"Logging OnNext({onNextValue}) @ { DateTime.Now}");
    ////    }
    ////    private static void Log(Exception onErrorValue) {
    ////        Console.WriteLine($"Logging OnError({onErrorValue}) @ {DateTime.Now}");
    ////    }

    ////    private static void Log() {
    ////        Console.WriteLine($"Logging OnCompleted()@ {DateTime.Now}");
    ////    }
    ////}

    //class Program {
    //    private static IObservable<long> GetNumbers() {
    //        return Observable.Interval(TimeSpan.FromMilliseconds(250))
    //            .Do(i => Console.WriteLine($"pushing {i} from GetNumbers"));
    //    }

    //    static void Main() {
    //        var source = GetNumbers();
    //        var result = source.Where(i => i % 3 == 0)
    //            .Take(3)
    //            .Select(i => (char)(i + 65));
    //        result.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("completed"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Encapsulating with AsObservable"
    //class ObscuredLeakinessLetterRepo {
    //    private readonly ReplaySubject<string> _letters;
    //    public ObscuredLeakinessLetterRepo() {
    //        _letters = new ReplaySubject<string>();
    //        _letters.OnNext("A");
    //        _letters.OnNext("B");
    //        _letters.OnNext("C");
    //    }
    //    public IObservable<string> Letters {
    //        get { return _letters; }
    //    }

    //    public IObservable<string> GetLetters() {
    //        return _letters;
    //    }

    //    static void Main() {
    //        var repo = new ObscuredLeakinessLetterRepo();
    //        var good = repo.GetLetters();
    //        var evil = repo.GetLetters();
    //        good.Subscribe(Console.WriteLine);
    //        //Be naughty
    //        var asSubject = evil as ISubject<string>;
    //        if (asSubject != null) {
    //            //So naughty, 1 is not a letter!
    //            asSubject.OnNext("1");
    //        }
    //        else {
    //            Console.WriteLine("could not sabotage");
    //        }
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Mutable elements cannot be protected"
    //public class Account {
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    //class Program {
    //    static void Main() {
    //        var source = new Subject<Account>();
    //        //Evil code. It modifies the Account object.
    //        source.Subscribe(account => account.Name = "Garbage");
    //        //unassuming well behaved code
    //        source.Subscribe(
    //        account => Console.WriteLine("{0} {1}", account.Id, account.Name),
    //        () => Console.WriteLine("completed"));
    //        source.OnNext(new Account { Id = 1, Name = "Microsoft" });
    //        source.OnNext(new Account { Id = 2, Name = "Google" });
    //        source.OnNext(new Account { Id = 3, Name = "IBM" });
    //        source.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion
    #endregion

    #region "Leaving the monad"
    #region "ForEach"
    //class Program {
    //    static void Main() {
    //        var source1 = Observable.Interval(TimeSpan.FromSeconds(1))
    //            .Take(5);
    //        source1.ForEach(i => Console.WriteLine($"received {i} @ {DateTime.Now}"));
    //        Console.WriteLine($"completed @{DateTime.Now}");

    //        var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
    //        source.Subscribe(i => Console.WriteLine($"received {i} @ {DateTime.Now}"));
    //        Console.WriteLine($"completed @ {DateTime.Now}");
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "ToEnumerable"
    //class Program {
    //    static void Main() {
    //        var period = TimeSpan.FromMilliseconds(2000);
    //        var source = Observable.Timer(TimeSpan.Zero,period).Take(5);
    //        var result = source.ToEnumerable();
    //        try {
    //            foreach (var value in result) {
    //                Console.WriteLine(value);
    //            }
    //        }
    //        catch (Exception e) {
    //            Console.WriteLine(e.Message);
    //        }
    //        Console.WriteLine("Done");
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "ToArray and ToList"
    //class Program {
    //    static void Main() {
    //        var period = TimeSpan.FromMilliseconds(2000);
    //        var source = Observable.Timer(TimeSpan.Zero, period).Take(5);
    //        var result = source.ToArray();
    //        result.Subscribe(
    //            arr => {
    //                Console.WriteLine("Received Array");
    //                foreach (var value in arr) {
    //                    Console.WriteLine(value);
    //                }
    //            },
    //            () => Console.WriteLine("Completed"));
    //        source.Dump("source");
    //        Console.WriteLine("Subscribed");
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "ToTask"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1))
    //            .Take(5);
    //        var result = source.ToTask(); //Will arrive in 5 seconds. 
    //        Console.WriteLine(result.Result);
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "ToEvent"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);
    //        var result = source.ToEvent();
    //        result.OnNext += val => Console.WriteLine(val);
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region ToEventPattern
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1))
    //             .Select(i => new EventPattern<MyEventArgs>(new { }, new MyEventArgs(i)));

    //    }
    //}

    //public class EventPattern<TEventArgs> : IEquatable<EventPattern<TEventArgs>>
    //    where TEventArgs : EventArgs {
    //    public EventPattern(object sender, TEventArgs e) {
    //        this.Sender = sender;
    //        this.EventArgs = e;
    //    }
    //    public object Sender { get; private set; }
    //    public TEventArgs EventArgs { get; set; }

    //    public bool Equals(EventPattern<TEventArgs> other) {
    //        throw new NotImplementedException();
    //    }
    //}

    //public interface IEventPatternSource<TEventArgs> where TEventArgs : EventArgs {
    //    event EventHandler<TEventArgs> OnNext;
    //}
    //public class MyEventArgs : EventArgs {
    //    private readonly long _value;
    //    public MyEventArgs(long value) {
    //        _value = value;
    //    }
    //    public long Value {
    //        get { return _value; }
    //    }
    //}

    #endregion

    #endregion

    #region "Advanced error handling"
    #region "Catch"
    //class Program {
    //    static void Main() {
    //        var source = new Subject<int>();
    //        // source.Dump("Source");
    //        var result = source.Catch(Observable.Empty<int>());
    //        result.Dump("Catch");
    //        source.OnNext(1);
    //        source.OnNext(2);
    //        source.OnNext(3);
    //        source.OnError(new Exception("Fail!"));

    //        Console.ReadLine();
    //    }


    //    //static void Main() {
    //    //    var source = new Subject<int>();
    //    //    var result = source.Catch<int, TimeoutException>(_ => Observable.Return(-1));
    //    //    //result.Dump("Catch");
    //    //    result.Subscribe(Console.WriteLine);
    //    //    source.OnNext(1);
    //    //    source.OnError(new TimeoutException());
    //    //    source.OnNext(2);
    //    //    Console.ReadLine();
    //    //}

    //    //static void Main() {
    //    //    var source = new Subject<int>();
    //    //    var result = source.Catch<int, TimeoutException>(tx => Observable.Return(-1));
    //    //    result.Dump("Catch");
    //    //    source.OnNext(1);
    //    //    source.OnNext(2);
    //    //    source.OnError(new ArgumentException("Fail!"));
    //    //    Console.ReadLine();
    //    //}
    //}
    #endregion

    #region "Finally"
    //class Program {
    //    //static void Main() {
    //    //    var source = new Subject<int>();
    //    //    var result = source.Finally(() => Console.WriteLine("Finally action ran"));
    //    //    result.Dump("Finally");
    //    //    source.OnNext(1);
    //    //    source.OnNext(2);
    //    //    source.OnNext(3);
    //    //    source.OnCompleted();
    //    //    Console.ReadLine();
    //    //}

    //    //static void Main() {
    //    //    var source = new Subject<int>();
    //    //    var result = source.Finally(()=>Console.WriteLine("Finally"));
    //    //    var subscription = result.Subscribe(
    //    //        Console.WriteLine,
    //    //        Console.WriteLine,
    //    //        ()=>Console.WriteLine("Completed"));
    //    //    source.OnNext(1);
    //    //    source.OnNext(2);
    //    //    source.OnNext(3);
    //    //    subscription.Dispose();
    //    //    Console.ReadLine();
    //    //}

    //    //static void Main() {
    //    //    var source = new Subject<int>();
    //    //    var result = source.Finally(() => Console.WriteLine("Finally"));
    //    //    result.Subscribe(
    //    //        Console.WriteLine,
    //    //        //Console.WriteLine,
    //    //        () => Console.WriteLine("Completed"));
    //    //    source.OnNext(1);
    //    //    source.OnNext(2);
    //    //    source.OnNext(3);
    //    //    //Brings the app down. Finally action is not called.
    //    //    source.OnError(new Exception("Fail"));
    //    //    Console.ReadLine();
    //    //}
    //}

    //static class FinallyExtension {
    //    public static IObservable<T> MyFinally<T>(this IObservable<T> source, Action finallyAction) {
    //        return Observable.Create<T>(o => {
    //            var finallyOnce = Disposable.Create(finallyAction);
    //            var subscription = source.Subscribe(
    //                o.OnNext,
    //                ex => {
    //                    try { o.OnError(ex); }
    //                    finally { finallyOnce.Dispose(); }
    //                },
    //                () => {
    //                    try { o.OnCompleted(); }
    //                    finally { finallyOnce.Dispose(); }
    //                });
    //            return new CompositeDisposable(subscription, finallyOnce);
    //        });
    //    }
    //}
    #endregion

    #region "Using"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1));
    //        var result = Observable.Using(
    //            () => new TimeIt("Subscription Timer"),
    //            TimeIt => source);
    //        result.Take(5).Dump("Using");
    //        Console.ReadLine();
    //    }
    //}

    //public class TimeIt : IDisposable {
    //    private readonly string _name;
    //    private readonly Stopwatch _watch;

    //    public TimeIt(string name) {
    //        _name = name;
    //        _watch = Stopwatch.StartNew();
    //    }

    //    public void Dispose() {
    //        _watch.Stop();
    //        Console.WriteLine($"{_name} take {_watch.Elapsed}");
    //    }

    //    static void DoSomeWork(string sample) { }
    //    static void Cleanup() { }

    //    //static void Main() {
    //    //    using (new TimeIt("Outer Scope")) {
    //    //        using (new TimeIt("Inner Scope A")) {
    //    //            DoSomeWork("A");
    //    //        }
    //    //        using (new TimeIt("Inner Scope B")) {
    //    //            DoSomeWork("B");
    //    //        }
    //    //        Cleanup();
    //    //    }
    //    //    Console.ReadKey();
    //    //}
    //}
    #endregion

    #region "Retry"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(0.5));
    //        RetrySample(source);
    //        Console.ReadLine();
    //    }
    //    static void RetrySample<T>(IObservable<T> source) {
    //        source.Retry(2).Subscribe(t => Console.WriteLine(t));
    //    }
    //}
    #endregion
    #endregion

    #region "Combining sequences"
    #region "Sequential concatenation"
    #region "Concat"
    //class Program {
    //    static void Main() {
    //        var s1 = Observable.Range(0, 3);
    //        var s2 = Observable.Range(5, 5);
    //        s1.Concat(s2)
    //            .Subscribe(Console.WriteLine);
    //        Console.ReadLine();
    //    }
    //}

    ////class Program {
    ////    static void Main() {
    ////        GetSequences().Concat().Dump("Concat");
    ////        Console.ReadLine();
    ////    }
    ////    public static IEnumerable<IObservable<long>> GetSequences() {
    ////        Console.WriteLine("GetSequences() called");
    ////        Console.WriteLine("Yield 1st sequence");
    ////        yield return Observable.Create<long>(o => {
    ////            Console.WriteLine("1st subscribed to");
    ////            return Observable.Timer(TimeSpan.FromMilliseconds(500))
    ////            .Select(i => 1L)
    ////            .Subscribe(o);
    ////        });
    ////        Console.WriteLine("Yield 2nd sequence");
    ////        yield return Observable.Create<long>(o => {
    ////            Console.WriteLine("2nd subscribed to");
    ////            return Observable.Timer(TimeSpan.FromMilliseconds(300))
    ////            .Select(i => 2L)
    ////            .Subscribe(o);
    ////        });
    ////        Thread.Sleep(1000); //Force a delay
    ////        Console.WriteLine("Yield 3rd sequence");
    ////        yield return Observable.Create<long>(o => {
    ////            Console.WriteLine("3rd subscribed to");
    ////            return Observable.Timer(TimeSpan.FromMilliseconds(100))
    ////            .Select(i => 3L)
    ////            .Subscribe(o);
    ////        });

    ////    }
    ////}
    #endregion

    #region "Repeat"
    //class program {
    //    static void Main() {
    //        var source = Observable.Range(0, 3);
    //        var result = source.Repeat(3);
    //        result.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "StartWith"
    //class program {
    //    static void Main() {
    //        var source = Observable.Range(0, 3);
    //        var result = source.StartWith(-3, -2, -1);
    //        result.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion
    #endregion

    #region "Concurrent sequences"
    #region "Amb"
    //class Program {
    //    static void Main() {
    //        var s1 = new Subject<int>();
    //        var s2 = new Subject<int>();
    //        var s3 = new Subject<int>();
    //        //var result = Observable.Amb(s1, s2, s3);
    //        var result = Observable.Amb(s1).Amb(s2).Amb(s3);
    //        result.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("Completed"));
    //        s1.OnNext(1);
    //        s2.OnNext(2);
    //        s3.OnNext(3);
    //        s1.OnNext(1);
    //        s2.OnNext(2);
    //        s3.OnNext(3);
    //        s1.OnCompleted();
    //        s2.OnCompleted();
    //        s3.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}

    ////class Program {
    ////    static void Main() {
    ////        GetSequences().Amb().Dump("Amb");
    ////        Console.ReadLine();
    ////    }

    ////    public static IEnumerable<IObservable<long>> GetSequences() {
    ////        Console.WriteLine("getsequences() called");
    ////        Console.WriteLine("yield 1st sequence");
    ////        yield return Observable.Create<long>(o => {
    ////            Console.WriteLine("1st subscribed to");
    ////            return Observable.Timer(TimeSpan.FromMilliseconds(500))
    ////            .Select(i => 1l)
    ////            .Subscribe(o);
    ////        });
    ////        Console.WriteLine("yield 2nd sequence");
    ////        yield return Observable.Create<long>(o => {
    ////            Console.WriteLine("2nd subscribed to");
    ////            return Observable.Timer(TimeSpan.FromMilliseconds(300))
    ////            .Select(i => 2l)
    ////            .Subscribe(o);
    ////        });
    ////        Thread.Sleep(5000); //force a delay
    ////        Console.WriteLine("yield 3rd sequence");
    ////        yield return Observable.Create<long>(o => {
    ////            Console.WriteLine("3rd subscribed to");
    ////            return Observable.Timer(TimeSpan.FromMilliseconds(100))
    ////            .Select(i => 3l)
    ////            .Subscribe(o);
    ////        });

    ////    }
    ////}
    #endregion

    #region "Merge"
    //class Program {
    //    static void Main() {
    //        ////    //Generate values 0,1,2 
    //        ////    var s1 = Observable.Interval(TimeSpan.FromMilliseconds(250))
    //        ////        .Take(3);
    //        ////    //Generate values 100,101,102,103,104 
    //        ////    var s2 = Observable.Interval(TimeSpan.FromMilliseconds(150))
    //        ////        .Take(5)
    //        ////        .Select(i => i + 100);
    //        ////    s1.Merge(s2)
    //        ////        .Subscribe(
    //        ////        Console.WriteLine,
    //        ////        () => Console.WriteLine("Completed"));
    //        ////    Console.ReadLine();

    //        GetSequences().Merge().Dump("Merge");
    //        Console.ReadLine();
    //    }

    //    public static IEnumerable<IObservable<long>> GetSequences() {
    //        Console.WriteLine("GetSequences() called");
    //        Console.WriteLine("Yield 1st sequence");
    //        yield return Observable.Create<long>(o => {
    //            Console.WriteLine("1st subscribed to");
    //            return Observable.Timer(TimeSpan.FromMilliseconds(500))
    //            .Select(i => 1L)
    //            .Subscribe(o);
    //        });
    //        Console.WriteLine("Yield 2nd sequence");
    //        yield return Observable.Create<long>(o => {
    //            Console.WriteLine("2nd subscribed to");
    //            return Observable.Timer(TimeSpan.FromMilliseconds(300))
    //            .Select(i => 2L)
    //            .Subscribe(o);
    //        });
    //        Thread.Sleep(1000); //Force a delay
    //        Console.WriteLine("Yield 3rd sequence");
    //        yield return Observable.Create<long>(o => {
    //            Console.WriteLine("3rd subscribed to");
    //            return Observable.Timer(TimeSpan.FromMilliseconds(100))
    //            .Select(i => 3L)
    //            .Subscribe(o);
    //        });
    //    }
    //}
    #endregion

    #region "Switch"
    //class Program {
    //    static void Main() {
    //        // for cleaning up the hot channel streams
    //        var disposable = new CompositeDisposable();

    //        // some channels
    //        var fuzz = CreateChannelStream("Fuzz", disposable);
    //        var channel1 = CreateChannelStream("Channel1", disposable);
    //        var channel2 = CreateChannelStream("Channel2", disposable);

    //        // the button press event streams
    //        var eButton1 = new Subject<Unit>();
    //        var eButton2 = new Subject<Unit>();

    //        //the button presses are projected to
    //        // the respective channel streams
    //        // note, you could obtain the channel via a function call here
    //        // if you wanted to - to keep it close to the slides I'm not.
    //        var eChan1 = eButton1.Select(_ => channel1);
    //        var eChan2 = eButton2.Select(_ => channel2);

    //        // create the selection "stream of streams"
    //        // an IObservable<IObservable<string>> here
    //        // that starts with "fuzz"
    //        var sel = Observable.Merge(eChan1, eChan2).StartWith(fuzz);

    //        // flatten and select the most recent stream with Switch
    //        var screen = sel.Switch();
    //        // subscribe to the screen and print the frames
    //        // it will start with "fuzz"
    //        disposable.Add(screen.Subscribe(Console.WriteLine));

    //        bool quit = false;
    //        // a little test loop
    //        // entering 1 or 2 will switch
    //        // to that channel
    //        while (!quit) {
    //            var chan = Console.ReadLine();
    //            switch (chan.ToUpper()) {
    //                case "1":
    //                    // raise a button 1 event
    //                    eButton1.OnNext(Unit.Default);
    //                    break;
    //                case "2":
    //                    // raise a button 2 event
    //                    eButton2.OnNext(Unit.Default);
    //                    break;
    //                case "Q":
    //                    quit = true;
    //                    break;
    //            }
    //        }
    //        disposable.Dispose();
    //    }

    //    private static IObservable<string> CreateChannelStream(
    //        string name, CompositeDisposable disposables) {
    //        // this hacks together a demo channel stream -
    //        // a stream of "frames" for the channel
    //        // for simplicity rather than using images, I use a string
    //        // message for each frame
    //        // how it works isn't important, just know you'll get a
    //        // message event every second
    //        var channel = Observable.Interval(TimeSpan.FromSeconds(1))
    //            .Select(x => name + "Frame : " + x)
    //            .Publish();
    //        disposables.Add(channel.Connect());
    //        return channel;
    //    }
    //}
    #endregion
    #endregion

    #region "Concurrent sequences"
    #region "CombineLatest"
    //public class CombineType {
    //    public long Int1;
    //    public long Int2;
    //    public string str;

    //    public CombineType() { }
    //    public CombineType(CombineType c1, CombineType c2) {
    //        Init(c1);
    //        Init(c2);
    //    }

    //    private void Init(CombineType c) {
    //        Int1 = c.Int1 == 0 ? Int1 : c.Int1;
    //        Int2 = c.Int2 == 0 ? Int2 : c.Int2;
    //        str = c.str ?? str;
    //    }
    //    public override string ToString() {
    //        return $"Int1:{Int1}, Int2:{Int2}, str:{str}";
    //    }
    //}
    //class Program {
    //    static void Main() {
    //        //Console.WriteLine("Press any key to unsubscribe");
    //        //using (Xs.CombineLatest(Ys, (x, y) => x + y).Timestamp().Subscribe(
    //        //    z => Console.WriteLine($"{z.Value,3}:{z.Timestamp}"),
    //        //    () => Console.WriteLine("Completed, press a key")
    //        //    )) {
    //        //    Console.ReadKey();
    //        //}
    //        //Console.WriteLine("Press any key to exit");
    //        //Console.ReadKey();
    //        var obsInt1 = Observable.Interval(TimeSpan.FromSeconds(1)).Select(i => new CombineType { Int1 = i });
    //        var obsString = Observable.Interval(TimeSpan.FromSeconds(0.5)).Select(i => new CombineType { str = $"Item {i}" });
    //        var obsInt2 = Observable.Interval(TimeSpan.FromSeconds(0.25)).Select(i => new CombineType { Int2 = i }); ;
    //        Observable.Merge(obsInt1, obsString, obsInt2)
    //            //.Scan((c1, c2) => new CombineType(c1, c2)).StartWith(new CombineType { Int1 = 1, Int2 = 2 })
    //            .Subscribe(c => Console.WriteLine(c));
    //        Console.ReadLine();
    //    }


    //    private static IObservable<int> Xs {
    //        get { return Generate(0, new List<int> { 1, 2, 2, 2, 2 }); }
    //    }

    //    private static IObservable<int> Ys {
    //        get { return Generate(100, new List<int> { 2, 2, 2, 2, 2 }); }
    //    }
    //    private static IObservable<int> Generate(int initialValue, IList<int> intervals) {
    //        // work-around for Observable.Generate calling timeInterval before resultSelector
    //        intervals.Add(0);
    //        return Observable.Generate(
    //            initialValue,
    //            x => x < initialValue + intervals.Count - 1,
    //            x => x + 1,
    //            x => x,
    //            x => TimeSpan.FromSeconds(intervals[x - initialValue]));
    //    }
    //}
    #endregion

    #region "Zip"
    //class Program {
    //    static void Main() {
    //        //Generate values 0,1,2 
    //        var num = Observable.Interval(TimeSpan.FromMilliseconds(250))
    //            .Take(3);
    //        //Generate values 0,1,2 
    //        var chars = Observable.Interval(TimeSpan.FromMilliseconds(150))
    //            .Take(6)
    //            .Select(i => Char.ConvertFromUtf32((int)i + 97));
    //        //Zip values together
    //        num.Zip(chars, (lhs, rhs) => new { Left = lhs, Right = rhs })
    //            .Dump("Zip");
    //        Console.ReadLine();
    //    }
    //}

    //class Program {
    //    static void Main() {
    //        var mm = new Subject<Coord>();
    //        var s1 = mm.Skip(1);
    //        var delta = mm.Zip(s1,
    //            (pre, curr) => new Coord {
    //                X = curr.X - pre.X,
    //                Y = curr.Y - pre.Y
    //            });
    //        delta.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("Completed"));
    //        mm.OnNext(new Coord { X = 0, Y = 0 });
    //        mm.OnNext(new Coord { X = 1, Y = 0 });//Move across 1
    //        mm.OnNext(new Coord { X = 3, Y = 2 });//Diagonally up 2
    //        mm.OnNext(new Coord { X = 0, Y = 0 });//Back to 0,0
    //        mm.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}

    //public class Coord {
    //    public int X { get; set; }
    //    public int Y { get; set; }
    //    public override string ToString() {
    //        return $"{X} {Y}";
    //    }
    //}
    #endregion

    #region "And-Then-When"
    //class Program {
    //    static void Main() {
    //        var one = Observable.Interval(TimeSpan.FromMilliseconds(1)).Take(5);
    //        var two = Observable.Interval(TimeSpan.FromMilliseconds(250)).Take(10);
    //        var three = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(14);

    //        //lhs represents 'Left Hand Side'
    //        //rhs represents 'Right Hand Side'

    //        //var zippedSequence = one
    //        //    .Zip(two, (lhs, rhs) => new { One = lhs, Two = rhs })
    //        //    .Zip(three, (lhs, rhs) => new { One = lhs.One, Two = lhs.Two, Three = rhs });

    //        ////Another way
    //        var pattern = one.And(two).And(three);
    //        var plan = pattern.Then((first, second, third) => new { One = first, Second = second, Three = third });
    //        var zippedSequence = Observable.When(plan);

    //        ////Another way
    //        //var zippedSequence = Observable.When(
    //        //    one.And(two)
    //        //    .And(three)
    //        //    .Then((first, second, third) => new {
    //        //        One=first,
    //        //        Two=second,
    //        //        Three=third
    //        //    }));

    //        zippedSequence.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("Completed"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion
    #endregion
    #endregion

    #region "Time-shifted sequences"
    #region "Buffer"
    //class Program {
    //    static void Main() {
    //        var idealBatchSize = 15;
    //        var maxTimeDelay = TimeSpan.FromSeconds(3);
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10)
    //        .Concat(Observable.Interval(TimeSpan.FromSeconds(0.01)).Take(100));
    //        source.Buffer(maxTimeDelay, idealBatchSize)
    //        .Subscribe(
    //        buffer => Console.WriteLine("Buffer of {1} @ {0}", DateTime.Now, buffer.Count),
    //        () => Console.WriteLine("Completed"));

    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Overlapping buffers by count"
    //class Program {
    //    /// Overlapping behavior
    //    ///     skip < count
    //    /// Standard behavior
    //    ///     skip = count           
    //    /// Skip behavior
    //    ///     skip > count
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);

    //        /// Overlapping behavior
    //        //source.Buffer(3, 1)
    //        //    .Subscribe(
    //        //    buffer => {
    //        //        Console.WriteLine("--Buffered values");
    //        //        foreach (var value in buffer) {
    //        //            Console.WriteLine(value);
    //        //        }
    //        //    }, () => Console.WriteLine("Completed"));

    //        //// Standard behavior
    //        //source.Buffer(3, 3)
    //        //   .Subscribe(
    //        //   buffer => {
    //        //       Console.WriteLine("--Buffered values");
    //        //       foreach (var value in buffer) {
    //        //           Console.WriteLine(value);
    //        //       }
    //        //   }, () => Console.WriteLine("Completed"));

    //        /// Skip behavior
    //        //source.Buffer(3, 5)
    //        //   .Subscribe(
    //        //   buffer => {
    //        //       Console.WriteLine("--Buffered values");
    //        //       foreach (var value in buffer) {
    //        //           Console.WriteLine(value);
    //        //       }
    //        //   }, () => Console.WriteLine("Completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Overlapping buffers by time"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
    //        var overlapped = source.Buffer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1));
    //        var standard = source.Buffer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
    //        var skipped = source.Buffer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5));

    //        overlapped.Subscribe(
    //            buffer => {
    //                Console.WriteLine("--Buffered values");
    //                foreach (var value in buffer) {
    //                    Console.WriteLine($"Overlapping Buffer => {value}");
    //                }
    //            }, () => Console.WriteLine("Completed"));

    //        //standard.Subscribe(
    //        //    buffer => {
    //        //        Console.WriteLine("--Buffered values");
    //        //        foreach (var value in buffer) {
    //        //            Console.WriteLine($"Standard Buffer => {value}");
    //        //        }
    //        //    }, () => Console.WriteLine("Completed"));

    //        //skipped.Subscribe(
    //        //    buffer => {
    //        //        Console.WriteLine("--Buffered values");
    //        //        foreach (var value in buffer) {
    //        //            Console.WriteLine($"Skipped Buffer => {value}");
    //        //        }
    //        //    }, () => Console.WriteLine("Completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Delay"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1))
    //            .Take(5)
    //            .Timestamp();
    //        var delay = source.Delay(TimeSpan.FromSeconds(2));
    //        source.Subscribe(
    //            value => Console.WriteLine($"source : {value}"),
    //            () => Console.WriteLine("source Completed"));
    //        delay.Subscribe(
    //            value => Console.WriteLine($"delay : {value}"),
    //            () => Console.WriteLine("delay Completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Sample"
    class Sample {
        static void Main() {
            //var interval = Observable.Interval(TimeSpan.FromMilliseconds(150));
            //interval.Subscribe(i => Console.WriteLine($"Interval => {i}"));
            //interval.Sample(TimeSpan.FromSeconds(1)).Subscribe(Console.WriteLine);

            var evenObs = Observable.Interval(TimeSpan.FromSeconds(5)).Where(t => t % 2 == 0);
            var oddObs = Observable.Interval(TimeSpan.FromSeconds(1)).Where(t => t % 2 != 0);
            evenObs.Subscribe(i => Debug.WriteLine($"e = {i}"));
            var sampleObs = oddObs.Sample(evenObs);
            // .Timestamp()
            sampleObs.Do(x => Debug.WriteLine($"o = {x}"))
            // .Materialize()
            .Zip(evenObs, (o, e) => new { e, o }).Subscribe(x => Debug.WriteLine(x));

            
            Console.ReadKey();
        }
    }
    #endregion

    #region "Throttle"
    //class Program {
    //    // Generates events with interval that alternates between 500ms and 1000ms every 5 events
    //    static IEnumerable<int> GenerateAlternatingFastAndSlowEvents() {
    //        int i = 0;

    //        while (true) {
    //            if (i > 1000) {
    //                yield break;
    //            }
    //            yield return i;
    //            Thread.Sleep(i++ % 10 < 5 ? 500 : 1000);
    //        }
    //    }

    //    static void Main() {
    //        //var observable = GenerateAlternatingFastAndSlowEvents().ToObservable().Timestamp();
    //        //var throttled = observable.Throttle(TimeSpan.FromMilliseconds(750));

    //        //using (throttled.Subscribe(x => Console.WriteLine("{0}: {1}", x.Value, x.Timestamp))) {
    //        //    Console.WriteLine("Press any key to unsubscribe");
    //        //    Console.ReadKey();
    //        //}

    //        //Console.WriteLine("Press any key to exit");


    //        var interval = Observable.Interval(TimeSpan.FromSeconds(2));
    //        interval.Subscribe(i => Console.WriteLine($"Interval => {i}"));
    //        interval.Throttle(TimeSpan.FromSeconds(1)).Subscribe(Console.WriteLine);
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Timeout"
    //class Program {
    //    static void Main() {
    //        ////var source = Observable.Interval(TimeSpan.FromMilliseconds(100)).Take(10)
    //        ////    .Concat(Observable.Interval(TimeSpan.FromSeconds(2)));
    //        ////var timeout = source.Timeout(TimeSpan.FromSeconds(1));
    //        ////timeout.Subscribe(
    //        ////    Console.WriteLine,
    //        ////    Console.WriteLine,
    //        ////    () => Console.WriteLine("Completed"));
    //        ////Console.ReadKey();

    //        var dueDate = DateTimeOffset.UtcNow.AddSeconds(4);
    //        var source = Observable.Interval(TimeSpan.FromSeconds(1));
    //        var timeout = source.Timeout(dueDate);
    //        timeout.Subscribe(
    //            Console.WriteLine,
    //            Console.WriteLine,
    //            () => Console.WriteLine("Completed"));
    //        Console.ReadKey();
    //    }
    //}

    //class Timeout_Simple {
    //    static void Main() {
    //        Console.WriteLine(DateTime.Now);
    //        // create a single event in 10 seconds time
    //        var observable = Observable.Timer(TimeSpan.FromSeconds(10)).Timestamp();
    //        // raise exception if no event received within 9 seconds
    //        var observableWithTimeout = Observable.Timeout(observable, TimeSpan.FromSeconds(10));
    //        using (observableWithTimeout.Subscribe(
    //            x => Console.WriteLine($"{x.Value}:{x.Timestamp}"),
    //            ex => Console.WriteLine())) {

    //        }
    //    }
    //}
    #endregion
    #endregion

    #region "Hot and Cold observables"
    //class Program {
    //    static void Main() {
    //        ReadFirstValue(LazyEvaluation);
    //        Console.ReadLine();
    //    }

    //    static void ReadFirstValue(Func<IEnumerable<int>> List) {
    //        foreach (var i in List()) {
    //            Console.WriteLine($"Read out first value of {i}");
    //            break;
    //        }
    //    }

    //    static IEnumerable<int> EagerEvaluation() {
    //        var result = new List<int>();
    //        Console.WriteLine("About to return 1");
    //        result.Add(1);
    //        //code below is executed but not used.
    //        Console.WriteLine("About to return 2");
    //        result.Add(2);
    //        return result;
    //    }

    //    static IEnumerable<int> LazyEvaluation() {
    //        Console.WriteLine("About to return 1");
    //        yield return 1;
    //        Console.WriteLine("About to return 2");
    //        yield return 2;
    //    }
    //}

    #region "Cold observables"
    //    class Program {
    //        private const string connectionString = @"Data Source=.;" +
    //@"Initial Catalog=AdventureWorks2012;Integrated Security=SSPI;";
    //        static void Main() {
    //            var products = GetProducts().Take(3);
    //            products.Subscribe(Console.WriteLine);           
    //            Console.ReadLine();
    //        }
    //        private static IObservable<string> GetProducts() {
    //           return Observable.Create<string>(o => {
    //                using (var conn = new SqlConnection(connectionString))
    //                using (var cmd = new SqlCommand("Select Name FROM Production.Product", conn)) {
    //                    conn.Open();
    //                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
    //                    while (reader.Read()) {
    //                        o.OnNext(reader.GetString(0));
    //                    }
    //                    o.OnCompleted();
    //                    return Disposable.Create(() => Console.WriteLine("--Disposed--"));
    //                }
    //            });           
    //        }
    //    }
    #endregion

    #region "Hot observables"
    //class Program {
    //    static void Main() {
    //        SimpleColdSample();
    //        Console.ReadKey();
    //    }

    //    static void SimpleColdSample() {
    //        var period = TimeSpan.FromSeconds(1);
    //        var observable = Observable.Interval(period);
    //        observable.Subscribe(i => Console.WriteLine($"first subscribtion : {i}"));
    //        Thread.Sleep(period);
    //        observable.Subscribe(i => Console.WriteLine($"second subscribtion {i}"));
    //    }
    //}
    #endregion

    #region "Publish and Connect"
    //class Program {
    //    static void Main() {
    //        var period = TimeSpan.FromSeconds(1);
    //        var observable = Observable.Interval(period);
    //        observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));
    //        Thread.Sleep(period);
    //        observable.Subscribe(i => Console.WriteLine("second subscription : {0}", i));
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Disposal of connections and subscriptions"
    //class Program {
    //    //static void Main() {
    //    //    var period = TimeSpan.FromSeconds(1);
    //    //    var observable = Observable.Interval(period).Publish();
    //    //    observable.Subscribe(i => Console.WriteLine($"subscribtion : {i}"));
    //    //    var exit = false;
    //    //    while (!exit) {
    //    //        Console.WriteLine("Press enter to connect, esc to exit");
    //    //        var key = Console.ReadKey(true);
    //    //        if (key.Key == ConsoleKey.Enter) {
    //    //            var connection = observable.Connect();  //--Connects here--
    //    //            Console.WriteLine("Press any key to dispose of connection.");
    //    //            Console.ReadKey();
    //    //            connection.Dispose(); //--Disconnects here--
    //    //        }
    //    //        if (key.Key == ConsoleKey.Escape) {
    //    //            exit = true;
    //    //        }
    //    //    }
    //    //}

    //    ////static void Main() {
    //    ////    var period = TimeSpan.FromSeconds(1);
    //    ////    var observable = Observable.Interval(period)
    //    ////        .Do(l => Console.WriteLine($"Publishing {l}"))  //Side effect to show it is running
    //    ////        .Publish();
    //    ////    observable.Connect();
    //    ////    Console.WriteLine("Press any key to subscribe");
    //    ////    Console.ReadKey();
    //    ////    var subscribtion = observable.Subscribe(i => Console.WriteLine($"subscription : {i}"));
    //    ////    Console.WriteLine("Press any key to unsubscribe");
    //    ////    Console.ReadKey();
    //    ////    subscribtion.Dispose();
    //    ////    Console.WriteLine("Press any key to exit.");
    //    ////    Console.ReadKey();
    //    ////}
    //}
    #endregion

    #region "RefCount"
    //class Program {
    //    static void Main() {
    //        var period = TimeSpan.FromSeconds(1);
    //        var observable = Observable.Interval(period)
    //            .Do(l => Console.WriteLine($"Publishing {l}"))    //side effect to show it is running
    //            .Publish()
    //            .RefCount();
    //        //observable.Connect(); Use RefCount instead now 
    //        Console.WriteLine("Press any key to subscribe");
    //        Console.ReadKey();
    //        var subscribtion = observable.Subscribe(i => Console.WriteLine($"subscription : {i}"));
    //        Console.WriteLine("Press any key to unsubscribe");
    //        Console.ReadKey();
    //        subscribtion.Dispose();
    //        Console.WriteLine("Press any key to exit.");
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Other connectable observables"
    #region "PublishLast"
    //class Program {
    //    static void Main() {
    //        var period = TimeSpan.FromSeconds(1);
    //        var observable = Observable.Interval(period)
    //            .Take(5)
    //            .Do(l => Console.WriteLine($"Publishing {l}")) //side effect to show it is running
    //            .PublishLast();
    //        observable.Connect();
    //        Console.WriteLine("Press any key to subscribe");
    //        Console.ReadKey();
    //        var subscription = observable.Subscribe(i => Console.WriteLine($"subscription : {i}"));
    //        Console.WriteLine("Press any key to unsubscribe.");
    //        Console.ReadKey();
    //        subscription.Dispose();
    //        Console.WriteLine("Press any key to exit");
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Replay"
    //class Program {
    //    static void Main() {
    //        var period = TimeSpan.FromSeconds(1);
    //        var hot = Observable.Interval(period)
    //            .Take(3)
    //            .Do(i => Console.WriteLine($"Publish {i}"))
    //            .Publish();
    //        hot.Connect();
    //        Thread.Sleep(period); //Run hot and ensure a value is lost
    //        var observable = hot.Replay();
    //        observable.Connect();
    //        observable.Subscribe(i => Console.WriteLine($"first subscription : {i}"));
    //        Thread.Sleep(period);
    //        observable.Subscribe(i => Console.WriteLine($"second subscription : {i}"));
    //        Console.ReadKey();
    //        observable.Subscribe(i => Console.WriteLine($"third subscription : {i}"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Multicast"
    //class Program {
    //    static void Main() {
    //        var period = TimeSpan.FromSeconds(1);
    //        //var observable = Observable.Interval(period).Publish();
    //        var observable = Observable.Interval(period);
    //        var shared = new Subject<long>();
    //        shared.Subscribe(i => Console.WriteLine($"first subscription : {i}"));
    //        observable.Subscribe(shared);   //'Connect' the observable.
    //        Thread.Sleep(period);
    //        Thread.Sleep(period);
    //        shared.Subscribe(i=>Console.WriteLine($"second subscription : {i}"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion
    #endregion
    #endregion
}
