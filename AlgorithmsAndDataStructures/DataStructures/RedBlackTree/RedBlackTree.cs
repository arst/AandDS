﻿using System;

namespace AlgorithmsAndDataStructures.DataStructures.RedBlackTree
{
    public class RedBlackTree
    {
        #region Fields
        private RedBlackTreeNode root;
        #endregion

        #region Properties
        public int Height => Math.Max(HeightInternal(this.root) - 1, 0);
        #endregion

        #region Constructors
        public RedBlackTree(RedBlackTreeNode root)
        {
            this.root = root;
        }

        public RedBlackTree()
        {
        }
        #endregion

        #region Insert
        public void Insert(int value)
        {
            var newNode = new RedBlackTreeNode()
            { 
                IsRed = true,
                Value = value,
            };

            if (root == null)
            {
                root = newNode;
                root.IsRed = false;
                return;
            }

            InsertInternal(root, newNode);
            CheckInsert(newNode);
        }

        private void InsertInternal(RedBlackTreeNode rootNode, RedBlackTreeNode toInsert)
        {
            if (toInsert.Value >= rootNode.Value)
            {
                if (rootNode.Right.IsLeafNode)
                {
                    rootNode.Right = toInsert;
                    toInsert.Parent = rootNode;
                    return;
                }

                InsertInternal(rootNode.Right, toInsert);
            }
            else
            {
                if (rootNode.Left.IsLeafNode)
                {
                    rootNode.Left = toInsert;
                    toInsert.Parent = rootNode;
                    return;
                }

                InsertInternal(root.Left, toInsert);
            }
        }

        private void CheckInsert(RedBlackTreeNode node)
        {
            if (node == root || node == null)
            {
                return;
            }

            //Check if there are two consequitive red nodes in a tree
            if (node.IsRed && node.Parent.IsRed)
            {
                CorrectInsert(node);
            }

            CheckInsert(node.Parent);
        }

        private void CorrectInsert(RedBlackTreeNode node)
        {
            if (IsUncleRed(node))
            {
                node.Parent.Parent.Left.IsRed = false;
                node.Parent.Parent.Right.IsRed = false;
                node.Parent.Parent.IsRed = true;
            }

            if(IsUncleBlack(node))
            {
                Rotate(node);
            }
        }

        private bool IsUncleRed(RedBlackTreeNode node)
        {
            var grandparent = node.Parent.Parent;

            if (grandparent == null)
            {
                return false;
            }

            if (node.Parent.IsLeft)
            {
                return grandparent.Right.IsRed;
            }

            return grandparent.Left.IsRed;
        }

        private bool IsUncleBlack(RedBlackTreeNode node)
        {
            var grandparent = node.Parent.Parent;

            if (grandparent == null)
            {
                return false;
            }

            return node.Parent.IsLeft ? grandparent.Right.IsBlack : grandparent.Left.IsBlack;
        }

        private void Rotate(RedBlackTreeNode node)
        {
            if (node.IsLeft && node.Parent.IsLeft)
            {
                RotateRight(node.Parent.Parent);
                node.IsRed = true;
                node.Parent.IsRed = false;

                if (!node.Parent.Right.IsLeafNode)
                    node.Parent.Right.IsRed = true;
            }
            else if (node.IsRight && node.Parent.IsRight)
            {
                RotateLeft(node.Parent.Parent);
                node.IsRed = true;
                node.Parent.IsRed = false;

                if (!node.Parent.Left.IsLeafNode)
                    node.Parent.Left.IsRed = true;
            }
            else if (node.IsRight && node.Parent.IsLeft)
            {
                RotateLeftRight(node.Parent.Parent);
                node.IsRed = false;
                node.Right.IsRed = true;
                node.Left.IsRed = true;
            }
            else if (node.IsLeft && node.Parent.IsRight)
            {
                RotateRightLeft(node.Parent.Parent);

                node.IsRed = false;
                node.Left.IsRed = true;
                node.Right.IsRed = true;
            }
        }
        #endregion

        #region Delete
        public void Delete(int value)
        {
            root = DeleteInternal(root, value, out RedBlackTreeNode doubleBlackNode);

            if (doubleBlackNode != null)
            {
                FixDelete(doubleBlackNode);
            }
        }

