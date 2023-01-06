using System;
using System.Collections.Generic;
using WAVL_Tree.Interfaces;
using WAVL_Tree.Models;

namespace WAVL_Tree.Implementations
{
    public class BSTree<ValueType> : IBinaryTree<ValueType>
    {
        public Node<ValueType> Root;

        public event Counter NodeCounter;
        public event RotationCounter RotationCounter;

        public void Delete(int k)
        {
            Node<ValueType> previous = null;
            Node<ValueType> current = Root;

            while (current != null)
            {
                NodeCounter?.Invoke();
                if (current.Key == k)
                    break;
                else if (current.Key > k)
                {
                    previous = current;
                    current = current.Left;
                }
                else
                {
                    previous = current;
                    current = current.Right;
                }
            }

            if (current == null)
                return;
            else
                DeleteNode(current, previous);
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

        public void Insert(int k, ValueType value)
        {
            var node = new WAVLNode<ValueType>(k, value);
            InsertNode(node);
        }

        private void InsertNode(Node<ValueType> node)
        {
            if (Root == null)
            {
                Root = node;
            }
            else
            {
                Node<ValueType> current = Root;
                while (true)
                {
                    NodeCounter?.Invoke();
                    if (current.Key == node.Key)
                    {
                        break;
                    }
                    else if (current.Key > node.Key)
                    {
                        if (current.Left != null)
                        {
                            current = current.Left;
                        }
                        else
                        {
                            current.Left = node;
                            break;
                        }
                    }
                    else
                    {
                        if (current.Right != null)
                        {
                            current = current.Right;
                        }
                        else
                        {
                            current.Right = node;
                            break;
                        }
                    }
                }
            }
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

        public int Size()
        {
            throw new NotImplementedException();
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

        private void DeleteNode(Node<ValueType> node, Node<ValueType> parentNode)
        {
            NodeCounter?.Invoke();
            if (Root.Key == node.Key)
            {
                if (Root.IsLeaf())
                {
                    Root = null;
                }
                else if (Root.Left == null)
                {
                    Root = Root.Right;
                }
                else if (Root.Right == null)
                {
                    Root = Root.Left;
                }
                else
                {
                    Node<ValueType> swapNode = Predecessor(Root);

                    var tempKey = swapNode.Key;
                    Delete(swapNode.Key);
                    Root.Key = tempKey;
                }
            }
            else if (node.IsLeaf())
            {
                if (parentNode != null)
                {
                    if (node.Key < parentNode.Key)
                        parentNode.Left = null;
                    else
                        parentNode.Right = null;
                }
            }
            else if (node.Right == null)
            {
                if (parentNode != null)
                {
                    if (node.Key < parentNode.Key)
                        parentNode.Left = node.Left;
                    else
                        parentNode.Right = node.Left;
                }
            }
            else if (node.Left == null)
            {
                if (parentNode != null)
                {
                    if (node.Key < parentNode.Key)
                        parentNode.Left = node.Right;
                    else
                        parentNode.Right = node.Right;
                }
            }
            else
            {
                Node<ValueType> swapNode = Predecessor(node);

                var tempKey = swapNode.Key;
                Delete(swapNode.Key);
                node.Key = tempKey;
            }
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
    }
}
