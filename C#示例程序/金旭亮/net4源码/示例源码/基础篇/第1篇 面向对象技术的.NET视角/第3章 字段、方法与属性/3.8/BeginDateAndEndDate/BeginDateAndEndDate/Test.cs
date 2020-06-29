using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BeginDateAndEndDate
{
    class Test : DependencyObject
    {
        public DateTime BeginDate
        {
            get { return (DateTime)GetValue(BeginDateProperty); }
            set { SetValue(BeginDateProperty, value); }
        }

     
        public static readonly DependencyProperty BeginDateProperty =
            DependencyProperty.Register("BeginDate", typeof(DateTime), typeof(Test),
             new PropertyMetadata(DateTime.Now,
            new PropertyChangedCallback(OnBeginDateChanged),
            new CoerceValueCallback(CoerceBeginDate)));

        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

 
        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime), typeof(Test),
            new FrameworkPropertyMetadata(DateTime.Now.AddDays(1),
            new PropertyChangedCallback(OnEndDateChanged),
            new CoerceValueCallback(CoerceEndDate)));

        private static void OnBeginDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(BeginDateProperty);
        }
        private static void OnEndDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(EndDateProperty);
        }
        private static object CoerceBeginDate(DependencyObject d, object value)
        {
            DateTime current = (DateTime)value;
            Test t = d as Test;
            if (t.EndDate < current) current = t.EndDate;
            return current;
        }
        private static object CoerceEndDate(DependencyObject d, object value)
        {
            DateTime current = (DateTime)value;
            Test t = d as Test;
            if (t.BeginDate > current) current = t.BeginDate;
            return current;
        }

    }
}
