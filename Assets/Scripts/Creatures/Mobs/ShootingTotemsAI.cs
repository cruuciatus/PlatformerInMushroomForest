using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTotemsAI : MonoBehaviour
{
    [SerializeField] private LayerCheckComponent _vision;
    [SerializeField] private Cooldown _cooldown;
    [SerializeField] private AnimationSprite _animation;

    private void Update()
    {
        if (_vision.IsTouchingLayer && _cooldown.IsReady)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _cooldown.Reset();
        _animation.SetClip("start-attack");
    }
}
