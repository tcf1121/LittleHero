using System.Collections;
using UnityEngine;

// 오우거 패턴
public class OgrePattern : MonoBehaviour, IBossPattern
{
    private Monster _monster;
    [SerializeField] private float _roarDelay = 2.0f;

    void Awake()
    {
        _monster = GetComponent<Monster>();
    }

    void Update() { }

    public void OnSpawn()
    {
        StartCoroutine(RoarRoutine());
    }

    public void OnDespawn()
    {
    }

    private IEnumerator RoarRoutine()
    {
        yield return new WaitForSeconds(4f);
        float timer = 0;
        Vector3 originScale = transform.localScale;
        while (timer < _roarDelay)
        {
            timer += Time.deltaTime;
            // 점점 커짐
            transform.localScale = originScale * (1f + (timer / _roarDelay) * 0.3f);
            yield return null;
        }

        Debug.Log("오우거가 포효합니다! 모든 몬스터 가속!");

        // 가속 로직 실행
        foreach (var normalMonster in MonsterRegistry.NormalMonsters)
        {
            if (normalMonster != null && normalMonster.gameObject.activeSelf)
            {
                // M
                // 맵에 있는 일반 몬스터 속도 2배
                float currentSpeed = normalMonster.GetMoveSpeed();
                normalMonster.SetMoveSpeed(currentSpeed * 2f);

                Debug.Log($"{normalMonster.name} 가속 적용됨");
            }
        }
        transform.localScale = originScale;
    }
}
