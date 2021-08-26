using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOvertext;
    [SerializeField] GameObject gameCleartext;
    [SerializeField] Text scoreText;

    const int MAX_SCORE = 9999;
    int score = 0;

    void Start()
    {
        scoreText.text = score.ToString();
    }

    public void AddScore(int val)
    {
        score += val;
        if(score > MAX_SCORE)
        {
            score = MAX_SCORE;
        }
        scoreText.text = score.ToString();
    }

    public void GameOver() 
    {
        gameOvertext.SetActive(true);
        Invoke("RestartScene", 1.5f);
    }
    public void GameClear()
    {
        gameCleartext.SetActive(true);
        Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
