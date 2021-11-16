using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.Win32;
using Studying_app_kg.Model;
using CG.Views;

namespace Studying_app_kg.Views
{
    public partial class FractalsPage : Page
    {
        private KochSnowflake fractal;
        private Page basePage;
        public FractalsPage(Page page)
        {
            InitializeComponent();
            fractal = new KochSnowflake(fractalCanvas);
            basePage = page;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int iterations = Convert.ToInt32(NumberOfIterations.Text);
                int firstRel = Convert.ToInt32(FirstPartOfRelation.Text);
                int secondRel = Convert.ToInt32(SecondPartOfRelation.Text);
                string figure = BasicFigure.Text;
                int figureAsInt = figure == "Triangle" ? 0 : 1;
                fractalCanvas.Children.Clear();
                fractal.RunGeometricKochSnowflake(iterations, firstRel, secondRel, figureAsInt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Invalid data\n "+ex.Message, "Study part", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void FractalsGuide_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new FractalGuide());
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.OverwritePrompt = true;

            saveFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Rect rect = new Rect(fractalCanvas.Margin.Left, fractalCanvas.Margin.Top, fractalCanvas.ActualWidth, fractalCanvas.ActualHeight);
                    double dpi = 96d;

                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right, (int)rect.Bottom, dpi, dpi, System.Windows.Media.PixelFormats.Default);
                    rtb.Render(fractalCanvas);
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(BitmapFrame.Create(rtb)));
                    encoder.Save(stream);
                }
            }
        }
        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ComboBoxColor_Selected(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void BasicFigure_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NumberOfIteraions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FirstPartOfRelation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SecondPartOfRelation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
