using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCheckComponent : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private bool _isTouchingLayer;
    private Collider2D _collider;


    public bool IsTouchingLayer=> _isTouchingLayer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _isTouchingLayer = _collider.IsTouchingLayers(_layer);

    }

    private void OnTriggerExit2D(Collider2D other)  
    {
        _isTouchingLayer = _collider.IsTouchingLayers(_layer);
    }
}
