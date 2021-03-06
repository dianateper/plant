﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Model
{
    public class Statistica
    {
        public static double Min(List<double> array)
        {
            return array.Min();
        }

        public static double Max(List<double> array)
        {
            return array.Max();
        }

        public static double Mean(List<double> array)
        {
            return array.Sum() / array.Count();
        }

        public static double Range(List<double> array)
        {
            return array.Max() - array.Min();
        }

        public static double Median(List<double> array)
        {
            array.Sort();
            int idx = array.Count() / 2;
            if(array.Count()%2 == 0)
            {
                return (array[idx] + array[idx - 1]) / 2;
            }

            return array[idx];
        }
        //Дисперсія
        public static double Variance(List<double> array)
        {
            double sum = 0;
            double mean = Mean(array);

            array.ForEach(a =>
            {
                sum += Math.Pow((a - mean), 2);
            });

            return Math.Round(sum / array.Count(), 4);
        }
        //Середньоквадратичне відхилення
        public static double StandartDeriation(List<double> array)
        {
            return Math.Round(Math.Sqrt(Variance(array)), 4);
        }
        //Асиметрія
        public static double Skewnes(List<double> array)
        {
            double mean = Mean(array);
            double sum = 0;
            double stdDev = StandartDeriation(array);

            int N = array.Count();

       
            array.ForEach(a =>
            {
                sum += Math.Pow(((a - mean) / stdDev), 3);
            });


            return Math.Round(N * sum / (N - 1) / (N - 2), 4);
        }
        //Ексцес
        public static double Kurtosis(List<double> array)
        {
            double mean = Mean(array);
            double sum = 0;
            double StdDev = StandartDeriation(array);

            int N = array.Count();

           
            array.ForEach(a =>
            {
                sum += Math.Pow((a-mean)/StdDev, 4);
            });

            return Math.Round(N * (N + 1) * sum / (N - 1) / (N - 2) / (N - 3) - 3 * (N - 1) * (N - 1) / (N - 2) / (N - 3), 4);
        }
    }
}
