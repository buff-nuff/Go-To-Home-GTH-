using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StatusEffect
{
    public string effectName;
    public int duration;
    public float power;

    public StatusEffect(string name, int dur, float pwr)
    {
        effectName = name;
        duration = dur;
        power = pwr;
    }

}
public class Character : MonoBehaviour
{
    [Header("기본 정보")]
    public string unitName;

    [Header("스텟")]
    public float strength;
    public float attackPower;
    public float baseDefense;

    [Header("상태")]
    public float currentHP;
    public float maxHP;
    public float currentDefense;
    public bool isMyTurn = false;

    public List<StatusEffect> activeEffects = new List<StatusEffect>();
    public bool isCharmed = false;

    void Awake()
    {
        currentDefense = baseDefense;
    }
    public void AddStatusErrect(string name, int duration, float power)
    {
        activeEffects.Add(new StatusEffect(name, duration, power));
        Debug.Log($"{unitName}에게 {name} 효과가 {duration}턴 동안 적용됩니다.");
    }

    public void OnTurnStart()
    {
        isCharmed = false;
        currentDefense = baseDefense;


        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            ApplyEffect(activeEffects[i]);
            activeEffects[i].duration--;

            if (activeEffects[i].duration <= 0)
            {
                Debug.Log($"{activeEffects[i].effectName}효과가 종료되었습니다.");
                activeEffects.RemoveAt(i);
            }
        }
    }

    private void ApplyEffect(StatusEffect effect)
    {
        switch (effect.effectName)
        {
            case "Charm":
                isCharmed = true;
                currentDefense = baseDefense * (1f - effect.power);
                Debug.Log($"{unitName}이 매혹되어 정신을 못 차립니다! (방어력 {effect.power * 100}% 감소)");
                break;
        }
    }
    public void TakeDamage(float attackerAtk)
    {
        float totalAtk = attackerAtk + (strength * 0.5f);
        float damage = Mathf.Max(totalAtk - baseDefense, 1);

        currentHP -= damage;
        Debug.Log($"{unitName}이(가) {damage}의 피해를 입었습니다! 남은 체력 : {currentHP}");
    }
}
