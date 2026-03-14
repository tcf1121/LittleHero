using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterCount
{
    public MonsterData data; // 몬스터 종류
    public int count;        // 소환할 마리수
}

// 스테이지에 대한 정보 SO
[CreateAssetMenu(fileName = "StageData", menuName = "SO/StageData")]
public class StageData : ScriptableObject
{
    public int stageNumber;
    // 노말 몬스터
    public List<MonsterCount> NmonsterList;
    // 엘리트 몬스터
    public List<MonsterCount> EmonsterList;
    // 보스 몬스터
    public List<MonsterCount> BmonsterList;
}
