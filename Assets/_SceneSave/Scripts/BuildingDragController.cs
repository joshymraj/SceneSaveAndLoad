using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDragController : MonoBehaviour
{
    public Vector2 DragBoundary { get; set; } // Can be set outside for planes of various sizes
    Vector3 mouseOffset;

    float _zPos;

    private void Start() {
        DragBoundary = new Vector2(9, 9); // For prototyping only. Otherwise set it in SceneController.
    }

    void OnMouseDown() {
        _zPos = Camera.main.WorldToScreenPoint(transform.position).z;

        mouseOffset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag() {
        Vector3 draggedPosition = GetMouseWorldPosition() + mouseOffset;

        if(Mathf.Abs(draggedPosition.x) > DragBoundary.x)
        {
            draggedPosition.x = Mathf.Sign(draggedPosition.x) * DragBoundary.x;
        }

        if(Mathf.Abs(draggedPosition.y) > DragBoundary.y)
        {
            draggedPosition.y = Mathf.Sign(draggedPosition.y) * DragBoundary.y;
        }

        transform.position = new Vector3(draggedPosition.x, 0, draggedPosition.y);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = _zPos;

        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