        private RedBlackTreeNode DeleteInternal(RedBlackTreeNode node, int value, out RedBlackTreeNode doubleBlackNode)
        {
            if (node == null)
            {
                doubleBlackNode = null;
                return null;
            }

            if (node.Value > value)
            {
                node.Left = DeleteInternal(node.Left, value, out doubleBlackNode);
            }
            else if (node.Value < value)
            {
                node.Right = DeleteInternal(node.Right, value, out doubleBlackNode);
            }
            else
            {
                // Leaf Node.
                if (node.Left.IsLeafNode && node.Right.IsLeafNode)
                {
                    if (node.IsRed)
                    {
                        doubleBlackNode = null;
                        return null;
                    }

                    doubleBlackNode = node.Left;
                    doubleBlackNode.Parent = node.Parent;

                    if (node.IsLeft)
                    {
                        doubleBlackNode.Parent.Left = doubleBlackNode;
                    }
                    else
                    {
                        doubleBlackNode.Parent.Right = doubleBlackNode;
                    }
                    return doubleBlackNode;
                }

                // One Children.
                if (node.Left.IsLeafNode || node.Right.IsLeafNode)
                {
                    var nonNullChild = node.Left.IsLeafNode ? node.Right : node.Left;

                    if (node.IsRed)
                    {
                        doubleBlackNode = nonNullChild;
                        doubleBlackNode.Parent = node.Parent;
                        if (node.IsLeft)
                        {
                            doubleBlackNode.Parent.Left = doubleBlackNode;
                        }
                        else
                        {
                            doubleBlackNode.Parent.Right = doubleBlackNode;
                        }
                        return doubleBlackNode;
                    }

                    if (nonNullChild.IsRed)
                    {
                        nonNullChild.IsRed = false;
                    }

                    doubleBlackNode = nonNullChild;
                    doubleBlackNode.Parent = node.Parent;
                    if (node.IsLeft)
                    {
                        doubleBlackNode.Parent.Left = doubleBlackNode;
                    }
                    else
                    {
                        doubleBlackNode.Parent.Right = doubleBlackNode;
                    }

                    return doubleBlackNode;
                }

                // Two children. Convert to one children case.
                var minNode = FindNodeWithMinValue(node.Right);

                node.Value = minNode.Value;

                DeleteInternal(node.Right, minNode.Value, out doubleBlackNode);
            }

            return node;
        }

        private RedBlackTreeNode FindNodeWithMinValue(RedBlackTreeNode right)
        {
            var current = right;

            while (!current.Left.IsLeafNode)
            {
                current = current.Left;
            }

            return current;
        }


        private void FixDelete(RedBlackTreeNode node)
        {
            if (node == root)
            {
                node.IsRed = false;
                return;
            }
            if (IsCase2(node))
            {
                FixCase2(node);
            }

            if (IsCase3(node))
            {
                FixCase3(node);
                FixDelete(node.Parent);
                return;
            }
            if (IsCase4(node))
            {
                FixCase4(node);
                return;
            }
            if (IsCase5(node))
            {
                FixCase5(node);
                return;
            }
            if (IsCase6(node))
            {
                FixCase6(node);
            }
        }

        #endregion

        #region Check Delete Cases

        private bool IsCase2(RedBlackTreeNode node)
        {
            var parent = node.Parent;
            var sibling = node.IsLeft ? parent.Right : parent.Left;
            var isSiblingChildrenBlack = (sibling.Left.IsLeafNode || sibling.Left.IsBlack) && (sibling.Right.IsLeafNode || sibling.Right.IsBlack);

            return parent.IsBlack && sibling.IsRed && isSiblingChildrenBlack;
        }

        private bool IsCase3(RedBlackTreeNode node)
        {
            var parent = node.Parent;
            var sibling = node.IsLeft ? parent.Right : parent.Left;
            var isSiblingChildrenBlack = (sibling.Left.IsLeafNode|| sibling.Left.IsBlack) && (sibling.Right.IsLeafNode || sibling.Right.IsBlack);

            return !node.Parent.IsRed && sibling.IsBlack && isSiblingChildrenBlack;
        }

        private bool IsCase4(RedBlackTreeNode node)
        {
            var parent = node.Parent;
            var sibling = node.IsLeft ? parent.Right : parent.Left;
            var isSiblingChildrenBlack = (sibling.Left.IsLeafNode || sibling.Left.IsBlack) && (sibling.Right.IsLeafNode || sibling.Right.IsBlack);

            return node.Parent.IsRed && sibling.IsBlack && isSiblingChildrenBlack;

        }

