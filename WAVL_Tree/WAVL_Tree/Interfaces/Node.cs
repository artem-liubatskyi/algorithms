
namespace WAVL_Tree.Interfaces
{
    public interface Node<ValueType>
    {
        int Key { get; set; }
        ValueType Value { get; set; }
        Node<ValueType> Left { get; set; }
        Node<ValueType> Right { get; set; }
        Node<ValueType> Parent { get; set; }
        public int SubtreeSize { get; set; }
        public int Rank { get; set; }

        bool IsLeaf();
        void SetRightChild(Node<ValueType> node);
        void SetLeftChild(Node<ValueType> node);
        void ReplaceWith(Node<ValueType> replacer, ref Node<ValueType> root);
        void UpdateSubtreeSize();
    }
}
