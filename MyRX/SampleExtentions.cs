using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRX {
    public static class SampleExtentions {
        public static void Dump<T>(this IObservable<T> source,string name) {
            source.Subscribe(
                i=>Console.WriteLine($"{name}-->{i}"),
                ex=>Console.WriteLine($"{name} failed-->{ex.Message}"),
                ()=>Console.WriteLine($"{name} completed"));
        }
    }
}
