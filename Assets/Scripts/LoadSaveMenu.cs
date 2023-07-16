using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadSaveMenu : MonoBehaviour
{
    private PlayerDataForSave DataForSave;
    public void LoadLastSave()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerSaveDataTest.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/PlayerSaveDataTest.dat", FileMode.Open);
            DataForSave = (PlayerDataForSave)bf.Deserialize(fileStream);
            fileStream.Close();

            StateLoadGame.sceneName = DataForSave.SceneName;
            //print("данные игрока загружены");
        }
        else
        {
            print("Данные игрока для загрузки не найдены");
        }
    }
}
