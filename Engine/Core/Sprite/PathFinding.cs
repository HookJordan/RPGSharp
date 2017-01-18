//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Core.Sprite
{
    public interface IPriorityQueue<T>
    {
        #region Methods
        int Push(T item);
        T Pop();
        T Peek();
        void Update(int i);
        #endregion
    }
    public class PriorityQueueB<T> : IPriorityQueue<T>
    {
        #region Variables Declaration
        protected List<T> InnerList = new List<T>();
        protected IComparer<T> mComparer;
        #endregion

        #region Contructors
        public PriorityQueueB()
        {
            mComparer = Comparer<T>.Default;
        }

        public PriorityQueueB(IComparer<T> comparer)
        {
            mComparer = comparer;
        }

        public PriorityQueueB(IComparer<T> comparer, int capacity)
        {
            mComparer = comparer;
            InnerList.Capacity = capacity;
        }
        #endregion

        #region Methods
        protected void SwitchElements(int i, int j)
        {
            T h = InnerList[i];
            InnerList[i] = InnerList[j];
            InnerList[j] = h;
        }

        protected virtual int OnCompare(int i, int j)
        {
            return mComparer.Compare(InnerList[i], InnerList[j]);
        }

        /// <summary>
        /// Push an object onto the PQ
        /// </summary>
        /// <param name="O">The new object</param>
        /// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
        public int Push(T item)
        {
            int p = InnerList.Count, p2;
            InnerList.Add(item); // E[p] = O
            do
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            return p;
        }

        /// <summary>
        /// Get the smallest object and remove it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Pop()
        {
            T result = InnerList[0];
            int p = 0, p1, p2, pn;
            InnerList[0] = InnerList[InnerList.Count - 1];
            InnerList.RemoveAt(InnerList.Count - 1);
            do
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);

            return result;
        }

        /// <summary>
        /// Notify the PQ that the object at position i has changed
        /// and the PQ needs to restore order.
        /// Since you dont have access to any indexes (except by using the
        /// explicit IList.this) you should not call this function without knowing exactly
        /// what you do.
        /// </summary>
        /// <param name="i">The index of the changed object.</param>
        public void Update(int i)
        {
            int p = i, pn;
            int p1, p2;
            do  // aufsteigen
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            if (p < i)
                return;
            do     // absteigen
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);
        }

        /// <summary>
        /// Get the smallest object without removing it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Peek()
        {
            if (InnerList.Count > 0)
                return InnerList[0];
            return default(T);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public void RemoveLocation(T item)
        {
            int index = -1;
            for (int i = 0; i < InnerList.Count; i++)
            {

                if (mComparer.Compare(InnerList[i], item) == 0)
                    index = i;
            }

            if (index != -1)
                InnerList.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return InnerList[index]; }
            set
            {
                InnerList[index] = value;
                Update(index);
            }
        }
        #endregion
    }

    public struct PathFinderNode
    {
        #region Variables Declaration
        public int F;
        public int G;
        public int H;  // f = gone + heuristic
        public int X;
        public int Y;
        public int PX; // Parent
        public int PY;
        #endregion
    }

    public enum PathFinderNodeType
    {
        Start = 1,
        End = 2,
        Open = 4,
        Close = 8,
        Current = 16,
        Path = 32
    }
    public enum HeuristicFormula
    {
        Manhattan = 1,
        MaxDXDY = 2,
        DiagonalShortCut = 3,
        Euclidean = 4,
        EuclideanNoSQR = 5,
        Custom1 = 6
    }

    public class PathFinderv2
    {
        private byte[,] mGrid = null;
        private PriorityQueueB<PathFinderNode> mOpen = new PriorityQueueB<PathFinderNode>(new ComparePFNode());
        private List<PathFinderNode> mClose = new List<PathFinderNode>();
        private bool mStop = false;
        private bool mStopped = true;
        private int mHoriz = 0;
        private HeuristicFormula mFormula = HeuristicFormula.Manhattan;
        private bool mDiagonals = false;
        private int mHEstimate = 2;
        private bool mPunishChangeDirection = true;
        private bool mTieBreaker = false;
        private bool mHeavyDiagonals = false;
        private int mSearchLimit = 2000;
        private double mCompletedTime = 0;
        private bool mDebugProgress = false;
        private bool mDebugFoundPath = false;

        public PathFinderv2(byte[,] grid)
        {
            if (grid == null)
                throw new Exception("Grid cannot be null");

            mGrid = grid;
        }

        #region Properties
        public bool Stopped
        {
            get { return mStopped; }
        }

        public HeuristicFormula Formula
        {
            get { return mFormula; }
            set { mFormula = value; }
        }

        public bool Diagonals
        {
            get { return mDiagonals; }
            set { mDiagonals = value; }
        }

        public bool HeavyDiagonals
        {
            get { return mHeavyDiagonals; }
            set { mHeavyDiagonals = value; }
        }

        public int HeuristicEstimate
        {
            get { return mHEstimate; }
            set { mHEstimate = value; }
        }

        public bool PunishChangeDirection
        {
            get { return mPunishChangeDirection; }
            set { mPunishChangeDirection = value; }
        }

        public bool TieBreaker
        {
            get { return mTieBreaker; }
            set { mTieBreaker = value; }
        }

        public int SearchLimit
        {
            get { return mSearchLimit; }
            set { mSearchLimit = value; }
        }

        public double CompletedTime
        {
            get { return mCompletedTime; }
            set { mCompletedTime = value; }
        }

        public bool DebugProgress
        {
            get { return mDebugProgress; }
            set { mDebugProgress = value; }
        }

        public bool DebugFoundPath
        {
            get { return mDebugFoundPath; }
            set { mDebugFoundPath = value; }
        }
        #endregion

        public void FindPathStop()
        {
            mStop = true;
        }

        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            // HighResolutionTime.Start();

            PathFinderNode parentNode;
            bool found = false;
            int gridX = mGrid.GetUpperBound(0);
            int gridY = mGrid.GetUpperBound(1);

            mStop = false;
            mStopped = false;
            mOpen.Clear();
            mClose.Clear();

            sbyte[,] direction;
            if (mDiagonals)
                direction = new sbyte[8, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
            else
                direction = new sbyte[4, 2] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };

            parentNode.G = 0;
            parentNode.H = mHEstimate;
            parentNode.F = parentNode.G + parentNode.H;
            parentNode.X = start.X;
            parentNode.Y = start.Y;
            parentNode.PX = parentNode.X;
            parentNode.PY = parentNode.Y;
            mOpen.Push(parentNode);
            while (mOpen.Count > 0 && !mStop)
            {
                parentNode = mOpen.Pop();

                if (parentNode.X == end.X && parentNode.Y == end.Y)
                {
                    mClose.Add(parentNode);
                    found = true;
                    break;
                }

                if (mClose.Count > mSearchLimit)
                {
                    mStopped = true;
                    return null;
                }

                if (mPunishChangeDirection)
                    mHoriz = (parentNode.X - parentNode.PX);

                //Lets calculate each successors
                for (int i = 0; i < (mDiagonals ? 8 : 4); i++)
                {
                    PathFinderNode newNode;
                    newNode.X = parentNode.X + direction[i, 0];
                    newNode.Y = parentNode.Y + direction[i, 1];

                    if (newNode.X < 0 || newNode.Y < 0 || newNode.X >= gridX || newNode.Y >= gridY)
                        continue;

                    int newG;
                    if (mHeavyDiagonals && i > 3)
                        newG = parentNode.G + (int)(mGrid[newNode.X, newNode.Y] * 2.41);
                    else
                        newG = parentNode.G + mGrid[newNode.X, newNode.Y];


                    if (newG == parentNode.G)
                    {
                        //Unbrekeable
                        continue;
                    }

                    if (mPunishChangeDirection)
                    {
                        if ((newNode.X - parentNode.X) != 0)
                        {
                            if (mHoriz == 0)
                                newG += 20;
                        }
                        if ((newNode.Y - parentNode.Y) != 0)
                        {
                            if (mHoriz != 0)
                                newG += 20;

                        }
                    }

                    int foundInOpenIndex = -1;
                    for (int j = 0; j < mOpen.Count; j++)
                    {
                        if (mOpen[j].X == newNode.X && mOpen[j].Y == newNode.Y)
                        {
                            foundInOpenIndex = j;
                            break;
                        }
                    }
                    if (foundInOpenIndex != -1 && mOpen[foundInOpenIndex].G <= newG)
                        continue;

                    int foundInCloseIndex = -1;
                    for (int j = 0; j < mClose.Count; j++)
                    {
                        if (mClose[j].X == newNode.X && mClose[j].Y == newNode.Y)
                        {
                            foundInCloseIndex = j;
                            break;
                        }
                    }
                    if (foundInCloseIndex != -1 && mClose[foundInCloseIndex].G <= newG)
                        continue;

                    newNode.PX = parentNode.X;
                    newNode.PY = parentNode.Y;
                    newNode.G = newG;

                    switch (mFormula)
                    {
                        default:
                        case HeuristicFormula.Manhattan:
                            newNode.H = mHEstimate * (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                            break;
                        case HeuristicFormula.MaxDXDY:
                            newNode.H = mHEstimate * (Math.Max(Math.Abs(newNode.X - end.X), Math.Abs(newNode.Y - end.Y)));
                            break;
                        case HeuristicFormula.DiagonalShortCut:
                            int h_diagonal = Math.Min(Math.Abs(newNode.X - end.X), Math.Abs(newNode.Y - end.Y));
                            int h_straight = (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                            newNode.H = (mHEstimate * 2) * h_diagonal + mHEstimate * (h_straight - 2 * h_diagonal);
                            break;
                        case HeuristicFormula.Euclidean:
                            newNode.H = (int)(mHEstimate * Math.Sqrt(Math.Pow((newNode.X - end.X), 2) + Math.Pow((newNode.Y - end.Y), 2)));
                            break;
                        case HeuristicFormula.EuclideanNoSQR:
                            newNode.H = (int)(mHEstimate * (Math.Pow((newNode.X - end.X), 2) + Math.Pow((newNode.Y - end.Y), 2)));
                            break;
                        case HeuristicFormula.Custom1:
                            Point dxy = new Point(Math.Abs(end.X - newNode.X), Math.Abs(end.Y - newNode.Y));
                            int Orthogonal = Math.Abs(dxy.X - dxy.Y);
                            int Diagonal = Math.Abs(((dxy.X + dxy.Y) - Orthogonal) / 2);
                            newNode.H = mHEstimate * (Diagonal + Orthogonal + dxy.X + dxy.Y);
                            break;
                    }
                    if (mTieBreaker)
                    {
                        int dx1 = parentNode.X - end.X;
                        int dy1 = parentNode.Y - end.Y;
                        int dx2 = start.X - end.X;
                        int dy2 = start.Y - end.Y;
                        int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                        newNode.H = (int)(newNode.H + cross * 0.001);
                    }
                    newNode.F = newNode.G + newNode.H;



                    //It is faster if we leave the open node in the priority queue
                    //When it is removed, all nodes around will be closed, it will be ignored automatically
                    //if (foundInOpenIndex != -1)
                    //    mOpen.RemoveAt(foundInOpenIndex);

                    //if (foundInOpenIndex == -1)
                    mOpen.Push(newNode);
                }

                mClose.Add(parentNode);

            }

            //mCompletedTime = HighResolutionTime.GetTime();
            if (found)
            {
                PathFinderNode fNode = mClose[mClose.Count - 1];
                for (int i = mClose.Count - 1; i >= 0; i--)
                {
                    if (fNode.PX == mClose[i].X && fNode.PY == mClose[i].Y || i == mClose.Count - 1)
                    {
                        fNode = mClose[i];
                    }
                    else
                        mClose.RemoveAt(i);
                }
                mStopped = true;
                return mClose;
            }
            mStopped = true;
            return null;
        }

        internal class ComparePFNode : IComparer<PathFinderNode>
        {
            #region IComparer Members
            public int Compare(PathFinderNode x, PathFinderNode y)
            {
                if (x.F > y.F)
                    return 1;
                else if (x.F < y.F)
                    return -1;
                return 0;
            }
            #endregion
        }
    }
}

