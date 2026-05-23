using System;
using System.Collections.Generic;
using UnityEngine;

public class SavedDataList<T>
{
    protected string _keyName;
    private Func<SaveSystem.SaveFileNames, string> _fileNameAction;
    protected string GetFileName(SaveSystem.SaveFileNames files) => _fileNameAction(files);
    public event Action OnDataModified;
    protected void RaiseOnDataModified() { OnDataModified?.Invoke(); }
    protected List<T> list = new();
    protected List<T> _defaultItems = new();
    public SavedDataList(string keyName, Func<SaveSystem.SaveFileNames, string> fileNameAction = null, List<T> defaultItems = null, bool isStatic = false)
    {
        _keyName = keyName;
        _fileNameAction = fileNameAction;
        if (fileNameAction == null) return;
        if (defaultItems != null) _defaultItems = defaultItems;

        if (isStatic)
        {
            SaveSystem.OnSaveStatic += SaveStatic;
        }
        else
        {
            SaveSystem.OnSaveFile += Save;
            SaveSystem.OnLoadFile += Load;
        }

        Load(SaveSystem.Files);
    }
    protected virtual void SaveStatic()
    {
        ES3.Save(_keyName, list, GetFileName(SaveSystem.Files));
    }
    protected virtual void Save(SaveSystem.SaveFileNames files)
    {
        ES3.Save(_keyName, list, GetFileName(files));
    }
    protected virtual void Load(SaveSystem.SaveFileNames files)
    {
        list = ES3.Load(_keyName, GetFileName(files), new List<T>(_defaultItems));
    }
    public bool Has(T flag)
    {
        return list.Contains(flag);
    }
    public bool Add(T flag)
    {
        bool b = list.AddUnique(flag);
        if (b) OnDataModified?.Invoke();
        return b;
    }
    public bool Remove(T flag)
    {
        bool b = list.RemoveAll(flag);
        if (b) OnDataModified?.Invoke();
        return b;
    }
    public void Clear()
    {
        list.Clear();
        OnDataModified?.Invoke();
    }
    public void Set(T flag, bool b)
    {
        if (b) Add(flag);
        else Remove(flag);
    }
    public int GetCount() => list.Count;
    public T[] GetValues() => list.ToArray();
}

public class SavedScriptableObjectList<T> : SavedDataList<T> where T : ScriptableObject
{
    public SavedScriptableObjectList(string keyName, Func<SaveSystem.SaveFileNames, string> fileNameAction = null, List<T> defaultItems = null, bool isStatic = false) : base(keyName, fileNameAction, defaultItems, isStatic)
    {
    }

    protected override void Save(SaveSystem.SaveFileNames files)
    {
        var names = ExtensionMethods.NamesFromAssets(list);
        ES3.Save(_keyName, names, GetFileName(files));
    }
    protected override void Load(SaveSystem.SaveFileNames files)
    {
        var names = ES3.Load(_keyName, GetFileName(files), new List<string>());
        if (names.Count == 0) { list = new(_defaultItems); return; }
        list = ExtensionMethods.AssetsFromNames<T>(names);
        RaiseOnDataModified();
    }
}
