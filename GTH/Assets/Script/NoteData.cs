using UnityEngine;

[System.Serializable]
public class NoteData
{
    public float beatTime;   // 노트가 판정선에 도달해야 하는 시간(초)
    public NoteType type;    // 노트 타입

    public NoteData(float beatTime, NoteType type)
    {
        this.beatTime = beatTime;
        this.type = type;
    }
}