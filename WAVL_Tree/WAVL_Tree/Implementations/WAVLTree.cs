using System.Collections.Generic;
using WAVL_Tree.Models;


namespace WAVL_Tree.Implementations
{
    public class WAVLTree
    {
        public WAVLNode Root;

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
            return (Root == null);
        }

        /**
         * Searches the tree for key k and returns its value, or null if k isn't in
         * the tree.<br>
         *
         * Used as a wrapper function that passes k and the tree root to findNode,
         * another, more general search function, findNode, to search each subtree along the path
         * from the root to the required key.<br>
         *
         * This method runs in O(h) = O(logn) time as it traverses a simple path
         * from the root to the deepest leaf in the worst case.
         *
         * @param k         the key being searched
         * @return          value associated with key k, or null if k is not
         *                  in the tree.
         */
        public string Search(int k)
        {
            WAVLNode found = FindNode(k, Root);
            if (found != null)
                return found.Value;
            return null;
        }

        /**
         * Search function with an additional node parameter. Returns a node with the
         * required key, or null if there is no such key in the tree. <br>
         *
         * This method utilizes the BST property of the WAVL tree to move recursively
         * from a root node to one of its subtrees, according to k and the key held at
         * the node.<br>
         *
         * This method runs in O(h) = O(logn) time as it traverses a simple path
         * from the root to the deepest leaf in the worst case.
         *
         * @param k         the key being searched
         * @param node      node at the root of the subtree in which k is searched
         * @return          node with key k, or null if k is not in the tree.
         */
        private WAVLNode FindNode(int k, WAVLNode node)
        {
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

        /**
         * Inserts new node, with provided key and value, to the tree. <br>
         *
         * If a node with key k already exists in the tree, -1 will be returned. Otherwise,
         * the function returns the number of rank changes, rotations and double rotations
         * required in order to maintain the WAVL tree invariants, that may have been violated
         * during insertion.<br>
         *
         * This method runs in O(h) = O(logn) time in the worst case. The runtime complexity
         * will be determined by the time it takes to find the insertion place, the time
         * complexity of the rebalancing process, and the time it takes to update the subtree
         * sizes of the nodes that have been affected by the stuctural changes in the tree.
         * All of these processes will involve all nodes along the path from the deepest leaf
         * in the tree to the root in the worst case so they run in O(h) = O(logn), hence the
         * time complexity of insertion will be O(logn) as well.
         *
         * @param k     the key of the new node added to the tree
         * @param i     the info of the new node added to the tree
         * @return      the number of rebalancing operations performed during the rebalancing
         *              process, or -1 if a node with key k already exists in the tree
         */
        public int Insert(int k, string i)
        {
            /* Step 1: Check if tree is empty. If it is, insert new node as root and finish */
            if (Root == null)
            {
                Root = new WAVLNode(k, i);
                return 0;
            }

            /* Step 2: Check if key k is already in the tree, or where to insert it if it
            isn't, using the findInsertionPlace method. */
            WAVLNode parentNode = FindInsertionPlace(k, Root);
            if (parentNode.Key == k)
                return -1;

            /* Step 3: Place new node in appropriate place.*/
            WAVLNode newNode = new WAVLNode(k, i, parentNode);

            /* Step 4: Rebalance */
            return InsertionRebalance(parentNode);
        }

        /**
         * Finds the appropriate parent node for a new node with key k to be inserted,
         * or an existing node with key k. <br>
         *
         * This method utilizes the BST property of the WAVL tree to move recursively
         * from a root node to one of its subtrees, according to k and the key held at
         * the node.<br>
         *
         * It searches the tree to find the unary node or the leaf which will be the
         * parent of the new node with key k. If the node returned has key k, then the
         * new node should not be reinserted. <br>
         *
         * This function runs in O(h) = O(logn) time as it traverses a path from the root
         * to the deepest leaf in the worst case.
         *
         * @param k         key of the new node inserted
         * @param node      node at the root of the subtree in which the insertion place
         *                  is searched
         * @return          the node to which the node inserted is assigned to as
         *                  its left or right child
         *
         */
        private WAVLNode FindInsertionPlace(int k, WAVLNode node)
        {
            if (k == node.Key)
                return node;
            else if (k > node.Key)
            {
                if (node.Right == null)
                    /* Node is free to accept new node with key k as its right child */
                    return node;
                else
                    /* Node has a right subtree, hence new node with key k will be placed there */
                    return FindInsertionPlace(k, node.Right);
            }
            else
            {
                if (node.Left == null)
                    /* Node is free to accept new node with key k as its left child */
                    return node;
                else
                    /* Node has a left subtree, hence new node with key k will be placed there */
                    return FindInsertionPlace(k, node.Left);
            }
        }

        /**
         * Characterizes the type of node by the rank differences with its children.<br>
         *
         * If the node is a unary node of a leaf, then the missing child is referred to as an external
         * node with rank -1. The function returns a size 2 array containing the rank differences,
         * left to right. This allows to determine the course of action during the rebalancing process.<br>
         *
         * This function runs in O(1) time as it only requires access to pointers, and the creation
         * of a fixed sized array.
         *
         * @param node      the node of which the rank differences are returned
         * @return          a size 2 array: array[0] - rank difference between the node
         *                  and its left child; array[1] - rank difference between the node
         *                  and its right child
         */
        private int[] VertexType(WAVLNode node)
        {
            int[] rankDiffs = new int[2];

            if (node.Left != null)
                rankDiffs[0] = node.Rank - node.Left.Rank;
            else
                rankDiffs[0] = node.Rank - (-1);

            if (node.Right != null)
                rankDiffs[1] = node.Rank - node.Right.Rank;
            else
                rankDiffs[1] = node.Rank - (-1);

            return rankDiffs;
        }

        /**
         * Receives a size 2 array of rank differences produced by the vertexType function,
         * and checks if the vertex type is valid according to the WAVL rules. <br>
         *
         * Returns a boolean value - true if the vertex type is one of the following: {(1,1),
         * (1,2), (2,1), (2,2)}, and false otherwise, implying rebalancing is required. <br>
         *
         * This function runs in O(1) time as it iterates on a fixed sized array.
         *
         * @param vType     a size 2 array of rank differences between a certain node in the tree
         *                  and its two children
         * @return          true if vertex type is in accordance with the WAVL invariants, false
         *                  otherwise
         */
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

        /**
         * Rebalances WAVL tree after insertion. <br>
         *
         * Receives a tree node where a violation of the WAVL tree invariants may have occurred
         * as a result of an insertion, and corrects it by performing a series of rebalancing actions
         * up the path from the given node to the root of the tree.<br>
         *
         * The function runs in O(h) = O(logn) time, as its runtime complexity is determined
         * by the number of promotions O(logn) promotions in the worst case), a the subtree size update
         * process, which takes place as a result of the structural changes in the tree.
         * The subtree size update traverses a path from the deepest leaf in the tree to the root
         * in the worst case, hence it runs in O(logn) time.
         *
         * @param node      WAVL node object where there is a violation of the WAVL tree invariants
         * @return          number of promotions, rotations and double rotations performed
         *                  during the rebalancing process
         */
        private int InsertionRebalance(WAVLNode node)
        {
            int counter = 0;
            WAVLNode curr = node;
            int[] currVType = VertexType(node);

            while (curr != null && !IsValidType(currVType))
            { // curr != null: Haven't reached the root yet

                if (currVType[0] + currVType[1] == 1)
                { // (0,1), (1,0) Promotion cases
                    counter++;
                    curr.Rank++;
                    curr = curr.Parent;
                    if (curr != null)
                        currVType = VertexType(curr);
                    continue;
                }
                // Rotation cases

                if (currVType[0] == 0)
                { // Rolling up from the left

                    int[] childVType = VertexType(curr.Left);

                    if (childVType[0] == 1 && childVType[1] == 2)
                    { // child is (1,2)
                        counter += RotateRight(curr, true);
                    }
                    else
                    {
                        counter += DoubleRotateRight(curr, true);
                    }

                }
                else if (currVType[1] == 0)
                { // Rolling up from the right

                    int[] childVType = VertexType(curr.Right);

                    if (childVType[0] == 2 && childVType[1] == 1)
                    { //child is (2,1)
                        counter += RotateLeft(curr, true);
                    }
                    else
                    {
                        counter += DoubleRotateLeft(curr, true);
                    }
                }
                return counter;
            }
            return counter;
        }


        /**
         * Performs a single rotation to the right of the subtree of which the node
         * provided is the root. <br>
         *
         * Illustration below is in accordance with variable names:
         *
         *        p                    p
         *        |                    |
         *        z                    x
         *      /  \  Rotate right   /  \
         *     x   y                a   z
         *    / \                      / \
         *   a  b                     b  y
         *
         * During insertion, node z is demoted during this rotation. All other nodes retain their ranks. <br>
         *
         * During deletion, node x is promoted, and z is demoted once or twice, depending
         * on z's children. If z becomes a leaf after the rotation and is only demoted once,
         * its rank difference with the external leaf y becomes 2, causing a violation of
         * the invariants, as leaves are only allowed to be (1,1) vertices.
         * To solve this, z is demoted once again. All other nodes retain their ranks.<br>
         *
         * The function runs in O(h) = O(logn) time, as its runtime complexity is determined
         * by the subtree size update process, which takes place as a result of the structural
         * changes in the tree. The rotation process itself runs in O(1) time as it only involves
         * a fixed number of pointers and fields. The subtree size update traverses a path from the
         * deepest leaf in the tree to the root in the worst case, hence it runs in O(logn) time.
         *
         * @param z         the node at the root of the subtree rotated
         * @param insert    true if the rotation takes place during insertion, false if it does
         *                  during deletion
         * @return          the number of rebalancing step made during this process -
         *                  1 rotation + the number of promotions and demotions made
         */

        private int RotateRight(WAVLNode z, bool insert)
        {
            int counter = 1;

            WAVLNode x = z.Left;
            WAVLNode b = x.Right;

            z.ReplaceWith(x, ref Root);

            x.SetRightChild(z);
            z.SetLeftChild(b);

            if (insert)
            {
                z.Rank--;
                counter++;
            }
            else
            {
                z.Rank--;
                x.Rank++;
                counter += 2;
                if (z.IsLeaf())
                {
                    z.Rank--;
                    counter++;
                }
            }
            return counter;
        }

        /**
         * Performs a single rotation to the left of the subtree of which the node
         * provided is the root. <br>
         *
         * Illustration below is in accordance with variable names:
         *
         *     p                     p
         *     |                     |
         *     z                     y
         *   /  \   Rotate left    /  \
         *  x   y                 z   b
         *     / \               / \
         *    a  b              x  a
         *
         * During insertion, node z is demoted during this rotation. All other nodes retain their ranks<br>
         *
         * During deletion, node y is promoted, and z is demoted once or twice, depending
         * on z's children. If z becomes a leaf after the rotation and is only demoted once,
         * its rank difference with the external leaf x becomes 2, causing a violation of
         * the invariants, as leaves are only allowed to be (1,1) vertices.
         * To solve this, z is demoted once again. All other nodes retain their ranks.<br>
         *
         * The function runs in O(h) = O(logn) time, as its runtime complexity is determined
         * by the subtree size update process, which takes place as a result of the structural
         * changes in the tree. The rotation process itself runs in O(1) time as it only involves
         * a fixed number of pointers and fields. The subtree size update traverses a path from the
         * deepest leaf in the tree to the root in the worst case, hence it runs in O(logn) time.
         *
         * @param z         the node at the root of the subtree rotated
         * @param insert    true if the rotation takes place during insertion, false if it does
         *                  during deletion
         * @return          the number of rebalancing step made during this process -
         *                  1 rotation + the number of promotions and demotions made
         */

        private int RotateLeft(WAVLNode z, bool insert)
        {
            int counter = 1;

            WAVLNode y = z.Right;
            WAVLNode a = y.Left;

            z.ReplaceWith(y, ref Root);

            y.SetLeftChild(z);
            z.SetRightChild(a);


            if (insert)
            {
                z.Rank--;
                counter++;
            }
            else
            {
                z.Rank--;
                y.Rank++;
                counter += 2;
                if (z.IsLeaf())
                {
                    z.Rank--;
                    counter++;
                }
            }
            return counter;
        }

        /**
         * Performs a double rotation to the right of the subtree of which the node
         * provided is the root. <br>
         *
         * Illustration below is in accordance with variable names:
         *
         *      p                            p
         *      |                            |
         *      z                            b
         *     / \                         /   \
         *    x  y  Double rotate right   x    z
         *   / \                         / \  / \
         *  a  b                        a  c d  y
         *    / \
         *   c  d
         *
         * During insertion, nodes y and z are demoted, and node a is promoted during this
         * rotation. All other nodes retain their ranks.<br>
         *
         * During deletion, the rank of node b increases by 2, that of node z decreases by 2,
         * and that of x decreases by 1. All other nodes retains their ranks. <br>
         *
         * The function runs in O(h) = O(logn) time, as its runtime complexity is determined
         * by the subtree size update process, which takes place as a result of the structural
         * changes in the tree. The rotation process itself runs in O(1) time as it only involves
         * a fixed number of pointers and fields. The subtree size update traverses a path from the
         * deepest leaf in the tree to the root in the worst case, hence it runs in O(logn) time.
         *
         * @param z         the node at the root of the subtree rotated
         * @param insert    true if the rotation takes place during insertion, false if it does
         *                  during deletion
         * @return          the number of rebalancing step made during this process -
         *                  1 rotation + the number of promotions and demotions made
         *
         */

        public int DoubleRotateRight(WAVLNode z, bool insert)
        {
            int counter = 1;

            WAVLNode x = z.Left;
            WAVLNode b = x.Right;
            WAVLNode c = b.Left;
            WAVLNode d = b.Right;

            z.ReplaceWith(b, ref Root);

            b.SetLeftChild(x);
            b.SetRightChild(z);
            x.SetRightChild(c);
            z.SetLeftChild(d);

            if (insert)
            {
                x.Rank--;
                z.Rank--;
                b.Rank++;
                counter += 3;
            }
            else
            {
                b.Rank += 2;
                z.Rank -= 2;
                x.Rank--;
                counter += 3;
            }
            return counter;
        }

        /**
         * Performs a double rotation to the right of the subtree of which the node
         * provided is the root. <br>
         *
         * Illustration below is in accordance with variable names:
         *
         *    p                            p
         *    |                            |
         *    z                            a
         *   / \                         /   \
         *  x  y   Double rotate left   z    y
         *    / \                      / \  / \
         *   a  b                     x  c d  b
         *  / \
         * c  d
         *
         * During insertion, nodes y and z are demoted, and node a is promoted during this
         * rotation. All other nodes retain their ranks.<br>
         *
         * During deletion, the rank of node a increases by 2, that of node z decreases by 2,
         * and that of y decreases by 1. All other nodes retains their ranks. <br>
         *
         * The function runs in O(h) = O(logn) time, as its runtime complexity is determined
         * by the subtree size update process, which takes place as a result of the structural
         * changes in the tree. The rotation process itself runs in O(1) time as it only involves
         * a fixed number of pointers and fields. The subtree size update traverses a path from the
         * deepest leaf in the tree to the root in the worst case, hence it runs in O(logn) time.
         *
         * @param z         the node at the root of the subtree rotated
         * @param insert    true if the rotation takes place during insertion, false if it does
         *                  during deletion
         * @return          the number of rebalancing step made during this process -
         *                  1 rotation + the number of promotions and demotions made
         *
         */
        public int DoubleRotateLeft(WAVLNode z, bool insert)
        {
            int counter = 1;

            WAVLNode y = z.Right;
            WAVLNode a = y.Left;
            WAVLNode c = a.Left;
            WAVLNode d = a.Right;

            z.ReplaceWith(a, ref Root);

            a.SetLeftChild(z);
            a.SetRightChild(y);
            z.SetRightChild(c);
            y.SetLeftChild(d);

            if (insert)
            {
                y.Rank--;
                z.Rank--;
                a.Rank++;
            }
            else
            {
                a.Rank += 2;
                z.Rank -= 2;
                y.Rank--;
                counter += 3;
            }
            return counter;
        }

        /**
         * Removes node with provided key from the tree. <br>
         *
         * If a node with key k does not exist in the tree, -1 will be returned. Otherwise,
         * the function returns the number of rank changes, rotations and double-rotations
         * required in order to maintain the WAVL tree invariants, that may have been violated
         * during deletion.<br>
         *
         * Note that all deletion cases are reduced to the deletion of a leaf: a binary node
         * or a unary node deleted, takes the key and value of its successor / predecessor
         * (as they are available), and the latter is then removed from the tree. If it's a unary
         * node as well, it takes the key and value of its leaf child, and the leaf child is
         * deleted. <br>
         *
         * This method runs in O(h) = O(logn) time in the worst case. The runtime complexity
         * will be determined by the time it takes to find the node to delete, the time
         * complexity of the rebalancing process. Both of these processes run in O(logn) time
         * so the deletion process runs in O(logn) as well.
         *
         * @param k     key of node to be removed from the tree, if it exists in it
         * @return      the number of rebalancing operations performed during the rebalancing
         *              process, or -1 if node with key k was not found in the tree
         */
        public int Delete(int k)
        {
            /* Step 1: Check if node with key k we wish to remove is in the tree, and find it
            * if it is */
            WAVLNode node = FindNode(k, Root);
            if (node == null)
                return -1;

            /* Step 2: Delete node */
            WAVLNode offender; // Node where WAVL invariant violation may have occurred

            /* Case a: Deleting a leaf */
            if (node.IsLeaf())
            { // leaf
                offender = DeleteLeaf(node);
            }
            /* Case b: Deleting a unary node with right child or a binary node */
            else if (node.Right != null)
            {
                /* Key and value of deleted node are replaced by the successor's, retaining all
                other info */
                WAVLNode successor = NodeWithMinKey(node.Right);

                /*      - b.1. Can happen in case of deleting a unary node with right child who
                         is necessarily a leaf, or in case of deleting a binary node whose
                         successor is a leaf */
                if (successor.IsLeaf())
                {
                    offender = DeleteLeaf(successor);
                    node.Key = successor.Key;
                    node.Value = successor.Value;
                }

                /*      - b.2. Can happen in case of deleting a binary node, whose successor is
                          unary node with right child, who is necessarily a leaf */
                else
                {
                    node.Key = successor.Key;
                    node.Value = successor.Value;
                    successor.Key = successor.Right.Key;
                    successor.Value = successor.Right.Value;
                    offender = DeleteLeaf(successor.Right);
                }
            }
            /* Case c: Deleting a unary node with left child, who is necessarily a leaf*/
            else
            {
                int newKey = node.Left.Key;
                string newVal = node.Left.Value;
                offender = DeleteLeaf(node.Left);
                node.Key = newKey;
                node.Value = newVal;
            }

            /* Step 3: Rebalance */
            return DeletionRebalance(offender);
        }

        /**
         * Rebalances WAVL tree after insertion. <br>
         *
         * Receives a tree node where a violation of the WAVL tree invariants may have occurred
         * as a result of a deletion, and corrects it by performing a series of rebalancing operations
         * up the path from the given node to the root of the tree.<br>
         *
         * The function runs in O(h) = O(logn) time, as its runtime complexity is determined
         * by the number of promotions (O(logn) promotions in the worst case), a the subtree size update
         * process, which takes place as a result of the structural changes in the tree.
         * The subtree size update traverses a path from the deepest leaf in the tree to the root
         * in the worst case, hence it runs in O(logn) time.
         *
         * @param node      WAVL node object where there is a violation of the WAVL tree invariants
         * @return          number of promotions, rotations and double rotations performed
         *                  during the rebalancing process
         */
        private int DeletionRebalance(WAVLNode node)
        {
            int counter = 0;
            WAVLNode curr = node;

            if (curr == null)
            { // Empty tree
                return 0;
            }

            int[] currVType = VertexType(node);

            // Check if offender is a leaf with non zero rank
            if (curr.IsLeaf() && curr.Rank != 0)
            {
                curr.Rank = 0;
                counter++;
                curr = curr.Parent;
                if (curr != null)
                    currVType = VertexType(curr);
            }

            while (curr != null && !IsValidType(currVType))
            {
                counter++;

                if (currVType[0] + currVType[1] == 5)
                { // (3,2), (2,3) Demotion Cases
                    curr.Rank--;
                    curr = curr.Parent;
                    if (curr != null)
                        currVType = VertexType(curr);
                    continue;
                }

                if (currVType[0] == 3)
                { // Rolling up from the right

                    int[] childVType = VertexType(curr.Right);

                    if (currVType[1] == 1 && childVType[0] + childVType[1] == 4)
                    { // child is (2,2) - Double demote
                        curr.Rank--;
                        curr.Right.Rank--;
                        counter += 2; // Two demotions
                        curr = curr.Parent;
                        if (curr != null)
                            currVType = VertexType(curr);
                    }
                    else if (childVType[1] == 1)
                    { // child is (1,1) or (2,1)
                        counter += RotateLeft(curr, false);
                        return counter;

                    }

                    else
                    {
                        counter += DoubleRotateLeft(curr, false);
                        return counter;
                    }

                }
                else if (currVType[1] == 3)
                { // Rolling up from the left

                    int[] childVType = VertexType(curr.Left);

                    if (currVType[0] == 1 && childVType[0] + childVType[1] == 4)
                    { // child is (2,2) - Double demote
                        curr.Rank--;
                        curr.Left.Rank--;
                        counter += 2;
                        curr = curr.Parent;
                        if (curr != null)
                            currVType = VertexType(curr);
                    }

                    else if (childVType[0] == 1)
                    { // child is (1,1) or (1,2)
                        counter += RotateRight(curr, false);
                        return counter;
                    }

                    else
                    {
                        counter += DoubleRotateRight(curr, false);
                        return counter;
                    }
                }
            }
            return counter;
        }



        /**
         * Deletes a leaf node by placing a null value in its parent's appropriate child field,
         * and a null value in its parent field. Returns the deleted node's parent. <br>
         *
         * That way, it becomes an isolated node object. With no reference left to it, the object
         * is eventually removed by the garbage collector.
         *
         * This method runs in O(1) time, as it only involves resetting a fixed number of pointers.
         *
         * @param node      leaf to be removed from the tree
         * @return          its parent node, or null in case the node deleted was the root
         *                  of a size 1 tree
         */
        public WAVLNode DeleteLeaf(WAVLNode node)
        {

            WAVLNode parent = node.Parent;

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
            else if (this.Root == node)
            {
                this.Root = null;
            }

            return parent;
        }

        public string Min()
        {
            if (Root == null)
            {
                return null;
            }
            return NodeWithMinKey(Root).Value;
        }

        /**
         * Returns the node with the minimal key in the tree, or null if the tree is empty.
         *
         * As a result of the BST property of the WAVL tree, the node with the smallest
         * key in the tree will always be at the leftmost node. The function traverses the
         * leftmost path in the tree until it is exhausted, i.e. when there is no longer
         * any node to the left, then returns the node at the end of it.
         *
         * @param node  the node at the root of the subtree of which the node with minimal key
         *              is required
         * @return      the node with the smallest key in the tree
         */
        public WAVLNode NodeWithMinKey(WAVLNode node)
        {
            if (node == null)
                return null;

            WAVLNode curr = node;
            while (curr.Left != null)
            {
                curr = curr.Left;
            }
            return curr;
        }

        public string Max()
        {
            if (Root == null)
                return null;

            WAVLNode curr = Root;
            while (curr.Right != null)
            {
                curr = curr.Right;
            }
            return curr.Value;
        }

        public List<int> KeysToArray()
        {
            var sortedKeys = new List<int>();

            WAVLNode iterate = null;
            for (int i = 0; i < Root.SubtreeSize; i++)
            {
                iterate = Next(iterate);
                sortedKeys.Add(iterate.Key);
            }


            return sortedKeys;


        }

        public List<string> InfoToArray()
        {
            if (Root == null)
            {
                return new List<string>();
            }
            var sortedKeys = new List<string>();

            WAVLNode iterate = null;
            for (int i = 0; i < Root.SubtreeSize; i++)
            {
                iterate = Next(iterate);
                sortedKeys.Add(iterate.Value);
            }


            return sortedKeys;


        }

        public WAVLNode Next(WAVLNode curr)
        {
            /* If this is the first next call, set curr to be the minimal node in the tree */
            if (curr == null)
            {
                return NodeWithMinKey(Root);
            }

            else
            {

                /* Case 1: The current node's successor is in its subtree. In that case, it's
                 * going to be at the node with the smallest key in curr's right subtree. */

                if (curr.Right != null)
                {
                    return NodeWithMinKey(curr.Right);
                }

                /* Case 2: The current node's successor is not in its subtree. In that case,
                * it's going to be the lowest ancestor of curr, whose left child is also an
                * ancestor of curr. */

                else
                {
                    WAVLNode parent = curr.Parent;

                    while (parent != null && curr == parent.Right)
                    {
                        parent = parent.Parent;
                    }
                    return parent;
                }
            }
        }
    }
}