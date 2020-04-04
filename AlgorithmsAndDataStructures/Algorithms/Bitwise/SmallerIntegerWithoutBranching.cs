﻿namespace AlgorithmsAndDataStructures.Algorithm.Bitwise
{
    public class SmallerIntegerWithoutBranching
    {
        public int GetSmaller(int x, int y)
        {
            const int bitSize = 8;

            // >> (sizeof(int) * bitSize - 1)) - if x - y is positive number then it would result in 0, 
            //if negative number than it would result in -1
            // In first case y + 0 is just y
            // In second case the result would be negative number which is the difference between y - x
            // y + (-difference between y and x) is x
            return y + ((x - y) & ((x - y) >> (sizeof(int) * bitSize - 1)));
        }
    }
}
