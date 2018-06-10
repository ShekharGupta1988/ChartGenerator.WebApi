using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartGenerator.WebAPI.Models
{
    public class PostBody
    {
        public Axis XAxis;

        public Axis YAxis;

        public ChartProperties Chart;
    }
}