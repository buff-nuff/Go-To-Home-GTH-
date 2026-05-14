using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int enemyCores = 3;
    public int stigmataCount = 0;

    public void SeletPattern()
    {
        switch (enemyCores)
        {
            case 3: PlayPattern("patternCore3"); break;
            case 2: PlayPattern("patternCore2"); break;
            case 1: PlayPattern("patternCore1"); break;
        }
    }

    void PlayPattern(string patternName)
    {
        Debug.Log(patternName + "실행 중...");
    }

    public void PlayerAttack()
    {
        if (stigmataCount >= 3)
        {
            Debug.Log("각인 개방! 코어 파괴");
            enemyCores--;
            stigmataCount = 0;
        }

        if (enemyCores <= 0)
        {
            Debug.Log("전투 승리!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GoToNextStory();
            }
        }
    }
}
