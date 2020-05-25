using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DCG
{
    // Mesh.triangles : The array is a list of triangles that contains indices into the vertex array. 
    // 使用mesh三角形面片构建一个R树,该R树外边界是固定的(mesh.bounds),不动态扩展
    // 类似四叉树,但不是直接划分为4个子节点(象限),而是先分裂同一级(平行)节点,直到节点数量超过m个(类似R树)才进行下一级分裂.
    // 分裂原则是:根据mesh的x,z进行排序后取固定m个数量,形成一个节点.如果多个三角形都重叠在一起,那也不会都放到一个节点中,而是分裂为多个重合的节点分别存放.
    // 查找:在这里简化查找为只查找一个点,直接直接循环子节点,使用bounds.contains()来确定查找点是否位于子节点的矩形内部(没有做常规R树的跨节点判定).
    // 插入时会有性能消耗,但降低了深度(平行节点个数更多),使得容纳的个数更多.
    public class MeshRTree
    {
        public RNode root;
        public float width;
        public float height;
        // childrenCapacity: 当子节点数量大于此值时会使得当前插入的point所在的node触发分裂.
        private int childrenCapacity = 4;
        // nodeDataCapacity: 当一个node下的数据容量超过此值时会使得所在node触发分裂.
        private int nodeDataCapacity = 4;
        // 查找时的计数
        private int findCount = 0;
        // private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public MeshFilter meshFilter;

        public void Build(float width, float height, int childrenCapacity = 15, int nodeDataCapacity = 15)
        {
            if (this.childrenCapacity < 4) this.childrenCapacity = 4;
            if (this.nodeDataCapacity < 4) this.nodeDataCapacity = 4;
            this.childrenCapacity = childrenCapacity;
            this.nodeDataCapacity = nodeDataCapacity;

            this.root = new RNode();
            this.width = width;
            this.height = height;

            root.bounds = new Bounds();
            root.bounds.min = new Vector3(0, 0, 0);
            root.bounds.max = new Vector3(width, 0, height);
        }

        public void FastBuildFromMesh(MeshFilter meshFilter, int childrenCapacity = 15, int nodeDataCapacity = 15)
        {
            this.meshFilter = meshFilter;
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            List<VertexRect> datas = new List<VertexRect>();
            // Debug.Log("顶点数:" + vertices.Length + "  三角形数:" + triangles.Length + "  宽高:" + this.root.bounds.size);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                VertexRect data = new VertexRect();
                data.AddVertexs(v0, v1, v2);
                datas.Add(data);
            }
            var bounds = mesh.bounds;
            this.FastBuild(bounds.size.x, bounds.size.z, datas, childrenCapacity, nodeDataCapacity);
            root.bounds = mesh.bounds;
        }

        
        // 通过一组Vertex2Triangle数据,快速分裂出tree,该方法比循环datas进行单个添加快4倍左右,但分布不精确.
        public void FastBuild(float width, float height, List<VertexRect> datas, int childrenCapacity = 15, int nodeDataCapacity = 15)
        {
            this.Build(width, height, childrenCapacity, nodeDataCapacity);
            this.root.SetDatas(datas);
            this.DirectSplit(this.root);
        }
        public void Add(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            VertexRect vr = new VertexRect();
            vr.AddVertexs(v0, v1, v2);
            this.Add(vr);
        }

        public void Add(VertexRect vertexRect)
        {
            // 从根节点往下查找point所属的叶子节点
            findCount = 0;
            RNode node = this.GetLeafNodeOnAdd(this.root, vertexRect);
            if (node == null)
            {
                // 需要进行区域扩展
                // ...

                Debug.Log("警告:超出添加范围 (" + vertexRect + ")");
                // this.GetLeafNode(this.root, vertexRect);
                return;
            }

            // 如果叶子节点内数据容量超过capacity,则需要分裂
            if (node.GetDatas().Count + 1 > this.nodeDataCapacity)
            {
                this.SplitAndAdd(node, vertexRect);
            }
            else
            {
                node.AddData(vertexRect);
            }
        }

        /// 查找位于哪些node内
        public List<RNode> Search(Vector3 pos)
        {
            // pos = meshFilter.transform.InverseTransformVector(pos);
            this.findCount = 0;
            Vector3 p = new Vector3(pos.x, this.root.bounds.center.y, pos.z);

            List<RNode> rs = new List<RNode>();
            this._Search(this.root, p, rs);
            return rs;
        }
        private void _Search(RNode currNode, Vector3 point, List<RNode> rs)
        {
            this.findCount++;
            if (currNode.IsLeaf())
            {
                if (this.Conatains(currNode.bounds, point))
                {
                    rs.Add(currNode);
                }
                else
                {
                    return;
                }
            }

            if (!this.Conatains(currNode.bounds, point)) return;

            List<RNode> children = currNode.children;
            for (int i = 0; i < children.Count; i++)
            {
                this._Search(children[i], point, rs);
            }
        }

        public int GetFindStepCount()
        {
            return this.findCount;
        }

        private bool Conatains(Bounds bounds, Vector3 point)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            //  point = vertexRect.v0;
            bool b1 = point.x >= min.x || Mathf.Abs(point.x - min.x) <= 0.0001f;
            bool b2 = point.z >= min.z || Mathf.Abs(point.z - min.z) <= 0.0001f;
            bool b3 = point.x <= max.x || Mathf.Abs(point.x - max.x) <= 0.0001f;
            bool b4 = point.z <= max.z || Mathf.Abs(point.z - max.z) <= 0.0001f;
            return b1 && b2 && b3 && b4;
        }
        private bool Conatains(Bounds bounds, VertexRect vertexRect)
        {
            Vector3[] vertexs = vertexRect.GetVertexs();
            for (int i = 0; i < vertexs.Length; i++)
            {
                Vector3 point = vertexs[i];
                if (this.Conatains(bounds, point)) return true;
            }
            return false;
        }

        /// 选择所属的叶子节点
        private RNode GetLeafNodeOnAdd(RNode node, VertexRect vertexRect)
        {
            findCount++;
            if (node.IsLeaf())
            {
                if (this.Conatains(node.bounds, vertexRect)) return node;
                return null;
            }

            var children = node.children;
            for (int i = 0; i < children.Count; i++)
            {
                // 父区域对point有包含才继续往下查找
                if (this.Conatains(children[i].bounds, vertexRect))
                {
                    RNode _node = GetLeafNodeOnAdd(children[i], vertexRect);
                    findCount++;
                    if (_node != null) return _node;
                }
            }
            return null;
        }

        /// 直接分裂
        private void DirectSplit(RNode node)
        {
            this._SplitAndAdd(node, null);
            List<RNode> children = node.children;

            if (children.Count >= this.childrenCapacity)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i].IsLeaf() && children[i].GetDatas().Count > this.nodeDataCapacity)
                        this.DirectSplit(children[i]);
                }
            }
        }

        // 分裂节点:
        // 当leafNode的平行节点数量大于childrenCapacity时,才会增加深度---leafNode扩展出子节点
        // 增加深度时,平行节点内的数据量应该是饱和的,平行节点的所有数据总量应该不大于 T = childrenCapacity*nodeDataCapacity
        private void SplitAndAdd(RNode node, VertexRect newData)
        {
            // 优化: 插入的点和上次没变化,直接添加
            if (!this.NeedSplit(node, newData))
            {
                node.AddData(newData);
                return;
            }

            if (node == this.root)
            {
                this._SplitAndAdd(node, newData);
                return;
            }
            if (node.parent.children.Count >= this.childrenCapacity)
            {
                // 如果父级不能再分裂,直接分裂当前节点
                this._SplitAndAdd(node, newData);
            }
            else
            {
                this._SplitAndAdd(node.parent, newData);
            }
        }

        /// 在容量已满时检查是否需要分裂:如果添加的数据和node中上次添加的数据信息是一样的,则不需要分裂
        private bool NeedSplit(RNode node, VertexRect newData)
        {
            List<VertexRect> datas = node.GetDatas();
            if (datas.Count < 1) return true;
            VertexRect lastData = datas[datas.Count - 1];
            // var pos1 = lastData.pos;
            // var pos2 = newData.pos;
            // return !((pos1.x == pos2.x && pos1.z == pos2.z) || (Mathf.Abs(pos1.x - pos2.x) <= 0.0001f && Mathf.Abs(pos1.z - pos2.z) <= 0.0001f));
            return newData != null && lastData.Equals(newData);
        }

        
        private bool _SplitAndAdd(RNode node, VertexRect newData)
        {
            // 叶子节点
            List<VertexRect> currDatas;
            Bounds nodeDataBounds = new Bounds();
            if (node.IsLeaf())
            {
                if (node.GetDatas().Count < this.nodeDataCapacity)
                {
                    if (newData != null) node.AddData(newData);
                    return false;
                }

                if (newData != null) node.AddData(newData); // 触发一次bounds更新
                nodeDataBounds = node.GetDataBounds();

                currDatas = node.GetDatas();
                node.EmptyData();
            }
            else
            {
                List<RNode> paralleledNodes = node.children;
                currDatas = new List<VertexRect>();
                for (int i = 0; i < paralleledNodes.Count; i++)
                {
                    if (newData != null)
                    {
                        if (this.Conatains(paralleledNodes[i].bounds, newData))
                        {
                            // 触发一次bounds更新
                            paralleledNodes[i].AddData(newData);
                        }
                    }
                    currDatas.AddRange(paralleledNodes[i].GetDatas());
                }
                nodeDataBounds = node.GetDataBounds();
                node.EmptyChildren();
            }

            List<RNode> currNodeChildren = node.children;

            // 循环处理分裂结果
            List<VertexRect> nextDatas = currDatas;
            int j = -1;

            // 对node的持续分裂
            while (true)
            {
                SplitInfo info = this.GetSplitInfo(nextDatas);
                if (info == null) // 不能分裂
                {
                    if (j == -1)
                    {
                        var child = new RNode();
                        child.heap = node.heap + 1;
                        child.index = 0;
                        child.parent = node;
                        child.SetDatas(nextDatas);
                        child.GetDataBounds();
                        currNodeChildren.Add(child);
                    }
                    else
                    {
                        currNodeChildren[j].SetDatas(nextDatas);
                        currNodeChildren[j].GetDataBounds();
                    }
                    break;
                }

                if (j == -1)
                { // j表示当前正在分裂的节点index,-1表示第一次分裂,需要添加
                    RNode child = new RNode();
                    child.heap = node.heap + 1;
                    child.index = 0;
                    child.parent = node;
                    child.SetDatas(info.group1);
                    child.GetDataBounds();
                    currNodeChildren.Add(child);
                }
                else
                {
                    // 如果已经分裂出来的节点总数超过容量,则不能直接添加,需要增加高度,进行下一级分裂
                    if (currNodeChildren.Count + 1 > this.childrenCapacity)
                    {
                        // 此时currNodeChildren中各个节点数据已经包含了newData,所以无需重复添加
                        this._SplitAndAdd(currNodeChildren[j], null);
                        break;
                    }

                    // j>-1表示基于上次的节点进行分裂
                    currNodeChildren[j].SetDatas(info.group1);
                    currNodeChildren[j].GetDataBounds();
                }

                // 第二个节点是分裂出来的,直接添加为新节点
                RNode child2 = new RNode();
                child2.heap = node.heap + 1;
                child2.index = currNodeChildren.Count;
                child2.parent = node;
                child2.SetDatas(info.group2);
                child2.GetDataBounds();

                currNodeChildren.Add(child2);

                // 选取节点数量较多的一组进行进一步的分裂
                int _nextIndex = -1;
                int cap = -1;
                for (int i = 0; i < currNodeChildren.Count; i++)
                {
                    List<VertexRect> _datas = currNodeChildren[i].GetDatas();
                    if (_datas.Count > this.nodeDataCapacity && _datas.Count > cap)
                    {
                        _nextIndex = i;
                        cap = _datas.Count;
                    }
                }

                // 没有可以进一步分裂的节点了
                if (_nextIndex == -1) break;

                nextDatas = currNodeChildren[_nextIndex].GetDatas();
                j = _nextIndex;
            }
            // sw.Stop();
            // Debug.Log(string.Format("t: {0}-{1} ms", sw.ElapsedMilliseconds, sw.ElapsedTicks));

            return true;
        }


        /// 分割信息
        private SplitInfo GetSplitInfo(List<VertexRect> allDatasInNode)
        {
            List<VertexRect> group1 = new List<VertexRect>();
            List<VertexRect> group2 = new List<VertexRect>();

            // 对allDatasInNode进行一次排序: 从做到右,从上到下
            allDatasInNode.Sort((VertexRect a, VertexRect b) =>
            {
                if (a.minX < b.minX) return -1;
                if (a.minX > b.minX) return 1;
                if (a.minZ < b.minZ) return -1;
                if (a.minZ > b.minZ) return 1;
                return 0;
            });

            // 将排序后的结果分为2类
            int len = (int)(allDatasInNode.Count * 0.5f);
            for (int i = 0; i < len; i++)
            {
                group1.Add(allDatasInNode[i]);
            }

            for (int i = len; i < allDatasInNode.Count; i++)
            {
                group2.Add(allDatasInNode[i]);
            }

            SplitInfo info = new SplitInfo();
            info.group1 = group1;
            info.group2 = group2;

            return info;
        }

        override public string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("MeshRectTree={");
            sb.Append(this.root.ToString());
            sb.Append("}");
            return sb.ToString();
        }



