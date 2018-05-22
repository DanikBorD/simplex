using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simplex_method
{
    public class Simplex
    {
        //source - симплекс таблица без базисных переменных
        double[,] table; //симплекс таблица

        int m, n;

        List<int> basis; //список базисных переменных

        public Simplex(double[,] source, ref int result_size)
        {
            m =source.GetLength(0); // берем высоту симплекс таблицы 
            n = source.GetLength(1); // берем длину симплекс таблицы
            table = new double[m, n + m - 1]; // определяем размеры массива (количество уранений + строка для целевой на ббазисные + свободные))
            basis = new List<int>();
            
            for (int i = 0; i < m; i++) // цикл по строкам СТ
            {
                for (int j = 0; j < table.GetLength(1); j++) //цикл по столбцам СТ
                {
                    //if(i == (m - 1))

                    if (j < n) // если свободная переменная
                        table[i, j] = source[i, j]; 
                    else
                        table[i, j] = 0;
                }
                //выставляем коэффициент 1 перед базисной переменной в строке
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1; // присваиваем значение соотв. базисной переменной
                    basis.Add(n + i);
                }
            }

            n = table.GetLength(1); // переназначаем длину СТ
            result_size = n - 1; // определяем число переменных в решении
        }

        //result - в этот массив будут записаны полученные значения X
        public double[,] Calculate(double[] result)
        {
            int mainCol, mainRow; //ведущие столбец и строка

            while (!IsItEnd())
            {
                mainCol = findMainCol();
                mainRow = findMainRow(mainCol);
                basis[mainRow] = mainCol; // поменяли местами ведущий стобец и строку

                double[,] new_table = new double[m, n]; // задаем размеры матрицы новой

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol]; // элементы главной строки делим на ведущий элемент

                for (int i = 0; i < m; i++) // перебираем все строки в таблице 
                {
                    if (i == mainRow) // кроме самой строки
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];  //  каждый элемент новой СТ ведущей строки умножается на ведущий элемент с минусом и результат произведения вычетаем из соответствующего значения нвоой СТ
                }
                table = new_table;
            }

            //заносим в result найденные значения X
            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1) // записываем в опорное решение значения БП 
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            return table;
        }

        private bool IsItEnd()
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        private int findMainCol() // ищем наибольшее по модулю отрицательное значение в коэф. целевой функции в СТ 
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        private int findMainRow(int mainCol)
        {
            int mainRow = 0;

            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
		{
 		// проверка на наименьшее отношение элементов
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;
		}
            return mainRow;
        }


    }
}