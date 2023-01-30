using System;
using System.Collections.Generic;
using System.Linq;
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

namespace sabatex.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ComboBoxWithButton.xaml
    /// </summary>
    public partial class ComboBoxWithButton : UserControl
    {
        public ComboBoxWithButton()
        {
            InitializeComponent();
        }


        public static DependencyProperty ItemsSourceProperty;
        public static DependencyProperty SelectedItemProperty;
        static ComboBoxWithButton()
        {
            ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(System.Collections.IEnumerable), typeof(ComboBoxWithButton), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceChanged)));
            SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ComboBoxWithButton));
            ButtonClickEvent = EventManager.RegisterRoutedEvent(nameof(ButtonClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler),typeof(ComboBoxWithButton));
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBoxWithButton = (ComboBoxWithButton)d;

        }

        public System.Collections.IEnumerable ItemsSource
        {
            get => (System.Collections.IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }


        public static readonly RoutedEvent ButtonClickEvent;
        public event System.Windows.RoutedEventHandler ButtonClick
        {
            add { AddHandler(ButtonClickEvent, value); }
            remove { RemoveHandler(ButtonClickEvent, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs();
            args.RoutedEvent = ButtonClickEvent;
            RaiseEvent(args);
        }
    }
}
