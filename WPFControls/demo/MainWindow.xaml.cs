using sabatex.WPF.Controls.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Controls.Demo.Models;

namespace WPF.Controls.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public MainWindowModel model;
        public MainWindow()
        {
            InitializeComponent();
        }
        

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //model = new MainWindowModel();
            DataContext = MainWindowModel.TestModel;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var editItems = new Views.EditComboboxItems();
            editItems.Owner = this;
            editItems.ShowDialog();
        }

        Timer timer;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //DataContext = MainWindowModel.TestModel;
            Trace.Listeners.Add(new TextBoxTraceListener(traceListner));
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Trace.WriteLine("Trace log ...");
        }
    }
}
