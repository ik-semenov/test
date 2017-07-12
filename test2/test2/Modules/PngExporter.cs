using OxyPlot;
using System.Drawing;
using System.IO;


namespace test2.Modules
{
    public class PngExporter : IExporter
    {
        public PngExporter()
        {
            this.Width = 700;
            this.Height = 400;
            this.Resolution = 96;
            this.Background = OxyColors.White;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Resolution { get; set; }
        public OxyColor Background { get; set; }

        /*public void Export(IPlotModel model, Stream stream)
        {
            throw new NotImplementedException();
        }*/
        /// <summary>
        /// Exports the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        public static void Export(IPlotModel model, string fileName, int width, int height)
        {
            var exporter = new PngExporter { Width = width, Height = height };
            using (var stream = File.Create(fileName))
            {
                exporter.Export(model, stream);
            }
        }
        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        public void Export(IPlotModel model, Stream stream)
        {
            using (var bm = this.ExportToBitmap(model))
            {
                bm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a <see cref="Bitmap" />.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <returns>A bitmap.</returns>
        public Bitmap ExportToBitmap(IPlotModel model)
        {
            var bm = new Bitmap(this.Width, this.Height);
            using (var g = Graphics.FromImage(bm))
            {
                if (this.Background.IsVisible())
                {
                    using (var brush = this.Background.ToBrush())
                    {
                        g.FillRectangle(brush, 0, 0, Width, this.Height);
                    }
                }
                using (var rc = new GraphicsRenderContext(g) { RendersToScreen = false })
                {
                    model.Update(true);
                    model.Render(rc, this.Width, this.Height);
                }

                bm.SetResolution(this.Resolution, this.Resolution);
                return bm;
            }
        }
    }
}
