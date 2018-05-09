using System;
using System.ComponentModel;
using System.Windows;
//using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Concurrency_Pitfalls {
    /// <summary>
    /// Interaction logic for Lock_ups2.xaml
    /// </summary>
    public partial class Lock_ups2 : Window, INotifyPropertyChanged {
        //Imagine DI here.
        private readonly MyService _service = new MyService();
        private string _value2;
        public string Value2 {
            get { return _value2; }
            set {
                _value2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value2"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Lock_ups2() {
            InitializeComponent();
            DataContext = this;
            Value2 = "Default value";
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            Value2 = _service.GetTemperature().First();
        }
    }
    class MyService {
        public IObservable<string> GetTemperature() {
            return Observable.Create<string>(
            o => {
                o.OnNext("27");
                o.OnNext("26");
                o.OnNext("24");
                return () => { };
            });
            // .SubscribeOnDispatcher();
        }
    }
}
