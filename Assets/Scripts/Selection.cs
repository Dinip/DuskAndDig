using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour {
    public GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // raycast from camera to mouse position
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000))
            {
                if (hitInfo.collider.gameObject.CompareTag("Placeable"))
                {
                    SelectObject(hitInfo.collider.gameObject);
                }
            }
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectObject();
        }
    }

    private void SelectObject(GameObject gObj)
    {
        if (gObj == selectedObject) return;
        if (selectedObject != null) DeselectObject();

        Outline outline = gObj.GetComponent<Outline>();

        if (outline == null)
        {
            gObj.AddComponent<Outline>();
        }
        else
        {
            outline.enabled = true;
        }

        selectedObject = gObj;
    }

    private void DeselectObject()
    {
        if(selectedObject == null) return;
        selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
    }

    public void MoveObject()
    {
        BuildingManager.Instance.pendingObject = selectedObject;
    }
}
