using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [SerializeField] private int _hpDelta;


    public void ApplyHealthDelta(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent == null) return;
        if (healthComponent.Immune) return;
        healthComponent.TakeDmg(_hpDelta);
        healthComponent.OnImmune();

    }


}
