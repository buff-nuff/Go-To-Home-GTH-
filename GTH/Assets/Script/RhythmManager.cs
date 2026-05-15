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

    [Header("기본 패턴 (테스트용)")]
    public List<NoteData> notePattern = new List<NoteData>
    {
        new NoteData(2.0f, NoteType.TapNote),
        new NoteData(3.0f, NoteType.TapNote),
        new NoteData(4.0f, NoteType.TapNote),
        new NoteData(5.0f, NoteType.TapNote),
        new NoteData(6.0f, NoteType.TapNote),
    };

    public KeyCode tapKey = KeyCode.Space;

    [Header("자동 시작 (BattleManager 없이 테스트용)")]
    public bool autoStartOnPlay = true;

    private List<NoteObject> activeNotes = new List<NoteObject>();
    private int spawnIndex = 0;
    private float songTime = 0f;
    private int direction = 1;
    private bool useAudioManager = false;

    private int perfectCount = 0;
    private int totalJudgedNotes = 0;
    private bool isRunning = false;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayMusic();

        direction = (hitZone.position.x > spawnPoint.position.x) ? 1 : -1;

        if (autoStartOnPlay && BattleManager.Instance == null)
        {
            StartPattern();
        }
    }

    public void PlayMusic()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            Debug.Log(audioSource.clip.name + "노래 재생 시작");
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    /// <summary>
    /// 패턴만 시작 (노래 없이). 기본 패턴 사용 또는 인자로 받음.
    /// </summary>
    public void StartPattern(List<NoteData> newPattern = null)
    {
        StartPattern(newPattern, null);
    }

    /// <summary>
    /// 패턴 + 음악 동시 시작. EnemyAI가 호출.
    /// </summary>
    public void StartPattern(List<NoteData> newPattern, AudioClip music)
    {
        if (newPattern != null) notePattern = newPattern;

        // 음악 재생
        if (music != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySong(music);
            useAudioManager = true;
        }
        else
        {
            useAudioManager = false;
        }

        ResetAndStart();
    }

    private void ResetAndStart()
    {
        foreach (var n in activeNotes) if (n != null) Destroy(n.gameObject);
        activeNotes.Clear();

        spawnIndex = 0;
        songTime = 0f;
        perfectCount = 0;
        totalJudgedNotes = 0;
        isRunning = true;

        Debug.Log($"패턴 시작! 노트 {notePattern.Count}개, AudioManager 사용: {useAudioManager}");
    }

    private void Update()
    {
        if (!isRunning) return;

        // 곡 시간: AudioManager 우선, 없으면 deltaTime 누적
        if (useAudioManager && AudioManager.Instance != null)
            songTime = AudioManager.Instance.GetSongTime();
        else
            songTime += Time.deltaTime;

        // 노트 스폰
        while (spawnIndex < notePattern.Count &&
               songTime >= notePattern[spawnIndex].beatTime - scrollTime)
        {
            SpawnNote(notePattern[spawnIndex]);
            spawnIndex++;
        }

        // 위치 갱신
        for (int i = 0; i < activeNotes.Count; i++)
        {
            if (activeNotes[i] != null)
                activeNotes[i].UpdatePosition(songTime);
        }

        // 입력
        if (Input.GetKeyDown(tapKey))
        {
            CheckJudgment();
        }

        // Miss 처리
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            if (activeNotes[i] == null) { activeNotes.RemoveAt(i); continue; }

            float signedDistance = (activeNotes[i].transform.position.x - hitZone.position.x) * direction;

            if (signedDistance > missDistance)
            {
                if (activeNotes[i].type == NoteType.TapNote)
                    RecordJudge(Judge.Miss);
                RemoveNote(i);
            }
        }

        // 종료 체크
        if (spawnIndex >= notePattern.Count && activeNotes.Count == 0)
        {
            EndPattern();
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
            RecordJudge(Judge.Bad);
            RemoveNote(closestIndex);
            return;
        }

        Judge result;
        if (minDistance <= perfectRange) result = Judge.Perfect;
        else if (minDistance <= goodRange) result = Judge.Good;
        else if (minDistance <= badRange) result = Judge.Bad;
        else return;

        RecordJudge(result);
        RemoveNote(closestIndex);
    }

    void RecordJudge(Judge result)
    {
        Debug.Log($"판정 결과 : {result}");
        totalJudgedNotes++;
        if (result == Judge.Perfect) perfectCount++;

        if (JudgeTextUI.Instance != null)
            JudgeTextUI.Instance.Show(result);
    }

    void RemoveNote(int index)
    {
        if (index < 0 || index >= activeNotes.Count) return;
        NoteObject note = activeNotes[index];
        activeNotes.RemoveAt(index);
        if (note != null) Destroy(note.gameObject);
    }

    void EndPattern()
    {
        isRunning = false;
        Debug.Log($"패턴 종료. Perfect: {perfectCount} / 전체 판정: {totalJudgedNotes}");

        int tapNoteCount = 0;
        foreach (var n in notePattern) if (n.type == NoteType.TapNote) tapNoteCount++;

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnRhythmPatternEnd(perfectCount, tapNoteCount);
        }
    }
}