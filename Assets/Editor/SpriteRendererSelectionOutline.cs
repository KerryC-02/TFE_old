using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class SpriteRendererSelectionOutline
{
    static SpriteRendererSelectionOutline()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        Selection.selectionChanged += OnSelectionChanged;
    }

    private static void OnSelectionChanged()
    {
        SceneView.RepaintAll();
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (Selection.gameObjects.Length > 0)
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null)
                {
                    DrawOutline(spriteRenderer);
                }
            }
        }
    }

    private static void DrawOutline(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer.sprite == null) return;

        // Get the sprite's corners
        Bounds bounds = spriteRenderer.sprite.bounds;
        Vector3 p1 = spriteRenderer.transform.TransformPoint(new Vector3(bounds.min.x, bounds.min.y, 0));
        Vector3 p2 = spriteRenderer.transform.TransformPoint(new Vector3(bounds.max.x, bounds.min.y, 0));
        Vector3 p3 = spriteRenderer.transform.TransformPoint(new Vector3(bounds.max.x, bounds.max.y, 0));
        Vector3 p4 = spriteRenderer.transform.TransformPoint(new Vector3(bounds.min.x, bounds.max.y, 0));

        // Draw the outline
        Handles.color = Color.green; // You can change the color
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);
        Handles.DrawLine(p3, p4);
        Handles.DrawLine(p4, p1);
    }
}
