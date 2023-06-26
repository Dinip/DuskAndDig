using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    private GameObject _pendingObject;

    private Vector3 _pos;

    private bool _canPlace = true;

    [SerializeField]
    private GameObject[] buildings;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float gridSize = 1f;

    [SerializeField]
    private float rotateAmount = 90f;

    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private BuildingsSet buildingsSet;

    private void OnEnable()
    {
        eventBus.pendingBuilding.AddListener(PendingBuildSelect);
        eventBus.canPlaceBuilding.AddListener(CanPlace);
        eventBus.placeBuilding.AddListener(SelectObject);
        LoadState();
    }

    private void OnDisable()
    {
        eventBus.pendingBuilding.RemoveListener(PendingBuildSelect);
        eventBus.canPlaceBuilding.RemoveListener(CanPlace);
        eventBus.placeBuilding.RemoveListener(SelectObject);
    }

    private void PendingBuildSelect(GameObject build)
    {
        _pendingObject = build;
    }

    private void CanPlace(bool can)
    {
        _canPlace = can;
    }

    private void LoadState()
    {
        foreach (var buildSet in buildingsSet.Items)
        {
            for (int i = 0; i < buildings.Length; i++)
            {
                var build = buildings[i].GetComponent<BuildingController>().building;
                if (buildSet.buildingType == build.buildingType)
                {
                    var b = Instantiate(buildings[i], buildSet.Position, buildSet.Rotation);
                    var bc = b.GetComponent<BuildingController>();
                    bc.building = buildSet;
                    bc.placed = true;
                }
            }
        }
    }

    //private void SaveState()
    //{
    //    buildingsSet.Clear();
    //    var currentBuildings = GameObject.FindGameObjectsWithTag("Placeable");
    //    for (int i = 0; i < currentBuildings.Length; i++)
    //    {
    //        BuildingStateModel bs = new BuildingStateModel
    //        {
    //            Name = currentBuildings[i].name.Replace("(Clone)", ""),
    //            Position = currentBuildings[i].transform.position,
    //            Rotation = currentBuildings[i].transform.rotation
    //        };
    //        buildingsSet.Add(bs);
    //    }
    //}

    void Update()
    {
        if (_pendingObject != null)
        {
            _pendingObject.transform.position = new Vector3(RoundToNearestGrid(_pos.x), RoundToNearestGrid(_pos.y), RoundToNearestGrid(_pos.z));
            _pendingObject.GetComponent<MeshCollider>().enabled = false;

            UpdateMaterials();

            if (Input.GetMouseButtonDown(0) && _canPlace)
            {
                PlaceObject();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectObject();
            }
        }
    }

    public void PlaceObject()
    {
        _pendingObject.GetComponent<MeshRenderer>().material = materials[2];
        _pendingObject.GetComponent<MeshCollider>().enabled = true;
        var bc = _pendingObject.GetComponent<BuildingController>();
        if (!bc.UpgradeBuilding())
        {
            _pendingObject = null;
        }
        var b = bc.building;
        b.Position = _pendingObject.transform.position;
        b.Rotation = _pendingObject.transform.rotation;
        bc.placed = true;
        buildingsSet.Add(b);
        _pendingObject = null;
    }

    public void RotateObject()
    {
        _pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }

    private void DeselectObject()
    {
        if (_pendingObject == null) return;
        Destroy(_pendingObject);
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000, layerMask))
        {
            _pos = raycastHit.point;
        }
    }

    private void UpdateMaterials()
    {
        if (_canPlace)
        {
            _pendingObject.GetComponent<MeshRenderer>().material = materials[0];
            return;
        }
        _pendingObject.GetComponent<MeshRenderer>().material = materials[1];
    }

    public void SelectObject(int index)
    {
        _pendingObject = Instantiate(buildings[index], _pos, transform.rotation);
        materials[2] = _pendingObject.GetComponent<MeshRenderer>().material;
    }

    private float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }

    public void ChangeScene(string name)
    {
        if (name == "CityBuilder2")
        {
            //gameStateObject.SaveState();
        }
        SceneManager.LoadScene(name);
    }
}
