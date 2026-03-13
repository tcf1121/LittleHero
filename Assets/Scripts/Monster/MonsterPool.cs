using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    public static MonsterPool Instance;
    private Transform poolParent;

    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private int poolSize = 30;

    private Queue<Monster> _pool = new Queue<Monster>();


    void Awake()
    {
        Instance = this;
        poolParent = this.transform;
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewInstance();
        }

    }

    private Monster CreateNewInstance()
    {
        GameObject obj = Instantiate(monsterPrefab, poolParent);
        obj.SetActive(false);
        Monster monster = obj.GetComponent<Monster>();
        _pool.Enqueue(monster);
        return monster;
    }

    public Monster Get()
    {
        if (_pool.Count > 0)
        {
            return _pool.Dequeue();
        }
        else
        {
            // 만약 30개가 다 쓰였다면 새로 하나 생성
            return CreateNewInstance();
        }
    }

    public void ReturnToPool(Monster monster)
    {
        monster.gameObject.SetActive(false);
        _pool.Enqueue(monster);
    }
}
