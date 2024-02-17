using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PolygonCollider2DController : MonoBehaviour
{
    public Vector3 checker;
    public Vector3 p1;
    public Vector3 p2;
}


[CustomEditor(typeof(PolygonCollider2DController))]
public class PolygonCollider2DControllerEditor : Editor
{
    private PolygonCollider2DController controller;

    private void OnEnable()
    {
        controller = (PolygonCollider2DController)target;
    }

    private void OnSceneGUI()
    {
        controller.p1 = Handles.PositionHandle(controller.p1, Quaternion.identity);
        controller.p2 = Handles.PositionHandle(controller.p2, Quaternion.identity);
        controller.checker = Handles.PositionHandle(controller.checker, Quaternion.identity);

        Handles.DrawLine(controller.p1, controller.p2);

        Debug.Log(IsPointOnLine(controller.checker, controller.p1, controller.p2));
    }

    // Check if point p is approximately on the line segment between points p1 and p2
    private bool IsPointOnLine(Vector3 p, Vector3 p1, Vector3 p2, float tolerance = 0.1f)
    {
        Vector3 t = Vector3.zero;

        // Handle division by zero
        if (p2.x - p1.x != 0)
            t.x = (p.x - p1.x) / (p2.x - p1.x);
        else
            t.x = float.PositiveInfinity;

        if (p2.y - p1.y != 0)
            t.y = (p.y - p1.y) / (p2.y - p1.y);
        else
            t.y = float.PositiveInfinity;

        // Compare t.x and t.y, and ensure they are within the range [0, 1], within tolerance
        return Mathf.Abs(t.x - t.y) <= tolerance && t.x >= 0 && t.y >= 0 && t.x <= 1 && t.y <= 1;
    }

}