using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery_com.DataModel
{
    public class Schedule
    {
        public Schedule()
        {
            business = new List<Day>();
            delivery = new List<Day>();
        }

        public List<Day> business { get; set; }

        public List<Day> delivery { get; set; }
    }

    public class CurrentSchedule : Schedule
    {
        public Availability availability { get; set; }
    }



    public class Day
    {
        public Day(string Name, List<Times> TimesOpen)
        {
            this.name = Name;
            this.times_open = TimesOpen;
        }


        public string name { get; set; }

        public List<Times> times_open { get; set; }

        // only is used in the Current Schedule
        public string date { get; set; }
    }


    public class Times
    {
        public Times(string Begin, string Finish)
        {
            this.start = Begin;
            this.end = Finish;
        }

        public string start { get; set; }

        public string end { get; set; }
    }

}
