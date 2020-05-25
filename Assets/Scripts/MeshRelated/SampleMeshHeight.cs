using UnityEngine;
using System.Collections.Generic;
// using XLua;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DCG
{
    [/*LuaCallCSharp, */DisallowMultipleComponent, RequireComponent(typeof(MeshFilter))]
    public class SampleMeshHeight : MonoBehaviour
    {
        private MeshRTree _tree;
        public MeshRTree tree
        {
            get { return _tree; }
        }

        private MeshFilter targetMesh;

        private Vector3 measuringStartPoint;
        private Vector3 measuringEndPoint;

        // [Header("构建方式. 0:慢速 1:快速"), Range(0, 1)]
        // public int buildTreeType = 1;

        // #if UNITY_EDITOR
        // [BlackList]
        public List<RNode> lastSearchAroundData;
        // [Header("测量点在RectTree中消耗步骤"), BlackList]
         public int searchPointCostStep;
        // [Header("获取高度消耗时间(ms)"), BlackList]
         public float searchPointCostTime;
        // [Header("构建RectTree消耗时间(ms)")]
        public float buildTreeTime;
        // #endif

        public bool measureCostTime = false;

        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private void Awake()
        {
            this.Init();
        }

#if UNITY_EDITOR
        [Header("显示测量位置的射线")]
        public bool showMeasuringLine = false;
        [Header("显示RectTree结构")]
        public bool showTree = false;
        [Header("显示搜索结果")]
        public bool showSearch = false;

        public bool showDebug = true;
        private void FixedUpdate()
        {
            if (this.tree != null)
            {
                this.tree.showTree = this.showTree;
                this.tree.ShowSearchResult(this.showSearch ? this.lastSearchAroundData : null);
                this.tree.FixedUpdate();
            }
            if (showMeasuringLine)
            {
                Debug.DrawLine(this.measuringStartPoint, this.measuringEndPoint, Color.cyan);
                Debug.DrawLine(this.tree.meshFilter.transform.TransformPoint(this.measuringStartPoint), this.tree.meshFilter.transform.TransformPoint(this.measuringEndPoint), Color.cyan);
            }
        }

        private void OnDrawGizmos()
        {
            if (EditorApplication.isPlaying) return;

            if (this.tree == null)
            {
                this.Init();
            }
            this.FixedUpdate();
        }
#endif

        public void Init()
        {
            _tree = new MeshRTree();
            this.targetMesh = this.GetComponent<MeshFilter>();

            if (measureCostTime) sw.Restart();


            tree.FastBuildFromMesh(this.targetMesh);

            if (measureCostTime)
            {
                sw.Stop();
                this.buildTreeTime = sw.ElapsedMilliseconds;
                // Debug.Log(string.Format("顶点数:{0:G}, 构建消耗时间:{1:G}ms", targetMesh.sharedMesh.vertexCount, this.buildTreeTime));
            }

            this.lastSearchAroundData = null;
        }

        // 获取高度
        public float SampleHeight(Vector3 localPos)
        {
            var meshFilter = tree.meshFilter;
            localPos.y -= 1000;
            this.measuringStartPoint = localPos;

            if (measureCostTime)
            {
                sw.Restart();
            }

            List<RNode> rs = this.tree.Search(localPos);

            if (rs == null || rs.Count < 1)
            {
                lastSearchAroundData = null;
                return 0;
            }

            this.searchPointCostStep = tree.GetFindStepCount();

            this.lastSearchAroundData = rs;

            measuringEndPoint = new Vector3(measuringStartPoint.x, measuringStartPoint.y + 2000f, measuringStartPoint.z);
            var dir = Vector3.Normalize(measuringEndPoint - measuringStartPoint);
            var samples = new List<Vector3>();
            var Util = this.gameObject.GetComponent<Util>();

            for (int i = 0; i < rs.Count; i++)
            {
                RNode node = rs[i];
                var datas = node.GetDatas();
                for (int j = 0; j < datas.Count; j++)
                {
                    var vertices = datas[j].GetVertexs();
                    var v0 = vertices[0];
                    var v1 = vertices[1];
                    var v2 = vertices[2];

                    float t, u, v;
                    
                    var b = Util.IntersectTriangle(measuringStartPoint, dir, v0, v1, v2, out t, out u, out v);
                    if (b)
                    {
                        var p = (1 - u - v) * v0 + u * v1 + v * v2;
                        samples.Add(p);
                    }
                }
            }

            if (samples.Count < 1)
            {
                if (measureCostTime)
                {
                    sw.Stop();
                    this.searchPointCostTime = sw.ElapsedMilliseconds;
                }
                return 0;
            }

            var h = float.MinValue;
            for (int i = 0; i < samples.Count; i++)
            {
                if (samples[i].y > h) h = samples[i].y;
            }

            if (measureCostTime)
            {
                sw.Stop();
                this.searchPointCostTime = sw.ElapsedMilliseconds;
                // Debug.Log(string.Format("查找消耗时间:{0:G}ms", searchPointCostTime));
            }

            return h;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SampleMeshHeight))]
    internal class SampleMeshHeightEditor : Editor
    {
        private Vector3 samplePos;
        private bool showTreeRect = false;
        private bool showTreeSearchAround = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);
            this.samplePos = EditorGUILayout.Vector3Field("测量位置", this.samplePos);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("获取高度"))
            {
                var meshHeight = this.target as SampleMeshHeight;
                if (meshHeight.tree == null)
                {
                    meshHeight.Init();
                }
                var h = meshHeight.SampleHeight(samplePos);
                SceneView.lastActiveSceneView.Repaint();
                Debug.Log("高度: " + h);
            }
            if (GUILayout.Button("Reset", GUILayout.Width(60)))
            {
                var meshHeight = this.target as SampleMeshHeight;
                meshHeight.Init();
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
}