using CG.Views;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using Studying_app_kg.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LinearAxis = OxyPlot.Axes.LinearAxis;
using LineSeries = OxyPlot.Series.LineSeries;

namespace Studying_app_kg.Views
{
    /// <summary>
    /// Interaction logic for AffineTransformations.xaml
    /// </summary>
    public partial class AffineTransformations : Page
    {
        private Page _basePage;
        private AffineTransformator _transformator;
        private bool _isInitialScaleSet = false;
        private double initialScale = 0;
        private CancellationTokenSource token;
        private bool buttonStopWasClicked = false;

        public AffineTransformations(Page page)
        {
            InitializeComponent();
            SetInitialWindow();
            _basePage = page;
            _transformator = new AffineTransformator();
            TextBox_OnTextChanged(this, null);
            AffineImage.Model.Updated += ModelOnUpdated;
        }

        private void Paint_OnClick(object sender, RoutedEventArgs e)
        {
            buttonStopWasClicked = !buttonStopWasClicked;
            token = new CancellationTokenSource();
            Task.Run(() => PaintFunction(token.Token), token.Token);
        }

        private void Stop_OnClick(object sender, RoutedEventArgs e)
        {
            buttonStopWasClicked = !buttonStopWasClicked;
            token?.Cancel();
        }

        private void PaintFunction(CancellationToken token)
        {
            const int paintTimeout = 50;

            try
            {
                #region start
                int x1 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_1.Text; }));
                int y1 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_1.Text; }));
                int x2 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_2.Text; }));
                int y2 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_2.Text; }));
                int x3 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_3.Text; }));
                int y3 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_3.Text; }));
                int x4 = Int32.Parse(x_1.Dispatcher.Invoke(() => { return x_4.Text; }));
                int y4 = Int32.Parse(y_1.Dispatcher.Invoke(() => { return y_4.Text; }));

                double scale = Double.Parse(y_1.Dispatcher.Invoke(() => { return Scale.Text; }));

                if (scale < 0)
                {
                    MessageBox.Show("Please enter positive integer coefficient. Try again \n", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (x1 == x3 && y1 == y3 || x2 == x4 && y2 == y4 ||
                         x1 == x2 && y1 == y2 || x3 == x4 && y3 == y4 ||
                         x1 == x4 && y1 == y4 || x2 == x3 && y2 == y3 || 
                         x1 == x2 && x2 == x3 ||  y1 == y2 && y2 == y3)
                {
                    MessageBox.Show("You entered coordinates for a line, not a parallelogram.\nTry to change same coordinates\n", "Input Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                #endregion

                string chosenVertex = x_1.Dispatcher.Invoke(() => { return Vertex.Text; });
                Point chosenVertexAsPoint;
                switch (chosenVertex)
                {
                    case "A":
                        chosenVertexAsPoint.X = x1;
                        chosenVertexAsPoint.Y = y1;
                        break;
                    case "B":
                        chosenVertexAsPoint.X = x2;
                        chosenVertexAsPoint.Y = y2;
                        break;
                    case "C":
                        chosenVertexAsPoint.X = x3;
                        chosenVertexAsPoint.Y = y3;
                        break;
                    case "D":
                        chosenVertexAsPoint.X = x4;
                        chosenVertexAsPoint.Y = y4;
                        break;
                }

                double[,] points = { { x1, y1, 1 }, { x2, y2, 1 }, { x3, y3, 1 }, { x4, y4, 1 } };

                while (!buttonStopWasClicked)
                {
                    points = _transformator.Transform(points, scale, chosenVertexAsPoint);
                    PaintParallelogram(points);
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(paintTimeout);
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Uncorrect input data. Only numbers allowed \n", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uncorrect input data. Try again \n" + ex.Message, "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ModelOnUpdated(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                double scale = AffineImage.Model.Axes[0].Scale;
                if (Math.Abs(scale - 0) < 0.0001)
                {
                    ScaleLabel.Content = "Scope: " + 100 + "%";
                }
                else if (!_isInitialScaleSet)
                {
                    initialScale = scale;
                    ScaleLabel.Content = "Scope: " + 100 + "%";
                    _isInitialScaleSet = true;
                }
                else
                {
                    ScaleLabel.Content = "Scope: " + Math.Abs(Math.Round(scale / initialScale * 100, 2)) + "%";
                }
            });
        }

        private void AffineTransformationGuide_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AffineTransformationsGuide());
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image file (*.png) | *.png";
            if (saveFileDialog.ShowDialog() == false)
            {
                return;
            }
            string fileName = saveFileDialog.FileName;
            var exporter = new PngExporter() { Width = 400, Height = 400, Background = OxyColors.White };
            exporter.ExportToFile(AffineImage.ActualModel, fileName);
        }

        private void SetInitialWindow()
        {
            var plotModel = SetPlot();
            AffineImage.Model = plotModel;
        }

        private PlotModel SetPlot()
        {
            const int maxScope = 10;
            var pm = new PlotModel();
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = -maxScope,
                Maximum = maxScope,
                PositionAtZeroCrossing = true,
                ExtraGridlines = new[] { 0.0 },
            });
            pm.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = -maxScope,
                Maximum = maxScope,
                PositionAtZeroCrossing = true,
                ExtraGridlines = new[] { 0.0 },
            });

            pm.Updated += ModelOnUpdated;
            pm.PlotType = PlotType.Cartesian;
            return pm;
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (x_1 != null && y_1 != null &&
                x_2 != null && y_2 != null &&
                x_3 != null && y_3 != null &&
                x_4 != null && y_4 != null)
            {
                try
                {
                    int x1 = Int32.Parse(x_1.Text);
                    int y1 = Int32.Parse(y_1.Text);
                    int x2 = Int32.Parse(x_2.Text);
                    int y2 = Int32.Parse(y_2.Text);
                    int x3 = Int32.Parse(x_3.Text);
                    int y3 = Int32.Parse(y_3.Text);

                    double[,] vertex_A = { { x1 }, { y1 }, { 1 } };
                    double[,] vertex_B = { { x2 }, { y2 }, { 1 } };
                    double[,] vertex_C = { { x3 }, { y3 }, { 1 } };

                    double[,] points =
                    {
                              { vertex_A[0, 0], vertex_A[1, 0] },
                              { vertex_B[0, 0], vertex_B[1, 0] },
                              { vertex_C[0, 0], vertex_C[1, 0] },
                    };

                    double[,] vertex_D = CalculateFourthParallelogramVertex(points);


                    double[,] newPoints =
                    {
                              { vertex_A[0, 0], vertex_A[1, 0] },
                              { vertex_B[0, 0], vertex_B[1, 0] },
                              { vertex_C[0, 0], vertex_C[1, 0] },
                              { vertex_D[0, 0], vertex_D[1, 0] }
                    };

                    x_4.Text = vertex_D[0, 0].ToString();
                    y_4.Text = vertex_D[1, 0].ToString();

                    PaintParallelogram(newPoints);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        private double[,] CalculateFourthParallelogramVertex(double [,] vertexes)
        {
            double center_M_x = (vertexes[0, 0] + vertexes[2, 0]) / 2d;
            double center_M_y = (vertexes[0, 1] + vertexes[2, 1]) / 2d;

            double vertex_D_x = 2d * center_M_x - vertexes[1, 0];
            double vertex_D_y = 2d * center_M_y - vertexes[1, 1];

            double[,] vertex_D = { { vertex_D_x }, { vertex_D_y }, { 1 } };

            return vertex_D;
        }

        private void PaintParallelogram(double[,] points)
        {
            if (AffineImage != null)
            {
                DataPoint A = new DataPoint(points[0, 0], points[0, 1]);
                DataPoint B = new DataPoint(points[1, 0], points[1, 1]);
                DataPoint C = new DataPoint(points[2, 0], points[2, 1]);
                DataPoint D = new DataPoint(points[3, 0], points[3, 1]);

                LineSeries paralellogram = new LineSeries();
                paralellogram.Points.Add(A);
                paralellogram.Points.Add(B);
                paralellogram.Points.Add(C);
                paralellogram.Points.Add(D);
                paralellogram.Points.Add(A);

                PlotModel plotModel = SetPlot();
                plotModel.Series.Add(paralellogram);

                double scale = plotModel.Axes[0].Scale;

                this.Dispatcher.Invoke(() =>
                {
                    AffineImage.Model = plotModel;
                });
            }
        }

        private void Reset_OnClick(object sender, RoutedEventArgs e)
        {
            AffineImage.ResetAllAxes();
            ScaleLabel.Content = "Scope: " + 100 + "%";
            TextBox_OnTextChanged(this, null);
        }

        private void AffineImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                double scale = AffineImage.Model.Axes[0].Scale;
                ScaleLabel.Content = "Scale: " + scale;
            });
        }
    }
}
