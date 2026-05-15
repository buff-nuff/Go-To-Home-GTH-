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
        Refresh();
    }

    /// <summary>
    /// BattleManager의 상태 변할 때마다 호출.
    /// </summary>
    public void Refresh()
    {
        if (BattleManager.Instance == null) return;

        if (lifeText != null)
            lifeText.text = $"목숨: {BattleManager.Instance.playerLife}";

        if (stigmataText != null)
            stigmataText.text = $"각인: {BattleManager.Instance.stigmataCount} / {BattleManager.Instance.stigmataRequired}";

        if (coreText != null)
            coreText.text = $"적 코어: {BattleManager.Instance.enemyCores}";
    }
}