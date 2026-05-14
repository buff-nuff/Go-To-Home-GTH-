using UnityEngine;

public enum NoteType { TapNote, AvoidNote }

[RequireComponent(typeof(SpriteRenderer))]
public class NoteObject : MonoBehaviour
{
    public NoteType type;
    public float beatTime;
    private float startX;
    private float targetX;
    private float scrollTime;
    private float spawnTime;
    private bool isInitialized = false;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(float startX, float targetX, float scrollTime, float beatTime, float currentSongTime)
    {
        this.startX = startX;
        this.targetX = targetX;
        this.scrollTime = scrollTime;
        this.beatTime = beatTime;
        this.spawnTime = currentSongTime;
        this.isInitialized = true;
    }

    public void UpdatePosition(float currentSongTime)
    {
        if (!isInitialized) return;

        float elapsed = currentSongTime - spawnTime;
        float t = elapsed / scrollTime;   // Clamp 제거! 판정선 지나서도 계속 흐름
        float x = Mathf.LerpUnclamped(startX, targetX, t);

        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }

    public void SetType(NoteType newType)
    {
        type = newType;
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        sr.color = (type == NoteType.TapNote) ? Color.blue : Color.red;
    }
}

