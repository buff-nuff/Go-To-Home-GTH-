using UnityEngine;
using TMPro;

public class BattleStatusUI : MonoBehaviour
{
    public static BattleStatusUI Instance;

    [Header("연결")]
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI stigmataText;
    public TextMeshProUGUI coreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log($"[BattleStatusUI] Start. lifeText: {lifeText}, stigmataText: {stigmataText}, coreText: {coreText}");
        Debug.Log($"[BattleStatusUI] BattleManager.Instance: {BattleManager.Instance}");
        Refresh();
    }


    /// <summary>
    /// BattleManager의 상태 변할 때마다 호출.
    /// </summary>
    public void Refresh()
    {
        Debug.Log("[BattleStatusUI] Refresh 호출");
        if (BattleManager.Instance == null)
        {
            Debug.LogWarning("[BattleStatusUI] BattleManager.Instance가 null!");
            return;
        }

        if (lifeText != null)
        {
            lifeText.text = $"목숨: {BattleManager.Instance.playerLife}";
            Debug.Log($"[BattleStatusUI] lifeText 갱신: {lifeText.text}");
        }
        else Debug.LogWarning("[BattleStatusUI] lifeText가 null!");

        if (stigmataText != null)
            stigmataText.text = $"각인: {BattleManager.Instance.stigmataCount} / {BattleManager.Instance.stigmataRequired}";

        if (coreText != null)
            coreText.text = $"적 코어: {BattleManager.Instance.enemyCores}";
    }
}