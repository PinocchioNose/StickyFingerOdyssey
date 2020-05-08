using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUI : MonoBehaviour
{
    public Material default_material;
    public Material highlight_material;

    [Tooltip("UI相对中心的下降幅度")]
    public float offset_ratio = 0.1f;
    public float k = 2000f;
    public float start = 70;
    public float end = 25;
    [Tooltip("为了方便起见，使用7的倍数")]
    public int segment_number = 210;
    

    private Vector3 origin;
    private Vector3 screen_center;
    private Vector3[] left_points;
    private Vector3[] right_points;

    /// <summary>
    /// 计算反比例函数采样之后得到的点
    /// </summary>
    /// <param name="y_start">y方向上的起点</param>
    /// <param name="y_end">y方向上的终点</param>
    /// <param name="segment_number">采样段数</param>
    void calcuPoints(float y_start, float y_end, int segment_number)
    {
        List<Vector3> left_container = new List<Vector3>();
        List<Vector3> right_container = new List<Vector3>();

        
        for (int i = 0; i<=segment_number - 1; i++)
        {
            float t = i * 1.0f / (segment_number - 1);
            float y_tmp = Mathf.Lerp(y_start, y_end, t);
            left_container.Add(new Vector3(-k / y_tmp, y_tmp) + origin);
            right_container.Add(new Vector3(k / y_tmp, y_tmp) + origin);
            
        }
        left_points = left_container.ToArray();
        right_points = right_container.ToArray();
    }

    /// <summary>
    /// 绘制函数
    /// </summary>
    void draw_default()
    {
        if (!Controller.isCharging) // not in charge
            return;

        GL.PushMatrix();
        default_material.SetPass(0);
        GL.LoadPixelMatrix();

        GL.Begin(GL.QUADS);
        for (int i = left_points.Length - 1; i >= 1; i--)
        {
            if ((i / 30) % 2 == 0)
            {
                float highlight_idx = left_points.Length * Controller.jump_force / Controller.Y_Force_Max;
                if (left_points.Length - i <= highlight_idx)
                    GL.Color(highlight_material.color);
                else
                    GL.Color(default_material.color);
                GL.Vertex(left_points[i - 1]);
                GL.Vertex(right_points[i - 1]);
                GL.Vertex(right_points[i]);
                GL.Vertex(left_points[i]);
            }
        }
        GL.End();

        GL.PopMatrix();
    }

    private void Awake()
    {
        screen_center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        origin = screen_center - new Vector3(0, Screen.height * offset_ratio, 0);

    }

    void Start()
    {
        calcuPoints(y_start: start, y_end: end, segment_number: segment_number);
    }

    

    private void OnPostRender()
    {
        draw_default();
    }


}
