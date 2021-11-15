using System;

namespace Studying_app_kg.Model
{
    class AfiineTransformator
    {
        private double[,] TurnTransformationMatrix =
        {
            {Math.Cos(0.523599), (Math.Sin(0.523599) * (-1d)), 0},
            {Math.Sin(0.523599), Math.Cos(0.523599), 0},
            {0, 0, 1}
        };

        private double[,] MoveTransformationMatrix =
        {
            {1, 0, 0},
            {0, 1, 0},
            {0, 0, 1}
        };

        private double[,] ReturnMoveTransformationMatrix =
        {
            {1, 0, 0},
            {0, 1, 0},
            {-3, -3, 1}
        };

        public double[,] Transform(double[,] inputTriangle, double speed)
        {
            MoveTransformationMatrix[2, 0] = speed;
            MoveTransformationMatrix[2, 1] = speed;

            double[,] resultMatrix = MatrixMultiplication(MoveTransformationMatrix, TurnTransformationMatrix);
            //double[,] resultMatrix = MoveTransformationMatrix;
            //double[,] resultMatrix = TurnTransformationMatrix;
            resultMatrix = MatrixMultiplication(inputTriangle, resultMatrix);
            return resultMatrix;
        }

        public double[,] TransformV2(double[,] inputTriangle, double speed)
        {
            double centerX = (inputTriangle[0, 0] + inputTriangle[1, 0] + inputTriangle[2, 0]) / 3d;
            double centerY = (inputTriangle[0, 1] + inputTriangle[1, 1] + inputTriangle[2, 1]) / 3d;
            MoveTransformationMatrix[2, 0] = -centerX;
            MoveTransformationMatrix[2, 1] = -centerY;

            double[,] transformationMatrix = MatrixMultiplication(MoveTransformationMatrix, TurnTransformationMatrix);

            MoveTransformationMatrix[2, 0] = speed;
            MoveTransformationMatrix[2, 1] = speed;
            
            ReturnMoveTransformationMatrix[2, 0] = centerX;
            ReturnMoveTransformationMatrix[2, 1] = centerY;
            
            transformationMatrix = MatrixMultiplication(transformationMatrix, ReturnMoveTransformationMatrix);
            transformationMatrix = MatrixMultiplication(transformationMatrix, MoveTransformationMatrix);
            
            return MatrixMultiplication(inputTriangle, transformationMatrix);
             //transformationMatrix;
        }

        public double[,] MatrixMultiplication(double[,] matrixA, double[,] matrixB)
        {
            if (matrixA.ColumnsCount() != matrixB.RowsCount())
            {
                throw new Exception(
                    "Множення неможливе! Кількість стовпців першої матриці не рівне кількості рядків другої матриці.");
            }

            var matrixC = new double[matrixA.RowsCount(), matrixB.ColumnsCount()];

            for (var i = 0; i < matrixA.RowsCount(); i++)
            {
                for (var j = 0; j < matrixB.ColumnsCount(); j++)
                {
                    matrixC[i, j] = 0;

                    for (var k = 0; k < matrixA.ColumnsCount(); k++)
                    {
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return matrixC;
        }
    }

    static class MatrixExt
    {
        // метод расширения для получения количества строк матрицы
        public static int RowsCount(this double[,] matrix)
        {
            return matrix.GetUpperBound(0) + 1;
        }

        // метод расширения для получения количества столбцов матрицы
        public static int ColumnsCount(this double[,] matrix)
        {
            return matrix.GetUpperBound(1) + 1;
        }
    }
}