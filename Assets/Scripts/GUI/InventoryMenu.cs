using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    //[SerializeField]
    //private EventBus eventBus;

    [SerializeField]
    private GameObject inventory;

    [SerializeField]
    private GameObject equipment;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.SetActive(!inventory.activeSelf);
            equipment.SetActive(!equipment.activeSelf);
        }
    }
}
