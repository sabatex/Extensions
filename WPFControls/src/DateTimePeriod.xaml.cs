using sabatex.Extensions.DateTimeExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for PeriodWPF.xaml
    /// </summary>
    public partial class DateTimePeriod : UserControl
    {


        public DateTimePeriod()
        {
            InitializeComponent();
        }

        public readonly static DependencyProperty PeriodProperty = DependencyProperty.Register(
                                     nameof(Period),
                                     typeof(Period),
                                     typeof(DateTimePeriod),
                                     new FrameworkPropertyMetadata(
                                         new Period(),
                                         FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                         new PropertyChangedCallback(OnPeriodChanged)));

        private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var period = (Period)e.NewValue;
            d.SetValue(BeginDateProperty, period.Begin);
            d.SetValue(EndDateProperty, period.End);
        }

        public readonly static DependencyProperty BeginDateProperty = DependencyProperty.Register(
                                     nameof(BeginDate),
                                     typeof(DateTime?),
                                     typeof(DateTimePeriod),
                                     new FrameworkPropertyMetadata(
                                new DateTime?(),
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                new PropertyChangedCallback(OnBeginDateChanged)));

        private static void OnBeginDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var period = (Period)d.GetValue(PeriodProperty);
            period.Begin = (DateTime?)e.NewValue;
            d.SetValue(PeriodProperty, period);
        }

        public readonly static DependencyProperty EndDateProperty = DependencyProperty.Register(
                                      nameof(EndDate),
                                      typeof(DateTime?),
                                      typeof(DateTimePeriod),
                                      new FrameworkPropertyMetadata(
                                new DateTime?(),
                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                new PropertyChangedCallback(OnEndDateTimeChanged)));

        private static void OnEndDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var period = (Period)d.GetValue(PeriodProperty);
            period.End = (DateTime?)e.NewValue;
            d.SetValue(PeriodProperty, period);
        }

        public Period Period
        {
            get => (Period)GetValue(PeriodProperty);
            set => SetValue(PeriodProperty, value);
        }
        private DateTime? BeginDate
        {
            get => (DateTime?)GetValue(BeginDateProperty);
            set => SetValue(BeginDateProperty, value);
        }
        public DateTime? EndDate
        {
            get => (DateTime?)GetValue(EndDateProperty);
            set => SetValue(EndDateProperty, value);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TogglePopUp();
        }

    }
}
