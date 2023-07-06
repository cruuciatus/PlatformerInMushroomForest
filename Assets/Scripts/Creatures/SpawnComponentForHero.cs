using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComponentForHero : SpawnComponent
{
    public override GameObject SpawnInstance()
    {
        var instantiate = Instantiate(_prefab, _target.position, Quaternion.identity);
        instantiate.transform.localScale = _target.lossyScale * 2;
        instantiate.SetActive(true);

        return instantiate;
    }
}
