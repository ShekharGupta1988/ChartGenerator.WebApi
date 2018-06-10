using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartGenerator.WebAPI.Models
{
    public class Axis
    {
        /// <summary>
        /// Title of the Axis
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the data (string, int, double ...)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Data Values
        /// </summary>
        public object[] Values { get; set; }
    }
}