using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MyRX {
    #region "Simple factory methods"
    #region "Observable.Return"
    //class Program {
    //    static void Main() {
    //        //var singleValue = Observable.Return<string>("Value");
    //        ////Can be reduced to the following
    //        var singleValue = Observable.Return("Value");
    //        ////which could have also been simulated with a replay subject         
    //        var subject = new ReplaySubject<string>();
    //        subject.OnNext("Value");
    //        singleValue.Dump("Observable Return");
    //        subject.Dump("Replay Subject");
    //        subject.OnCompleted();
    //        Console.Read();
    //    }
    //}
    #endregion

    #region "Observable.Empty"
    //class Program {
    //    static void Main() {
    //        var empty = Observable.Empty<string>();
    //        //Behaviorally equivalent to
    //        var subject = new ReplaySubject<string>();

    //        empty.Dump("Empty Observable");

    //        subject.Dump("ReplaySubject");
    //        subject.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Observable.Never"
    //class Program {
    //    static void Main() {
    //        var never = Observable.Never<string>();
    //        //similar to a subject without notifications
    //        var subject = new Subject<string>();

    //        never.Dump("Never Sequence");
    //        subject.Dump("Subject Sequence");
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Observable.Throw"
    //class Program {
    //    static void Main() {
    //        var throws = Observable.Throw<string>(new Exception());
    //        //Behaviorally equivalent to
    //        var subject = new ReplaySubject<string>();

    //        throws.Dump("Throw Sequence");
    //        subject.Dump("Subject Throw Sequence");

    //        subject.OnError(new Exception());
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Observable.Create"
    #region "BlockingAndNonBlocking"
    //class CreatingSequence {
    //    private static IObservable<string> BlockingMethod() {
    //        var subject = new ReplaySubject<string>();
    //        subject.OnNext("Blocking Call => A ");
    //        subject.OnNext("Blocking Call => B ");
    //        subject.OnCompleted();
    //        Thread.Sleep(1000);
    //        return subject;
    //    }

    //    private static IObservable<string> NonBlocking() {
    //        return Observable.Create<string>((IObserver<string> observer) => {
    //            observer.OnNext("NonBlocking Call => A ");
    //            observer.OnNext("NonBlocking Call => B ");
    //            observer.OnCompleted();
    //            Thread.Sleep(1000);
    //            return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
    //            //or can return an Action like 
    //            //return () => Console.WriteLine("Observer has unsubscribed");
    //        });
    //    }

    //    static void Main() {
    //        var blockSequence = BlockingMethod();
    //        var nonBlockSequence = NonBlocking();
    //        blockSequence.Subscribe(n => Console.WriteLine(n));
    //        nonBlockSequence.Subscribe(s => Console.WriteLine(s));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Empty, Return, Never and Throw recreated with Observable.Create:"
    //class Program {
    //    private static IObservable<T> Empty<T>() {
    //        return Observable.Create<T>(o => {
    //            o.OnCompleted();
    //            return Disposable.Empty;
    //        });
    //    }

    //    private static IObservable<T> Return<T>(T value) {
    //        return Observable.Create<T>(o => {
    //            o.OnNext(value);
    //            o.OnCompleted();
    //            return Disposable.Empty;
    //        });
    //    }

    //    private static IObservable<T> Never<T>() {
    //        return Observable.Create<T>(o => {
    //            return Disposable.Empty;
    //        });
    //    }

    //    private static IObservable<T> Throws<T>(Exception exception) {
    //        return Observable.Create<T>(o => {
    //            o.OnError(exception);
    //            return Disposable.Empty;
    //        });
    //    }

    //    static void Main() {
    //        Console.WriteLine("using Empty method");
    //        var emptySequence = Empty<string>();
    //        emptySequence.Subscribe(Console.WriteLine);
    //        Console.WriteLine();

    //        Console.WriteLine("using Return method");
    //        var value = "Hello ReactiveX";
    //        var returnSequence = Return<string>(value);
    //        returnSequence.Subscribe(v => Console.WriteLine(v));
    //        Console.WriteLine();

    //        Console.WriteLine("using Never method");
    //        var neverSequence = Never<string>();
    //        neverSequence.Subscribe(n => Console.WriteLine(n));
    //        Console.WriteLine();

    //        Console.WriteLine("using Throws method");
    //        var throwsSequence = Throws<string>(new Exception("Hello Exception"));
    //        throwsSequence.Subscribe(
    //            n => Console.WriteLine(n),
    //            e => Console.WriteLine(e));
    //        Console.ReadKey();
    //    }
    //}
    #endregion
    #endregion

    #region "dispose for subscription only, not for timer "
    //class Program {
    //    static void Main() {
    //        NonBlocking_event_driven();
    //    }

    //    public static void NonBlocking_event_driven() {
    //        var ob = Observable.Create<string>(
    //            observer => {
    //                var timer = new System.Timers.Timer();
    //                timer.Interval = 1000;
    //                timer.Elapsed += (s, e) => observer.OnNext("tick");
    //                //timer.Elapsed += (s, e) => Console.WriteLine(e.SignalTime);
    //                timer.Elapsed += OnTimerElapsed;
    //                timer.Start();
    //                return Disposable.Empty;
    //            });
    //        var subscription = ob.Subscribe(Console.WriteLine);
    //        Console.WriteLine("press Enter to call Despose()\n");
    //        Console.ReadLine();
    //        subscription.Dispose();
    //        Console.ReadKey();
    //    }

    //    private static void OnTimerElapsed(object sender, ElapsedEventArgs e) {
    //        Console.WriteLine(e.SignalTime);
    //    }
    //}
    #endregion

    #region "dispose timer and subscription"
    //class Program {
    //    static void Main() {
    //        NonBlocking_event_driven();
    //    }
    //    public static void NonBlocking_event_driven() {
    //        var ob = Observable.Create<string>(
    //            observer => {
    //                var timer = new System.Timers.Timer();
    //                timer.Interval = 1000;
    //                timer.Elapsed += (s, e) => observer.OnNext("tick");
    //                //timer.Elapsed += (s, e) => Console.WriteLine(e.SignalTime);
    //                timer.Elapsed += OnTimerElapsed;
    //                timer.Start();
    //                return timer;
    //            });
    //        var subscription = ob.Subscribe(Console.WriteLine);
    //        Console.WriteLine("press Enter to call Dispose");
    //        Console.ReadLine();
    //        subscription.Dispose();
    //        Console.ReadKey();
    //    }

    //    private static void OnTimerElapsed(object sender, ElapsedEventArgs e) {
    //        Console.WriteLine(e.SignalTime);
    //    }
    //}
    #endregion

    #region "observable for timer"
    //class program {
    //    static void Main() {
    //        var ob = Observable.Create<string>(observer => {
    //            var timer = new System.Timers.Timer();
    //            timer.Interval = 1000;
    //            timer.Elapsed += OnTimerElapsed;
    //            timer.Start();
    //            return () => {
    //                timer.Elapsed -= OnTimerElapsed;
    //                timer.Dispose();
    //            };
    //        });
    //        var subscription = ob.Subscribe(Console.WriteLine);
    //        Console.ReadLine();
    //        subscription.Dispose();
    //    }

    //    private static void OnTimerElapsed(object sender, ElapsedEventArgs e) {
    //        Console.WriteLine(e.SignalTime);
    //    }
    //}
    #endregion

    #region "Functional unfolds"
    #region "Corecursion"
    //class program {
    //    private static IEnumerable<T> Unfold<T>(T seed, Func<T, T> accumulator) {
    //        var nextValue = seed;
    //        while (true) {
    //            yield return nextValue;
    //            nextValue = accumulator(nextValue);
    //        }
    //    }

    //    static int sum(int v) {
    //        return v + 1;
    //    }
    //    static void Main() {
    //        var naturalNumbers = Unfold(1, i => i + 1);
    //        //var naturalNumbers = Unfold(1, sum);
    //        Console.WriteLine("1st 10 Natural numbers");
    //        foreach (var naturalNumber in naturalNumbers.Take(10)) {
    //            Console.WriteLine(naturalNumber);
    //        }
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Observable.Range"
    //class Program {
    //    static void Main() {
    //        //var ob = Observable.Range(10, 15);
    //        //ob.Subscribe(Console.WriteLine);
    //        //Console.ReadKey();

    //        var sequence = Range(10, 15);
    //        sequence.Subscribe(Console.WriteLine);
    //        Console.ReadKey();
    //    }

    //    static IObservable<int> Range(int start, int count) {
    //        var max = start + count;
    //        return Observable.Generate(
    //            start,
    //            value => value < max,
    //            value => value + 1,
    //            value => value
    //            );
    //    }
    //}
    #endregion

    #region "Observable.Timer"
    //class Program {
    //    static void Main() {
    //        ////using buildin observable class
    //        //var interval = Observable.Interval(TimeSpan.FromMilliseconds(250));
    //        //interval.Subscribe(
    //        //    Console.WriteLine,
    //        //    () => Console.WriteLine("completed"));
    //        //Console.ReadLine();

    //        //var timer = Observable.Timer(TimeSpan.FromSeconds(1));
    //        //timer.Subscribe(
    //        //    Console.WriteLine,
    //        //    () => Console.WriteLine("completed"));
    //        //Console.ReadLine();

    //        //var timer = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(1000));
    //        //timer.Subscribe(
    //        //    Console.WriteLine,
    //        //    () => Console.WriteLine("completed"));
    //        //Console.ReadLine();

    //        ////using own method
    //        var interval = Interval(TimeSpan.FromSeconds(1));
    //        interval.Subscribe(i => Console.WriteLine($"Interval => {i}"));
    //        Console.ReadLine();


    //        var timer = Timer(TimeSpan.FromSeconds(1));
    //        timer.Subscribe(t => Console.WriteLine($"Timer => {t}"));
    //        Console.ReadLine();

    //        var timer2 = Timer(TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(250));
    //        timer2.Subscribe(p => Console.WriteLine($"Timer period => {p}"));
    //        Console.ReadLine();
    //    }

    //    static IObservable<long> Timer(TimeSpan dueTime) {
    //        return Observable.Generate(
    //            0l,
    //            i => i < 1,
    //            i => i + 1,
    //            i => i,
    //            i => dueTime);
    //    }

    //    static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period) {
    //        return Observable.Generate(
    //            0l,
    //            i => true,
    //            i => i + 1,
    //            i => i,
    //            i => i == 0 ? dueTime : period);
    //    }

    //    static IObservable<long> Interval(TimeSpan period) {
    //        return Observable.Generate(
    //            0l,
    //            i => true,
    //            i => i + 1,
    //            i => i,
    //            i => period);
    //    }
    //}
    #endregion
    #endregion

    #region "Transitioning into IObservable<T>"
    #region "From delegates"
    //class Program {
    //    static void Main() {
    //        StartAction();
    //       // StartFunc();
    //        Console.ReadLine();
    //    }

    //    static void StartAction() {
    //        var start = Observable.Start(() => {
    //            Console.WriteLine("Working away");
    //            for (int i = 0; i < 10; i++) {
    //                Thread.Sleep(100);
    //                Console.WriteLine(".");
    //            }
    //        });
    //        start.Subscribe(
    //            unit => Console.WriteLine("Unit published"),
    //            () => Console.WriteLine("Action completed"));
    //    }

    //    static void StartFunc() {
    //        var start = Observable.Start(() => {
    //            Console.WriteLine("Working away");
    //            for (int i = 0; i < 10; i++) {
    //                Thread.Sleep(100);
    //                Console.WriteLine(".");
    //            }
    //            return "Published value";
    //        });
    //        start.Subscribe(value =>
    //            Console.WriteLine(value),
    //            () => Console.WriteLine("completed"));
    //    }
    //}
    #endregion

    #region "From Task"

    //class Program {
    //    static void Main() {
    //        var t = Task.Factory.StartNew(() => "Test");
    //        var source = t.ToObservable();
    //        source.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("completed"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "From IEnumerable<T>"
    //static class Program {
    //    static void Main() {
    //        var Letters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" };
    //        var source = Letters.ToObservable();
    //        source.Subscribe(Console.WriteLine);
    //        Console.ReadKey();
    //    }
    //    static IObservable<T> ToObservable<T>(this IEnumerable<T> source) {
    //        return Observable.Create<T>(o => {
    //            foreach (var item in source) {
    //                o.OnNext(item);
    //            }
    //            //Incorrect disposal pattern
    //            return Disposable.Empty;
    //        });
    //    }
    //}
    #endregion
    #endregion
    #endregion

    #region "Reducing a sequence"
    #region "Where"
    //class Program {
    //    static void Main() {
    //        var oddNumbers = Observable.Range(0, 10)
    //            .Where(i => i % 2 == 0)
    //            .Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("Completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Distinct and DistinctUntilChanged"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        //var distinct = subject.Distinct();
    //        var distinct = subject.DistinctUntilChanged();
    //        subject.Subscribe(
    //            i => Console.WriteLine($"{i}"),
    //            () => Console.WriteLine("subject.OnCompleted()"));
    //        distinct.Subscribe(
    //            i => Console.WriteLine($"distinct.OnNext({i})"),
    //            () => Console.WriteLine("distince.OnCompleted()"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        subject.OnNext(1);
    //        subject.OnNext(1);
    //        subject.OnNext(4);
    //        subject.OnCompleted();
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "IgnoreElements"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        ////Could use subject.Where(_=>false);
    //        //var noElements = subject.Where(_ => false);
    //        var noElements = subject.IgnoreElements();
    //        subject.Subscribe(
    //            i => Console.WriteLine($"subject.OnNext({i})"),
    //            () => Console.WriteLine("subject.OnCompleted"));
    //        noElements.Subscribe(
    //            i => Console.WriteLine($"noElements.OnNext({i})"),
    //            () => Console.WriteLine("noElements.OnCompleted"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        subject.OnCompleted();
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Skip and Take"
    //class Program {
    //    static void Main() {
    //        Observable.Range(0, 10)
    //            .Skip(3)
    //            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));

    //        Observable.Interval(TimeSpan.FromMilliseconds(100))
    //            .Take(3)
    //            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "SkipWhile and TakeWhile"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        //subject
    //        //    .SkipWhile(i => i < 4)
    //        //    .Subscribe(Console.WriteLine, () => Console.WriteLine("SkipWhile Completed"));
    //        subject
    //            .TakeWhile(i => i < 4)
    //            .Subscribe(Console.WriteLine, () => Console.WriteLine("TakeWhile Completed"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        subject.OnNext(4);
    //        subject.OnNext(5);
    //        subject.OnNext(6);
    //        subject.OnNext(0);
    //        subject.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "SkipLast and TakeLast"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        //subject
    //        //    .SkipLast(2)
    //        //    .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
    //        subject
    //            .TakeLast(2)
    //            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
    //        Console.WriteLine("Pushing 1");
    //        subject.OnNext(1);
    //        Console.WriteLine("Pushing 2");
    //        subject.OnNext(2);
    //        Console.WriteLine("Pushing 3");
    //        subject.OnNext(3);
    //        Console.WriteLine("Pushing 4");
    //        subject.OnNext(4);
    //        subject.OnCompleted();
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "SkipUntil and TakeUntil"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        var otherSubject = new Subject<Unit>();
    //        //subject
    //        //    .SkipUntil(otherSubject)
    //        //    .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
    //        subject
    //            .TakeUntil(otherSubject)
    //            .Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        otherSubject.OnNext(Unit.Default);
    //        subject.OnNext(4);
    //        subject.OnNext(5);
    //        subject.OnNext(6);
    //        subject.OnNext(7);
    //        subject.OnNext(8);
    //        subject.OnCompleted();
    //        Console.ReadKey();
    //    }
    //}
    #endregion
    #endregion

    #region "Inspection"
    #region "Any"
    //class Program
    //{
    //    static void Main()
    //    {
    //        //var subject = new Subject<int>();
    //        //subject.Subscribe(Console.WriteLine, () => Console.WriteLine("Subject completed"));
    //        //var any = subject.Any();
    //        //any.Subscribe(b => 
    //        //Console.WriteLine($"The subject has any values? {b}"),
    //        //()=>Console.WriteLine("Any completed"));
    //        //subject.OnNext(1);
    //        //subject.OnNext(2);
    //        //subject.OnCompleted();
    //        //Console.ReadKey();


    //        //var subject = new Subject<int>();
    //        //subject.Subscribe(Console.WriteLine,
    //        //    ex => Console.WriteLine($"Subject OnError : {ex}"),
    //        //    () => Console.WriteLine("Subject completed"));
    //        //var any = subject.Any();
    //        //any.Subscribe(b => Console.WriteLine($"The subject has any values? {b}"),
    //        //    ex => Console.WriteLine($".Any() OnError : {ex}"),
    //        //    () => Console.WriteLine(".Any() completed"));
    //        //subject.OnError(new Exception());
    //        //Console.ReadLine();

    //        var subject = new Subject<int>();
    //        var any = subject.Any(i => i < 2);
    //        ////Funtionally equivalent to
    //        //var any = subject.Where(i => i < 2).Any();
    //        any.Subscribe(i =>
    //        Console.WriteLine($"Overload Any=>{i}"),
    //        () => Console.WriteLine("Overload Any() completed"));
    //        subject.OnNext(3);
    //        subject.OnNext(1);
    //        subject.OnNext(0);            
    //        Console.ReadLine();
    //    }
    //}

    #region "Any extension"
    //static class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        //var myAny = subject.MyAny();
    //        var myAny = subject.MyAny(i => i < 2);
    //        myAny.Subscribe(i => Console.WriteLine($"My Extension Method => {i}"));
    //        subject.OnNext(1);
    //        subject.OnNext(1);
    //        Console.ReadLine();
    //    }

    //    public static IObservable<bool> MyAny<T>(
    //        this IObservable<T> source) {
    //        return Observable.Create<bool>(o => {
    //            var hasValues = false;
    //            return source
    //            .Take(1)
    //            .Subscribe(
    //                      _ => hasValues = true,
    //                      o.OnError,
    //                      () => {
    //                          o.OnNext(hasValues);
    //                          o.OnCompleted();
    //                      });
    //        });
    //    }

    //    public static IObservable<bool> MyAny<T>(
    //        this IObservable<T> source, Func<T, bool> predicate) {
    //        return source.Where(predicate).MyAny();
    //    }
    //}
    #endregion
    #endregion

    #region "All"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        subject.Subscribe(Console.WriteLine, () => Console.WriteLine("Subject completed"));
    //        var all = subject.All(i => i < 5);
    //        all.Subscribe(
    //            b => Console.WriteLine($"All value less than 5? {b}"),
    //            () => Console.WriteLine("All sequence completed."));

    //        //IsEmpty test
    //        subject.IsEmpty().Subscribe(b => Console.WriteLine($"IsEmpty =>{b}"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(6);
    //        subject.OnNext(2);
    //        subject.OnNext(1);
    //        subject.OnCompleted();

    //        Console.ReadLine();

    //    }
    //}
    #endregion

    #region "Contains"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        subject.Subscribe(Console.WriteLine,
    //            () => Console.WriteLine("Subject completed"));
    //        var contains = subject.Contains(2);
    //        contains.Subscribe(
    //            b => Console.WriteLine($"Contains the value 2? {b}"),
    //            ()=>Console.WriteLine("contains completed"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        subject.OnCompleted();
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "DefaultIfEmpty"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        subject.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("Subject completed"));
    //        var defaultIfEmpty = subject.DefaultIfEmpty();
    //        defaultIfEmpty.Subscribe(
    //            b=>Console.WriteLine($"defaultIfEmpty value: {b}"),
    //            ()=>Console.WriteLine("defaultIfEmpty completed"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        subject.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        subject.Subscribe(
    //            Console.WriteLine,
    //            ()=>Console.WriteLine("Subject completed"));
    //        var defaultIfEmpty = subject.DefaultIfEmpty();
    //        defaultIfEmpty.Subscribe(
    //            b=>Console.WriteLine($"defaultIfEmpty value: {b}"),
    //            ()=>Console.WriteLine("defaultIfEmpty completed"));
    //        var default42IfEmpty = subject.DefaultIfEmpty(42);
    //        default42IfEmpty.Subscribe(
    //            b => Console.WriteLine($"defaultIfEmpty value: {b}"),
    //            () => Console.WriteLine("defaultIfEmpty completed"));
    //        subject.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "ElementAt"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        subject.Subscribe(
    //            Console.WriteLine,
    //            () => Console.WriteLine("Subject completed"));
    //        var elementAt1 = subject.ElementAt(5);
    //        //var elementAt1 = subject.ElementAtOrDefault(5);
    //        elementAt1.Subscribe(
    //            b => Console.WriteLine($"elementAt1 value: {b}"),
    //            () => Console.WriteLine("elementAt1 completed"));
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        subject.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "SequenceEqual"
    //class Program {
    //    static void Main() {
    //        var subject1 = new Subject<int>();
    //        subject1.Subscribe(
    //            i=>Console.WriteLine($"subject1.OnNext({i})"),
    //            ()=>Console.WriteLine("subject1 completed"));
    //        var subject2 = new Subject<int>();
    //        subject2.Subscribe(
    //            i => Console.WriteLine($"subject2.OnNext({i})"),
    //            ()=>Console.WriteLine("subject2 completed"));
    //        var areEqual = subject1.SequenceEqual(subject2);
    //        areEqual.Subscribe(
    //            i=>Console.WriteLine($"areEqual.OnNext({i})"),
    //            ()=>Console.WriteLine("areEqual completed"));
    //        subject1.OnNext(1);
    //        subject1.OnNext(2);
    //        subject2.OnNext(1);
    //        subject2.OnNext(2);
    //        subject2.OnNext(3);
    //        subject1.OnNext(3);
    //        subject1.OnCompleted();
    //        subject2.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion
    #endregion

    #region "Aggregation"
    #region "Count"
    //static class Program {
    //    static void Main() {
    //        var number = Observable.Range(0, 3);
    //        number.Dump("numbers");
    //        number.Count().Dump("count");
    //        Console.ReadLine();
    //    }

    //    static void Dump<T>(this IObservable<T> source,string name) {
    //        source.Subscribe(
    //            i=>Console.WriteLine($"{name}-->{i}"),
    //            ex=>Console.WriteLine($"{name} failed-->{ex.Message}"),
    //            ()=>Console.WriteLine($"{name} completed"));
    //    }
    //}
    #endregion

    #region "Min, Max, Sum and Average"
    //static class Program {
    //    static void Main() {
    //        var number = new Subject<int>();
    //        number.Dump("number");
    //        number.Min().Dump("Min");
    //        number.Average().Dump("Average");
    //        number.Sum().Dump("Sum");
    //        number.OnNext(1);
    //        number.OnNext(2);
    //        number.OnNext(3);
    //        number.OnCompleted();
    //        Console.ReadLine();
    //    }

    //    static void Dump<T>(this IObservable<T> source, string name) {
    //        source.Subscribe(
    //            i => Console.WriteLine($"{name}-->{i}"),
    //            ex => Console.WriteLine($"{name} failed-->{ex.Message}"),
    //            () => Console.WriteLine($"{name} completed"));
    //    }
    //}
    #endregion

    #region "First"
    //class Program {
    //    static void Main() {
    //        var interval = Observable.Interval(TimeSpan.FromSeconds(3));
    //        //Will block for 3s before returning            
    //        Console.WriteLine(interval.First());
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Build your own aggregations"
    #region "Aggregate"
    //class Program {
    //    static void Main() {
    //        var source = new Subject<int>();           
    //        var sum = source.Aggregate((acc, currentValue) => acc + currentValue);
    //        sum.Subscribe(s => Console.WriteLine($"Sum => {s}"));
    ////this overload needs at least one value to be produced from the source 
    ////or the output will error with an InvalidOperationException
    //        source.OnNext(1);
    //        source.OnNext(2);
    //        source.OnNext(3);
    //        source.OnCompleted();
    //        Console.ReadLine();          
    //    }
    //}

    //class Program {
    //    static void Main() {
    //        var source = new Subject<int>();
    //        var sum = source.Aggregate(0, (acc, cuurentValue) => acc + cuurentValue);
    //        //var count = source.Aggregate(0, (acc, cuurentValue) => acc + 1);
    //        ////or using '_' to signify that the value is not used.
    //        var count = source.Aggregate(0, (acc, _) => acc + 1);
    //        sum.Subscribe(s => Console.WriteLine($"Sum => {s}"));
    //        count.Subscribe(s => Console.WriteLine($"Count => {s}"));
    //        source.OnNext(1);
    //        source.OnNext(2);
    //        source.OnNext(3);
    //        source.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "creating Min and Max from Aggregate"
    //static class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        subject.MyMin().Subscribe(v => Console.WriteLine($"Min value => {v}"));
    //        subject.MyMax().Subscribe(v => Console.WriteLine($"Max value => {v}"));
    //        subject.OnNext(2);
    //        subject.OnNext(1);
    //        subject.OnNext(3);
    //        subject.OnNext(5);
    //        subject.OnNext(0);
    //        subject.OnCompleted();
    //        Console.ReadLine();
    //    }

    //    public static IObservable<T> MyMin<T>(this IObservable<T> source) {
    //        return source.Aggregate(
    //            (min, current) => Comparer<T>
    //            .Default.Compare(min, current) > 0
    //            ? current
    //            : min);
    //    }
    //    public static IObservable<T> MyMax<T>(this IObservable<T> source) {
    //        var comparer = Comparer<T>.Default;
    //        Func<T, T, T> max =
    //           (x, y) => {
    //               return comparer.Compare(x, y) > 0
    //                   ? x
    //                   : y;
    //           };
    //        return source.Aggregate(max);
    //    }
    //}
    #endregion
    #endregion

    #region "Scan"
    //static class Program {
    //    //static void Dump<T>(this IObservable<T> source, string name) {
    //    //    source.Subscribe(
    //    //        i => Console.WriteLine($"{name}-->{i}"),
    //    //        ex => Console.WriteLine($"{name} failed-->{ex.Message}"),
    //    //        () => Console.WriteLine($"{name} completed"));
    //    //}
    //    static void Main() {
    //        var numbers = new Subject<int>();
    //        var scan = numbers.Scan(0, (acc, current) => acc + current);
    //        var aggregate = numbers.Aggregate(0, (acc, current) => acc + current);
    //        numbers.Dump("numbers");
    //        scan.Dump("scan");
    //        aggregate.Dump("Aggregate");
    //        numbers.OnNext(1);
    //        numbers.OnNext(2);
    //        numbers.OnNext(3);
    //        numbers.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}

    #region "Min Sequences"
    //static class Program {
    //    static Comparer<int> comparer = Comparer<int>.Default;
    //    static Func<int, int, int> minOf = (x, y) => comparer.Compare(x, y) < 0 ? x : y;
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        var minScan = subject.Scan(minOf).DistinctUntilChanged();
    //        minScan.Subscribe(Console.WriteLine);
    //        subject.OnNext(2);
    //        subject.OnNext(1);
    //        subject.OnNext(3);
    //        subject.OnNext(5);
    //        subject.OnNext(0);
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Max Sequences"
    //static class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        var maxOf = subject.RunningMax();
    //        maxOf.Subscribe(m => Console.WriteLine($"Max Sequences =>{m}"));
    //        subject.OnNext(2);
    //        subject.OnNext(1);
    //        subject.OnNext(3);
    //        subject.OnNext(5);
    //        subject.OnNext(0);
    //        Console.ReadLine();
    //    }

    //    public static IObservable<T> RunningMax<T>(this IObservable<T> source) {
    //        return source.Scan(MaxOf)
    //            .Distinct();
    //    }

    //    public static T MaxOf<T>(T x, T y) {
    //        var comparer = Comparer<T>.Default;
    //        if (comparer.Compare(x, y) > 0)
    //            return x;
    //        return y;
    //    }
    //}
    #endregion
    #endregion

    #region "Partitioning"
    #region "MinBy / MaxBy"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        var minBy = subject.MinBy(i => i % 3);
    //        var maxBy = subject.MaxBy(i => i % 3);
    //        minBy.SelectMany(grp => grp).Subscribe(m => Console.WriteLine($"Min value => {m}"));
    //        maxBy.SelectMany(grp => grp).Subscribe(m => Console.WriteLine($"Max value => {m}"));
    //        subject.OnNext(0);
    //        subject.OnNext(1);
    //        subject.OnNext(2);
    //        subject.OnNext(3);
    //        subject.OnNext(4);
    //        subject.OnNext(5);
    //        subject.OnNext(6);
    //        subject.OnNext(7);
    //        subject.OnNext(8);
    //        subject.OnNext(9);
    //        subject.OnCompleted();
    //        Console.ReadLine();
    //    }
    //}
    #endregion
    #region "GroupBy Min"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10);
    //        var group = source.GroupBy(i => i % 3);
    //        group.Subscribe(
    //            grp =>
    //            grp.Min().Subscribe(
    //                minValue =>
    //                Console.WriteLine($"Key = {grp.Key} value = {minValue}")),
    //                () => Console.WriteLine("Completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "GroupBy Max"
    //static class Program {

    //    static void Dump<T>(this IObservable<T> source, string number) {
    //        source.Subscribe(
    //            i => Console.WriteLine($"{number}-->{i}"),
    //            ex => Console.WriteLine($"{number} failed-->{ex.Message}"),
    //            () => Console.WriteLine($"{number} completed"));
    //    }
    //    static void Main() {
    //        var source = Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10);
    //        var group = source.GroupBy(i => i % 3);
    //        group.SelectMany(
    //            grp => grp.Max()
    //            .Select(value => new { grp.Key, value }))
    //            .Dump("group");
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #endregion
    #endregion

    #region "Transformation of sequences"
    #region "Select"
    //class Program {
    //    static void Main() {
    //        var source = Observable.Range(0, 5);
    //        //var sequence = source.Select(i => i + 3);            
    //        //var sequence = source.Select(i => (char)(i + 64));
    //        //var sequence = source.Select(i => new { Number = i, Character = (char)(i + 64) });
    //        var sequence = from i in Observable.Interval(TimeSpan.FromSeconds(5)) select new { Number = i, Character = (char)(i + 64) };

    //        sequence.Subscribe(
    //            i => Console.WriteLine($"+3-->{i}"),
    //            ex => Console.WriteLine($"+3 fail -->{ex.Message}"),
    //            () => Console.WriteLine("+3 completed"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Cast and OfType"
    //class Program {
    //    ////Testing Cast
    //    //static void Main() {
    //    //    var objects = new Subject<object>();
    //    //    var sequence = objects.Cast<int>();// is equivalent to
    //    //    //var sequence = objects.Select(i => (int)i);
    //    //    sequence.Subscribe(
    //    //        i => Console.WriteLine($"cast -->{i}"),
    //    //        ex => Console.WriteLine($"cast fail-->{ex.Message}"),
    //    //        () => Console.WriteLine("cast completed"));
    //    //    objects.OnNext(1);
    //    //    objects.OnNext(2);
    //    //    objects.OnNext(3);
    //    //    //objects.OnNext("4");//Fail
    //    //    Console.ReadLine();
    //    //}

    //    ////Testing OfType
    //    static void Main() {
    //        var objects = new Subject<object>();
    //        var sequence = objects.OfType<int>();// is equivalent to
    //        //var sequence = objects.Where(i => i is int).Select(i => (int)i);
    //        sequence.Subscribe(
    //            i => Console.WriteLine($"OfType -->{i}"),
    //            ex => Console.WriteLine($"OfType fail-->{ex.Message}"),
    //            () => Console.WriteLine("OfType completed"));
    //        objects.OnNext(1);
    //        objects.OnNext(2);
    //        objects.OnNext("3");//Ignored
    //        objects.OnNext(4);
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "Timestamp and TimeInterval"
    //class Program {
    //    static void Main() {
    //        ////Using Timestamp
    //        Observable.Interval(TimeSpan.FromSeconds(1))
    //            .Take(3)
    //            .Timestamp()
    //            .Subscribe(
    //            i => Console.WriteLine($"TimeStamp-->{i}"),
    //            ex => Console.WriteLine($"TimeStamp fail-->{ex.Message}"),
    //            () => Console.WriteLine("TimeStamp completed"));
    //        Console.ReadLine();

    //        ////Using TimeInterval
    //        Observable.Interval(TimeSpan.FromSeconds(1))
    //            .Take(3)
    //            .TimeInterval()
    //            .Subscribe(
    //            i => Console.WriteLine($"TimeInterval-->{i}"),
    //            ex => Console.WriteLine($"TimeInterval fail-->{ex.Message}"),
    //            () => Console.WriteLine("TimeInterval completed"));
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "Materialize and Dematerialize"
    //class Program {
    //    static void Main() {
    //        Observable.Range(1, 3)
    //            .Materialize()
    //            .Subscribe(
    //            i => Console.WriteLine($"Materialize-->{i}"),
    //            ex => Console.WriteLine($"Materialize fail -->{ex.Message}"),
    //            () => Console.WriteLine("Materialize completed"));
    //        Console.ReadLine();

    //        var source = new Subject<int>();
    //        source.Materialize()
    //            .Subscribe(
    //            i => Console.WriteLine($"Materialize-->{i}"),
    //            ex => Console.WriteLine($"Materialize-->{ex.Message}"),
    //            () => Console.WriteLine("Materialize completed"));
    //        source.OnNext(1);
    //        source.OnNext(2);
    //        source.OnNext(3);
    //        source.OnError(new Exception("Fail?"));
    //        Console.ReadLine();
    //    }
    //}
    #endregion

    #region "SelectMany"
    //class Program {
    //    static void Main() {
    //        //Observable.Return(3)
    //        //    .SelectMany(i => Observable.Range(1, i))
    //        //    .Subscribe(
    //        //    i=>Console.WriteLine($"SelectMany-->{i}"),
    //        //    ex=>Console.WriteLine($"SelectMany--> fail {ex.Message}"),
    //        //    ()=>Console.WriteLine($"SelectMany completed"));
    //        //Console.ReadKey();

    //        //Observable.Range(1, 3)
    //        //    .SelectMany(i => Observable.Range(1, i))
    //        //    .Dump("SelectMany");
    //        //Console.ReadKey();

    //        //Func<int, char> letter = i => (char)(i + 64);
    //        //Observable.Return(1)
    //        //    .SelectMany(i => Observable.Return(letter(i)))
    //        //    .Dump("SelectMany");
    //        //Console.ReadKey();

    //        //Func<int, char> letter = i => (char)(i + 64);
    //        //Observable.Range(1, 3)
    //        //    .SelectMany(i => Observable.Return(letter(i)))
    //        //    .Dump("SelectMany");
    //        //Console.ReadKey();

    //        Func<int, char> letter = i => (char)(i + 64);
    //        Observable
    //            .Range(1, 30)
    //            .SelectMany(
    //            i => {
    //                if (0 < i && i < 27) {
    //                    return Observable.Return(letter(i));
    //                }
    //                else {
    //                    return Observable.Empty<char>();
    //                }
    //            })
    //            .Dump("SelectMany");
    //        Console.ReadKey();
    //    }
    //}

    //static class Program {
    //    static void Main() {
    //        var source = Observable.Range(1, 26);
    //        source
    //            .Where(i => i % 2 == 0)
    //            .Subscribe(r => Console.WriteLine($"My Where => {r}"));

    //        source
    //            .Select(i => (char)(i + 64))
    //            .Subscribe(r => Console.WriteLine($"My Select => {r}"));
    //        Console.ReadLine();
    //    }

    //    static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate) {
    //        return source.SelectMany(
    //            item => {
    //                if (predicate(item)) {
    //                    return Observable.Return(item);
    //                }
    //                else {
    //                    return Observable.Empty<T>();
    //                }
    //            });
    //    }

    //    static IObservable<TResult> Select<TSource, TResult>(
    //        this IObservable<TSource> source,
    //        Func<TSource, TResult> selector) {
    //        return source.SelectMany(value => Observable.Return(selector(value)));
    //    }
    //}
    #endregion

    #region "IEnumerable<T> vs. IObservable<T> SelectMany"
    //class Program {
    //    static void Main() {
    //        var enumerableSource = new[] { 1, 2, 3 };
    //        var enumerableResult = enumerableSource.SelectMany(GetSubValues);
    //        foreach (var value in enumerableResult) {
    //            Console.WriteLine(value);
    //        }
    //        Console.ReadKey();
    //    }

    //    private static IEnumerable<int> GetSubValues(int offset) {
    //        yield return offset * 10;
    //        yield return (offset * 10) + 1;
    //        yield return (offset * 10) + 2;
    //    }
    //}
    #endregion

    #region "Visualizing sequences"
    //class Program {
    //    static void Main() {
    //        // Values [1,2,3] 3 seconds apart.
    //        Observable.Interval(TimeSpan.FromSeconds(3))
    //        .Select(i => i + 1) //Values start at 0, so add 1.
    //        .Take(3)            //We only want 3 values
    //        .SelectMany(GetSubValues) //project into child sequences
    //        .Dump("SelectMany");
    //        Console.ReadKey();
    //    }
    //    private static IObservable<long> GetSubValues(long offset) {
    //        ///Produce values [x*10, (x*10)+1, (x*10)+2] 4 seconds apart, but starting immediately.
    //        return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(4))
    //            .Select(x => (offset * 10) + x)
    //            .Take(3);
    //    }
    //}

    //class MyClass {
    //    static void Main() {
    //        //////var query = from i in Observable.Range(1, 5)
    //        //////            select i;
    //        //////query.Dump("SelectMany 1");
    //        //////Console.ReadKey();


    //        ////var query2 = from i in Observable.Range(1, 5)
    //        ////             where i % 2 == 0
    //        ////             select i;
    //        ////query2.Dump("SelectMany 2");
    //        ////Console.ReadKey();


    //        //var query3 = from i in Observable.Range(1, 5)
    //        //             where i % 2 == 0
    //        //             from j in GetSubValues(i)
    //        //             select j;
    //        //query3.Dump("SelectMany 3");
    //        //Console.ReadKey();

    //        ////Equivalent to
    //        //var query4 = Observable.Range(1, 5)
    //        //    .Where(i => i % 2 == 0)
    //        //    .SelectMany(GetSubValues);
    //        //query4.Dump("SelectMany 4");
    //        //Console.ReadKey();

    //        //var query5 = from i in Observable.Range(1, 5)
    //        //             where i % 2 == 0
    //        //             from j in GetSubValues(i)
    //        //             select new { i, j };
    //        //query5.Dump("SelectMany 5");
    //        //Console.ReadKey();
    //    }

    //    private static IObservable<long> GetSubValues(int offset) {
    //        ///Produce values [x*10, (x*10)+1, (x*10)+2] 4 seconds apart, but starting immediately.
    //        return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(4))
    //            .Select(x => (offset * 10) + x)
    //            .Take(3);
    //    }
    //}
    #endregion
    #endregion
}

