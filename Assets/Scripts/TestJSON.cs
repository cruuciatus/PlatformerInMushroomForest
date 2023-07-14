using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJSON : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int maxHp;
    [SerializeField] float damage;
    private TestJSON testJSON;
    public void Save()
    {
        var json = JsonUtility.ToJson(this);
        testJSON = JsonUtility.FromJson<TestJSON>(json);

    }
    public void Load()
    {
   //     this = Save();
    }

}