        private bool IsCase5(RedBlackTreeNode node)
        {
            var parent = node.Parent;
            var sibling = node.IsLeft ? parent.Right : parent.Left;
            var isSiblingChildrenRedBlack = (!sibling.Left.IsLeafNode && sibling.Left.IsRed) && (sibling.Right.IsLeafNode || sibling.Right.IsBlack);

            return sibling.IsBlack && isSiblingChildrenRedBlack;
        }

        private bool IsCase6(RedBlackTreeNode node)
        {
            var isSiblingRightChildRed = false;

            bool isSiblingBlack;

            if (node.IsLeft)
            {
                var rightSibling = node.Parent.Right;

                isSiblingBlack = rightSibling == null || !rightSibling.IsRed;

                if (rightSibling != null)
                {
                    isSiblingRightChildRed = rightSibling.Right != null && rightSibling.Right.IsRed;
                }

                return isSiblingBlack && isSiblingRightChildRed;
            }

            var leftSibling = node.Parent.Left;

            isSiblingBlack = leftSibling == null || !leftSibling.IsRed;

            if (leftSibling != null)
            {
                isSiblingRightChildRed = leftSibling.Right != null && leftSibling.Right.IsRed;
            }

            return isSiblingBlack && isSiblingRightChildRed;
        }
        #endregion

        #region Fix Delete Cases
        private void FixCase2(RedBlackTreeNode node)
        {
            var parent = node.Parent;
            var originalParentColor = node.Parent.IsRed;
            var sibling = node.IsLeft ? node.Parent.Right : node.Parent.Left;
            var originalSiblingColor = sibling.IsRed;

            if (node.IsLeft)
            {
                RotateLeft(node.Parent);
            }
            else
            {
                RotateRight(node.Parent);
            }

            parent.IsRed = originalSiblingColor;
            sibling.IsRed = originalParentColor;
        }

        private void FixCase3(RedBlackTreeNode node)
        {
            var parent = node.Parent;

            if (node.IsLeft)
            {
                if (parent.Right != null)
                {
                    parent.Right.IsRed = true;
                }
            }
            else
            {
                if (parent.Left != null)
                {
                    parent.Left.IsRed = true;
                }
            }
        }

        private void FixCase4(RedBlackTreeNode node)
        {
            node.Parent.IsRed = false;

            if (node.IsLeft)
            {
                node.Parent.Right.IsRed = true;
            }
            else
            {
                node.Parent.Left.IsRed = true;
            }

        }

        private void FixCase5(RedBlackTreeNode node)
        {
            var parent = node.Parent;
            if (node.IsLeft)
            {
                RotateRight(parent.Right);
                if (parent.Right.Left != null)
                {
                    parent.Right.Left.IsRed = false;
                }
                parent.Right.IsRed = true;
            }
            else
            {
                RotateLeft(parent.Left);
                if (parent.Right.Right != null)
                {
                    parent.Right.Right.IsRed = false;
                }
                parent.Left.IsRed = true;
            }

            FixCase6(node);
        }

        private void FixCase6(RedBlackTreeNode node)
        {
            if (node.IsLeft)
            {
                var originalColor = node.Parent.IsRed;
                var rightSibling = node.Parent.Right;
                RotateLeft(node.Parent);
                rightSibling.IsRed = originalColor;
                rightSibling.Right.IsRed = false;
                rightSibling.Left.IsRed = false;
            }
            else
            {
                var originalColor = node.Parent.IsRed;
                var leftSibling = node.Parent.Left;
                RotateRight(node.Parent);
                leftSibling.IsRed = false;
                leftSibling.Right.IsRed = false;
                leftSibling.Left.IsRed = false;

            }
        }
        #endregion

        #region Rotations
        private void RotateRightLeft(RedBlackTreeNode node)
        {

            /*
             RotateRight(node.Left);
             RotateLeft(node);
             */
            var parent = node.Parent;

            var rightChild = node.Right;

            var leftGrandChild = node.Right.Left;

            var rightGrandChildLeft = leftGrandChild.Left;
            var rightGrandChildRight = leftGrandChild.Right;

            leftGrandChild.Left = node;
            node.Parent = leftGrandChild;
            leftGrandChild.Right = rightChild;
            rightChild.Parent = leftGrandChild;
            rightChild.Left = rightGrandChildRight;

            if (rightChild.Left != null)
            {
                rightChild.Left.Parent = rightChild;
            }

            node.Right = rightGrandChildLeft;

            if (node.Right != null)
            {
                node.Right.Parent = node;
            }

            if (parent == null)
            {
                root = leftGrandChild;
                root.Parent = null;
                return;
            }

            leftGrandChild.Parent = parent;

            if (node.IsRight)
            {
                parent.Right = leftGrandChild;
            }
            else
            {
                parent.Left = leftGrandChild;
            }
        }

