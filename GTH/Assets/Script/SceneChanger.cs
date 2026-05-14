using UnityEngine;
public class SceneChanger : MonoBehaviour
{
    
    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StoryScene1");
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료 버튼이 눌렸습니다!");
        Application.Quit();
    }
}
