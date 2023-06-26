using UnityEngine;

public class CityBuildingCameraController : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private float dragSpeed = 2;

    [SerializeField]
    private float zoomSpeed = 2;

    [SerializeField]
    private float maxY = 12;

    [SerializeField]
    private float minY = 5;

    private bool isDragging = false;

    private Vector3 dragOrigin;

    private Vector3 previousMousePosition;

    private bool _paused;

    private void OnEnable()
    {
        eventBus.gamePaused.AddListener(HandlePause);
    }

    private void OnDisable()
    {
        eventBus.gamePaused.RemoveListener(HandlePause);
    }

    private void HandlePause(bool paused)
    {
        _paused = paused;
    }

    void Update()
    {
        if (_paused) return;
        HandleCameraDrag();
        HandleCameraZoom();
    }

    void HandleCameraDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            dragOrigin = Input.mousePosition;
            previousMousePosition = dragOrigin;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - previousMousePosition;
            Vector3 move = new Vector3(-mouseDelta.x, 0, -mouseDelta.y) * dragSpeed;
            transform.Translate(move, Space.World);
            previousMousePosition = currentMousePosition;
        }
    }

    void HandleCameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = scroll * zoomSpeed * transform.forward;
        var currentTransform = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.Translate(zoom, Space.World);
        if (transform.position.y > maxY || transform.position.y < minY)
        {
            transform.position = currentTransform;
        }
    }
}
