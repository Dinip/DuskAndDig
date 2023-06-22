using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemUI : MonoBehaviour {
    [SerializeField]
    private EventBus eventBus;

    [SerializeField]
    private TextMeshProUGUI fromAmount;

    [SerializeField]
    private Image fromImage;

    [SerializeField]
    private TextMeshProUGUI toAmount;

    [SerializeField]
    private Image toImage;

    private ItemToItem _item;

    private ItemToItem _currentCraft;

    private void OnEnable()
    {
        eventBus.itemToCraft.AddListener(SelectedCraft);
    }

    private void OnDisable()
    {
        eventBus.itemToCraft.RemoveListener(SelectedCraft);
    }

    private void SelectedCraft(ItemToItem item)
    {
        if (_currentCraft == _item)
        {
            GetComponent<Image>().color = new Color(0.45f, 0.45f, 0.45f);
            _currentCraft = null;
            return;
        }

        if (item == _item)
        {
            GetComponent<Image>().color = Color.green;
            _currentCraft = item;
            return;
        }

        GetComponent<Image>().color = new Color(0.45f, 0.45f, 0.45f);
        _currentCraft = item;
    }

    public void Initialize(ItemToItem item, ItemToItem currentCraft)
    {
        fromAmount.text = $"{item.fromAmount:D2}x";
        fromImage.sprite = item.from.uiDisplay;
        toAmount.text = $"{item.toAmount:D2}x";
        toImage.sprite = item.to.uiDisplay;
        _item = item;
        _currentCraft = currentCraft;
        SelectedCraft(_currentCraft);
    }

    public void SelectCraft()
    {
        eventBus.itemToCraft?.Invoke(_item == _currentCraft ? null : _item);
    }
}
