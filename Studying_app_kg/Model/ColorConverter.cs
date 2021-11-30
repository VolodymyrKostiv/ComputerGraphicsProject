using System;
using System.Drawing;

namespace Studying_app_kg.Model
{
    static class MyColorConverter
    {
        public static double[] RgbToHsv(byte r, byte g, byte b, double coef = 1)
        {
            double hue, saturation, value;

            // Convert RGB as 0.0 to 1.0 range
            double red = r / 255.0;
            double green = g / 255.0;
            double blue = b / 255.0;

            // Find max and min from R, G and B and diff between them
            double max = Math.Max(red, Math.Max(green, blue));
            double min = Math.Min(red, Math.Min(green, blue));
            double difference = max - min;
             
            // To compare double values 
            double precision = 0.0000001;

            // Setting Hue
            if (Math.Abs(difference) < precision)
            {
                hue = 0;
            }
            else
            {
                hue = 60;
                if (Math.Abs(max - red) < precision && green >= blue)
                {
                    hue *= (green - blue) / difference;
                }
                else if (Math.Abs(max - red) < precision && green < blue)
                {
                    hue *= (green - blue) / difference;
                    hue += 360;
                }
                else if (Math.Abs(max - green) < precision)
                {
                    hue *= (blue - red) / difference;
                    hue += 120;
                }
                else if (Math.Abs(max - blue) < precision)
                {
                    hue *= (red - green) / difference;
                    hue += 240;
                }
            }

            if (max < precision)
            {
                saturation = 0.0;
            }
            else
            {
                saturation = 1.0 - min / max;
            }

            value = max;

            const int hueMinValue = 90;
            const int hueMaxValue = 150;

            if (hue >= hueMinValue - 1 && hue <= hueMaxValue + 1)
            {
                saturation *= coef;
                if (saturation > 1)
                {
                    saturation = 1;
                }
            }

            return new[] { hue, saturation, value };
        }

        // Convert an HSV value into an RGB value.
        public static byte[] HsvToRgb(double h, double S, double V)
        {
            double H = h;

            while (H < 0) 
            { 
                H += 360; 
            }
            while (H >= 360) 
            { 
                H -= 360; 
            }

            double R, G, B;

            if (V <= 0)
            {
                R = G = B = 0; 
            }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double p = V * (1 - S);
                double q = V * (1 - f * S);
                double t = V * (1 - (1 - f) * S);
                switch (i)
                {

                    // Red is the dominant color
                    case 0:
                        R = V;
                        G = t;
                        B = p;
                        break;

                    // Green is the dominant color
                    case 1:
                        R = q;
                        G = V;
                        B = p;
                        break;
                    case 2:
                        R = p;
                        G = V;
                        B = t;
                        break;

                    // Blue is the dominant color
                    case 3:
                        R = p;
                        G = q;
                        B = V;
                        break;
                    case 4:
                        R = t;
                        G = p;
                        B = V;
                        break;

                    // Red is the dominant color
                    case 5:
                        R = V;
                        G = p;
                        B = q;
                        break;

                    // Just in case we overshoot
                    case 6:
                        R = V;
                        G = t;
                        B = p;
                        break;
                    case -1:
                        R = V;
                        G = p;
                        B = q;
                        break;

                    // The color is not defined
                    default:
                        R = G = B = V; 
                        break;
                }
            }

            byte r = (byte)Clamp((int)(R * 255.0));
            byte g = (byte)Clamp((int)(G * 255.0));
            byte b = (byte)Clamp((int)(B * 255.0));

            return new[] { r, g, b };
        }

        static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
    }
}