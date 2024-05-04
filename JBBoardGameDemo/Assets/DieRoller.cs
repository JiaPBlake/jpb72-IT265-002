using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRoller : MonoBehaviour
{
    //[SerializeField] private DieRollerMode mode = DieRollerMode.EQ;
    [SerializeField] private TMPro.TextMeshProUGUI action;
    [SerializeField] private TMPro.TextMeshProUGUI resultOfRoll;
    //private string rollAction;

    [ContextMenu("Test Roll")]


    public int Roll(string rollAction)
    {
        int result = -1;
        result = 1 + Random.Range(0, 6);
        
        Debug.Log($"Rolled the dice and got {result}");
        if (resultOfRoll != null)
        {
            resultOfRoll.text = $"{result}";
        }
        if (action != null)
        {
            action.text = $"{rollAction}";
        }

        return result;

    }
}

/*public enum DieRollerMode
{

}*/
