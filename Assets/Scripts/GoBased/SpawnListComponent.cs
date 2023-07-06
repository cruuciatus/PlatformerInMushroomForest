using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnListComponent : MonoBehaviour
{
    [SerializeField] private SpawnnData[] _spawners;


    public void SpawnAll()
    {
        foreach (var spawnData in _spawners)
        {
            spawnData.Component.Spawn();
        }
    }
    public void Spawn(string id)
    {

        var spawner = _spawners.FirstOrDefault(element => element.id == id);
        spawner?.Component.Spawn();
       
    }
    [Serializable]
    public class SpawnnData
    {
        public string id;
        public SpawnComponent Component;
    }

   
}
