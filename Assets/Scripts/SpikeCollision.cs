using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpikeCollision : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private EnterEvent _action;
    [SerializeField] private float delayDamage;
    GameObject other;

    private void OnCollisionEnter2D(Collision2D other)
    {
        this.other = other.gameObject;
        if (other.gameObject.CompareTag(_tag))
        {
            StartCoroutine(StartDamage());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (other.gameObject.CompareTag(_tag))
        {
            StopAllCoroutines();
        }
    }
    private IEnumerator StartDamage()
    {
      
        while (true)
        {

            _action?.Invoke(other);
            yield return new WaitForSeconds(delayDamage);


        }
    }
    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {

    }
}
