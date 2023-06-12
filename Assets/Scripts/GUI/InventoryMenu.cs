using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    //[SerializeField]
    //private EventBus eventBus;

    [SerializeField]
    private GameObject inventory;

    [SerializeField]
    private GameObject equipment;

    private bool _showing = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _showing = !_showing;
            inventory.SetActive(_showing);
            equipment.SetActive(_showing);
        }
    }
}
