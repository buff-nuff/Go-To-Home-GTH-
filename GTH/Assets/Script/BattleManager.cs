using UnityEngine;
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("상태 관리")]
    public int playerLife = 2;
    public int enemyCores = 3;
    public int stigmataCount = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OnRhythmPatternEnd(int perfectCount, int totalNotes)
    {
        bool isSuccess = perfectCount >= totalNotes * 0.8f;

        if (isSuccess)
        {
            Debug.Log("방어 성공! 각인을 1개 획득합니다.");
            stigmataCount++;
        }
        else
        {
            Debug.Log("방어 실패! 목숨이 감소합니다.");
            playerLife--;
        }

        if (playerLife <= 0)
        {
            Debug.Log("플레이어 패배...게임 오버");
            return;
        }
        StartPlayerTurn();
    }
    void StartPlayerTurn()
    {
        Debug.Log($"현재 각인 : {stigmataCount}. 공격을 선택하세요.");
    }

    public void PlayerAttack()
    {
        if (stigmataCount >= 3)
        {
            Debug.Log("각인 개방! 적의 코어를 1개 파괴했습니다.");
            enemyCores--;
            stigmataCount = 0;
        }
        else
        {
            Debug.Log("각인이 부족하여 일반 공격을 일반 공격을 수행했지만 코어에 흠집도 나지 않았습니다.");
        }

        if (enemyCores <= 0)
        {
            Debug.Log("모든 코어 파괴! 승리!");
            // GameManager.Instance.GoToNextStory;
        }
      }
    }
