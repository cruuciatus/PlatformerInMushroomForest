using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
public class LocaleDef : ScriptableObject
{    //en
     //https://docs.google.com/spreadsheets/d/e/2PACX-1vQ12eCMvZ7gfnlsT89VF91_F-rh7b7VYEyw5GYQ51llZjc9dSAAPE65RTsQ9pRQ359CrHVcgPG4m4O7/pub?gid=0&single=true&output=tsv
     //ru
     //https://docs.google.com/spreadsheets/d/e/2PACX-1vQ12eCMvZ7gfnlsT89VF91_F-rh7b7VYEyw5GYQ51llZjc9dSAAPE65RTsQ9pRQ359CrHVcgPG4m4O7/pub?gid=2062971808&single=true&output=tsv



    [SerializeField] private string _url;
    [SerializeField] private List<LocaleItem> _localeItems;


    private UnityWebRequest _request;

    public Dictionary<string, string> GetData()
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var localeItem in _localeItems)
        {
            dictionary.Add(localeItem.Key, localeItem.Value);
        }

        return dictionary;
    }


    [ContextMenu("Update locale")]
    public void UpdateLocale()
    {
        if(_request != null) return;

        _request = UnityWebRequest.Get(_url);
        _request.SendWebRequest().completed += OnDataLoaded;
    }
    private void OnDataLoaded(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            var data = _request.downloadHandler.text;
            ParseData(data);
        }
    }
    private void ParseData(string data)
    {
        var rows = data.Split('\n');
        _localeItems.Clear();
        foreach (var row in rows)
        {
            AddLocaleItem(row);
        }
    }
    private void AddLocaleItem(string row)
    {
        try
        {
            var parts = row.Split('\t');
            _localeItems.Add(new LocaleItem { Key = parts[0], Value = parts[1] });
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't parse row: {row}.\n {e}");
        }
    }

    [Serializable]
    private class LocaleItem
    {
        public string Key;
        public string Value;
    }
}
