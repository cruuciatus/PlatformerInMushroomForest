using System;
using UnityEngine;



    public class ModifyHealthComponent : MonoBehaviour
{
    [SerializeField] private int _hpDelta;

   

    public void SetDelta(int delta)
        {
            _hpDelta = delta;
        }

        public void Apply(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.TakeDmg(_hpDelta);
            }
        }

  
}
