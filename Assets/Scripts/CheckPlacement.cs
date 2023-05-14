using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour {

    //private BuildingManager _buildingManager;

    //void Start()
    //{
    //    _buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            BuildingManager.Instance.canPlace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            BuildingManager.Instance.canPlace = true;
        }
    }
}
