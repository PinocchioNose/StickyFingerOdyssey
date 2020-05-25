using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IntersectTriangle(Vector3 orig, Vector3 dir, Vector3 v0, Vector3 v1, Vector3 v2, out float t, out float u, out float v)
    {
        // E1,三角形中指向v1的方向向量
        Vector3 E1 = v1 - v0;

        // E2,三角形中指向v2的方向向量
        Vector3 E2 = v2 - v0;

        // P,垂直于E2的法向量
        Vector3 P = Vector3.Cross(dir, E2);

        // determinant, E2的法向量与E1的夹角
        float det = Vector3.Dot(E1, P);

        // keep det > 0, modify T accordingly
        Vector3 T;
        if (det > 0)
        {
            T = orig - v0;
        }
        else
        {
            T = v0 - orig;
            det = -det;
        }
        t = 0;
        u = 0;
        v = 0;

        // If determinant is near zero, ray lies in plane of triangle
        if (det < 0.00001f)
            return false;

        // Calculate u and make sure u <= 1
        u = Vector3.Dot(T, P);
        if (u < 0.0f || u - det > 0.00001f) // u > det
            return false;

        // Q
        Vector3 Q = Vector3.Cross(T, E1);

        // Calculate v and make sure u + v <= 1
        v = Vector3.Dot(dir, Q);
        if (v < 0.0f || (u + v - det > 0.00001f)) // u + v > det
            return false;

        // Calculate t, scale parameters, ray intersects triangle
        t = Vector3.Dot(E2, Q);

        float fInvDet = 1.0f / det;
        t *= fInvDet;
        u *= fInvDet;
        v *= fInvDet;

        return true;
    }

    // 测试相交点
    public void TestPos(float x, float z)
    {
        var mesh = this.GetComponent<MeshFilter>().mesh;
        var vertices = mesh.vertices;
        var startPos = new Vector3(x, 0.2f, z);
        var endPos = new Vector3(x, 10, z);
        var dir = Vector3.Normalize(endPos - startPos);
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            var v0 = vertices[mesh.triangles[i]];
            var v1 = vertices[mesh.triangles[i + 1]];
            var v2 = vertices[mesh.triangles[i + 2]];

            float t, u, v;
            var b = IntersectTriangle(startPos, dir, v0, v1, v2, out t, out u, out v);
            if (b)
            {
                var p = (1 - u - v) * v0 + u * v1 + v * v2;
            }
        }
    }
}
