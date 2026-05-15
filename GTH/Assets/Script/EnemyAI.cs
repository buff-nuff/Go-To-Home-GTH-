using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// BattleManager의 enemyCores 상태에 따라 패턴을 선택.
    /// </summary>
    public void SelectPattern()
    {
        int cores = (BattleManager.Instance != null) ? BattleManager.Instance.enemyCores : 3;

        switch (cores)
        {
            case 3: PlayPattern("patternCore3"); break;
            case 2: PlayPattern("patternCore2"); break;
            case 1: PlayPattern("patternCore1"); break;
            default: PlayPattern("patternDefault"); break;
        }
    }

    void PlayPattern(string patternName)
    {
        Debug.Log(patternName + " 실행 중...");
        // TODO: 실제로 코어 수에 따라 RhythmManager.notePattern을 교체
        // 예: BattleManager.Instance.rhythmManager.notePattern = newPattern;
    }
}