//namespace Engine.Core.Sprite
//{

///// <summary>
///// Represents a single node on a grid that is being searched for a path between two points
///// </summary>
//public class Node
//{
//    private Node parentNode;

//    /// <summary>
//    /// The node's location in the grid
//    /// </summary>
//    public Point Location { get; private set; }

//    /// <summary>
//    /// True when the node may be traversed, otherwise false
//    /// </summary>
//    public bool IsWalkable { get; set; }

//    /// <summary>
//    /// Cost from start to here
//    /// </summary>
//    public float G { get; private set; }

//    /// <summary>
//    /// Estimated cost from here to end
//    /// </summary>
//    public float H { get; private set; }

//    /// <summary>
//    /// Flags whether the node is open, closed or untested by the PathFinder
//    /// </summary>
//    public NodeState State { get; set; }

//    /// <summary>
//    /// Estimated total cost (F = G + H)
//    /// </summary>
//    public float F
//    {
//        get { return this.G + this.H; }
//    }

//    /// <summary>
//    /// Gets or sets the parent node. The start node's parent is always null.
//    /// </summary>
//    public Node ParentNode
//    {
//        get { return this.parentNode; }
//        set
//        {
//            // When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
//            this.parentNode = value;
//            this.G = this.parentNode.G + GetTraversalCost(this.Location, this.parentNode.Location);
//        }
//    }

