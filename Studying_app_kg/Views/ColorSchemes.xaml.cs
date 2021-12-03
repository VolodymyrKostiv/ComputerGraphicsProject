using CG.Views;
using Microsoft.Win32;
using Studying_app_kg.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;

namespace Studying_app_kg.Views
{
    public partial class ColorSchemes : Page
    {
        private Page _basePage;
        private BitmapImage _basicImage;
        private BitmapImage _RGBimage;
        private BitmapImage _HSVimage;

        private int _step = 0;
        private int _size = 0;

        byte[] pixels;
        private byte[] _RGBpixels;
        private byte[] _HSVpixels;

        public ColorSchemes(Page page)
        {
            InitializeComponent();
            _basePage = page;

            SetDefaultLabelValues();
        }

        private void SetDefaultLabelValues()
        {
            if (RGB_Red != null)
            {
                RGB_Red.Content = $"-";
            }
            if (RGB_Green != null)
            {
                RGB_Green.Content = $"-";
            }
            if (RGB_Blue != null)
            {
                RGB_Blue.Content = $"-";
            }

            if (HSV_Hue != null)
            {
                HSV_Hue.Content = "-";
            }
            if (HSV_Saturation != null)
            {
                HSV_Saturation.Content = "-";
            }
            if (HSV_Value != null)
            {
                HSV_Value.Content = "-";
            }
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void ColorsSchemeGuide_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ColorSchemesGuide());
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
                _basicImage = new BitmapImage(new Uri(op.FileName));
            }
            else
            {
                return;
            }

            OriginLabel.Visibility = Visibility.Visible;
            ConvertedLabel.Visibility = Visibility.Visible;
            BasicImage.Source = _basicImage;
            ChangedImage.MouseMove += ChangedImage_OnMouseMove;
            _step = _basicImage.PixelWidth * 4;
            _size = _basicImage.PixelHeight * _step;
            pixels = new byte[_size];
            _basicImage.CopyPixels(pixels, _step, 0);

            if (HSV_RadioButton.IsChecked == true)
            {
                ConvertAsRgb();
                ConvertFromRgbToHsv();
            }
            else
            {
                ConvertFromRgbToHsv();
                ConvertAsRgb();
            }

