using UnityEngine;

public class SceneName : MonoBehaviour
{
    public static SceneName Instance { get; private set; }
    public string nextScneneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public bool TryGetNextSceneName(out string outNextSceneName)
    {
        outNextSceneName = nextScneneName;
        if (Instance == null)
        {
            return false;
        }
        else
        {
            if (string.IsNullOrEmpty(nextScneneName))
            {
                Debug.LogWarning("nextSceneName이 존재하나,없거나 적지 않음");
                return false;
            }
            return true;
        }
    }
}