        private void RotateLeftRight(RedBlackTreeNode node)
        {
            /*
             RotateLeft(node.Left);
             RotateRight(node);
             */

            var parent = node.Parent;

            var leftChild = node.Left;

            var rightGrandChild = node.Left.Right;

            var rightGrandChildLeft = rightGrandChild.Left;
            var rightGrandChildRight = rightGrandChild.Right;

            rightGrandChild.Left = leftChild;
            leftChild.Parent = rightGrandChild;
            rightGrandChild.Right = node;
            node.Parent = rightGrandChild;
            leftChild.Right = rightGrandChildLeft;
            if (leftChild.Right != null)
            {
                leftChild.Right.Parent = leftChild;
            }
            node.Left = rightGrandChildRight;

            if (node.Left != null)
            {
                node.Left.Parent = node;
            }

            if (parent == null)
            {
                root = rightGrandChild;
                root.Parent = null;
                return;
            }

            rightGrandChild.Parent = parent;

            if (node.IsRight)
            {
                parent.Right = rightGrandChild;
            }
            else
            {
                parent.Left = rightGrandChild;
            }
        }

        private void RotateRight(RedBlackTreeNode node)
        {
            var parent = node.Parent;

            var leftChild = node.Left;

            node.Left = leftChild.Right;
            if (node.Left != null)
            {
                node.Left.Parent = node;
            }
            leftChild.Right = node;
            node.Parent = leftChild;

            if (parent == null)
            {
                root = leftChild;
                root.Parent = null;
                return;
            }

            leftChild.Parent = parent;

            if (node.IsRight)
            {
                parent.Right = leftChild;
            }
            else
            {
                parent.Left = leftChild;
            }
        }

        private void RotateLeft(RedBlackTreeNode node)
        {
            var parent = node.Parent;
            var originalPositionIsRight = node.IsRight;
            var rightChild = node.Right;

            node.Right = rightChild.Left;
            if (node.Right != null)
                node.Right.Parent = node;
            rightChild.Left = node;
            node.Parent = rightChild;

            if (parent == null)
            {
                root = rightChild;
                root.Parent = null;
                return;
            }

            rightChild.Parent = parent;

            if (originalPositionIsRight)
            {
                parent.Right = rightChild;
            }
            else
            {
                parent.Left = rightChild;
            }
        }
        #endregion

        #region Tree Validation
        public void CheckTreeValidity()
        {
            if (root == null)
            {
                return;
            }

            if (root.IsRed)
            {
                throw new Exception("Root is red.");
            }

            CheckBlackNodesBalance(root);
            CheckNoConsequentRedNodes(root);
        }

        private void CheckNoConsequentRedNodes(RedBlackTreeNode node)
        {
            if (node.IsLeafNode)
            {
                return;
            }

            if (node.IsRed && (node.Left.IsRed || node.Right.IsRed))
            {
                throw new Exception($"Consequent red nodes. Node value: {node.Value}. Left: {node.Left.Value}. Right {node.Right.Value}");
            }
        }

        private int CheckBlackNodesBalance(RedBlackTreeNode node)
        {
            if (node == null || node.IsLeafNode)
            {
                return 1;
            }

            var leftHeight = CheckBlackNodesBalance(node.Left);
            var rightHeight = CheckBlackNodesBalance(node.Right);

            if (leftHeight != rightHeight)
            {
                throw new Exception("Tree is unbalanced.");
            }

            return node.IsRed ? leftHeight : leftHeight + 1;
        }

        private int HeightInternal(RedBlackTreeNode node)
        {
            if (node == null || node.IsLeafNode)
            {
                return 0;
            }

            var leftHeight = HeightInternal(node.Left) + 1;
            var rightHeight = HeightInternal(node.Right) + 1;

            return Math.Max(leftHeight, rightHeight);
        }
        #endregion
    }
}