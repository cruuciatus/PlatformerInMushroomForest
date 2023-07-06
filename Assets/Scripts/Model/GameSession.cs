using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{

    [SerializeField] private PlayerData _data;
    [SerializeField] private string _defaultCheckPoint;

    private readonly CompositeDisposable _trash = new CompositeDisposable();
    public PlayerData Data => _data;
    private PlayerData _save;


    public QuickInventoryModel QuickInventory { get; private set; }

    public PerksModel PerksModel { get; private set; }
    public StatsModel StatsModel { get; private set; }

    private readonly List<string> _checkpoints = new List<string>();

    private void Start()
    {
        var existsSession = GetExistsSession();
        if (existsSession != null)
        {
            existsSession.StartSession(_defaultCheckPoint);

            QuickInventory?.Subscribe();
            Destroy(gameObject);
        }
        else
        {
            Save();
            InitModels();
            DontDestroyOnLoad(this);
            StartSession(_defaultCheckPoint);
        }
    }

    private void StartSession(string defaultCheckPoint)
    {
        SetChecked(defaultCheckPoint);
        LoadHud();
        LoadUIs();
        SpawnHero();
    }

    private void SpawnHero()
    {
        var checkpoints = FindObjectsOfType<CheckPointComponent>();
        var lastCheckPoint = _checkpoints.Last();
        foreach (var checkPoint in checkpoints)
        {
            if (checkPoint.Id == lastCheckPoint)
            {
                checkPoint.SpawnHero();
                break;
            }
        }
    }
    private void InitModels()
    {
        QuickInventory = new QuickInventoryModel(_data);
        QuickInventory.Subscribe();
        //_trash.Retain(QuickInventory);

        PerksModel = new PerksModel(_data);
        _trash.Retain(PerksModel);

        StatsModel = new StatsModel(_data);
        _trash.Retain(StatsModel);

        //_data.HP.Value = (int)StatsModel.GetValue(StatId.Hp);
        //_data.MaxHP.Value = (int)StatsModel.GetValue(StatId.MaxHP); 
    }

    private void LoadUIs()
    {
        SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        LoadOnScreenControls();
    }


    [Conditional("USE_ONSCREEN_CONTROLS")]
    private void LoadOnScreenControls()
    {
        SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
    }

    private void LoadHud()
    {
        SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
    }
    private GameSession GetExistsSession()
    {
        var sessions = FindObjectsOfType<GameSession>();

        foreach (var gameSession in sessions)
        {
            if (gameSession != this)

                return gameSession;
        }
        return null;
    }

    public void Save()
    {
        _save = _data.Clone();
    }
    public void LoadLastSave()
    {
        PlayerData tmp = _save.Clone();
        //  _data = _save.Clone();
        //_data.HP = tmp.HP;
        _data.MaxHP = tmp.MaxHP;
        _data.HP = _data.MaxHP;
        _trash.Dispose();
    }

    public bool IsChecked(string id)
    {
        return _checkpoints.Contains(id);
    }

    public void SetChecked(string id)
    {
        if (!_checkpoints.Contains(id))
        {
            Save();
            _checkpoints.Add(id);
        }

    }

    private readonly List<string> _removedItems = new List<string>();

    public bool RestoreState(string id)
    {
        return _removedItems.Contains(id);
    }

    public void StoreState(string id)
    {
        if (!_removedItems.Contains(id))
            _removedItems.Add(id);
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
