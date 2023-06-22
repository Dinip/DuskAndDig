using UnityEngine;

public class BuildingSelection : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    private GameObject _selectedObject;

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
        if (gObj == _selectedObject) return;
        if (_selectedObject != null)
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

        _selectedObject = gObj;
    }

    private void DeselectObject()
    {
        if (_selectedObject == null) return;
        _selectedObject.GetComponent<Outline>().enabled = false;
        _selectedObject = null;
    }

    public void MoveObject()
    {
        eventBus.pendingBuilding.Invoke(_selectedObject);
    }
}
