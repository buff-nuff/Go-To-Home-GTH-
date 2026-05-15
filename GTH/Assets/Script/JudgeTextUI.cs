using UnityEngine;
using TMPro;
using System.Collections;

public class JudgeTextUI : MonoBehaviour
{
    public static JudgeTextUI Instance;

    [Header("연결")]
    public TextMeshProUGUI judgeText;

    [Header("표시 시간")]
    public float displayDuration = 0.5f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        Instance = this;
        if (judgeText != null) judgeText.text = "";
    }

    /// <summary>
    /// 외부에서 호출. 판정 결과를 화면에 표시.
    /// </summary>
    public void Show(Judge judge)
    {
        if (judgeText == null) return;

        // 판정별 색상
        switch (judge)
        {
            case Judge.Perfect: judgeText.color = Color.orange; break;
            case Judge.Good: judgeText.color = Color.green; break;
            case Judge.Bad: judgeText.color = new Color(1f, 0.5f, 0f); break; // 주황
            case Judge.Miss: judgeText.color = Color.red; break;
        }

        judgeText.text = judge.ToString().ToUpper();

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        judgeText.text = "";
    }
}
