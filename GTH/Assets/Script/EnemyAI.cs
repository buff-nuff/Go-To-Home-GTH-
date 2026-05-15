using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [Header("스테이지 데이터 세팅")]
    // 생성한 StagePatternData 에셋 4개를 여기에 등록합니다.
    public List<StagePatternData> stagePatterns;

    public void SelectPattern()
    {
        // GameManager에서 현재 스테이지 인덱스를 가져옵니다.
        int currentStage = (GameManager.Instance != null) ? GameManager.Instance.currentStageIndex : 1;

        // 현재 스테이지에 맞는 데이터 찾기
        StagePatternData currentData = stagePatterns.Find(x => x.stageIndex == currentStage);

        if (currentData == null)
        {
            Debug.LogError($"{currentStage} 스테이지 데이터를 찾을 수 없습니다! 인스펙터를 확인해주세요.");
            return;
        }

        Debug.Log($"스테이지 {currentStage} 패턴 및 음악 실행 준비 완료");

        // 코어 수와 무관하게 해당 스테이지의 고정 패턴 실행
        if (BattleManager.Instance != null && BattleManager.Instance.rhythmManager != null)
        {
            BattleManager.Instance.rhythmManager.StartPattern(currentData.stagePattern, currentData.stageMusic);
        }
    }
}