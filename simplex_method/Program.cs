
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace simplex_method
{
    class Program
    {
        static void Main(string[] args)
        {
            string text_table = string.Empty;
            string path = @"D:\simplex.txt"; // !!! путь до файла с исходниками
            var rows = new List<string>();
            var columns = new List<string>();

            if (System.IO.File.Exists(path))
                text_table = System.IO.File.ReadAllText(path);

            rows = text_table.Split(';').ToList();

            if (rows.Last() == "")
                rows.RemoveAt(rows.IndexOf(rows.Last()));

            columns = rows[0].Split(',').ToList();
            double[,] table = new double[rows.Count, columns.Count];

            for (var str = 0; str < rows.ToArray().Length; str++)
                for (var col = 0; col < columns.ToArray().Length; col++)
                    table[str, col] = Convert.ToDouble(rows[str].Split(',')[col]);

            //double[,] table = { {25, -3,  5}, (25 >= -3x1 + 5x2)
            //                    {30, -2,  5}, 
            //                    {10,  1,  0}, 
            //                    { 6,  3, -8}, 
            //                    { 0, -6, -5} }; // целевое уравнение вконце

            Console.WriteLine("Исходная система:");
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    Console.Write(table[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();

            //double[] result = new double[10];
            int result_size = 0;
            double[,] table_result;
            Simplex S = new Simplex(table, ref result_size);
            double[] result = new double[result_size];
            table_result = S.Calculate(result);

            Console.WriteLine("Решенная симплекс-таблица:");
            for (int i = 0; i < table_result.GetLength(0); i++)
            {
                for (int j = 0; j < table_result.GetLength(1); j++)
                    Console.Write(table_result[i, j] + " ");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Решение:");
            for(var x = 1; x <= result.Length; x++)
                Console.WriteLine("X" + x.ToString() + "=" + result[x-1]);
            //Console.WriteLine("X[2] = " + result[1]);
            Console.ReadLine();
        }
    }
}