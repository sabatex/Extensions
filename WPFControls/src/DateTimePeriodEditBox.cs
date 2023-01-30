using sabatex.Extensions.DateTimeExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace sabatex.WPF.Controls
{
    [TemplatePart(Name = DatePeriodTextBox.ElementContentName, Type = typeof(ContentControl))]
    public sealed partial class DatePeriodTextBox : TextBox
    {
        #region Constants
        private const string ElementContentName = "PART_Watermark";

        #endregion

        #region Data

        private ContentControl elementContent;

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor
        /// </summary>
        static DatePeriodTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePeriodTextBox), new FrameworkPropertyMetadata(typeof(DatePeriodTextBox)));
            TextProperty.OverrideMetadata(typeof(DatePeriodTextBox), new FrameworkPropertyMetadata(OnVisualStatePropertyChanged));
        }

        private static void OnVisualStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatePickerTextBox"/> class.
        /// </summary>
        public DatePeriodTextBox()
        {
            //this.SetCurrentValue(WatermarkProperty, SR.Get(SRID.DatePickerTextBox_DefaultWatermarkText));
            this.Loaded += OnLoaded;
            this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(OnDatePickerTextBoxIsEnabledChanged);
        }

        private void OnDatePickerTextBoxIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Public Properties

        #region Watermark
        /// <summary>
        /// Watermark dependency property
        /// </summary>
        internal static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(object), typeof(DatePeriodTextBox), new PropertyMetadata(OnWatermarkPropertyChanged));

        private static void OnWatermarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Watermark content
        /// </summary>
        /// <value>The watermark.</value>
        internal object Watermark
        {
            get { return (object)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        #endregion

        #endregion Public Properties

        #region Protected

        /// <summary>
        /// Called when template is applied to the control.
        /// </summary>
        //public override void OnApplyTemplate()
        //{
        //    base.OnApplyTemplate();

        //    elementContent = ExtractTemplatePart<ContentControl>(ElementContentName);

        //    // We dont want to expose watermark property as public yet, because there
        //    // is a good chance in future that the implementation will change when
        //    // a WatermarkTextBox control gets implemented. This is mostly to
        //    // mimc SL. Hence setting the binding in code rather than in control template.
        //    if (elementContent != null)
        //    {
        //        Binding watermarkBinding = new Binding("Watermark");
        //        watermarkBinding.Source = this;
        //        elementContent.SetBinding(ContentControl.ContentProperty, watermarkBinding);
        //    }

        //    OnWatermarkChanged();
        //}

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (IsEnabled)
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    Select(0, this.Text.Length);
                }
            }
        }

        #endregion Protected

        #region Private

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ApplyTemplate();
        }

        /// <summary>
        /// Change to the correct visual state for the textbox.
        /// </summary>
        /// <param name="useTransitions">
        /// true to use transitions when updating the visual state, false to
        /// snap directly to the new visual state.
        /// </param>


        //internal void ChangeVisualState(bool useTransitions)
        //{
        //    //base.ChangeVisualState(useTransitions);

        //    // Update the WatermarkStates group
        //    if (this.Watermark != null && string.IsNullOrEmpty(this.Text))
        //    {
        //        VisualStates.GoToState(this, useTransitions, VisualStates.StateWatermarked, VisualStates.StateUnwatermarked);
        //    }
        //    else
        //    {
        //        VisualStates.GoToState(this, useTransitions, VisualStates.StateUnwatermarked);
        //    }
        //}

        //private T ExtractTemplatePart<T>(string partName) where T : DependencyObject
        //{
        //    DependencyObject obj = GetTemplateChild(partName);
        //    return ExtractTemplatePart<T>(partName, obj);
        //}

        //private static T ExtractTemplatePart<T>(string partName, DependencyObject obj) where T : DependencyObject
        //{
        //    Debug.Assert(
        //        obj == null || typeof(T).IsInstanceOfType(obj),
        //        string.Format(CultureInfo.InvariantCulture, SR.Get(SRID.DatePickerTextBox_TemplatePartIsOfIncorrectType), partName, typeof(T).Name));
        //    return obj as T;
        //}

        /// <summary>
        /// Called when the IsEnabled property changes.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Property changed args</param>
        //private void OnDatePickerTextBoxIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    Debug.Assert(e.NewValue is bool);
        //    bool isEnabled = (bool)e.NewValue;

        //    SetCurrentValueInternal(IsReadOnlyProperty, MS.Internal.KnownBoxes.BooleanBoxes.Box(!isEnabled));
        //}

        private void OnWatermarkChanged()
        {
            if (elementContent != null)
            {
                Control watermarkControl = this.Watermark as Control;
                if (watermarkControl != null)
                {
                    watermarkControl.IsTabStop = false;
                    watermarkControl.IsHitTestVisible = false;
                }
            }
        }

        /// <summary>
        /// Called when watermark property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        //private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        //{
        //    DatePickerTextBox datePickerTextBox = sender as DatePickerTextBox;
        //    Debug.Assert(datePickerTextBox != null, "The source is not an instance of a DatePickerTextBox!");
        //    datePickerTextBox.OnWatermarkChanged();
        //    datePickerTextBox.UpdateVisualState();
        //}

        #endregion Private
    }

}
