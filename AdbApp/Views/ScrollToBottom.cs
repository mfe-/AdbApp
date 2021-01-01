using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdbApp.Views
{
    public class ScrollToBottom
    {

        public static readonly BindableProperty EnableProperty =
            BindableProperty.CreateAttached("Enable", typeof(bool), typeof(ScrollToBottom), false, propertyChanged: ScrollToBottomPropertyChanged);

        public static void ScrollToBottomPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            if(bindableObject is ListView listView)
            {
                listView.ItemAppearing += ListView_ItemAppearing;
            }
        }

        private static void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (sender is ListView listView)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    listView.ScrollTo(e.Item, ScrollToPosition.MakeVisible, false);
                });
            }
        }

        public static bool GetEnable(BindableObject view)
        {
            return (bool)view.GetValue(EnableProperty);
        }

        public static void SetEnable(BindableObject view, bool value)
        {
            view.SetValue(EnableProperty, value);
        }
    }
}
