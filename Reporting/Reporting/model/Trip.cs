using System;
using System.Collections.Generic;
using System.Text;

namespace Reporting.model
{
    public class Trip
    {
        public string From { get; set; }
        public string To { get; set; }
        public double Distance { get; set; }
        public string Code { get; set; }
        public DateTime? DateTrip { get; set; }
    }
}
