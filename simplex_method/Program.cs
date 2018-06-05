
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
            int coef = 1;
            int dop_ind = 0;

            if (System.IO.File.Exists(path))
                text_table = System.IO.File.ReadAllText(path);

            rows = text_table.Split(';').ToList();

            if (rows.Last() == "")
                rows.RemoveAt(rows.IndexOf(rows.Last()));

            columns = rows[0].Split(',').ToList();
            double[,] table_tmp = new double[rows.Count, columns.Count - 1];

            for (var str = 0; str < rows.ToArray().Length; str++)
            {
                if (rows[str].Contains(">=") || rows[str].Contains("max"))
                    coef = -1;
                rows[str] = rows[str].Trim();
                for (var col = 0; col < columns.ToArray().Length; col++)
                {
                    rows[str].Split(',')[col] = rows[str].Split(',')[col].Trim();
                    if (Double.TryParse(rows[str].Split(',')[col], out table_tmp[str, col - dop_ind]))
                    {
                        table_tmp[str, col - dop_ind] = table_tmp[str, col - dop_ind] * coef;
                    }
                    else
                    {
                        dop_ind = 1;
                    }
                }

                dop_ind = 0;           
                coef = 1;
            }

            double[,] table = new double[rows.Count, columns.Count - 1];

            for (var i = 0; i < rows.Count; i++)
            {
                table[i, 0] = table_tmp[i, columns.Count - 2];
            }

            for (var i = 0; i < rows.Count; i++)
                for (var j = 1; j < columns.Count -1; j++)
                {
                    table[i, j] = table_tmp[i, j-1];
                }

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