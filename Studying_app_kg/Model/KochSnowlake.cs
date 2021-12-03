using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Studying_app_kg.Model
{
    public class KochSnowflake
    {
        public List<Segment> Segments;
        public Canvas FractalCanvas;

        public KochSnowflake(Canvas fractalCanvas)
        {
            FractalCanvas = fractalCanvas;
        }

        public void RunGeometricKochSnowflake(int numberOfIterations, int firstRelNum, int secondRelNum, int figure)
        {
            InitializeKochSnowflake(numberOfIterations, figure, firstRelNum - secondRelNum);
            for (int i = 0; i < numberOfIterations; ++i)
            {
                List<Segment> nextGenerationSegments = new List<Segment>();

                foreach (Segment segment in Segments)
                {
                    Segment[] childSegments = segment.Generate(firstRelNum, secondRelNum, figure);
                    nextGenerationSegments.AddRange(childSegments);
                }

                Segments = nextGenerationSegments;
            }

            foreach (Segment segment in Segments)
                Show(segment.a, segment.b);

        }

        private void InitializeKochSnowflake(int numberOfIterations, int figure, int scope)
        {
            FractalCanvas.Children.Clear();
            Segments = new List<Segment>();

            if (figure == 0)
            {
                int paddingFromBottom = scope < 0 ? 100 + (-scope * 25) : 20;
                int paddingFromTop = scope < 0 ? 200 + (-scope * 25) : 200;
                const double tgsNumerator = 1.73205080757;
                const double tgsDenumerator = 3.0;

                double halfOfSide = (FractalCanvas.Height - (paddingFromTop + paddingFromBottom)) * tgsNumerator / tgsDenumerator;

                Point a = new Point(FractalCanvas.Width / 2 - halfOfSide, paddingFromTop);
                Point b = new Point(FractalCanvas.Width / 2 + halfOfSide, paddingFromTop);
                Point c = new Point(FractalCanvas.Width / 2, FractalCanvas.Height - paddingFromBottom);


                Segment s1 = new Segment(a, b);
                Segment s2 = new Segment(b, c);
                Segment s3 = new Segment(c, a);
                Segments.Add(s1);
                Segments.Add(s2);
                Segments.Add(s3);
            }
            else if (figure == 1)
            {
                int padding = 0;
                if (scope <= 3 && scope > 0)
                    padding = 200;
                else if (scope >= 3)
                    padding = 200;
                else if (scope < 0 && scope > -2)
                    padding = 250;
                else if (scope <= -2)
                    padding = 300;
                else if (scope == 0)
                    padding = 200;

                double side = FractalCanvas.Height - (padding * 2);

                Point a = new Point(FractalCanvas.Width / 2 - side / 2, padding);
                Point b = new Point(FractalCanvas.Width / 2 + side / 2, padding);
                Point c = new Point(FractalCanvas.Width / 2 + side / 2, FractalCanvas.Height - padding);
                Point d = new Point(FractalCanvas.Width / 2 - side / 2, FractalCanvas.Height - padding);


                Segment s1 = new Segment(a, b);
                Segment s2 = new Segment(b, c);
                Segment s3 = new Segment(c, d);
                Segment s4 = new Segment(d, a);

                Segments.Add(s1);
                Segments.Add(s2);
                Segments.Add(s3);
                Segments.Add(s4);
            }
        }

        public void Show(Point a, Point b)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;

            line.X1 = a.X;
            line.Y1 = a.Y;

            line.X2 = b.X;
            line.Y2 = b.Y;

            line.StrokeThickness = 2;
            FractalCanvas.Children.Add(line);
        }
    }

    public class Segment
    {
        public Point a;
        public Point b;

        public Segment(Point _a, Point _b)
        {
            a = _a;
            b = _b;
        }

        public Segment[] Generate(int first, int second, int figure)
        {
            if (figure == 0)
            {
                Segment[] children = new Segment[4];

                Vector v = Point.Subtract(b, a);
                v = Vector.Divide(v, 2 * first + second);

                // Segment 1
                Point b1 = Point.Add(a, first * v);
                children[0] = new Segment(a, b1);

                // Segment 4
                Point a1 = Point.Subtract(b, first * v);
                children[3] = new Segment(a1, b);

                v = RotateRadians(v, -Math.PI / 3);
                Point c = Point.Add(b1, second * v);

                // Segment 2
                children[1] = new Segment(b1, c);

                // Segment 3
                children[2] = new Segment(c, a1);

                return children;
            }
            else
            {
                Segment[] children = new Segment[5];

                Vector v = Point.Subtract(b, a);
                v = Vector.Divide(v, 2 * first + second);

                // Segment 1
                Point b1 = Point.Add(a, first * v);
                children[0] = new Segment(a, b1);

                // Segment 5
                Point a1 = Point.Subtract(b, first * v);
                children[4] = new Segment(a1, b);

                v = RotateRadians(v, -Math.PI / 2);

                Point c1 = Point.Add(b1, second * v);
                Point c2 = Point.Add(a1, second * v);

                // Segment 2
                children[1] = new Segment(b1, c1);

                // Segment 4
                children[3] = new Segment(c2, a1);

                // Segment 3
                children[2] = new Segment(c1, c2);

                return children;
            }
        }

        public Vector RotateRadians(Vector v, double radians)
        {
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            return new Vector(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }
    }
}