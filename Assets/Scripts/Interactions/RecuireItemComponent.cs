using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecuireItemComponent : MonoBehaviour
{
    [SerializeField] private InventoryItemData[] _required;
    //[InventoryIdAtribute] [SerializeField] private string _id;
    //[SerializeField] private int _coint;
    [SerializeField] private bool _removeAfterUse;

    [SerializeField] private UnityEvent _onSuccess;
    [SerializeField] private UnityEvent _inFail;
    public void Check()
    {
        var session = FindObjectOfType<GameSession>();
        var areAllRequirementsMet = true;
        foreach (var item in _required)
        {
            var numItems = session.Data.Inventory.Count(item.Id);
            if (numItems < item.Value)
            {
                areAllRequirementsMet = false;
            }
        }

        if (areAllRequirementsMet )
        {
            if (_removeAfterUse)
            {
                foreach (var item in _required)
                {
                    session.Data.Inventory.Remove(item.Id, item.Value);
                }
            }
            _onSuccess?.Invoke();
        }
        else
        {
            _inFail?.Invoke();

        }
    }

}
