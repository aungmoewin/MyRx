using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Concurrency_Pitfalls {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        Subject<string> _subject = new Subject<string>();
        public MainWindow() {
            InitializeComponent();
            DataContext = this;
            Value = "Default value";
            //Deadlock! 
            //We need the dispatcher to continue to allow me to click the button to produce a value
            ///Value = _subject.First();
            //This will give same result but will not be blocking (deadlocking). 
            _subject.Take(1).Subscribe(value => Value = value);
        }
        private string _value;
        public string Value {
            get { return _value; }
            set {
                _value = value;
                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void button_Click(object sender, RoutedEventArgs e) {
            _subject.OnNext("New Value");
        }
    }
}
