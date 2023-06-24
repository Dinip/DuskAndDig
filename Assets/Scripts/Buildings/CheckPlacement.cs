using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    [SerializeField]
    private EventBus buildingObject;

    private bool _collidingPlayer = false;
    private bool _collidingPlaceable = false;
    private bool _collidingCenter = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            _collidingPlaceable = true;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            _collidingPlayer = true;
        }
        else if (other.gameObject.CompareTag("Center"))
        {
            _collidingCenter = true;
        }

        buildingObject.canPlaceBuilding?.Invoke(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            _collidingPlaceable = false;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            _collidingPlayer = false;
        }
        else if (other.gameObject.CompareTag("Center"))
        {
            _collidingCenter = false;
        }

        if (!_collidingPlayer && !_collidingPlaceable && !_collidingCenter)
        {
            buildingObject.canPlaceBuilding?.Invoke(true);
        }
    }
}
