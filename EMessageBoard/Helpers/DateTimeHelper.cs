using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EMessageBoard.Helpers
{
    class DateTimeHelper
    {
        static DispatcherTimer timerTick = new DispatcherTimer();
        static System.Windows.Controls.Label lbl;

        public static void timeUpdater(System.Windows.Controls.Label l)
        {
            lbl = l;
            timerTick.Tick += timerTick_Tick;
            timerTick.Interval = new TimeSpan(00, 00, 01);
            timerTick.Start();
        }

        static void timerTick_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            lbl.Content = date.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-us"));
            Console.WriteLine(date.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("en-us")));
        }

    }
}
