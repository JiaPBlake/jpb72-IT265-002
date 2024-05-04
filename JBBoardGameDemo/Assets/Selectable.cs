using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] private GameObject selectedIndicator;
    
    public void SetSelected(bool isSelected) {
        if(selectedIndicator != null)
        {
            selectedIndicator.SetActive(isSelected);
        }
    }
}
