using System.Collections.Generic;
using UnityEngine;


// 상자 관련 오브젝트 풀
public class ChestPool : MonoBehaviour
{
    public static ChestPool Instance;
    private Transform poolParent;

    [SerializeField] private GameObject ChestPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        poolParent = this.transform;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(ChestPrefab, poolParent);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        if (_pool.Count > 0)
        {
            return _pool.Dequeue();
        }
        else
        {
            // 만약 10개가 다 쓰였다면 새로 하나 생성
            GameObject obj = Instantiate(ChestPrefab, poolParent);
            return obj;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}
