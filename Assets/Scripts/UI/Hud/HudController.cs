using System.Collections;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private ProgressBarWidget _healthBar;
    [SerializeField] private CurrentPerkWidget _currentPerk;

    private GameSession _session;
    private readonly CompositeDisposable _trash = new CompositeDisposable();


    private HealthComponent healthComponent;

    private void Start()
    {
        StartCoroutine(GetLinks());

        _session = FindObjectOfType<GameSession>();
        //_trash.Retain(_session.Data.HP.SubscribeAndInvoke(OnHealthChanged));
        _trash.Retain(_session.PerksModel.Subscribe(OnPerkChanged));

        OnPerkChanged();
    }

    private IEnumerator GetLinks()
    {
        Hero hero = null;

        while (hero == null)
        {
            hero = FindObjectOfType<Hero>();
            yield return new WaitForSeconds(0.2f);
        }

        while (healthComponent == null)
        {
            healthComponent = hero.GetComponent<HealthComponent>();
            yield return new WaitForSeconds(0.2f);
        }

        healthComponent.OnHealthChange += HealthChanged;
    }

    private void OnPerkChanged()
    {
        var usedPerkId = _session.PerksModel.Used;
        var hasPerk = !string.IsNullOrEmpty(usedPerkId);
        if (hasPerk)
        {
            var perkDef = DefsFacade.I.Perks.Get(usedPerkId);
            _currentPerk.Set(perkDef);
        }

        _currentPerk.gameObject.SetActive(hasPerk);
    }



    private void HealthChanged(int health, int maxHealth)
    {
        var value = (float)health / maxHealth;
        _healthBar.SetProgress(value);
    }

    public void OnSettings()
    {
        WindowUtils.CreateWindow("UI/InGameMenuWindow");
    }
    private void OnDestroy()
    {
        _trash.Dispose();
    }

    public void ShowDebud()
    {
        WindowUtils.CreateWindow("UI/PlayerStatsWindow");
    }

    public void ShowPerks()
    {
        WindowUtils.CreateWindow("UI/ManagePerksWindow");
    }


    /////////////////// OBSOLETE  //////////////////////////////////

    private void OnHealthChanged(int newValue, int oldValue)
    {
        var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
        var value = (float)newValue / maxHealth;
        _healthBar.SetProgress(value);
    }
}
