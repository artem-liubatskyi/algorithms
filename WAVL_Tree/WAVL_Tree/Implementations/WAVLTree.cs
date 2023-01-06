using System.Collections.Generic;
using WAVL_Tree.Interfaces;
using WAVL_Tree.Models;

namespace WAVL_Tree.Implementations
{
    public class WAVLTree<ValueType> : IBinaryTree<ValueType>
    {
        public Node<ValueType> Root;

        public event Counter NodeCounter;
        public event RotationCounter RotationCounter;

        public int Size()
        {
            if (Root == null)
            {
                return 0;
            }

            return Root.SubtreeSize;
        }

        public bool Empty()
        {
            return Root == null;
        }

        public ValueType Search(int k)
        {
            Node<ValueType> found = FindNode(k, Root);
            if (found != null)
                return found.Value;
            return default;
        }

        private Node<ValueType> FindNode(int k, Node<ValueType> node)
        {
            NodeCounter?.Invoke();

            if (node == null)
                return null;
            else if (k == node.Key)
                return node;
            else if (k > node.Key && node.Right != null)
                return FindNode(k, node.Right);
            else if (k < node.Key && node.Left != null)
                return FindNode(k, node.Left);
            return null;
        }

        public void Insert(int k, ValueType i)
        {
            if (Root == null)
            {
                Root = new WAVLNode<ValueType>(k, i);
                return;
            }

            Node<ValueType> parentNode = FindInsertionPlace(k, Root);
            if (parentNode.Key == k)
                return;

            new WAVLNode<ValueType>(k, i, parentNode);

            InsertionRebalance(parentNode);
        }

        private Node<ValueType> FindInsertionPlace(int k, Node<ValueType> node)
        {
            NodeCounter?.Invoke();

            if (k == node.Key)
                return node;
            else if (k > node.Key)
            {
                if (node.Right == null)
                    return node;
                else
                    return FindInsertionPlace(k, node.Right);
            }
            else
            {
                if (node.Left == null)
                    return node;
                else
                    return FindInsertionPlace(k, node.Left);
            }
        }

        private int[] GetRankDifference(Node<ValueType> node)
        {
            int[] rankDiffs = new int[2];

            if (node != null)
            {
                if (node.Left != null)
                    rankDiffs[0] = node.Rank - node.Left.Rank;
                else
                    rankDiffs[0] = node.Rank - (-1);

                if (node.Right != null)
                    rankDiffs[1] = node.Rank - node.Right.Rank;
                else
                    rankDiffs[1] = node.Rank - (-1);
            }

            return rankDiffs;
        }

        private bool IsValidType(int[] vType)
        {
            foreach (int rankDiff in vType)
            {
                if (rankDiff != 1)
                {
                    if (rankDiff != 2)
                        return false;
                }
            }

            return true;
        }

        private void InsertionRebalance(Node<ValueType> node)
        {
            Node<ValueType> curr = node;
            int[] currVType = GetRankDifference(node);

            while (curr != null && !IsValidType(currVType))
            {
                if (currVType[0] + currVType[1] == 1)
                {
                    curr.Rank++;
                    curr = curr.Parent;
                    if (curr != null)
                        currVType = GetRankDifference(curr);
                    continue;
                }
                // Rotation cases

                if (currVType[0] == 0)
                { // Rolling up from the left

                    int[] childVType = GetRankDifference(curr.Left);

                    if (childVType[0] == 1 && childVType[1] == 2)
                    { // child is (1,2)
                        RotateRight(curr, true);
                    }
                    else
                    {
                        DoubleRotateRight(curr, true);
                    }

                }
                else if (currVType[1] == 0)
                { // Rolling up from the right

                    int[] childVType = GetRankDifference(curr.Right);

                    if (childVType[0] == 2 && childVType[1] == 1)
                    { //child is (2,1)
                        RotateLeft(curr, true);
                    }
                    else
                    {
                        DoubleRotateLeft(curr, true);
                    }
                }
                return;
            }
            return;
        }