//    /// <summary>
//    /// Creates a new instance of Node.
//    /// </summary>
//    /// <param name="x">The node's location along the X axis</param>
//    /// <param name="y">The node's location along the Y axis</param>
//    /// <param name="isWalkable">True if the node can be traversed, false if the node is a wall</param>
//    /// <param name="endLocation">The location of the destination node</param>
//    public Node(int x, int y, bool isWalkable, Point endLocation)
//    {
//        this.Location = new Point(x, y);
//        this.State = NodeState.Untested;
//        this.IsWalkable = isWalkable;
//        this.H = GetTraversalCost(this.Location, endLocation);
//        this.G = 0;
//    }

//    public override string ToString()
//    {
//        return string.Format("{0}, {1}: {2}", this.Location.X, this.Location.Y, this.State);
//    }

//    /// <summary>
//    /// Gets the distance between two points
//    /// </summary>
//    internal static float GetTraversalCost(Point location, Point otherLocation)
//    {
//        float deltaX = otherLocation.X - location.X;
//        float deltaY = otherLocation.Y - location.Y;

//        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
//    }
//}

///// <summary>
///// Represents the search state of a Node
///// </summary>
//public enum NodeState
//{
//    /// <summary>
//    /// The node has not yet been considered in any possible paths
//    /// </summary>
//    Untested,
//    /// <summary>
//    /// The node has been identified as a possible step in a path
//    /// </summary>
//    Open,
//    /// <summary>
//    /// The node has already been included in a path and will not be considered again
//    /// </summary>
//    Closed
//}

