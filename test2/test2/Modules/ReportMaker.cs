using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;
using test2.Models;
using System.IO;
using System.Drawing;
using OxyPlot.Axes;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace test2.Modules
{
    public class ReportMaker
    {
        public Bitmap DrawPie(Report rp)
        {
            var plPie = new PlotModel { Title = "Pie Chart" };
            var pie = new PieSeries() { AngleSpan = 360, StartAngle = 0, StrokeThickness = 5.0, InsideLabelPosition = 0.65, FontSize = 22 };
            foreach (KeyValuePair<string, double> tmp in rp.PieChartData)
            {
                pie.Slices.Add(new PieSlice(tmp.Key, tmp.Value));
            }
            plPie.Series.Add(pie);
            PngExporter png = new PngExporter();
            /*{
                Width = 500,
                Height = 500,
                Resolution = 128
            };*/
            //png.ExportToFile(plPie, path + @"\Temp\Report_" + rp.Id.ToString() + "_pie.png");
            Bitmap pieBmp = png.ExportToBitmap(plPie);
            return pieBmp;
        }
        public Bitmap DrawBar(Report rp)
        {
            var plBar = new PlotModel() { Title = "Bar Chart" };
            var bar = new ColumnSeries();
            var cat = new CategoryAxis() { Position = AxisPosition.Bottom };
            foreach (KeyValuePair<string, double> tmp in rp.BarChartData)
            {
                bar.Items.Add(new ColumnItem() { Value = tmp.Value });
                cat.Labels.Add(tmp.Key);
            }
            plBar.Series.Add(bar);
            plBar.Axes.Add(cat);
            PngExporter png = new PngExporter();
            /*{
                Width = 500,
                Height = 500,
                Resolution = 128
            };*/
            //png.ExportToFile(plBar, path + @"\Temp\Report_" + rp.Id.ToString() + "_bar.png");
            Bitmap barBmp = png.ExportToBitmap(plBar);
            return barBmp;
        }
        public void CreateReport(Report rp, string path)
        {
            var doc = new Document(iTextSharp.text.PageSize.A4);
            var pieImg = iTextSharp.text.Image.GetInstance(DrawPie(rp), BaseColor.White);
            var barImg = iTextSharp.text.Image.GetInstance(DrawBar(rp), BaseColor.White);
            pieImg.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            barImg.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            barImg.ScalePercent(75);
            pieImg.ScalePercent(75);
            var fs = new FileStream(path + @"\Temp\Report_" + rp.Id.ToString() + ".pdf", FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            doc.Add(new Paragraph(rp.Name));
            doc.AddTitle(rp.Name);
            doc.Add(pieImg);
            doc.Add(barImg);
            doc.Add(new Paragraph(rp.Text));
            doc.Add(new Paragraph("Path to image: " + rp.ImagePath));
            doc.Close();
            writer.Close();
            fs.Flush();
            fs.Dispose();
            //DrawBar(rp, path);
            //DrawPie(rp, path);
        }
    }
}
