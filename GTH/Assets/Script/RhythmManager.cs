using UnityEngine;
using System.Collections.Generic;


public enum Judge { Perfect, Good, Bad, Miss}
public class RhythmManager : MonoBehaviour
{
    public RectTransform hitZone;
    public NoteObject notePrefab;
    public float noteSpeed = 500f;

    private List<NoteObject> activeNotes = new List<NoteObject>();
    public static List<Judge> judges = new List<Judge>();
    private void Update()
    {
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            activeNotes[i].transform.Translate(Vector3.left * noteSpeed * Time.deltaTime);
            if (activeNotes[i].transform.localPosition.x < -600f)
            {
                Destroy(activeNotes[i]);
                activeNotes.RemoveAt(i);
                RecordResult(Judge.Miss);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckJudgment();
        }

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

        Destroy(closestNote);
        activeNotes.RemoveAt(0);
    }
    void RecordResult(Judge result)
    {
        
    }
}
