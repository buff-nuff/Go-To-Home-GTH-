using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentStageIndex = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GoToNextStory()
    {
        currentStageIndex++;
        SceneManager.LoadScene("Story_" + currentStageIndex);
    }

    public void StartBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