#if UNITY_EDITOR
        private List<RNode> searchResult;
        // private Color[] debugColors;
        private Color showTreeColor = new Color(94f / 255f, 154f / 255f, 154f / 255f, 1f);
        public bool showTree = false;
        public void FixedUpdate()
        {
            if (this.showTree) this._ShowTreeRect(this.root, this.showTreeColor);

            this._ShowSearchResult();
        }
        public void ShowSearchResult(List<RNode> result)
        {
            this.searchResult = result;
        }

        // public void ShowSearchResult(List<VertexRect> result)
        private void _ShowSearchResult()
        {
            if (this.searchResult == null) return;

            for (int i = 0; i < searchResult.Count; i++)
            {
                var datas = searchResult[i].GetDatas();
                for (int j = 0; j < datas.Count; j++)
                {
                    var vertices = datas[j].GetVertexs();
                    var v0 = vertices[0];
                    var v1 = vertices[1];
                    var v2 = vertices[2];
                    Debug.DrawLine(v0, v1, Color.white);
                    Debug.DrawLine(v1, v2, Color.white);
                    Debug.DrawLine(v2, v0, Color.white);
                }

                this._ShowTreeRect(searchResult[i], Color.yellow);
            }
        }
        private void _ShowTreeRect(RNode node, Color _color)
        {
            var min = node.bounds.min;
            var max = node.bounds.max;
            Vector3 p0 = new Vector3(min.x, this.root.bounds.center.y, min.z);
            Vector3 p1 = new Vector3(p0.x, p0.y, max.z);
            Vector3 p2 = new Vector3(max.x, p1.y, p1.z);
            Vector3 p3 = new Vector3(p2.x, p2.y, p0.z);
            Debug.DrawLine(p0, p1, _color);
            Debug.DrawLine(p1, p2, _color);
            Debug.DrawLine(p2, p3, _color);
            Debug.DrawLine(p3, p0, _color);

            if (!node.IsLeaf())
            {
                for (int i = 0; i < node.children.Count; i++)
                {
                    _ShowTreeRect(node.children[i], showTreeColor);
                }
            }
            else
            {
                var datas = node.GetDatas();
                var dist = 0.2f;
                var offx = Mathf.Cos(Mathf.Deg2Rad * 30) * dist;
                var offz = Mathf.Sin(Mathf.Deg2Rad * 30) * dist;
                for (int i = 0; i < datas.Count; i++)
                {
                    var vertexs = datas[i].GetVertexs();
                    // 对每个vertex画个小三角,方便查看
                    for (int j = 0; j < vertexs.Length; j++)
                    {
                        var p = vertexs[j];
                        Gizmos.color = Color.red;
                        var pos0 = new Vector3(p.x - offx, p.y, p.z - offz);
                        var pos1 = new Vector3(p.x, pos0.y, pos0.z + dist);
                        var pos2 = new Vector3(pos1.x + offx, pos0.y, pos1.z - offz);
                        Debug.DrawLine(pos0, pos1, Color.red);
                        Debug.DrawLine(pos1, pos2, Color.red);
                        Debug.DrawLine(pos2, pos0, Color.red);
                    }
                }
            }
        }
