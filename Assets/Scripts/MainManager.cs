using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [Header("References")]
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [Header("UI")]
    public Text userText;
    public Text ScoreText;
    public GameObject GameOverText;

    [Header("Effects")]
    public ParticleSystem[] ParticleSystems;

    private bool m_Started;
    private int m_Points;
    private bool _reachHighScore;

    private bool m_GameOver;

    // Start is called before the first frame update
    void Start()
    {
        SetHighScoreText(GameManager.Instance.CurrentUser.Name, GameManager.Instance.CurrentUser.HighScore);
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points > GameManager.Instance.CurrentUser.HighScore)
        {
            GameManager.Instance.CurrentUser.HighScore = m_Points;
            SetHighScoreText(GameManager.Instance.CurrentUser.Name, m_Points);

            if (!_reachHighScore)
            {
                foreach (var particle in ParticleSystems)
                {
                    particle.Play();
                }

                _reachHighScore = true;
            }
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void SetHighScoreText(string userName, int points)
    {
        userText.text = $"Best Score : {userName} : {points}";
    }
}
