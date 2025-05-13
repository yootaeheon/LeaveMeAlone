using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] UIInventoryPage _inventoryUI;

    [SerializeField] InventorySO _inventoryData;

    private void Start()
    {
        _inventoryUI.InityInventoryUI(_inventoryData.Size);
        /*_inventoryData.Init();*/
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_inventoryUI.isActiveAndEnabled == false)
            {
                _inventoryUI.Show();
                foreach (var item in _inventoryData.GetCurInventoryState())
                {
                    _inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.Quantity);
                }
            }
            else
            {
                _inventoryUI.Hide();
            }
        }
    }
}
