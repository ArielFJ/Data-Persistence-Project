using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private InputField nameInput;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private GameObject rankingTextPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        nameInput.text = GameManager.Instance.CurrentUser.Name;
        var users = GameManager.Instance.AllUsers
            .OrderByDescending(u => u.HighScore)
            .ToList();
        for (int i = 0; i < users.Count; i++)
        {
            AddUserToRanking(i + 1, users[i]);
        }
    }

    public void StartGame()
    {
        GameManager.Instance.SetCurrentUserByName(nameInput.text.Trim());
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        GameManager.Instance.SaveAttributes();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void AddUserToRanking(int rank, User user)
    {
        var ranking = Instantiate(rankingTextPrefab, rankingPanel.transform, false);
        var texts = ranking.GetComponentsInChildren<Text>();
        texts[0].text = $"{rank}. {user.Name}";
        texts[1].text = $"{user.HighScore}";
    }
}
