using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Dialog", fileName = "Dialog")]
public class DialogDef : ScriptableObject
{
    [SerializeField] private DialogData _data;
    public DialogData Data => _data;
}