﻿using System;

namespace AlgorithmsAndDataStructures.Algorithms.String.Search
{
    public class RabinKarp : IStringPatternSearchAlgorithm
    {
        public int Search(string input, string pattern)
        {
            if (input.Length < pattern.Length)
            {
                return -1;
            }

            var primeNumber = 3;
            var hash = ComputeHash(pattern, pattern.Length, primeNumber);
            var index = 0;
            var startingHash = ComputeHash(input, pattern.Length, primeNumber);
            do
            {
                if (hash == startingHash)
                {
                    var isMatched = true;

                    for (int i = index; i < index + pattern.Length; i++)
                    {
                        if (input[i] != pattern[i - index])
                        {
                            isMatched = false;
                        }
                    }

                    if (isMatched)
                    {
                        return index;
                    }
                }

                index += 1;
                startingHash = RecalculateHash(input, startingHash, index, primeNumber, pattern);
            }
            while (index <= input.Length - pattern.Length);

            return -1;
        }

        private int RecalculateHash(string input, int currentHash, int index, int primeNumber, string pattern)
        {
            int suffixHash = currentHash - input[index - 1];
            int fullHash = suffixHash + (input[index + pattern.Length - 1] * (int)Math.Pow(primeNumber, pattern.Length));
            var normalizedHash = fullHash / primeNumber;
            return normalizedHash;
        }

        private int ComputeHash(string pattern, int stopAt, int primeNumber)
        {
            var hash = 0;

            for (int i = 0; i < stopAt; i++)
            {
                hash += pattern[i] * (int)Math.Pow(primeNumber, i);
            }

            return hash;
        }
    }
}