//public class PathFinder
//{
//    private int width;
//    private int height;
//    private Node[,] nodes;
//    private Node startNode;
//    private Node endNode;
//    private SearchParameters searchParameters;

//    /// <summary>
//    /// Create a new instance of PathFinder
//    /// </summary>
//    /// <param name="searchParameters"></param>
//    public PathFinder(SearchParameters searchParameters)
//    {
//        this.searchParameters = searchParameters;
//        InitializeNodes(searchParameters.Map);
//        this.startNode = this.nodes[searchParameters.StartLocation.X, searchParameters.StartLocation.Y];
//        this.startNode.State = NodeState.Open;
//        this.endNode = this.nodes[searchParameters.EndLocation.X, searchParameters.EndLocation.Y];
//    }

//    /// <summary>
//    /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
//    /// </summary>
//    /// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
//    public List<Point> FindPath()
//    {
//        // The start node is the first entry in the 'open' list
//        List<Point> path = new List<Point>();
//        bool success = Search(startNode);
//        if (success)
//        {
//            // If a path was found, follow the parents from the end node to build a list of locations
//            Node node = this.endNode;
//            while (node.ParentNode != null)
//            {
//                path.Add(node.Location);
//                node = node.ParentNode;
//            }

//            // Reverse the list so it's in the correct order when returned
//            path.Reverse();
//        }

//        return path;
//    }

//    /// <summary>
//    /// Builds the node grid from a simple grid of booleans indicating areas which are and aren't walkable
//    /// </summary>
//    /// <param name="map">A boolean representation of a grid in which true = walkable and false = not walkable</param>
//    private void InitializeNodes(bool[,] map)
//    {
//        this.width = map.GetLength(0);
//        this.height = map.GetLength(1);
//        this.nodes = new Node[this.width, this.height];
//        for (int y = 0; y < this.height; y++)
//        {
//            for (int x = 0; x < this.width; x++)
//            {
//                this.nodes[x, y] = new Node(x, y, map[x, y], this.searchParameters.EndLocation);
//            }
//        }
//    }

