using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogContent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _icon;

    public TextMeshProUGUI Text => _text;

    public void TrySetIcon(Sprite icon)
    {
        if (_icon != null)
            _icon.sprite = icon;
    }
}
