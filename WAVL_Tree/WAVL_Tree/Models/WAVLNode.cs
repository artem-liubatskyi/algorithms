using WAVL_Tree.Interfaces;

namespace WAVL_Tree.Models
{
    public class WAVLNode<ValueType> : Node<ValueType>
    {
        public int Key { get; set; }
        public ValueType Value { get; set; }
        public Node<ValueType> Left { get; set; }
        public Node<ValueType> Right { get; set; }
        public Node<ValueType> Parent { get; set; }
        public int SubtreeSize { get; set; }
        public int Rank { get; set; }

        public WAVLNode(int key, ValueType value)
        {
            Key = key;
            Value = value;
            SubtreeSize = 1;
        }

        public WAVLNode(int key, ValueType value, Node<ValueType> parent)
        {
            Rank = 0;
            Key = key;
            Value = value;
            if (key > parent.Key)
                parent.SetRightChild(this);
            else
                parent.SetLeftChild(this);

            parent.UpdateSubtreeSize();

        }


        public bool IsLeaf()
        {
            return (Left == null && Right == null);
        }

        public void SetRightChild(Node<ValueType> node)
        {
            Right = node;
            if (node != null)
            {
                node.Parent = this;
            }

            UpdateSubtreeSize();
        }

        public void SetLeftChild(Node<ValueType> node)
        {

            Left = node;
            if (node != null)
            {
                node.Parent = this;
            }

            UpdateSubtreeSize();
        }

        public void UpdateSubtreeSize()
        {

            SubtreeSize = 1 +
                    (Right == null ? 0 : Right.SubtreeSize) +
                    (Left == null ? 0 : Left.SubtreeSize);

            if (Parent != null)
            {
                Parent.UpdateSubtreeSize();
            }
        }

        public void ReplaceWith(Node<ValueType> replacer, ref Node<ValueType> root)
        {
            Node<ValueType> parent = Parent;
            Node<ValueType> repParent = replacer.Parent;

            if (repParent != null)
            {
                if (replacer.Key > repParent.Key)
                    repParent.Right = null;
                else
                    repParent.Left = null;

                repParent.UpdateSubtreeSize();
            }

            if (parent != null)
            {
                if (Key < parent.Key)
                {
                    parent.SetLeftChild(replacer);
                }
                else
                {
                    parent.SetRightChild(replacer);
                }
            }
            else
            {
                replacer.Parent = null;
                root = replacer;
                root.UpdateSubtreeSize();
            }
        }
    }
}
