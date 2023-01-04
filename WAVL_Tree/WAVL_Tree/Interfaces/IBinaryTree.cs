using System.Collections.Generic;

namespace WAVL_Tree.Interfaces
{

    public delegate void Counter();
    public delegate void RotationCounter(int count);
    public interface IBinaryTree<ValueType>
    {
        public event Counter NodeCounter;
        public event RotationCounter RotationCounter;

        ValueType Search(int k);
        void Insert(int k, ValueType i);
        void Delete(int k);

        int Size();

        List<Node<ValueType>> InorderWalk();

        Node<ValueType> Successor(Node<ValueType> root);

        Node<ValueType> Predecessor(Node<ValueType> root);

    }
}
