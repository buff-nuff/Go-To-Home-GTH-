using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    [Header("씬 이동")]
    public string nextSceneName;

    [Header("상태 관리")]
    public int playerLife = 2;
    public int enemyCores = 3;
    public int stigmataCount = 0;
    public int stigmataRequired = 3;
    public float successThreshold = 0.8f;

    [Header("연결")]
    public RhythmManager rhythmManager;
    public EnemyAI enemyAI;

    private bool isPlayerTurn = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        stigmataCount = 0;
        RefreshUI();
        Invoke(nameof(StartEnemyTurn), 0.1f);
    }

    private void Update()
    {
        if (isPlayerTurn && stigmataCount >= stigmataRequired && Input.GetKeyDown(KeyCode.A))
        {
            PlayerAttack();
        }
    }

    public void StartEnemyTurn()
    {
        isPlayerTurn = false;
        Debug.Log("=== 적의 공격! 리듬 패턴 시작 ===");

        // EnemyAI가 GameManager.currentStageIndex에 따라 패턴+음악 시작
        if (enemyAI != null)
        {
            enemyAI.SelectPattern();
        }
        else if (rhythmManager != null)
        {
            // EnemyAI 없으면 RhythmManager의 기본 패턴 사용 (테스트용)
            rhythmManager.StartPattern();
        }
    }

    public void OnRhythmPatternEnd(int perfectCount, int totalNotes)
    {
        bool isSuccess = (totalNotes > 0) && (perfectCount >= totalNotes * successThreshold);

        if (isSuccess)
        {
            stigmataCount++;
            Debug.Log($"방어 성공! 각인 획득 ({stigmataCount}/{stigmataRequired})");
        }
        else
        {
            playerLife--;
            Debug.Log($"방어 실패! 목숨 감소 (남은 목숨: {playerLife})");
        }

        RefreshUI();

        if (playerLife <= 0)
        {
            GameOver();
            return;
        }

        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        if (stigmataCount >= stigmataRequired)
        {
            isPlayerTurn = true;
            Debug.Log($"플레이어 턴! 각인 풀충전. A키로 공격!");
        }
        else
        {
            Debug.Log($"각인 부족 ({stigmataCount}/{stigmataRequired}). 다음 적 공격 대기...");
            Invoke(nameof(StartEnemyTurn), 1.5f);
        }
    }

    public void PlayerAttack()
    {
        if (!isPlayerTurn) return;
        if (stigmataCount < stigmataRequired) return;

        isPlayerTurn = false;

        enemyCores--;
        stigmataCount = 0;
        Debug.Log($"각인 개방! 코어 파괴 (남은 코어: {enemyCores})");

        RefreshUI();

        if (enemyCores <= 0)
        {
            Victory();
        }
        else
        {
            Invoke(nameof(StartEnemyTurn), 1.5f);
        }
    }

    void Victory()
    {
        Debug.Log("=== 모든 코어 파괴! 승리! ===");
        if (AudioManager.Instance != null) AudioManager.Instance.StopSong();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToNextStory();
        }
        else if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void GameOver()
    {
        Debug.Log("=== 플레이어 패배... 게임 오버 ===");
        if (AudioManager.Instance != null) AudioManager.Instance.StopSong();
    }

    void RefreshUI()
    {
        if (BattleStatusUI.Instance != null)
            BattleStatusUI.Instance.Refresh();
    }

    public bool TryGetNextSceneName(out string outNextSceneName)
    {
        outNextSceneName = nextSceneName;
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("nextSceneName이 비어있음");
            return false;
        }
        return true;
    }
}