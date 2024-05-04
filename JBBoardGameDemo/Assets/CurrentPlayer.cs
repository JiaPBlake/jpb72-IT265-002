using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayer : MonoBehaviour
{
    //[SerializeField] private Player currentPlayer;  I wanted to just get the player number by assigning each object a number but I really don't know how to do that
    [SerializeField] private TMPro.TextMeshProUGUI Inventorytext;
    [SerializeField] private TMPro.TextMeshProUGUI InventoryTitle;

    public void SetTitle(string text)
    {
        if (InventoryTitle != null)
        {
            InventoryTitle.text = text;
        }

    }
    public void SetInventory(string text)
    {
        if (Inventorytext != null)
        {
            Inventorytext.text = text;
        }
    }
    
}