//    /// <summary>
//    /// Attempts to find a path to the destination node using <paramref name="currentNode"/> as the starting location
//    /// </summary>
//    /// <param name="currentNode">The node from which to find a path</param>
//    /// <returns>True if a path to the destination has been found, otherwise false</returns>
//    private bool Search(Node currentNode)
//    {
//        // Set the current node to Closed since it cannot be traversed more than once
//        currentNode.State = NodeState.Closed;
//        List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);

//        // Sort by F-value so that the shortest possible routes are considered first
//        nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
//        foreach (var nextNode in nextNodes)
//        {
//            // Check whether the end node has been reached
//            if (nextNode.Location == this.endNode.Location)
//            {
//                return true;
//            }
//            else
//            {
//                // If not, check the next set of nodes
//                if (Search(nextNode)) // Note: Recurses back into Search(Node)
//                    return true;
//            }
//        }

//        // The method returns false if this path leads to be a dead end
//        return false;
//    }

//    /// <summary>
//    /// Returns any nodes that are adjacent to <paramref name="fromNode"/> and may be considered to form the next step in the path
//    /// </summary>
//    /// <param name="fromNode">The node from which to return the next possible nodes in the path</param>
//    /// <returns>A list of next possible nodes in the path</returns>
//    private List<Node> GetAdjacentWalkableNodes(Node fromNode)
//    {
//        List<Node> walkableNodes = new List<Node>();
//        IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

//        foreach (var location in nextLocations)
//        {
//            int x = location.X;
//            int y = location.Y;

//            // Stay within the grid's boundaries
//            if (x < 0 || x >= this.width || y < 0 || y >= this.height)
//                continue;

//            Node node = this.nodes[x, y];
//            // Ignore non-walkable nodes
//            if (!node.IsWalkable)
//                continue;

//            // Ignore already-closed nodes
//            if (node.State == NodeState.Closed)
//                continue;

//            // Already-open nodes are only added to the list if their G-value is lower going via this route.
//            if (node.State == NodeState.Open)
//            {
//                float traversalCost = Node.GetTraversalCost(node.Location, fromNode.Location);
//                float gTemp = fromNode.G + traversalCost;
//                if (gTemp < node.G)
//                {
//                    node.ParentNode = fromNode;
//                    walkableNodes.Add(node);
//                }
//            }
//            else
//            {
//                // If it's untested, set the parent and flag it as 'Open' for consideration
//                node.ParentNode = fromNode;
//                node.State = NodeState.Open;
//                walkableNodes.Add(node);
//            }
//        }

//        return walkableNodes;
//    }

//    /// <summary>
//    /// Returns the eight locations immediately adjacent (orthogonally and diagonally) to <paramref name="fromLocation"/>
//    /// </summary>
//    /// <param name="fromLocation">The location from which to return all adjacent points</param>
//    /// <returns>The locations as an IEnumerable of Points</returns>
//    private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
//    {
//        return new Point[]
//        {
//                //new Point(fromLocation.X-1, fromLocation.Y-1),
//                new Point(fromLocation.X-1, fromLocation.Y  ),
//                //new Point(fromLocation.X-1, fromLocation.Y+1),
//                new Point(fromLocation.X,   fromLocation.Y+1),
//                //new Point(fromLocation.X+1, fromLocation.Y+1),
//                new Point(fromLocation.X+1, fromLocation.Y  ),
//                //new Point(fromLocation.X+1, fromLocation.Y-1),
//                new Point(fromLocation.X,   fromLocation.Y-1)
//        };
//    }


//    /// <summary>
//    /// Defines the parameters which will be used to find a path across a section of the map
//    /// </summary>
//    public class SearchParameters
//    {
//        public Point StartLocation { get; set; }

//        public Point EndLocation { get; set; }

//        public bool[,] Map { get; set; }

//        public SearchParameters(Point startLocation, Point endLocation, bool[,] map)
//        {
//            this.StartLocation = startLocation;
//            this.EndLocation = endLocation;
//            this.Map = map;
//        }
//    }
//}
//}