#endif
    }

    public class RNode
    {
        public int heap;
        public int index;
        public RNode parent;

        public Bounds bounds;
        /// 数据
        private List<VertexRect> datas = new List<VertexRect>();

        /// 子节点
        public List<RNode> children = new List<RNode>();

        /// 将child的bounds缓存下来,避免频繁计算bounds
        private bool _childrenBoundsDirty = true;
        private Bounds _childrenBounds;

        public void MarkChildrenBoundsDirty()
        {
            this._childrenBoundsDirty = true;
        }
        public bool childrenBoundsDirty
        {
            get { return this._childrenBoundsDirty; }
            set
            {
                if (value)
                {
                    // 向上传播脏标记
                    RNode p = this.parent;
                    while (p != null)
                    {
                        p.MarkChildrenBoundsDirty();
                        p = p.parent;
                    }
                }
                this._childrenBoundsDirty = value;
            }
        }

        public void EmptyData()
        {
            this.childrenBoundsDirty = true;
            this.datas = new List<VertexRect>();
        }
        public void EmptyChildren()
        {
            this.children = new List<RNode>();
        }

        public void AddData(VertexRect data)
        {
            this.childrenBoundsDirty = true;
            this.datas.Add(data);
        }
        public void SetDatas(List<VertexRect> datas)
        {
            this.childrenBoundsDirty = true;
            this.datas = datas;
        }

        public List<VertexRect> GetDatas()
        {
            return datas;
        }

        public bool IsLeaf()
        {
            if (this.children.Count > 0) return false;
            return true;
        }

        public List<VertexRect> GetAllDatas()
        {
            List<VertexRect> list = new List<VertexRect>();
            list.AddRange(this.datas);
            for (int i = 0; i < this.children.Count; i++)
            {
                list.AddRange(children[i].GetAllDatas());
            }
            return list;
        }

        /// 获取data在本节点分布下形成的bounds
        public Bounds GetDataBounds()
        {
            if (this.childrenBoundsDirty == false) return this._childrenBounds;
            this.childrenBoundsDirty = false;

            Vector3 min = new Vector3(float.MaxValue, 0, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, 0, float.MinValue);

            if (this.IsLeaf())
            {
                for (int i = 0; i < datas.Count; i++)
                {
                    VertexRect vr = datas[i];
                    Vector3[] vertexs = vr.GetVertexs();
                    for (int j = 0; j < vertexs.Length; j++)
                    {
                        Vector3 p = vertexs[j];
                        if (p.x < min.x) min.x = p.x;
                        else if (p.x > max.x) max.x = p.x;

                        if (p.z < min.z) min.z = p.z;
                        else if (p.z > max.z) max.z = p.z;
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.children.Count; i++)
                {
                    Bounds childBounds = children[i].GetDataBounds();
                    Vector3 chiMin = childBounds.min;
                    Vector3 chiMax = childBounds.max;
                    if (chiMin.x < min.x) min.x = chiMin.x;
                    if (chiMin.z < min.z) min.z = chiMin.z;
                    if (chiMax.x > max.x) max.x = chiMax.x;
                    if (chiMax.z > max.z) max.z = chiMax.z;
                }
            }

            this._childrenBounds = new Bounds();
            _childrenBounds.SetMinMax(min, max);

            if (this.parent != this && this.parent != null)
            {
                // 非根节点的bounds就是dataBounds
                this.bounds = this._childrenBounds;
            }

            return this._childrenBounds;
        }

        override public string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 1; i < this.heap + 1; i++)
            {
                sb.Append("    ");
            }
            sb.Append("RN:{");
            sb.Append("h=");
            sb.Append(heap);
            sb.Append(",i=");
            sb.Append(index);
            // sb.Append(",leaf=");
            // sb.Append(this.IsLeaf());
            // sb.Append(",bund=[");
            // sb.Append(this.bounds.min);
            // sb.Append(",");
            // sb.Append(this.bounds.max);
            // sb.Append("]");
            sb.Append(",ds=[L:");
            sb.Append(datas.Count);
            sb.Append("> ");
            for (int i = 0; i < datas.Count; i++)
            {
                // sb.Append(datas[i].pos);
                // sb.Append(",");
            }
            sb.Append("],");

            sb.Append("ch=[L:");
            sb.Append(this.children.Count);
            sb.Append(">");
            for (int i = 0; i < this.children.Count; i++)
            {
                sb.Append("\n");
                sb.Append(children[i].ToString());
                sb.Append(",");
            }
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }
    }

    /// 顶点构成的三角形区域
    public class VertexRect
    {
        // public int[] triangle;
        private float _minX;
        public float minX
        {
            get { return this._minX; }
        }
        private float _maxX;
        public float maxX
        {
            get { return this._maxX; }
        }
        private float _minZ;
        public float minZ
        {
            get { return this._minZ; }
        }
        private float _maxZ;
        public float maxZ
        {
            get { return this._maxZ; }
        }

        private Vector3[] vertexs = new Vector3[3];

        public void AddVertexs(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            this.vertexs[0] = v0;
            this.vertexs[1] = v1;
            this.vertexs[2] = v2;
            this._minX = Mathf.Min(v0.x, v1.x, v2.x);
            this._maxX = Mathf.Min(v0.x, v1.x, v2.x);
            this._minZ = Mathf.Min(v0.z, v1.z, v2.z);
            this._maxZ = Mathf.Min(v0.z, v1.z, v2.z);
        }
        public Vector3[] GetVertexs()
        {
            return this.vertexs;
        }

        public bool Equals(VertexRect tag)
        {
            Vector3 v0 = vertexs[0];
            Vector3 v1 = vertexs[1];
            Vector3 v2 = vertexs[2];

            Vector3 _v0 = tag.GetVertexs()[0];
            Vector3 _v1 = tag.GetVertexs()[1];
            Vector3 _v2 = tag.GetVertexs()[2];

            bool b1 = Mathf.Abs(v0.x - _v0.x) <= 0.0001f;
            bool b2 = Mathf.Abs(v0.y - _v0.y) <= 0.0001f;
            bool b3 = Mathf.Abs(v0.z - _v0.z) <= 0.0001f;
            return b1 && b2 && b3;
        }

        override public string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("{v0={");
            sb.Append(vertexs[0]);
            sb.Append(",v1=");
            sb.Append(vertexs[1]);
            sb.Append(",v2=");
            sb.Append(vertexs[2]);
            sb.Append("},");
            sb.Append("}");
            return sb.ToString();
        }
    }

    internal class SplitInfo
    {
        public List<VertexRect> group1;
        /// 划分后右|上的数据
        public List<VertexRect> group2;
    }
}