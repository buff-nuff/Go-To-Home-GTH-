using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea(3, 5)]
    public string sentence;
}


public class StoryManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI storyText;

    [Header("대사 설정")]
    public Dialogue[] dialogues;

    private int currentIndex = 0;

    [Header("이동할 씬 이름")]
    public string nextSceneName = "GameScene";

    void Start()
    {
        DisplayCurrentDialogue();  
    }




    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0))
        {
            NextDialogue();
        }
    }

    void NextDialogue()
    {
        currentIndex++;

        if (currentIndex < dialogues.Length)
        {
            DisplayCurrentDialogue();
        }
        else
        {
            Debug.Log("모든 대사가 끝났습니다. 다음 씬으로 이동합니다."); 
            nameText.text = "";
            storyText.text = "";
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void OnStoryEnded()
    {
        Debug.Log("스토리가 끝났습니다. 전투를 시작합니다.");
        GameManager.Instance.StartBattle();
    }

    void DisplayCurrentDialogue()
    {
        nameText.text = dialogues[currentIndex].name;
        storyText.text = dialogues[currentIndex].sentence;
    }
}
