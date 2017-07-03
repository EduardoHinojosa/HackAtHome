using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HackAtHome.Entities
{
    public enum Status
    {
        Error = 0,
        Success = 1,
        InvalidUserOrNotInEvent = 2,
        OutOfDate = 3,
        AllSuccess = 999
    }
}