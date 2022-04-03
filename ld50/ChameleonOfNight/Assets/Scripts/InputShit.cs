using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputShit : MonoBehaviour
{
    public EventSystem eventSystem;
    public LayerMask insectPlaneLayer;
    public ChameleonController chameleonController;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //if over UI dont to the stuff
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            var cursorWorldPos = GetPointUnderCursor();
            
            if(Input.GetMouseButtonDown(0))
            {
                chameleonController.Click(cursorWorldPos);
            }
        }
    }

    private Vector3 GetPointUnderCursor()
    {
        var ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, 100f, insectPlaneLayer);
        return hit.point;
    }
}
