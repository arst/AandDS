﻿using AlgorithmsAndDataStructures.Algorithms.StringAlgorithms.Search;

namespace AlgorithmsAndDataStructures.Tests.Algorithm.String.Search
{
    public class NaivePatternSearchTests : StringSearchAlgorithmBaseTests
    {
        protected override IStringPatternSearchAlgorithm GetSystemUnderTest()
        {
            return new NaivePatternSearch();
        }
    }
}
