using sabatex.Extensions.DateTimeExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace sabatex.WPF.Controls
{
    /// <summary>
    /// Represents a control that allows the user to select a date.
    /// </summary>
    [TemplatePart(Name = DateTimePeriodPicker.ElementRoot, Type = typeof(Grid))]
    [TemplatePart(Name = DateTimePeriodPicker.ElementDateTimePeriod, Type = typeof(DateTimePeriod))]
    [TemplatePart(Name = DateTimePeriodPicker.ElementButton, Type = typeof(Button))]
    [TemplatePart(Name = DateTimePeriodPicker.ElementPopup, Type = typeof(Popup))]
    public class DateTimePeriodPicker:Control
    {
        #region Constants

        private const string ElementRoot = "PART_Root";
        private const string ElementDateTimePeriod = "PART_DateTimePeriod";
        private const string ElementButton = "PART_Button";
        private const string ElementPopup = "PART_Popup";

        #endregion Constants


        #region Data
        private DateTimePeriodSelect _periodSelector;
        private bool _disablePopupReopen;
        private Popup _popUp;
        private Period? _originalSelectedPeriod;
        #endregion Data

        public DateTimePeriodPicker()
        {
            //InitializeDateTimePeriod();
            //this._defaultText = string.Empty;

            //this.SetCurrentValue(FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateFormat().FirstDayOfWeek);
            //this.SetCurrentValue(DisplayDateProperty, DateTime.Today);

        }


        //#region Public Properties

        //#region FirstDayOfWeek

        ///// <summary>
        ///// Gets or sets the day that is considered the beginning of the week.
        ///// </summary>
        //public DayOfWeek FirstDayOfWeek
        //{
        //    get { return (DayOfWeek)GetValue(FirstDayOfWeekProperty); }
        //    set { SetValue(FirstDayOfWeekProperty, value); }
        //}

        ///// <summary>
        ///// Identifies the FirstDayOfWeek dependency property.
        ///// </summary>
        //public static readonly DependencyProperty FirstDayOfWeekProperty =
        //    DependencyProperty.Register(
        //    "FirstDayOfWeek",
        //    typeof(DayOfWeek),
        //    typeof(DateTimePeriodPicker),
        //    null,
        //    Calendar.IsValidFirstDayOfWeek);

        //#endregion FirstDayOfWeek


        //#region IsDropDownOpen

        ///// <summary>
        ///// Gets or sets a value that indicates whether the drop-down Calendar is open or closed.
        ///// </summary>
        //public bool IsDropDownOpen
        //{
        //    get { return (bool)GetValue(IsDropDownOpenProperty); }
        //    set { SetValue(IsDropDownOpenProperty, value); }
        //}

        ///// <summary>
        ///// Identifies the IsDropDownOpen dependency property.
        ///// </summary>
        //public static readonly DependencyProperty IsDropDownOpenProperty =
        //    DependencyProperty.Register(
        //    nameof(IsDropDownOpen),
        //    typeof(bool),
        //    typeof(DateTimePeriodPicker),
        //    new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged, OnCoerceIsDropDownOpen));

        //private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue)
        //{
        //    var dp = d as DateTimePeriodPicker;
        //    Debug.Assert(dp != null);

        //    if (!dp.IsEnabled)
        //    {
        //        return false;
        //    }

        //    return baseValue;
        //}

        ///// <summary>
        ///// IsDropDownOpenProperty property changed handler.
        ///// </summary>
        ///// <param name="d">DatePicker that changed its IsDropDownOpen.</param>
        ///// <param name="e">DependencyPropertyChangedEventArgs.</param>
        //private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var dp = d as DateTimePeriodPicker;
        //    Debug.Assert(dp != null);

        //    bool newValue = (bool)e.NewValue;
        //    if (dp._popUp != null && dp._popUp.IsOpen != newValue)
        //    {
        //        dp._popUp.IsOpen = newValue;
        //        if (newValue)
        //        {
        //            dp._originalSelectedPeriod = dp.SelectedPeriod;

        //            // When the popup is opened set focus to the DisplayDate button.
        //            // Do this asynchronously because the IsDropDownOpen could
        //            // have been set even before the template for the DatePicker is
        //            // applied. And this would mean that the visuals wouldn't be available yet.

        //            dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, (Action)delegate ()
        //            {
        //                // setting the focus to the calendar will focus the correct date.
        //                dp._periodSelector.Focus();
        //            });
        //        }
        //    }
        //}

        //private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DatePicker dp = d as DatePicker;
        //    Debug.Assert(dp != null);

        //    dp.CoerceValue(IsDropDownOpenProperty);

        //    OnVisualStatePropertyChanged(d, e);
        //}

        //#endregion IsDropDownOpen

        //#region SelectedPeriod

        ///// <summary>
        ///// Gets or sets the currently selected date.
        ///// </summary>
        /////
        //public Period? SelectedPeriod
        //{
        //    get { return (Period?)GetValue(SelectedPeriodProperty); }
        //    set { SetValue(SelectedPeriodProperty, value); }
        //}

        ///// <summary>
        ///// Identifies the SelectedDate dependency property.
        ///// </summary>
        //public static readonly DependencyProperty SelectedPeriodProperty =
        //    DependencyProperty.Register(
        //    nameof(SelectedPeriod),
        //    typeof(Period?),
        //    typeof(DateT),
        //    new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged, CoerceSelectedDate));

        ///// <summary>
        ///// SelectedDateProperty property changed handler.
        ///// </summary>
        ///// <param name="d">DatePicker that changed its SelectedDate.</param>
        ///// <param name="e">DependencyPropertyChangedEventArgs.</param>
        //private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DatePicker dp = d as DatePicker;
        //    Debug.Assert(dp != null);

        //    Collection<DateTime> addedItems = new Collection<DateTime>();
        //    Collection<DateTime> removedItems = new Collection<DateTime>();
        //    DateTime? addedDate;
        //    DateTime? removedDate;

        //    dp.CoerceValue(DisplayDateStartProperty);
        //    dp.CoerceValue(DisplayDateEndProperty);
        //    dp.CoerceValue(DisplayDateProperty);

        //    addedDate = (DateTime?)e.NewValue;
        //    removedDate = (DateTime?)e.OldValue;

        //    if (dp.SelectedDate.HasValue)
        //    {
        //        DateTime day = dp.SelectedDate.Value;
        //        dp.SetTextInternal(dp.DateTimeToString(day));

        //        // When DatePickerDisplayDateFlag is TRUE, the SelectedDate change is coming from the Calendar UI itself,
        //        // so, we shouldn't change the DisplayDate since it will automatically be changed by the Calendar
        //        if ((day.Month != dp.DisplayDate.Month || day.Year != dp.DisplayDate.Year) && !dp._calendar.DatePickerDisplayDateFlag)
        //        {
        //            dp.SetCurrentValueInternal(DisplayDateProperty, day);
        //        }

        //        dp._calendar.DatePickerDisplayDateFlag = false;
        //    }
        //    else
        //    {
        //        dp.SetWaterMarkText();
        //    }

        //    if (addedDate.HasValue)
        //    {
        //        addedItems.Add(addedDate.Value);
        //    }

        //    if (removedDate.HasValue)
        //    {
        //        removedItems.Add(removedDate.Value);
        //    }

        //    dp.OnSelectedDateChanged(new CalendarSelectionChangedEventArgs(DatePicker.SelectedDateChangedEvent, removedItems, addedItems));

        //    DatePickerAutomationPeer peer = UIElementAutomationPeer.FromElement(dp) as DatePickerAutomationPeer;
        //    // Raise the propetyChangeEvent for Value if Automation Peer exist
        //    if (peer != null)
        //    {
        //        string addedDateString = addedDate.HasValue ? dp.DateTimeToString(addedDate.Value) : "";
        //        string removedDateString = removedDate.HasValue ? dp.DateTimeToString(removedDate.Value) : "";
        //        peer.RaiseValuePropertyChangedEvent(removedDateString, addedDateString);
        //    }
        //}

        //private static object CoerceSelectedDate(DependencyObject d, object value)
        //{
        //    DatePicker dp = d as DatePicker;

        //    // We set _calendar.SelectedDate in order to get _calendar to compute the coerced value
        //    dp._calendar.SelectedDate = (DateTime?)value;
        //    return dp._calendar.SelectedDate;
        //}

        //#endregion SelectedDate


        //#endregion



        //#region Private Methods
        //private void TogglePopUp()
        //{
        //    if (this.IsDropDownOpen)
        //    {
        //        this.SetCurrentValue(IsDropDownOpenProperty, BooleanBoxes.FalseBox);
        //    }
        //    else
        //    {
        //        if (this._disablePopupReopen)
        //        {
        //            this._disablePopupReopen = false;
        //        }
        //        else
        //        {
        //            SetSelectedDate();
        //            this.SetCurrentValue(IsDropDownOpenProperty, BooleanBoxes.TrueBox);
        //        }
        //    }
        //}

        //private void Button_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    this._disablePopupReopen = false;
        //}
        //private void InitializeDateTimePeriod()
        //{
        //    _periodSelector = new DateTimePeriodSelect();
        //    //_calendar.DayButtonMouseUp += new MouseButtonEventHandler(Calendar_DayButtonMouseUp);
        //    //_calendar.DisplayDateChanged += new EventHandler<CalendarDateChangedEventArgs>(Calendar_DisplayDateChanged);
        //    //_calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(Calendar_SelectedDatesChanged);
        //    //_calendar.DayOrMonthPreviewKeyDown += new RoutedEventHandler(CalendarDayOrMonthButton_PreviewKeyDown);
        //    _periodSelector.HorizontalAlignment = HorizontalAlignment.Left;
        //    _periodSelector.VerticalAlignment = VerticalAlignment.Top;

        //    //_calendar.SelectionMode = CalendarSelectionMode.SingleDate;
        //    //_calendar.SetBinding(Calendar.ForegroundProperty, GetDatePickerBinding(DatePicker.ForegroundProperty));
        //    //_calendar.SetBinding(Calendar.StyleProperty, GetDatePickerBinding(DatePicker.CalendarStyleProperty));
        //    //_calendar.SetBinding(Calendar.IsTodayHighlightedProperty, GetDatePickerBinding(DatePicker.IsTodayHighlightedProperty));
        //    //_calendar.SetBinding(Calendar.FirstDayOfWeekProperty, GetDatePickerBinding(DatePicker.FirstDayOfWeekProperty));
        //    //_calendar.SetBinding(Calendar.FlowDirectionProperty, GetDatePickerBinding(DatePicker.FlowDirectionProperty));

        //    RenderOptions.SetClearTypeHint(_periodSelector, ClearTypeHint.Enabled);
        //}

        //#endregion Private Methods


    }
}