        private void RotateRight(Node<ValueType> source, bool insert)
        {
            RotationCounter?.Invoke(1);
            Node<ValueType> left = source.Left;
            Node<ValueType> right = left.Right;

            source.ReplaceWith(left, ref Root);

            left.SetRightChild(source);
            source.SetLeftChild(right);

            if (insert)
            {
                source.Rank--;
            }
            else
            {
                source.Rank--;
                left.Rank++;
                if (source.IsLeaf())
                {
                    source.Rank--;
                }
            }
        }

        private void RotateLeft(Node<ValueType> source, bool insert)
        {
            RotationCounter?.Invoke(1);
            Node<ValueType> y = source.Right;
            Node<ValueType> a = y.Left;

            source.ReplaceWith(y, ref Root);

            y.SetLeftChild(source);
            source.SetRightChild(a);

            if (insert)
            {
                source.Rank--;
            }
            else
            {
                source.Rank--;
                y.Rank++;
                if (source.IsLeaf())
                {
                    source.Rank--;
                }
            }
        }

        public void DoubleRotateRight(Node<ValueType> source, bool insert)
        {
            RotationCounter?.Invoke(2);

            Node<ValueType> x = source.Left;
            Node<ValueType> a = x.Right;
            Node<ValueType> b = a.Left;
            Node<ValueType> c = a.Right;

            source.ReplaceWith(a, ref Root);

            a.SetLeftChild(x);
            a.SetRightChild(source);
            x.SetRightChild(b);
            source.SetLeftChild(c);

            if (insert)
            {
                x.Rank--;
                source.Rank--;
                a.Rank++;
            }
            else
            {
                a.Rank += 2;
                source.Rank -= 2;
                x.Rank--;
            }

        }

        public void DoubleRotateLeft(Node<ValueType> source, bool insert)
        {
            RotationCounter?.Invoke(2);

            Node<ValueType> y = source.Right;
            Node<ValueType> a = y.Left;
            Node<ValueType> c = a.Left;
            Node<ValueType> d = a.Right;

            source.ReplaceWith(a, ref Root);

            a.SetLeftChild(source);
            a.SetRightChild(y);
            source.SetRightChild(c);
            y.SetLeftChild(d);

            if (insert)
            {
                y.Rank--;
                source.Rank--;
                a.Rank++;
            }
            else
            {
                a.Rank += 2;
                source.Rank -= 2;
                y.Rank--;
            }
        }

        public void Delete(int k)
        {
            Node<ValueType> node = FindNode(k, Root);
            if (node == null)
                return;

            Node<ValueType> offender;

            if (node.IsLeaf())
            {
                offender = DeleteLeaf(node);
            }
            else if (node.Right != null)
            {

                Node<ValueType> successor = NodeWithMinKey(node.Right);

                if (successor.IsLeaf())
                {
                    offender = DeleteLeaf(successor);
                    node.Key = successor.Key;
                    node.Value = successor.Value;
                }
                else
                {
                    node.Key = successor.Key;
                    node.Value = successor.Value;
                    successor.Key = successor.Right.Key;
                    successor.Value = successor.Right.Value;
                    offender = DeleteLeaf(successor.Right);
                }
            }
            else
            {
                int newKey = node.Left.Key;
                ValueType newVal = node.Left.Value;
                offender = DeleteLeaf(node.Left);
                node.Key = newKey;
                node.Value = newVal;
            }

            DeletionRebalance(offender);
        }

