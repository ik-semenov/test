using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test2.Models
{
    public class Report
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }
		public Dictionary<string, double> PieChartData { get; set; }
		public Dictionary<string, double> BarChartData { get; set; }
		public string ImagePath { get; set; }
	}
}
