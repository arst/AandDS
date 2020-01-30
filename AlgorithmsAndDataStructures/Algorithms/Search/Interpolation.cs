﻿using System;

namespace AlgorithmsAndDataStructures.Algorithms.Search
{
    public class Interpolation: ISearchAlgorithm<int>
    {
        public int Search(int[] target, int value)
        {
            if (target.Length == 0)
            {
                return -1;
            }

            if (target.Length == 1)
            {
                return target[0] == value ? 0 : -1;
            }

            var start = 0;
            var end = target.Length - 1;
            while(end >= start && target[start] <= value && target[end] >= value)
            {
                if (start == end)
                {
                    if (target[start] == value)
                    {
                        return start;
                    }

                    return -1;
                }

                var position = start + 
                    (((end - start) / (target[end] - target[start])) 
                    * (value - target[start])); 

                if (target[position] == value)
                {
                    return position;
                }
                if (target[position] > value)
                {
                    end = position - 1;
                }
                else
                {
                    start = position + 1;
                }
            }

            return -1;
        }
    }
}