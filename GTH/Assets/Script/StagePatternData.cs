using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Stage_1_Pattern", menuName = "RhythmGame/StagePatternData")]
public class StagePatternData : MonoBehaviour
{
    public int stageIndex = 1;
    public AudioClip stageMusic;

    [Header("스테이지 노트 패턴")]
    public List<NoteData> stagePattern;
}