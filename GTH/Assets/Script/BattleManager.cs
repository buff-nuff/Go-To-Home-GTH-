using UnityEngine;
using System.Collections.Generic;
public class BattleManager : MonoBehaviour
{
    public List<Character> allUnits;
    private int currentTurnIndex = 0;
    private bool allEnemiesDead;

    void Start()
    {
        StartTurn();
    }

    void StartTurn()
    {
        Debug.Log($"{allUnits[currentTurnIndex].unitName}의 차례입니다.");
        allUnits[currentTurnIndex].isMyTurn = true;

        Character currentUnit = allUnits[currentTurnIndex];

        currentUnit.OnTurnStart();

        if (currentUnit.isCharmed)
        {
            Debug.Log($"{currentUnit.unitName}은 매혹 상태라 이번 턴을 쉽니다.");
            Invoke("EndTurn", 1.5f);
            return;
        }

        currentUnit.isMyTurn = true;
        if (currentUnit.currentHP <= 0)
        {
            EndTurn();
            return;
        }

        Debug.Log($"{currentUnit.unitName}의 차례입니다.");
        currentUnit.isMyTurn = true;
    }

    public void OnAttackButtonClicked(Character target)
    {
        Character currentUnit = allUnits[currentTurnIndex];

        if (currentUnit.isMyTurn)
        {
            target.TakeDamage(currentUnit.attackPower);
            EndTurn();
        }
    }
    
    void EndTurn()
    {
        allUnits[currentTurnIndex].isMyTurn = false;
        currentTurnIndex = (currentTurnIndex + 1) % allUnits.Count;
    }

    void CheckBattleVictory()
    {
        if (allEnemiesDead)
        {
            Debug.Log("전투 승리! 다음 스토리로 이동합니다.");
            Invoke("LoadNextStory", 2.0f);
        }
    }

    void LoadNextStory()
    {
        GameManager.Instance.GoToNextStory();
    }

}
