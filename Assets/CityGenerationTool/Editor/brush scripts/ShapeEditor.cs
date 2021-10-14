using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator shapeCreator;
    bool needsRepaint;
    SelectionInfo selectionInfo;

    void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if(guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        else if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            HandleInput(guiEvent);
            if (needsRepaint)
            {
                HandleUtility.Repaint();
            }
        }
    }

    void HandleInput(Event guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float dstToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 mousePosition = mouseRay.GetPoint(dstToDrawPlane);

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
        {
            Undo.RecordObject(shapeCreator, "Add point");
            shapeCreator.points.Add(mousePosition);

            needsRepaint = true;
        }

        UpdateMouseOverSelection(mousePosition);

        
    }

    void UpdateMouseOverSelection(Vector3 mousePosition)
    {
        int mouseOverPointIndex = -1;
        for (int i = 0; i < shapeCreator.points.Count; i++)
        {
            if (Vector3.Distance(mousePosition, shapeCreator.points[i]) < shapeCreator.handleRadius)
            {
                mouseOverPointIndex = i;
                break;
            }
        }

        if (mouseOverPointIndex != selectionInfo.pointIndex)
        {
            selectionInfo.pointIndex = mouseOverPointIndex;
            selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

            needsRepaint = true;
        }
    }

    void Draw()
    {
        for(int i = 0; i < shapeCreator.points.Count; i++)
        {
            Vector3 nextPoint = shapeCreator.points[(i + 1) % shapeCreator.points.Count];
            Handles.DrawDottedLine(shapeCreator.points[i], nextPoint, 4);
            Handles.DrawDottedLine(shapeCreator.points[i], nextPoint, 4);

            if(i == selectionInfo.pointIndex)
            {
                Handles.color = Color.red;
            }
            else
            {
                Handles.color = Color.white;
            }
            
            Handles.DrawSolidDisc(shapeCreator.points[i], Vector3.up, shapeCreator.handleRadius);
        }
        needsRepaint = false;
    }

    void OnEnable()
    {
        shapeCreator = target as ShapeCreator;
        selectionInfo = new SelectionInfo();
    }

    public class SelectionInfo
    {
        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
    }
}
