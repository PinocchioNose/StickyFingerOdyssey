using UnityEngine;
using UnityEditor;

public class pivot : MonoBehaviour
{
[MenuItem("Tool/ResetCenterPosition")]
public static void ResetCenterPosition()
{
    Transform transform = Selection.activeTransform;
    Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
    Vector3 center = Vector3.zero;
    //获取物体的Bound最终合成信息
    foreach (var item in transform.GetComponentsInChildren<MeshRenderer>())
    {
        bounds.Encapsulate(item.bounds);
    }
    center = bounds.center;
    //新建空物体，将原来中心点有问题的物体放置到该物体下作为子物体存在
    GameObject obj = new GameObject();
    obj.name = transform.name;
    obj.transform.position = center;
    obj.transform.rotation = Quaternion.identity;
    //获取原物体在模型中的路径
    string selectedObjPath = "";
    Transform currentSelectTransform = transform;
    while (currentSelectTransform.parent != null)
    {
        selectedObjPath = currentSelectTransform.parent.name + "/" + selectedObjPath;
        currentSelectTransform = currentSelectTransform.parent;
    }
    //设置空物体的层级，使之与原物体处于同一层级
    obj.transform.SetParent(GameObject.Find(selectedObjPath).transform);
    transform.SetParent(obj.transform);
}
}
