using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyRX {
    #region "KeyTypes"
    #region "Implementing IObserver<T> and IObservable<T>"
    //class Program {
    //    static void Main(string[] args) {
    //        var numbers = new MySequenceOfNumbers();
    //        var observer = new MyConsoleObserver<int>();
    //        //numbers.Subscribe(observer);
    //        numbers.Subscribe(i => Console.WriteLine(i),()=> Console.WriteLine("Test"));
    //        Console.ReadLine();
    //    }
    //}

    //class MyConsoleObserver<T> : IObserver<T> {
    //    public void OnCompleted() {
    //        Console.WriteLine("Sequence terminated");
    //    }

    //    public void OnError(Exception error) {
    //        Console.WriteLine($"Received value {error}");
    //    }

    //    public void OnNext(T value) {
    //        Console.WriteLine($"Received value {value}");
    //    }
    //}

    //public class MySequenceOfNumbers : IObservable<int> {
    //    public IDisposable Subscribe(IObserver<int> observer) {
    //        observer.OnNext(1);
    //        observer.OnNext(2);
    //        observer.OnNext(3);
    //        observer.OnCompleted();
    //        return Disposable.Empty;
    //    }
    //}
    #endregion

    #region "Subject<T>"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<string>();
    //        WriteSequenceToConsole(subject);
    //        subject.OnNext("a");
    //        subject.OnNext("b");
    //        subject.OnNext("c");
    //        Console.ReadKey();
    //    }

    //    //Takes an IObservable<string> as its parameter. 
    //    //Subject<string> implements this interface.
    //    static void WriteSequenceToConsole(IObservable<string> sequence) {
    //        //The next two lines are equivalent.
    //        sequence.Subscribe(value => Console.WriteLine(value));
    //        //sequence.Subscribe(Console.WriteLine);
    //    }
    //}
    #endregion

    #region "ReplaySubject<T>"
    //class Program
    //{
    //    static void Main()
    //    {
    //        #region "with bufferSize"
    //        //var bufferSize = 2;
    //        //var subject = new ReplaySubject<string>(bufferSize);
    //        //subject.OnNext("a");
    //        //subject.OnNext("b");
    //        //subject.OnNext("c");
    //        //subject.Subscribe(Console.WriteLine);
    //        //subject.OnNext("d");
    //        //Console.ReadLine();
    //        #endregion

    //        var window = TimeSpan.FromMilliseconds(150);
    //        var subject = new ReplaySubject<string>(window);
    //        subject.OnNext("w");
    //        Thread.Sleep(TimeSpan.FromMilliseconds(100));
    //        subject.OnNext("x");
    //        Thread.Sleep(TimeSpan.FromMilliseconds(100));
    //        subject.OnNext("y");
    //        subject.Subscribe(Console.WriteLine);
    //        subject.OnNext("z");
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "BehaviorSubject<T>"
    //class Program {
    //    static void Main() {
    //        //BehaviorSubjectExample();
    //        //BehaviorSubjectExample2();
    //        //BehaviorSubjectExample3();
    //        BehaviorSubjectCompletedExample();
    //        Console.ReadKey();
    //    }
    //    public static void BehaviorSubjectExample() {
    //        //Need to provide a default value.
    //        var subject = new BehaviorSubject<string>("a");
    //        subject.Subscribe(Console.WriteLine);
    //    }
    //    public static void BehaviorSubjectExample2() {
    //        var subject = new BehaviorSubject<string>("a");
    //        subject.OnNext("b");
    //        subject.Subscribe(Console.WriteLine);
    //    }

    //    public static void BehaviorSubjectExample3() {
    //        var subject = new BehaviorSubject<string>("a");
    //        subject.OnNext("b");
    //        subject.Subscribe(Console.WriteLine);
    //        subject.OnNext("c");
    //        subject.OnNext("d");
    //    }

    //    public static void BehaviorSubjectCompletedExample() {
    //        var subject = new BehaviorSubject<string>("a");
    //        subject.OnNext("b");
    //        subject.OnNext("c");
    //        subject.OnCompleted();
    //        subject.Subscribe(Console.WriteLine);
    //    }

    //}
    #endregion

    #region "AsyncSubject<T>"
    //class Program {
    //    static void Main() {
    //        var subject = new AsyncSubject<string>();
    //        subject.OnNext("a");
    //        WriteSequenceToConsole(subject);
    //        subject.OnNext("b");
    //        subject.OnCompleted();
    //        subject.OnNext("c");         
    //        Console.ReadKey();
    //    }

    //    static void WriteSequenceToConsole(IObservable<string> sequence) {
    //        sequence.Subscribe(value => Console.WriteLine(value));
    //    }
    //}
    #endregion

    #region "Implicit contracts"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<string>();
    //        subject.Subscribe(Console.WriteLine);
    //        subject.OnNext("a");
    //        subject.OnNext("b");
    //        subject.OnCompleted();
    //        subject.OnNext("c");
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #endregion

    #region "Lifetime management"
    #region "Subscribing"
    //class Program {
    //    static void Main() {
    //        #region "Raise OnError"
    //        //var values = new Subject<int>();
    //        //try {
    //        //    values.Subscribe(value => Console.WriteLine($"1st subscription received {value}"));
    //        //}
    //        //catch (Exception) {
    //        //    Console.WriteLine("Won't catch anything here!");
    //        //}
    //        //values.OnNext(0);
    //        ////Exception will be thrown here causing the app to fail.
    //        //values.OnError(new Exception("Dummy exception"));
    //        //Console.ReadKey();
    //        #endregion

    //        #region "Catching OnError"
    //        //var values = new Subject<int>();
    //        //values.Subscribe(
    //        //    value => Console.WriteLine($"1st scription received {value}"),
    //        //    ex => Console.WriteLine($"Caught an exception : {ex}"));
    //        //values.OnNext(0);
    //        //values.OnError(new Exception("Dummy exception"));
    //        //Console.ReadKey();
    //        #endregion
    //    }
    //}
    #endregion

    #region "Unsubscribing"
    //class Program {
    //    static void Main() {
    //        var values = new Subject<int>();
    //        var firstSubscription = values.Subscribe(value =>
    //        Console.WriteLine($"1st subscription received {value}"));
    //        var secondSubscription = values.Subscribe(value =>
    //        Console.WriteLine($"2nd subscription received {value}"));
    //        values.OnNext(0);
    //        values.OnNext(1);
    //        values.OnNext(2);
    //        values.OnNext(3);
    //        firstSubscription.Dispose();
    //        Console.WriteLine("Disposed of 1st subscription");
    //        values.OnNext(4);
    //        values.OnNext(5);
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "OnError and OnCompleted"
    //class Program {
    //    static void Main() {
    //        var subject = new Subject<int>();
    //        subject.Subscribe(Console.Write,
    //            () => Console.WriteLine("Completed"));
    //        subject.OnCompleted();
    //        subject.OnNext(2);
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "IDisposable"
    #region "TimeIt"
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

    //    static void Main() {
    //        using (new TimeIt("Outer Scope")) {
    //            using (new TimeIt("Inner Scope A")) {
    //                DoSomeWork("A");
    //            }
    //            using (new TimeIt("Inner Scope B")) {
    //                DoSomeWork("B");
    //            }
    //            Cleanup();
    //        }
    //        Console.ReadKey();
    //    }
    //}
    #endregion

    #region "ConsoleColor"
    //class ConsoleColor : IDisposable {
    //    private readonly System.ConsoleColor _previousColor;
    //    public ConsoleColor(System.ConsoleColor color) {
    //        _previousColor = Console.ForegroundColor;
    //        Console.ForegroundColor = color;
    //    }
    //    public void Dispose() {
    //        Console.ForegroundColor = _previousColor;
    //    }
    //    static void Main() {
    //        Console.WriteLine("Normal color");
    //        using (new ConsoleColor(System.ConsoleColor.Red)) {
    //            Console.WriteLine("Now I am Red");
    //            using (new ConsoleColor(System.ConsoleColor.Green)) {
    //                Console.WriteLine("Now I am Green");
    //            }
    //            Console.WriteLine("and back to Red");
    //        }
    //        Console.WriteLine("Now I am Normal color again");
    //        Console.ReadKey();
    //    }
    //}
    #endregion
    #endregion
    #endregion
}