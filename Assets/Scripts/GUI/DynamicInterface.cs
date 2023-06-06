using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    [SerializeField]
    private GameObject slotPrefab;

    [SerializeField]
    private int spaceBetweenItems;

    [SerializeField]
    private int NumberOfColumns;

    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i, inventory.GetSlots.Length);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventory.GetSlots[i].slotDisplay = obj;

            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
        }
    }

    //calculate position of slot in inventory based on the index and
    //available space in the display panel
    //width/2, height/2 as center and use space between items to calculate
    private Vector3 GetPosition(int i, int slots)
    {
        int numRows = Mathf.CeilToInt((float)slots / NumberOfColumns);

        float xPos = ((i % NumberOfColumns) - (NumberOfColumns - 1) / 2.0f) * spaceBetweenItems;
        float yPos = ((numRows - 1) / 2.0f - (i / NumberOfColumns)) * spaceBetweenItems;

        return new Vector3(xPos, yPos, 0f);
    }
}
