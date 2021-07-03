using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string SaveFileName = "save.json";

    public static GameManager Instance;

    public User CurrentUser;
    public List<User> AllUsers => _allUsers;

    private List<User> _allUsers;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadAttributes();
    }

    public void SetCurrentUserByName(string name)
    {
        if (_allUsers == null) return;

        var user = _allUsers.FirstOrDefault(u => u.Name == name);
        // User does not exists, so add it to the list
        if (user == null)
        {
            User newUser = new User { Name = name };
            CurrentUser = newUser;
            _allUsers.Add(newUser);
        }
        else
        {
            CurrentUser = user;
        }
    }

    public void SaveAttributes()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        var data = new SaveData
        {
            Users = _allUsers.Where(u => u != User.Empty).ToList(),
            LastUser = CurrentUser
        };
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void LoadAttributes()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<SaveData>(json);
            _allUsers = data.Users;
            CurrentUser = data.LastUser;
        }
        else
        {
            CurrentUser = User.Empty;
            _allUsers = new List<User>();
        }
    }

    [Serializable]
    private class SaveData
    {
        public List<User> Users;
        public User LastUser;
    }
}
