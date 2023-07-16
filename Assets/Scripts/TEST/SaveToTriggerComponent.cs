using UnityEngine;

public class SaveToTriggerComponent : MonoBehaviour
{
    CheckPointComponent checkPointComponent;
    GameSession _session;

    public void Start()
    {
        checkPointComponent = GetComponent<CheckPointComponent>();
        _session = FindObjectOfType<GameSession>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _session.Save();
        _session.SetChecked(checkPointComponent.Id);
        Destroy(this);
    }
}
