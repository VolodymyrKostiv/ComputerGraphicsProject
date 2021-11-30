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

namespace Studying_app_kg.Model
{
    class AffineTransformator
    {
        private const double radians = 0.1; //nearly 6 degrees

        public double[,] Transform(double[,] inputParallelogram, double scale, Point vertex)
        {
            var parallelogramAtCenter = MatrixMultiplication(inputParallelogram, ReturnMoveTransformationMatrix(vertex.X, vertex.Y));
            var turnedParallelogramAtCenter = MatrixMultiplication(parallelogramAtCenter, ReturnTurnTransformationMatrix(radians));
            var backPositionTurnedParallelogram = MatrixMultiplication(turnedParallelogramAtCenter, ReturnReversedMoveTransformationMatrix(vertex.X, vertex.Y));

            var parallelogramOnCenter = MatrixMultiplication(backPositionTurnedParallelogram, ReturnMoveTransformationMatrix(vertex.X, vertex.Y));
            var centerScaledParallelogram = MatrixMultiplication(parallelogramOnCenter, ReturnScopeTransformationMatrix(1 - (scale / 180)));
            var backPositioScaledParallelogram = MatrixMultiplication(centerScaledParallelogram, ReturnReversedMoveTransformationMatrix(vertex.X, vertex.Y));

            return backPositioScaledParallelogram;
        }

        public double[,] ReturnTurnTransformationMatrix(double a)
        {
            double[,] matr = {{  Math.Cos(a), -Math.Sin(a), 0 },
                              {  Math.Sin(a),  Math.Cos(a), 0 },
                              {            0,            0, 1 }};
            return matr;
        }

        public double[,] ReturnMoveTransformationMatrix(double a, double b)
        {
            double[,] matr = {{  1,  0, 0 },
                              {  0,  1, 0 },
                              { -a, -b, 1 }};
            return matr;
        }

        public double[,] ReturnReversedMoveTransformationMatrix(double a, double b)
        {
            double[,] matr = {{ 1, 0, 0 },
                              { 0, 1, 0 },
                              { a, b, 1 }};
            return matr;
        }

        public double[,] ReturnScopeTransformationMatrix(double k)
        {
            double[,] matr = {{ k, 0, 0 },
                              { 0, k, 0 },
                              { 0, 0, 1 }};
            return matr;
        }

        public double[,] ReturnReversedScopeTransformationMatrix(double k)
        {
            double[,] matr = {{ 1/k,   0,  0 },
                              {   0, 1/k,  0 },
                              {   0,   0,  1 }};
            return matr;
        }

        public double[,] MatrixMultiplication(double[,] matrixA, double[,] matrixB)
        {
            if (MatrixColumnsCount(matrixA) != MatrixRowsCount(matrixB))
            {
                throw new Exception("Multiplication is forbidden.");
            }

            double [,] matrixC = new double[MatrixRowsCount(matrixA), MatrixColumnsCount(matrixB)];

            for (int i = 0; i < MatrixRowsCount(matrixA); i++)
            {
                for (int j = 0; j < MatrixColumnsCount(matrixB); j++)
                {
                    matrixC[i, j] = 0;

                    for (var k = 0; k < MatrixColumnsCount(matrixA); k++)
                    {
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return matrixC;
        }

        public int MatrixRowsCount(double[,] matrix)
        {
            return matrix.GetUpperBound(0) + 1;
        }

        public int MatrixColumnsCount(double[,] matrix)
        {
            return matrix.GetUpperBound(1) + 1;
        }
    }
}