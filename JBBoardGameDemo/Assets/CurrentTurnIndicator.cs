using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTurnIndicator : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textField;

    public void SetText(string text)
    {
        if (textField != null)
        {
            textField.text = text;
        }
    }
   
}
