using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] UIInventoryPage _inventoryUI;

    //추후 Inventory Model 만들어 뺄것
    public int InventorySize = 10;


    private void Start()
    {
        _inventoryUI.InityInventoryUI(InventorySize);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_inventoryUI.isActiveAndEnabled == false)
            {
                _inventoryUI.Show();
            }
            else
            {
                _inventoryUI.Hide();
            }
        }
    }
}
