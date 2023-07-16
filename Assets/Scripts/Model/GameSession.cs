using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    [SerializeField] private string _defaultCheckPoint;

    private readonly CompositeDisposable _trash = new CompositeDisposable();
    public PlayerData Data => _data;
    private PlayerDataForSave DataForSave;

    private bool isStartGame;
    private bool nextLevel;


    public QuickInventoryModel QuickInventory { get; private set; }

    public PerksModel PerksModel { get; private set; }
    public StatsModel StatsModel { get; private set; }


    private void Start()
    {
        var existsSession = GetExistsSession();
        if (existsSession != null)
        {
            if (StateLoadGame.IsBegin == false)
                existsSession.LoadLastSave();
            existsSession.StartSession();

            StateLoadGame.IsBegin = false;

            QuickInventory?.Subscribe();
            Destroy(gameObject);
            existsSession.Invoke(nameof(StartOff), 2f);
        }
        else
        {
            InitModels();
            DontDestroyOnLoad(this);

            if (StateLoadGame.IsBegin == false)
                LoadLastSave();
            StartSession();

            StateLoadGame.IsBegin = false;
            Invoke(nameof(StartOff), 2f);
        }
    }

    public void StartOff()
    {
        isStartGame = false;
        nextLevel = false;
    }

    public void NextLevel()
    {
        nextLevel = true;
    }

    public void StartSession()
    {
        CheckIsBeginGame();
        LoadUIs();
        SpawnHero(_data.CurrentCheckPoint);
    }

    public void CheckIsBeginGame()
    {
        if (StateLoadGame.IsBegin || _data.CurrentCheckPoint == "" || nextLevel)
        {
            _data.CurrentCheckPoint = _defaultCheckPoint;
        }
    }



    private void SpawnHero(string lastCheckPoint)
    {
        var checkpoints = FindObjectsOfType<CheckPointComponent>();
        
        foreach (var checkPoint in checkpoints)
        {
            if (checkPoint.Id == lastCheckPoint)
            {               
                checkPoint.SpawnHero();
                Save();
                isStartGame = true;
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
       // _trash.Retain(PerksModel);

        StatsModel = new StatsModel(_data);
       // _trash.Retain(StatsModel);

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

    private void CollectDataForSave()
    {
        DataForSave = new PlayerDataForSave();
        DataForSave._inventoryData = _data.Inventory.GetAll();
        DataForSave.HP = _data.HP;
        DataForSave.CurrentCheckPoint = _data.CurrentCheckPoint;
        
        DataForSave._used = _data.Perks.Used.Value;
        DataForSave._unlocked = _data.Perks.Unlocked;

        DataForSave.Levels = _data.Levels;

        StateLoadGame.sceneName = SceneManager.GetActiveScene().name;
       DataForSave.SceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadData()
    {
        _data.Inventory.SetData(DataForSave._inventoryData);
        _data.HP = DataForSave.HP;
        _data.CurrentCheckPoint = DataForSave.CurrentCheckPoint;

        _data.Perks.Used.Value = DataForSave._used;
        _data.Perks.SetUnlocked(DataForSave._unlocked);
        _data.Levels = DataForSave.Levels;

        QuickInventory = new QuickInventoryModel(_data);
        QuickInventory.Subscribe();

        StateLoadGame.sceneName = DataForSave.SceneName;
    }

    public void Save()
    {
        CollectDataForSave();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/PlayerSaveDataTest.dat");
        bf.Serialize(fileStream, DataForSave);
        fileStream.Close();
    }

    public void LoadLastSave()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerSaveDataTest.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/PlayerSaveDataTest.dat", FileMode.Open);
            DataForSave = (PlayerDataForSave)bf.Deserialize(fileStream);
            fileStream.Close();

            LoadData();
            //print("данные игрока загружены");
        }
        else
        {
            print("Данные игрока для загрузки не найдены");
        }
    }

    public bool IsChecked(string id)
    {
        return _data.CurrentCheckPoint == id;// _checkpoints.Contains(id);
    }

    public void SetChecked(string id)
    {
        if (isStartGame) return;
        _data.CurrentCheckPoint = id;
        Save();
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
