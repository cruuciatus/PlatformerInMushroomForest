using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDataForSave
{
    public List<InventoryItemData> _inventoryData = new List<InventoryItemData>();
    public IntProperty HP = new IntProperty();
    //public PerksData Perks = new PerksData();
    public LevelData Levels = new LevelData();

    public string _used = "";
    public List<string> _unlocked;

    public string CurrentCheckPoint = "";

    public string SceneName = "";
}
