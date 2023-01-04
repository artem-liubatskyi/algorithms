namespace WAVL_Tree.Models
{
    public class WAVLNode
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public WAVLNode Left { get; set; }
        public WAVLNode Right { get; set; }
        public WAVLNode Parent { get; set; }
        public int SubtreeSize { get; set; }
        public int Rank { get; set; }

        public WAVLNode(int key, string value)
        {
            Key = key;
            Value = value;
            SubtreeSize = 1;
        }

        public WAVLNode(int key, string value, WAVLNode parent)
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

        public void SetRightChild(WAVLNode node)
        {
            Right = node;
            if (node != null)
            {
                node.Parent = this;
            }

            UpdateSubtreeSize();
        }

        public void SetLeftChild(WAVLNode node)
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

        public void ReplaceWith(WAVLNode replacer, ref WAVLNode root)
        {

            WAVLNode parent = Parent;
            WAVLNode repParent = replacer.Parent;

            /* Step 1: Remove replacer as child of its parent */
            if (repParent != null)
            {
                if (replacer.Key > repParent.Key)
                    repParent.Right = null;
                else
                    repParent.Left = null;

                repParent.UpdateSubtreeSize();
            }

            /* Step 2: Set replacer as child of the replaced node, doubly linked */
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
