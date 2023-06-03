using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    [SerializeField]
    private EventBus buildingObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            buildingObject.canPlaceBuilding.Invoke(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            buildingObject.canPlaceBuilding.Invoke(true);
        }
    }
}