            Saturation_Slider.Value = 50;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            if (_basicImage != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "_basicImage file (*.png) | *.png";
                if (saveFileDialog.ShowDialog() == false)
                {
                    return;
                }
                string filename = saveFileDialog.FileName;
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    if (RGB_RadioButton.IsChecked == true && _RGBimage != null)
                    {
                        encoder.Frames.Add(BitmapFrame.Create(_RGBimage));
                    }
                    else if (HSV_RadioButton.IsChecked == true && _HSVimage != null)
                    {
                        encoder.Frames.Add(BitmapFrame.Create(_HSVimage));
                    }
                    encoder.Save(stream);
                }
            }
            else
            {
                MessageBox.Show("There is no file to save", "Saving error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BasicImage_OnMouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(((IInputElement)e.Source));

            if (_basicImage != null)
            {
                int[] position = CountImagePositionBasicImage(e);
                int x = position[0];
                int y = position[1];
                int index = y * _step + 4 * x;
                if (index != 0 && index % 4 != 0)
                {
                    index += 4 - index % 4;
                }
                if (index + 4 < pixels.Length)
                {
                    RGB_Red.Content = $"{pixels[index + 2]}";
                    RGB_Green.Content = $"{pixels[index + 1]}";
                    RGB_Blue.Content = $"{pixels[index]}";

                    Color color = Color.FromArgb(_HSVpixels[index + 2], _HSVpixels[index + 1], _HSVpixels[index]);
                    double[] hsv = MyColorConverter.RgbToHsv(_HSVpixels[index + 2], _HSVpixels[index + 1], _HSVpixels[index]);

                    HSV_Hue.Content = $"{Math.Round(hsv[0], 0)}";
                    HSV_Saturation.Content = $"{Math.Round(hsv[1], 2)}";
                    HSV_Value.Content = $"{Math.Round(hsv[2], 2)}";
                }
            }
        }

        private void ChangedImage_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_basicImage != null)
            {
                int[] position = CountImagePositionChangedImage(e);
                int x = position[0];
                int y = position[1];
                int index = y * _step + 4 * x;
                if (index != 0 && index % 4 != 0)
                {
                    index += 4 - index % 4;
                }

                if (index + 4 < _size)
                {
                    Color color = Color.FromArgb(_HSVpixels[index + 2], _HSVpixels[index + 1], _HSVpixels[index]);
                    double[] hsv = MyColorConverter.RgbToHsv(_HSVpixels[index + 2], _HSVpixels[index + 1], _HSVpixels[index]);

                    HSV_Hue.Content = $"{Math.Round(hsv[0], 0)}";
                    HSV_Saturation.Content = $"{Math.Round(hsv[1], 2)}";
                    HSV_Value.Content = $"{Math.Round(hsv[2], 2)}";

                    RGB_Red.Content = $"{_HSVpixels[index + 2]}";
                    RGB_Green.Content = $"{_HSVpixels[index + 1]}";
                    RGB_Blue.Content = $"{_HSVpixels[index]}";

                }
            }
        }

        public int[] CountImagePositionChangedImage(MouseEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(ChangedImage);
            double pixelWidth = _basicImage.PixelWidth;
            double pixelHeight = _basicImage.PixelHeight;
            double x = pixelWidth * p.X / ChangedImage.ActualWidth;
            double y = pixelHeight * p.Y / ChangedImage.ActualHeight;
            return new[] { (int)x, (int)y };
        }

        public int[] CountImagePositionBasicImage(MouseEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(BasicImage);
            double pixelWidth = _basicImage.PixelWidth;
            double pixelHeight = _basicImage.PixelHeight;
            double x = pixelWidth * p.X / BasicImage.ActualWidth;
            double y = pixelHeight * p.Y / BasicImage.ActualHeight;
            return new[] { (int)x, (int)y };
        }

        private void ConvertAsRgb()
        {
            _RGBpixels = new byte[_size];

            _basicImage.CopyPixels(_RGBpixels, _step, 0);
            WriteableBitmap newImage = new WriteableBitmap(_basicImage.PixelWidth, _basicImage.PixelHeight, _basicImage.DpiX, _basicImage.DpiY, _basicImage.Format, _basicImage.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, _basicImage.PixelWidth, _basicImage.PixelHeight), _RGBpixels, _step, 0);
            _RGBimage = newImage.ToBitmapImage();
            ChangedImage.Source = _RGBimage;
        }

        private void ConvertFromRgbToHsv()
        {
            _HSVpixels = new byte[_size];

            _basicImage.CopyPixels(_HSVpixels, _step, 0);
            for (int i = 0; i < _size; i += 4)
            {
                if (i + 4 < pixels.Length)
                {
                    Color color = Color.FromArgb(_HSVpixels[i + 2], _HSVpixels[i + 1], _HSVpixels[i]);
                    double[] hsv = MyColorConverter.RgbToHsv(_HSVpixels[i + 2], _HSVpixels[i + 1], _HSVpixels[i]);
                    byte[] RGB = MyColorConverter.HsvToRgb(hsv[0], hsv[1], hsv[2]);
                    _HSVpixels[i] = RGB[2];
                    _HSVpixels[i + 1] = RGB[1];
                    _HSVpixels[i + 2] = RGB[0];
                }
            }
            WriteableBitmap newImage = new WriteableBitmap(_basicImage.PixelWidth, _basicImage.PixelHeight, _basicImage.DpiX, _basicImage.DpiY, _basicImage.Format, _basicImage.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, _basicImage.PixelWidth, _basicImage.PixelHeight), _HSVpixels, _step, 0);
            _HSVimage = newImage.ToBitmapImage();
            ChangedImage.Source = _HSVimage;
        }

        private void BasicImage_OnMouseLeave(object sender, MouseEventArgs e)
        {
            RGB_Red.Content = "-";
            RGB_Green.Content = "-";
            RGB_Blue.Content = "-";

            HSV_Hue.Content = "-";
            HSV_Saturation.Content = "-";
            HSV_Value.Content = "-";
        }

        private void ChangedImage_OnMouseLeave(object sender, MouseEventArgs e)
        {
            BasicImage_OnMouseLeave(this, e);
        }

        private void RGB_OnChecked(object sender, RoutedEventArgs e)
        {
            if (_RGBimage != null && ChangedImage != null)
            {
                ChangedImage.Source = _RGBimage;
            }

            if (RGB_Red != null)
            {
                RGB_Red.Content = $"-";
            }
            if (RGB_Green != null)
            {
                RGB_Green.Content = $"-";
            }
            if (RGB_Blue != null)
            {
                RGB_Blue.Content = $"-";
            }
        }

        private void HSV_OnChecked(object sender, RoutedEventArgs e)
        {
            if (_HSVimage != null && ChangedImage != null)
                ChangedImage.Source = _HSVimage;

            if (HSV_Hue != null)
            {
                HSV_Hue.Content = "-";
            }
            if (HSV_Saturation != null)
            {
                HSV_Saturation.Content = "-";
            }
            if (HSV_Value != null)
            {
                HSV_Value.Content = "-";
            }
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Saturation_Persents != null && e != null && _basicImage != null)
            {
                Saturation_Persents.Content = Math.Round(e.NewValue, 0) + " %";
            }
            if (_basicImage != null)
            {
                ChangeSaturation(e.NewValue / 50d);
            }
        }

        private void ChangeSaturation(double coef)
        {
            byte[] pixelsLData = new byte[_size];
            if (HSV_RadioButton.IsChecked == true && _HSVimage != null)
            {
                _basicImage.CopyPixels(pixelsLData, _step, 0);
            }
            else if (RGB_RadioButton.IsChecked == true && _RGBimage != null)
            {
                _basicImage.CopyPixels(pixelsLData, _step, 0);
            }
            else
            {
                return;
            }

            for (int i = 0; i < _size; i += 4)
            {
                if (i + 4 < pixels.Length)
                {
                    double[] HSV;
                    byte[] RGB;
                    Color color = Color.FromArgb(pixelsLData[i + 2], pixelsLData[i + 1], pixelsLData[i]);

                    HSV = MyColorConverter.RgbToHsv(pixelsLData[i + 2], pixelsLData[i + 1], pixelsLData[i], coef);
                    RGB = MyColorConverter.HsvToRgb(HSV[0], HSV[1], HSV[2]);

                    pixelsLData[i] = RGB[2];
                    pixelsLData[i + 1] = RGB[1];
                    pixelsLData[i + 2] = RGB[0];
                }
            }
            WriteableBitmap newImage = new WriteableBitmap(_basicImage.PixelWidth, _basicImage.PixelHeight,
                                                            _basicImage.DpiX, _basicImage.DpiY,
                                                            _basicImage.Format, _basicImage.Palette);
            newImage.WritePixels(new Int32Rect(0, 0, _basicImage.PixelWidth, _basicImage.PixelHeight), pixelsLData, _step, 0);

            if (HSV_RadioButton.IsChecked == true)
            {
                _HSVimage = newImage.ToBitmapImage();
                ChangedImage.Source = _HSVimage;
                _HSVpixels = pixelsLData;
            }
            else if (RGB_RadioButton.IsChecked == true)
            {
                _RGBimage = newImage.ToBitmapImage();
                ChangedImage.Source = _RGBimage;
                _RGBpixels = pixelsLData;
            }
        }
    }

    public static class ImageHelper
    {
        public static BitmapImage ToBitmapImage(this WriteableBitmap wbm)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }
    }
}
