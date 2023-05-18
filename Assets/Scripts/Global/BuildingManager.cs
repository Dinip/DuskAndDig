using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public GameObject pendingObject;

    private Vector3 pos;

    public bool canPlace = true;

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
    private BuildingsGameStateObject gameStateObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        gameStateObject.LoadState(buildings);
    }

    void Update()
    {
        if (pendingObject != null)
        {
            pendingObject.transform.position = new Vector3(RoundToNearestGrid(pos.x), RoundToNearestGrid(pos.y), RoundToNearestGrid(pos.z));

            UpdateMaterials();

            if (Input.GetMouseButtonDown(0) && canPlace)
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
        pendingObject.GetComponent<MeshRenderer>().material = materials[2];
        pendingObject = null;
    }

    public void RotateObject()
    {
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }

    private void DeselectObject()
    {
        if (pendingObject == null) return;
        Destroy(pendingObject);
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000, layerMask))
        {
            pos = raycastHit.point;
        }
    }

    private void UpdateMaterials()
    {
        if (canPlace)
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[0];
        }
        else
        {
            pendingObject.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    public void SelectObject(int index)
    {
        pendingObject = Instantiate(buildings[index], pos, transform.rotation);
        materials[2] = pendingObject.GetComponent<MeshRenderer>().material;
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
            gameStateObject.SaveState();
        }
        SceneManager.LoadScene(name);
    }
}
