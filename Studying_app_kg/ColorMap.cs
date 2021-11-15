using System;
using System.Collections;
using System.Drawing;

namespace Studying_app_kg.Model
{
    class ColorMap
    {
        public static Color[] GetColors(int color_map_index)
        {
            try
            {
                string[] ColMaps = System.IO.Directory.GetFiles(@"D:\University\CG\Studying_app_kg\Studying_app_kg\Studying_app_kg\colormap", "*.ColorMap");
                Color[] c = new Color[256];
                System.IO.StreamReader sr = new System.IO.StreamReader(ColMaps[color_map_index]);
                ArrayList lines = new ArrayList();
                string s = sr.ReadLine();
                while (s != null)
                {
                    lines.Add(s);
                    s = sr.ReadLine();
                }
                int i = 0;
                for (i = 0; i < Math.Min(256, lines.Count); i++)
                {
                    string curC = (string)lines[i];
                    Color temp = Color.FromArgb(int.Parse(curC.Split(' ')[0]),
                                                int.Parse(curC.Split(' ')[1]),
                                                int.Parse(curC.Split(' ')[2]));
                    c[i] = temp;
                }
                for (int j = i; j < 256; j++)
                {
                    c[j] = Color.White;
                }
                return c;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid ColorMap file.", ex);
            }
        }
    }
}
