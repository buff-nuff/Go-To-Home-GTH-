using UnityEngine;
using System.Collections.Generic;

public enum Judge { Perfect, Good, Bad, Miss }

public class RhythmManager : MonoBehaviour
{
    [Header("프리팹 / 위치")]
    public NoteObject notePrefab;
    public Transform spawnPoint;
    public Transform hitZone;

    [Header("스크롤 설정")]
    public float scrollTime = 2.0f;

    [Header("판정 범위")]
    public float perfectRange = 0.3f;
    public float goodRange = 0.6f;
    public float badRange = 1.0f;
    public float missDistance = 1.5f;

    [Header("노트 패턴")]
    public List<NoteData> notePattern = new List<NoteData>
    {
        new NoteData(2.0f, NoteType.TapNote),
        new NoteData(3.0f, NoteType.TapNote),
        new NoteData(4.0f, NoteType.TapNote),
        new NoteData(5.0f, NoteType.TapNote),
        new NoteData(6.0f, NoteType.TapNote),
    };

    public KeyCode tapKey = KeyCode.Space;

    private List<NoteObject> activeNotes = new List<NoteObject>();
    private int spawnIndex = 0;
    private float songTime = 0f;
    private int direction = 1;

    private void Start()
    {
        direction = (hitZone.position.x > spawnPoint.position.x) ? 1 : -1;
    }

    private void Update()
    {
        songTime += Time.deltaTime;

        while (spawnIndex < notePattern.Count &&
               songTime >= notePattern[spawnIndex].beatTime - scrollTime)
        {
            SpawnNote(notePattern[spawnIndex]);
            spawnIndex++;
        }

        for (int i = 0; i < activeNotes.Count; i++)
        {
            if (activeNotes[i] != null)
                activeNotes[i].UpdatePosition(songTime);
        }

        if (Input.GetKeyDown(tapKey))
        {
            CheckJudgment();
        }

        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            if (activeNotes[i] == null) { activeNotes.RemoveAt(i); continue; }

            float signedDistance = (activeNotes[i].transform.position.x - hitZone.position.x) * direction;

            if (signedDistance > missDistance)
            {
                if (activeNotes[i].type == NoteType.TapNote)
                    Debug.Log("판정 결과 : Miss");
                RemoveNote(i);
            }
        }
    }

    void SpawnNote(NoteData data)
    {
        NoteObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        newNote.Initialize(spawnPoint.position.x, hitZone.position.x, scrollTime, data.beatTime, songTime);
        newNote.SetType(data.type);
        activeNotes.Add(newNote);
    }

    void CheckJudgment()
    {
        if (activeNotes.Count == 0) return;

        int closestIndex = -1;
        float minDistance = float.MaxValue;

        for (int i = 0; i < activeNotes.Count; i++)
        {
            if (activeNotes[i] == null) continue;
            float d = Mathf.Abs(activeNotes[i].transform.position.x - hitZone.position.x);
            if (d < minDistance) { minDistance = d; closestIndex = i; }
        }

        if (closestIndex == -1) return;

        NoteObject closest = activeNotes[closestIndex];

        if (closest.type == NoteType.AvoidNote)
        {
            Debug.Log("판정 결과 : Bad");
            RemoveNote(closestIndex);
            return;
        }

        if (minDistance <= perfectRange) Debug.Log("판정 결과 : Perfect");
        else if (minDistance <= goodRange) Debug.Log("판정 결과 : Good");
        else if (minDistance <= badRange) Debug.Log("판정 결과 : Bad");
        else return;

        RemoveNote(closestIndex);
    }

    void RemoveNote(int index)
    {
        if (index < 0 || index >= activeNotes.Count) return;
        NoteObject note = activeNotes[index];
        activeNotes.RemoveAt(index);
        if (note != null) Destroy(note.gameObject);
    }
}