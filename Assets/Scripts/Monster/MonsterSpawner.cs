using System.Collections.Generic;
using UnityEngine;

// 몬스터 스포너
public class MonsterSpawner : MonoBehaviour
{
    // 몬스터 종류에 따라 q를 나눔 (다른 종류의 몬스터끼리는 서로 겹칠 수 있음)
    private Queue<MonsterData> _NspawnQueue = new Queue<MonsterData>();
    private Queue<MonsterData> _EspawnQueue = new Queue<MonsterData>();
    private Queue<MonsterData> _BspawnQueue = new Queue<MonsterData>();
    private Vector2 spawnPos;

    void Start()
    {
        GenerateQueue(GameManager.Instance.GetStage().NmonsterList, _NspawnQueue);
        GenerateQueue(GameManager.Instance.GetStage().EmonsterList, _EspawnQueue);
        GenerateQueue(GameManager.Instance.GetStage().BmonsterList, _BspawnQueue);
        InGameManager.Instance.SetMonNum();
        BeginningSpawn();
        spawnPos = new Vector2(0.5f, 0f) + (Vector2)transform.position;
    }

    // 스테이지 정보 가져와서 큐에 몬스터 정보 넣기
    void GenerateQueue(List<MonsterCount> dataList, Queue<MonsterData> targetQueue)
    {
        List<MonsterData> tempList = new List<MonsterData>();

        // 리스트 구성
        foreach (var monsterCount in dataList)
        {
            for (int i = 0; i < monsterCount.count; i++)
            {
                tempList.Add(monsterCount.data);
            }
        }

        // 리스트를 섞기
        for (int i = 0; i < tempList.Count; i++)
        {
            int rnd = Random.Range(i, tempList.Count);
            MonsterData temp = tempList[rnd];
            tempList[rnd] = tempList[i];
            tempList[i] = temp;
        }

        // 큐에 삽입
        targetQueue.Clear();
        foreach (var data in tempList)
        {
            targetQueue.Enqueue(data);
        }
        InGameManager.Instance.StageMonsterNum += targetQueue.Count;
    }

    // 맨 처음 스폰하기 일반 몬스터는 9마리, 엘리트 및 보스는 한마리 소환
    void BeginningSpawn()
    {
        for (int i = 0; i < 9; i++)
        {
            Vector3 pos = new Vector3(0 - i, 0, 0);
            SpawnMonster(pos, MonsterType.Normal);
        }
        if (_EspawnQueue.Count > 0) SpawnMonster(Vector2.zero, MonsterType.Elite);
        if (_BspawnQueue.Count > 0) SpawnMonster(Vector2.zero, MonsterType.Boss);
    }


    // 스폰 지점에 아무것도 없을 시 몬스터 스폰 시도
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            // 왼쪽으로 전진해서 나갔는지 확인
            if (other.transform.localPosition.x < -0.1f)
            {
                Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.1f, LayerMask.GetMask("Monster"));

                if (hit == null)
                {

                    Debug.Log("0.5 지점이 비었음: 새 몬스터 소환");
                    if (MonsterRegistry.ColliderMonster.TryGetValue(other, out Monster outMonster))
                    {
                        Debug.Log("몬스터 소환 종류 : " + outMonster.GetMonsterType());
                        SpawnMonster(Vector2.zero, outMonster.GetMonsterType());
                    }

                }
            }
        }
    }

    // 몬스터 스폰
    private void SpawnMonster(Vector2 spawnPos, MonsterType monsterType)
    {
        MonsterData nextData = GetNextData(monsterType);
        if (nextData == null) return;

        Debug.Log("몬스터 소환!");
        Monster monster = MonsterPool.Instance.Get();
        monster.transform.localPosition = new Vector3(spawnPos.x, spawnPos.y, 0);
        monster.gameObject.SetActive(true);
        monster.Init(nextData);
    }

    // 나간 몬스터 타입 확인 후 해당 몬스터 타입이 큐에 있으면 반환
    private MonsterData GetNextData(MonsterType monsterType)
    {
        switch (monsterType)
        {
            case MonsterType.Normal:
                return _NspawnQueue.Count > 0 ? _NspawnQueue.Dequeue() : null;
            case MonsterType.Elite:
                return _EspawnQueue.Count > 0 ? _EspawnQueue.Dequeue() : null;
            case MonsterType.Boss:
                return _BspawnQueue.Count > 0 ? _BspawnQueue.Dequeue() : null;
            default:
                return null;
        }
    }


}


