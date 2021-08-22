using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdbApp.Droid
{
    public class ToastService : IToastService
    {
        private readonly Context context;

        public ToastService(Context context)
        {
            this.context = context;
        }
        public Task ShowToastAsync(string message)
        {
            var toast = Toast.MakeText(context, message, ToastLength.Short);
            toast.Show();
            return Task.CompletedTask;
        }
    }
}