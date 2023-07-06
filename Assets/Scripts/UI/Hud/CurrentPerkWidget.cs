using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPerkWidget : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _cooldownImage;
    private GameSession _session;


    public void Start()
    {
        _session = FindObjectOfType<GameSession>();
    }
    public void Set(PerkDef perkDef)
    {
        _icon.sprite = perkDef.Icon;
    }

    private void Update()
    {

        var cooldown = _session.PerksModel.Cooldown;
        _cooldownImage.fillAmount = cooldown.RemainingTime / cooldown.Value;

    }
}
