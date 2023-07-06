using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CheckCircleOvetlap : MonoBehaviour
{

    [SerializeField] private Vector2 _radius;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private OnOverLapEvent _onOverLap;
    [SerializeField] private string[] _tags;

    private readonly Collider2D[] _interactionResults = new Collider2D[10];
   
    public void Check()
    {
        var size = Physics2D.OverlapAreaNonAlloc(transform.position, _radius, _interactionResults, _mask);
       
        for (var i = 0; i < size; i++)
        {
            var overLapResult = _interactionResults[i];
            var isInTags = _tags.Any(tag => overLapResult.CompareTag(tag));
            if (isInTags)
            {
                _onOverLap?.Invoke(_interactionResults[i].gameObject);
            }
            
        }
    }
    [Serializable]
    public class OnOverLapEvent : UnityEvent<GameObject>
    {

    }
}
