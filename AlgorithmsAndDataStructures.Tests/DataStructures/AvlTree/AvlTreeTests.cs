﻿using System;
using Xunit;

namespace AlgorithmsAndDataStructures.Tests.DataStructures.AvlTree
{
    public class AvlTreeTests
    {
        [Fact]
        public void CanInsertRootNode()
        {
            var sut = new AlgorithmsAndDataStructures.DataStructures.AVLTree.AvlTree();
            sut.Insert(10);
            Assert.True(sut.IsBalanced);
        }

        [Fact]
        public void SimpleTreeIsBalanced()
        {
            var sut = new AlgorithmsAndDataStructures.DataStructures.AVLTree.AvlTree();
            sut.Insert(10);
            sut.Insert(11);
            sut.Insert(9);
            Assert.True(sut.IsBalanced);
        }

        [Fact]
        public void TreeIsBalancedWhenUnbalanceIsEqualToOne()
        {
            var sut = new AlgorithmsAndDataStructures.DataStructures.AVLTree.AvlTree();
            sut.Insert(10);
            sut.Insert(11);
            sut.Insert(9);
            sut.Insert(8);
            Assert.True(sut.IsBalanced);
        }

        [Fact]
        public void CanDeleteRootNode()
        {
            var sut = new AlgorithmsAndDataStructures.DataStructures.AVLTree.AvlTree();
            sut.Insert(10);
            sut.Delete(10);
            Assert.True(sut.IsBalanced);
        }

        [Fact]
        public void CanDeleteRightNonRootNode()
        {
            var sut = new AlgorithmsAndDataStructures.DataStructures.AVLTree.AvlTree();
            sut.Insert(10);
            sut.Insert(11);
            sut.Delete(10);
            Assert.True(sut.IsBalanced);
        }

        [Fact]
        public void CanDeleteLeftNonRootNode()
        {
            var sut = new AlgorithmsAndDataStructures.DataStructures.AVLTree.AvlTree();
            sut.Insert(10);
            sut.Insert(11);
            sut.Delete(10);
            Assert.True(sut.IsBalanced);
        }

        [Fact]
        public void TreeIsRebalanacedAfterDelete()
        {
            var sut = new AlgorithmsAndDataStructures.DataStructures.AVLTree.AvlTree();
            var random = new Random();
            var seed = new int[10000];

            for (int j = 0; j < seed.Length; j++)
            {
                seed[j] = random.Next(100000);
                sut.Insert(seed[j]);
                Assert.True(sut.IsBalanced);
            }

            for (int j = 0; j < seed.Length; j++)
            {
                sut.Delete(seed[j]);
                Assert.True(sut.IsBalanced);
            }
        }
    }
}