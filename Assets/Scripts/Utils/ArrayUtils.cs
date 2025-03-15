using System;

namespace Utils
{
    public class ArrayUtils
    {
        public static T[,] Resize2DArray<T>(T[,] original, int newRows, int newCols)
        {
            var newArray = new T[newRows, newCols];
            int minRows = Math.Min(original.GetLength(0), newRows);
            int minCols = Math.Min(original.GetLength(1), newCols);

            for (int i = 0; i < minRows; i++)
            {
                for (int j = 0; j < minCols; j++)
                {
                    newArray[i, j] = original[i, j];
                }
            }

            return newArray;
        }
    }
}