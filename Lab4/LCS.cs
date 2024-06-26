﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCS
{
    public class LCS
    {
        public int CalcLCS(string x, string y)
        {
            int m = x.Length;
            int n = y.Length;

            //Minimizes size of array, m = min(m,n)
            if (m > n)
            {
                string tmpStr = x;
                x = y;
                y = tmpStr;
                int tmpLen = m;
                m = n;
                n = tmpLen;
            }

            //Initilize only one array of size m
            int[] matrix = new int[m + 1];

            for (int i = 1; i <= n; i++)
            {
                //Reset prev for every new iteration
                int prev = matrix[0];
                for (int j = 1; j <= m; j++)
                {
                    //Store matrix[j] temporaily,
                    //used with prev to "simulate" having a 2d array rather then a 1d array
                    int tmp = matrix[j];
                    if (x[j - 1] == y[i - 1])
                    {
                        matrix[j] = prev + 1;                        
                    }
                    else
                    {
                        matrix[j] = Math.Max(matrix[j], matrix[j - 1]);
                    }
                    //Assign tmp to prev
                    prev = tmp;
                }
            }
            return matrix[matrix.Length - 1];
        }
    }
}
