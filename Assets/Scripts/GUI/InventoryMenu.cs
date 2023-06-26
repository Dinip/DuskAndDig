using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private GameObject inventory;

    [SerializeField]
    private GameObject equipment;

    private bool _paused;

    private bool _lastStateOpen = true;

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
        if (paused)
        {
            inventory.SetActive(false);
            equipment.SetActive(false);
        } else
        {
            inventory.SetActive(_lastStateOpen);
            equipment.SetActive(_lastStateOpen);
        }
    }

    private void Update()
    {
        if (_paused) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _lastStateOpen = !_lastStateOpen;
            inventory.SetActive(_lastStateOpen);
            equipment.SetActive(_lastStateOpen);
        }
    }
}
