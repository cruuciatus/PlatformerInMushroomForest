using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInventoryController : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private InventoryItemWidget _prefab;


    private readonly CompositeDisposable _trash = new CompositeDisposable();

    private GameSession _session;
    private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;

    private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();


    private void Start()
    {
        _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefab, _container);
        _session = FindObjectOfType<GameSession>();
        _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));
       // _session.Data.Inventory.onAdd += Rebuild;
        Rebuild();
    }


    private void Rebuild()
    {
        var inventory = _session.QuickInventory.Inventory;
        _dataGroup.SetData(inventory);
    }
    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
