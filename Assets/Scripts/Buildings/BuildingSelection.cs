using UnityEngine;

public class BuildingSelection : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private GameObject selectedObject;

    private bool _anyMenuOpen;

    private void OnEnable()
    {
        eventBus.selectedBuilding.AddListener(SelectObject);
        eventBus.buildingUIOpened.AddListener(AnyMenuOpen);
    }

    private void OnDisable()
    {
        eventBus.selectedBuilding.RemoveListener(SelectObject);
        eventBus.buildingUIOpened.RemoveListener(AnyMenuOpen);
    }

    private void AnyMenuOpen(bool open)
    {
        _anyMenuOpen = open;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_anyMenuOpen)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // raycast from camera to mouse position
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000, LayerMask.GetMask("Placeable")))
            {
                eventBus.selectedBuilding.Invoke(hitInfo.collider.gameObject);
            }
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            eventBus.selectedBuilding.Invoke(null);
        }
    }

    private void SelectObject(GameObject gObj)
    {
        if (gObj == selectedObject) return;
        if (selectedObject != null)
        {
            DeselectObject();
            return;
        }

        if (gObj.TryGetComponent<Outline>(out var outline))
        {
            outline.enabled = true;
        }
        else
        {
            gObj.AddComponent<Outline>();
        }

        selectedObject = gObj;
    }

    private void DeselectObject()
    {
        if (selectedObject == null) return;
        selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
    }

    public void MoveObject()
    {
        eventBus.pendingBuilding.Invoke(selectedObject);
    }
}
