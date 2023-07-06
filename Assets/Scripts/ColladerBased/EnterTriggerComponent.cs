using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnterCollisionComponent;

public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;
        [SerializeField] private LayerMask _layer =~0;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.IsLayer(_layer)) return;
        if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return;

        _action?.Invoke(other.gameObject);
    }

}

