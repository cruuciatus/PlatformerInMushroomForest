using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]
public class ItemsRepository : DefRepository<ItemDef>
{
    
}


[Serializable]

public struct ItemDef : IHaveId
{
    [SerializeField] private string _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private ItemTag[] _tags;
    public string Id => _id;


    public bool IsVoid => string.IsNullOrEmpty(_id);

    public Sprite Icon => _icon;

    public bool HasTag(ItemTag tag)
    {
        return _tags?.Contains(tag)??false;
    }
}