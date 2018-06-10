using ChartGenerator.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartGenerator.WebAPI.Controllers
{
    public class ChartController : ApiController
    {
        // POST: api/Chart
        public IHttpActionResult Post([FromBody]PostBody body)
        {
            if(body == null)
            {
                return BadRequest();
            }

            var chart = new Chart()
            {
                DataSource = GetDataTable(body),
                Width = Convert.ToInt32(body.Chart.Width),
                Height = Convert.ToInt32(body.Chart.Height)
            };

            var ca = new ChartArea();

            ca.AxisX.Title = body.XAxis.Name;
            ca.AxisX.MajorGrid.LineColor = Color.LightGray;
            ca.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            ca.AxisX.MajorGrid.LineWidth = 0;
            ca.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            ca.AxisX.IsMarginVisible = false;

            ca.AxisY.Title = body.YAxis.Name;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(226, 226, 226);
            ca.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Solid;
            ca.AxisY.MajorGrid.LineWidth = 1;
            ca.AxisY.IsStartedFromZero = true;
            ca.AxisY.MajorTickMark = new TickMark() { Interval = 5, IntervalOffset = 5, TickMarkStyle = TickMarkStyle.OutsideArea };
            ca.AxisY.MinorTickMark = new TickMark() { Interval = 5, Enabled = false };

            chart.ChartAreas.Add(ca);

            var series = new Series
            {
                ChartType = GetChartType(body.Chart.ChartType),
                Color = Color.FromArgb(body.Chart.Color.Red, body.Chart.Color.Green, body.Chart.Color.Blue),
                IsVisibleInLegend = true,
                IsXValueIndexed = false,
                BorderWidth = 1,
                XValueMember = body.XAxis.Name,
                YValueMembers = body.YAxis.Name
            };

            chart.Series.Add(series);

            string filePath = Directory.GetCurrentDirectory() + $@"\temp\{Guid.NewGuid().ToString()}";
            FileInfo fs = new FileInfo(filePath);
            if (fs.Exists)
            {
                fs.Delete();
            }

            chart.SaveImage(fs.FullName, ChartImageFormat.Png);

            Byte[] imageArray = File.ReadAllBytes(fs.FullName);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            string imageContent = $@"data:image/png;base64,{base64ImageRepresentation}";

            fs.Delete();
            return Ok(imageContent);
        }

        private SeriesChartType GetChartType(string chartType)
        {
            switch (chartType.ToLower())
            {
                case "area":
                    return SeriesChartType.Area;
                case "bar":
                    return SeriesChartType.Bar;
                default:
                    return SeriesChartType.Line;
            }
        }

        private DataTable GetDataTable(PostBody postBody)
        {
            DataTable dt = new DataTable();
            
            dt.Columns.Add(postBody.XAxis.Name, GetType(postBody.XAxis.Type));
            dt.Columns.Add(postBody.YAxis.Name, GetType(postBody.XAxis.Type));

            for(int iter = 0; iter < postBody.XAxis.Values.Length; iter++)
            {
                dt.Rows.Add(postBody.XAxis.Values[iter], postBody.YAxis.Values[iter]);
            }
            
            return dt;
        }

        private Type GetType(string type)
        {
            switch (type.ToLower())
            {
                case "int":
                    return typeof(int);
                case "double":
                    return typeof(double);
                case "float":
                    return typeof(float);
                case "long":
                    return typeof(long);
                default:
                    return typeof(string);
            }
        }
    }
}
