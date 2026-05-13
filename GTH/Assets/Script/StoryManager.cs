using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
            storyText.text = "이야기가 끝났습니다.";
            nameText.text = "";
        }
    }

    void DisplayCurrentDialogue()
    {
        nameText.text = dialogues[currentIndex].name;
        storyText.text = dialogues[currentIndex].sentence;
    }
}