        private void DeletionRebalance(Node<ValueType> node)
        {
            Node<ValueType> curr = node;

            if (curr == null)
            {
                return;
            }

            int[] currVType = GetRankDifference(node);

            if (curr.IsLeaf() && curr.Rank != 0)
            {
                curr.Rank = 0;
                curr = curr.Parent;
                if (curr != null)
                    currVType = GetRankDifference(curr);
            }

            while (curr != null && !IsValidType(currVType))
            {
                if (currVType[0] + currVType[1] == 5)
                {
                    curr.Rank--;
                    curr = curr.Parent;
                    if (curr != null)
                        currVType = GetRankDifference(curr);
                    continue;
                }

                if (currVType[0] == 3)
                {
                    int[] childVType = GetRankDifference(curr.Right);

                    if (currVType[1] == 1 && childVType[0] + childVType[1] == 4)
                    { // child is (2,2) - Double demote
                        curr.Rank--;
                        curr.Right.Rank--;
                        curr = curr.Parent;
                        if (curr != null)
                            currVType = GetRankDifference(curr);
                    }
                    else if (childVType[1] == 1)
                    {
                        RotateLeft(curr, false);
                        return;

                    }
                    else
                    {
                        DoubleRotateLeft(curr, false);
                        return;
                    }

                }
                else if (currVType[1] == 3)
                {
                    int[] childVType = GetRankDifference(curr.Left);

                    if (currVType[0] == 1 && childVType[0] + childVType[1] == 4)
                    {
                        curr.Rank--;
                        curr.Left.Rank--;
                        curr = curr.Parent;
                        if (curr != null)
                            currVType = GetRankDifference(curr);
                    }

                    else if (childVType[0] == 1)
                    {
                        RotateRight(curr, false);
                        return;
                    }

                    else
                    {
                        DoubleRotateRight(curr, false);
                        return;
                    }
                }
            }
            return;
        }

        public Node<ValueType> DeleteLeaf(Node<ValueType> node)
        {
            Node<ValueType> parent = node.Parent;

            if (parent != null)
            {
                if (node.Key >= parent.Key)
                {
                    parent.Right = null;
                }
                else
                {
                    parent.Left = null;
                }
                node.Parent = null;
            }
            else if (Root == node)
            {
                Root = null;
            }

            return parent;
        }

        public ValueType Min()
        {
            if (Root == null)
            {
                return default;
            }
            return NodeWithMinKey(Root).Value;
        }

        public Node<ValueType> NodeWithMinKey(Node<ValueType> node)
        {
            if (node == null)
                return null;

            Node<ValueType> curr = node;
            while (curr.Left != null)
            {
                curr = curr.Left;
            }
            return curr;
        }

        public ValueType Max()
        {
            if (Root == null)
                return default;

            Node<ValueType> curr = Root;
            while (curr.Right != null)
            {
                curr = curr.Right;
            }
            return curr.Value;
        }

        public List<Node<ValueType>> InorderWalk()
        {
            var nodes = new List<Node<ValueType>>();

            Node<ValueType> iterate = null;
            for (int i = 0; i < Root?.SubtreeSize; i++)
            {
                iterate = Next(iterate);
                nodes.Add(iterate);
            }

            return nodes;
        }

        public Node<ValueType> Next(Node<ValueType> curr)
        {
            if (curr == null)
            {
                return NodeWithMinKey(Root);
            }
            else
            {
                if (curr.Right != null)
                {
                    return NodeWithMinKey(curr.Right);
                }
                else
                {
                    Node<ValueType> parent = curr.Parent;

                    while (parent != null && curr == parent.Right)
                    {
                        parent = parent.Parent;
                    }
                    return parent;
                }
            }
        }

        public Node<ValueType> Successor(Node<ValueType> root)
        {
            Node<ValueType> previous = null;
            Node<ValueType> current = root.Right;
            while (current != null)
            {
                previous = current;
                current = current.Left;
            }

            return previous;
        }

        public Node<ValueType> Predecessor(Node<ValueType> root)
        {
            Node<ValueType> previous = null;
            Node<ValueType> current = root.Left;
            while (current != null)
            {
                NodeCounter?.Invoke();
                previous = current;
                current = current.Right;
            }

            return previous;
        }
    }
}