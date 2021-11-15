using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using Studying_app_kg.Model;
using LinearAxis = OxyPlot.Axes.LinearAxis;
using LineSeries = OxyPlot.Series.LineSeries;

namespace Studying_app_kg.Views
{
    /// <summary>
    /// Interaction logic for AffineTransformations.xaml
    /// </summary>
    public partial class AffineTransformations : Page
    {
        private AfiineTransformator _transformator;
        private bool _isInitialScaleSet = false;
        private double initialScale = 0;
        public AffineTransformations()
        {
            InitializeComponent();
            SetInitialWindow();
            //Paint_OnClick(this, new RoutedEventArgs());
            _transformator = new AfiineTransformator();
            X1_OnTextChanged(this, null);
            AffineImage.Model.Updated += ModelOnUpdated;
        }

        private void ModelOnUpdated(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                double scale = AffineImage.Model.Axes[0].Scale;
                if (Math.Abs(scale - 0) < 0.0001)
                {
                    ScaleLabel.Content = "Масштаб: " + 100 + "%";
                }
                else if (!_isInitialScaleSet)
                {
                        initialScale = scale;
                        ScaleLabel.Content = "Масштаб: " + 100 + "%";
                        _isInitialScaleSet = true;
                }
                else
                {
                    ScaleLabel.Content = "Масштаб: " + Math.Abs(Math.Round(scale / initialScale * 100, 2)) + "%";
                }
                //ScaleLabel.Content = scale;
                /*if (initialScale != 0)
                    ScaleLabel.Content = "Масштаб: " + Math.Abs(Math.Round(scale/initialScale *100,2)) + "%";
                else
                {
                    ScaleLabel.Content = "Масштаб: " + 100 + "%";
                }*/
            });
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new MainPage());
            
        }

        private PlotModel SetPlot()
        {
            var pm = new PlotModel();
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -60,
                Maximum = 60,
                PositionAtZeroCrossing = true,
                ExtraGridlines = new[] { 0.0 },
                //Title = "x"
            });
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -60,
                Maximum = 60,
                PositionAtZeroCrossing = true,
                ExtraGridlines = new[] { 0.0 },
                //Title = "y"
            });
            pm.Title = "Triangle";
            pm.Updated += ModelOnUpdated;
            pm.PlotType = PlotType.Cartesian;
            //AffineImage.Model = pm;
            return pm;
        }
        private void Paint_OnClick(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(PaintFunction);
            thread.Start();
        }

        private void PaintFunction()
        {
            try
            {
                int x1 = Int32.Parse(this.x1.Dispatcher.Invoke(() => { return this.x1.Text; }));
                int y1 = Int32.Parse(this.y1.Dispatcher.Invoke(() => { return this.y1.Text; }));
                int x2 = Int32.Parse(this.x1.Dispatcher.Invoke(() => { return this.x2.Text; }));
                int y2 = Int32.Parse(this.y1.Dispatcher.Invoke(() => { return this.y2.Text; }));
                int x3 = Int32.Parse(this.x1.Dispatcher.Invoke(() => { return this.x3.Text; }));
                int y3 = Int32.Parse(this.y1.Dispatcher.Invoke(() => { return this.y3.Text; }));
                int speed = Int32.Parse(this.y1.Dispatcher.Invoke(() => { return this.Speed.Text; }));
                int iterationCount = Int32.Parse(this.y1.Dispatcher.Invoke(() =>
                {
                    return this.IterationsCount.Text;
                }));
                double[,] StartPoints = {{x1, y1, 1}, {x2, y2, 1}, {x3, y3, 1}};
                if (iterationCount < 0)
                {
                    throw new Exception("Кількість ітерацій менше нуля");
                }
                //double[,] point1 = {{x1}, {y1}, {1}};
                //double[,] point2 = {{x2}, {y2}, {1}};
                //double[,] point3 = {{x3}, {y3}, {1}};

                for (int i = 0; i < iterationCount; ++i)
                {
                    ///StartPoints = _transformator.Transform(StartPoints, speed);
                    StartPoints = _transformator.TransformV2(StartPoints, speed);
                    PaintRectangle(StartPoints);
                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Неправильно введені дані. Cпробуйте ще раз \n" + e.Message, "Помилка данних",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        private void SetInitialWindow()
        {
            var pm = SetPlot();
            AffineImage.Model = pm;
        }
        private void PaintRectangle(double[,] points)
        {
            if (AffineImage != null)
            {
                var Point1 = new DataPoint(points[0, 0], points[0, 1]);
                var Point2 = new DataPoint(points[1, 0], points[1, 1]);
                var Point3 = new DataPoint(points[2, 0], points[2, 1]);
                var pm = SetPlot();
                var Triangle = new LineSeries();
                Triangle.Points.Add(Point1);
                Triangle.Points.Add(Point2);
                Triangle.Points.Add(Point3);
                Triangle.Points.Add(Point1);
                var Line = new LineSeries();
                Line.Points.Add(new DataPoint(-200,-200));
                Line.Points.Add(new DataPoint(200,200));
                pm.Series.Add(Triangle);
                pm.Series.Add(Line);
                double scale = pm.Axes[0].Scale;
                this.Dispatcher.Invoke(() => {
                    AffineImage.Model = pm;
                });
               
            }
        }

        private void X1_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.y1 != null && this.x1 != null && this.y2 != null && this.x2 != null && this.y3 != null && this.x3 != null)
            {
                try
                {
                    int x1 = Int32.Parse(this.x1.Text);
                    int y1 = Int32.Parse(this.y1.Text);
                    int x2 = Int32.Parse(this.x2.Text);
                    int y2 = Int32.Parse(this.y2.Text);
                    int x3 = Int32.Parse(this.x3.Text);
                    int y3 = Int32.Parse(this.y3.Text);

                    //double[,] points = {{x1, y1,1}, {x2, y2,1}, {x3, y3,1}};
                    double[,] point1 = {{x1}, {y1}, {1}};
                    double[,] point2 = {{x2}, {y2}, {1}};
                    double[,] point3 = {{x3}, {y3}, {1}};

                    double[,] points =
                        {{point1[0, 0], point1[1, 0]}, {point2[0, 0], point2[1, 0]}, {point3[0, 0], point3[1, 0]}};
                    PaintRectangle(points);
                }
                catch (Exception exception)
                {
                    
                }
                
            }
        }

        private void AffineImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            this.Dispatcher.Invoke(() => {
                double scale = AffineImage.Model.Axes[0].Scale;
                ScaleLabel.Content = "Scale: " + scale;
            });
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            AffineImage.ResetAllAxes();
            ScaleLabel.Content = "Масштаб: " + 100 + "%";
            X1_OnTextChanged(this, null);
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image file (*.png) | *.png";
            if (saveFileDialog1.ShowDialog() == false)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            //string filename = "image.png";
            var exporter = new PngExporter() { Width = 384, Height = 384, Background = OxyColors.White };
            exporter.ExportToFile(AffineImage.ActualModel, filename);
        }

        private void UserGuideButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserGuidePage(4));
        }

        private void InstructionsButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Instructions(4));
        }
    }
}
