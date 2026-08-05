#if UNITY_EDITOR
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
public class HideCanvasInEditor : MonoBehaviour
{
    private Canvas canvas;

    [Tooltip("If true, the canvas will be enabled in the editor, otherwise it will be hidden.")]
    [SerializeField]
    private bool habilitarCanvas = false;

    void OnEnable()
    {
        canvas = GetComponent<Canvas>();
        UpdateCanvasVisibility();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            UpdateCanvasVisibility();
        }
    }

    void UpdateCanvasVisibility()
    {
        if (canvas != null)
        {
            canvas.enabled = Application.isPlaying || habilitarCanvas;
        }
    }
}
#endif