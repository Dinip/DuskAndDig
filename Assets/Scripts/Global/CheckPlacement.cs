using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
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
