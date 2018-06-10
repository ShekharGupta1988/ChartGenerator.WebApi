using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartGenerator.WebAPI.Models
{
    public class ChartProperties
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string ChartType { get; set; }

        public RGBColor Color { get; set; }
    }
}