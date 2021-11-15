using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Studying_app_kg.Model;
using Color = System.Drawing.Color;

namespace Studying_app_kg.Views
{
    /// <summary>
    /// Interaction logic for ColorSchemes.xaml
    /// </summary>
    public partial class ColorSchemes : Page
    {
        private BitmapImage Image;
        private BitmapImage HSLImage;
        private BitmapImage CMYKImage;

        private int stride = 0;
        private int size = 0;
        
        byte[] pixels;
        private byte[] pixelsHSL;
        private byte[] pixelsData;

        public ColorSchemes()
        {
            InitializeComponent();
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a Picture",
                Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF;*.jpeg",
                Multiselect = false
            };
            if (op.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(op.FileName));
            }
            else
            {
                return;
            }

            StockImage.Source = Image;
            ChangedImage.MouseMove += ChangedImage_OnMouseMove;
            stride = Image.PixelWidth * 4;
            size = Image.PixelHeight * stride;
            pixels = new byte[size];
            Image.CopyPixels(pixels, stride, 0);
            if (HSL.IsChecked == true)
            {
                ConvertFromRgbToCmyk();
                ConvertToRgbFromHsl();
            }
            else
            {
                ConvertToRgbFromHsl();
                ConvertFromRgbToCmyk();
            }

            Slider.Value = 50;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (Image != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Image file (*.png) | *.png";
                if (saveFileDialog1.ShowDialog() == false)
                    return;

                string filename = saveFileDialog1.FileName;
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    if (HSL.IsChecked == true && HSLImage != null)
                        encoder.Frames.Add(BitmapFrame.Create(HSLImage));
                    else if (CMYKImage != null && CMYK.IsChecked == true)
                    {
                        encoder.Frames.Add(BitmapFrame.Create(CMYKImage));
                    }

                    encoder.Save(stream);
                }
            }
            else
            {
                MessageBox.Show( "No file to save", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StockImage_OnMouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(((IInputElement)e.Source));

            if (Image != null)
            {
                int[] postion = CountImagePositionStockImage(e);
                int x = postion[0];
                int y = postion[1];
                int index = y * stride + 4 * x;
                if (index != 0 && index % 4 != 0)
                {
                    index += 4 - index % 4;
                }
                if (index + 4 < pixels.Length)
                    RGBLabel.Content = $"R: {pixels[index+2]},G: {pixels[index + 1]}, B: {pixels[index]}";
            }
        }

        
        private void ChangedImage_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Image != null)
            {
                int[] position = CountImagePositionChangedImage(e);
                int x = position[0];
                int y = position[1];
                int index = y * stride + 4 * x;
                if(index!= 0 && index % 4 != 0)
                {
                    index += 4 - index % 4;
                }
                
                if (index + 4 < size)
                {
                    if (HSL.IsChecked == true)
                    {
                        Color color = Color.FromArgb(pixelsHSL[index+2], pixelsHSL[index + 1], pixelsHSL[index]);
                        double[] hsl = ColorConverter.RgbToHsl(pixelsHSL[index + 2], pixelsHSL[index + 1], pixelsHSL[index]);
                        ColorScheme2Label.Content = $"H: {Math.Round(hsl[0], 0)} S: {Math.Round(hsl[1], 2)} L: {Math.Round(hsl[2], 2)}";
                        RGBLabel.Content = $"R: {pixelsHSL[index + 2]}  G: {pixelsHSL[index + 1]}  B: {pixelsHSL[index]}";
                    }
                    else
                    {
                        Color color = Color.FromArgb(pixelsData[index + 2], pixelsData[index + 1], pixelsData[index]);
                        double[] cmyk = ColorConverter.RgbToCmyk(color);
                        ColorScheme2Label.Content = $"C: {Math.Round(cmyk[0]*100, 2)}% M: {Math.Round(cmyk[1]*100, 2)}% Y: {Math.Round(cmyk[2]*100, 2)}% K:{Math.Round(cmyk[3]*100, 2)}%";
                        RGBLabel.Content = $"R: {pixelsData[index+2]}  G: {pixelsData[index + 1]}  B: {pixelsData[index]}";
                    }
                }
            }
        }

        public int[] CountImagePositionChangedImage(MouseEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(ChangedImage);
            double pixelWidth = Image.PixelWidth;
            double pixelHeight = Image.PixelHeight;
            double x = pixelWidth * p.X / ChangedImage.ActualWidth;
            double y = pixelHeight * p.Y / ChangedImage.ActualHeight;
            return new[] {(int) x, (int) y};
        }

        public int[] CountImagePositionStockImage(MouseEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(StockImage);
            double pixelWidth = Image.PixelWidth;
            double pixelHeight = Image.PixelHeight;
            double x = pixelWidth * p.X / StockImage.ActualWidth;
            double y = pixelHeight * p.Y / StockImage.ActualHeight;
            return new[] { (int)x, (int)y };
        }

        private void ConvertFromRgbToCmyk()
        {
            pixelsData = new byte[size];

            Image.CopyPixels(pixelsData, stride, 0);
            for (int i = 0; i < size; i += 4)
            {
                if (i + 4 < pixels.Length)
                {
                    Color color = Color.FromArgb(pixelsData[i+2], pixelsData[i + 1], pixelsData[i]);
                    double[] cmyk = ColorConverter.RgbToCmyk(color);
                    byte[] RGB = ColorConverter.CmykToRgb(cmyk[0], cmyk[1], cmyk[2], cmyk[3]);
                    pixelsData[i] = RGB[2];
                    pixelsData[i + 1] = RGB[1];
                    pixelsData[i + 2] = RGB[0];
                }
            }
            WriteableBitmap newImage = new WriteableBitmap(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, Image.Format, Image.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, Image.PixelWidth, Image.PixelHeight), pixelsData, stride, 0);
            CMYKImage = newImage.ToBitmapImage();
            ChangedImage.Source = CMYKImage;
        }

        private void ConvertToRgbFromHsl()
        {
            pixelsHSL = new byte[size];

            Image.CopyPixels(pixelsHSL, stride, 0);
            for (int i = 0; i < size; i += 4)
            {
                if (i + 4 < pixels.Length)
                {
                    Color color = Color.FromArgb(pixelsHSL[i + 2], pixelsHSL[i + 1], pixelsHSL[i]);
                    double[] hsv = ColorConverter.RgbToHsl(pixelsHSL[i + 2], pixelsHSL[i + 1], pixelsHSL[i]);
                    byte[] RGB = ColorConverter.HslToRgb(hsv[0], hsv[1], hsv[2]);
                    pixelsHSL[i] = RGB[2];
                    pixelsHSL[i + 1] = RGB[1];
                    pixelsHSL[i + 2] = RGB[0];
                }
            }
            WriteableBitmap newImage = new WriteableBitmap(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, Image.Format, Image.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, Image.PixelWidth, Image.PixelHeight), pixelsHSL, stride, 0);
            HSLImage = newImage.ToBitmapImage();
            ChangedImage.Source = HSLImage;
        }

        private void StockImage_OnMouseLeave(object sender, MouseEventArgs e)
        {
            RGBLabel.Content = "R:   G:   B:";
        }
        private void ChangedImage_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ColorScheme2Label.Content = HSL.IsChecked == true ? "H:   S:   L:" : "C:   M:   Y:   K:";
            StockImage_OnMouseLeave(this, e);
        }
        private void HSL_OnChecked(object sender, RoutedEventArgs e)
        {
            if(HSLImage != null && ChangedImage != null)
                ChangedImage.Source = HSLImage;
            if(ColorScheme2Label != null)
                ColorScheme2Label.Content = "H:   S:   L:";
        }

        private void CMYK_OnChecked(object sender, RoutedEventArgs e)
        {
            if (CMYKImage != null && ChangedImage != null)
                ChangedImage.Source = CMYKImage;
            if (ColorScheme2Label != null)
                ColorScheme2Label.Content = "C:   M:   Y:   K:";
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Persents != null && e != null && Image != null)
                Persents.Content = Math.Round(e.NewValue, 0) + "%";
            if(Image != null)
            {
                ChangeLightness(e.NewValue/50d);
            }
        }

        private void ChangeLightness(double coef)
        {
            byte[] pixelsLData = new byte[size];
            if (HSL.IsChecked == true && HSLImage != null)
            {
                Image.CopyPixels(pixelsLData, stride, 0);
            }
            else if(CMYKImage != null)
            {
                Image.CopyPixels(pixelsLData, stride, 0);
            }
            else
            {
                return;
            }

            for (int i = 0; i < size; i += 4)
            {
                if (i + 4 < pixels.Length)
                {
                    double[] hsl;
                    byte[] RGB;
                    Color color = Color.FromArgb(pixelsLData[i + 2], pixelsLData[i + 1], pixelsLData[i]);
                    if (HSL.IsChecked == true)
                    {
                        hsl = ColorConverter.RgbToHsl(pixelsLData[i + 2], pixelsLData[i + 1], pixelsLData[i], coef);
                        RGB = ColorConverter.HslToRgb(hsl[0], hsl[1], hsl[2]);
                    }
                    else 
                    {
                        hsl = ColorConverter.RgbToCmyk(color,coef);
                        RGB = ColorConverter.CmykToRgb(hsl[0], hsl[1], hsl[2], hsl[3]);
                    }
                    pixelsLData[i] = RGB[2];
                    pixelsLData[i + 1] = RGB[1];
                    pixelsLData[i + 2] = RGB[0];
                }
            }
            WriteableBitmap newImage = new WriteableBitmap(Image.PixelWidth, Image.PixelHeight, Image.DpiX, Image.DpiY, Image.Format, Image.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, Image.PixelWidth, Image.PixelHeight), pixelsLData, stride, 0);
            if(HSL.IsChecked == true) {
                HSLImage = newImage.ToBitmapImage();
                ChangedImage.Source = HSLImage;
                pixelsHSL = pixelsLData;
            }else
            {
                CMYKImage = newImage.ToBitmapImage();
                ChangedImage.Source = CMYKImage;
                pixelsData = pixelsLData;
            }
            
        }
        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void UserGuideButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserGuidePage(3));
        }

        private void InstructionsButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Instructions(3));
        }
    }

    public static class ImageHelpers
    {
        public static BitmapImage ToBitmapImage(this WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }
    }
}
