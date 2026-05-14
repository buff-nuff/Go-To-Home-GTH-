using UnityEngine;
using System.Collections.Generic;


public enum Judge { Perfect, Good, Bad, Miss}
public class RhythmManager : MonoBehaviour
{
    [Header("설정")]
    public NoteObject notePrefab;
    public Transform spawnPoint;
    public Transform hitZone;
    public float scrollTime = 2.0f;

    [Header("데이터")]
    private List<NoteObject> activeNotes = new List<NoteObject>();
    public List<float> noteTimes = new List<float> { 1.0f, 2.0f, 3.0f, 4.0f, 5.0f };

    private int noteIndex = 0;
    private float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (noteIndex < noteTimes.Count && currentTime >= noteTimes[noteIndex] - scrollTime)
        {
            SpawnNote();
            noteIndex++;
        }
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            if (activeNotes[i] == null) continue;

            float distance = activeNotes[i].transform.localPosition.x - hitZone.localPosition.x;


            if (distance < -100f)
            {
                RecordResult(Judge.Miss);
                RemoveNote(i);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckJudgment();
            }
        }
    }
        void SpawnNote()
        {
            NoteObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
            newNote.Initialize(spawnPoint.position.x, hitZone.position.x, scrollTime);
            activeNotes.Add(newNote);
        }
        void CheckJudgment()
        {
            if (activeNotes.Count == 0) return;

            NoteObject closestNote = activeNotes[0];

            if (closestNote.type == NoteType.AvoidNote)
            {
                RecordResult(Judge.Bad);
            }
            else
            {
                float distance = Mathf.Abs(closestNote.transform.localPosition.x - hitZone.localPosition.x);

                if (distance < 20f) RecordResult(Judge.Perfect);
                else if (distance < 50f) RecordResult(Judge.Good);
                else RecordResult(Judge.Bad);
            }

            RemoveNote(0);
        }
        void RemoveNote(int index)
        {
            if (index < 0 || index >= activeNotes.Count) return;

            NoteObject note = activeNotes[index];
            activeNotes.RemoveAt(index);
            Destroy(note.gameObject);
        }

        void RecordResult(Judge result)
        {
            Debug.Log($"판정 결과 : {result}");
        }

    }
