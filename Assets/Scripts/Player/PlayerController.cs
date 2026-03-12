using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private bool _isDash = false;
    private bool _isParrying = false;
    private Coroutine _attackCor;
    private Coroutine _dashCor;
    private Coroutine _comeBackCor;
    [SerializeField] private Collider2D attackColl;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        _attackCor = null;
        _dashCor = null;
        _comeBackCor = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAttack()
    {
        if (_attackCor == null)
        {
            Debug.Log("공격");
            _attackCor = StartCoroutine(AttackCoroutine());
        }

    }

    private void OnDash()
    {

        Debug.Log("대쉬");
        if (_dashCor == null)
        {
            _dashCor = StartCoroutine(DashCoroutine());
        }
    }

    private void OnParrying()
    {
        Debug.Log("막기");
        if (_isParrying) return;
        StartCoroutine(ParryCoroutine());
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            _isDash = false;
            Debug.Log("몬스터 충돌! ");
        }
    }

    /// <summary>
    /// 대쉬 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator DashCoroutine()
    {
        _isDash = true;
        Vector3 startPos = transform.position;
        Vector3 target = new Vector3(7, 0, 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, transform.right, 20f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Monster"))
        {
            target = hit.point + Vector2.down;
            Debug.Log("몬스터 발견, 좌표 " + target);
        }

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration && _isDash)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, target, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.Rigid.velocity = Vector2.zero;
        float currentTime = 0.5f;
        while (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        _isDash = false;
        StopCoroutine(_dashCor);
        _dashCor = null;
    }

    /// <summary>
    /// 패링 후 원점으로 복귀
    /// </summary>
    /// <returns></returns>
    public IEnumerator ComebackCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 zeroZone = new Vector3(-7, 0, 0);
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, zeroZone, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = zeroZone;
        player.Rigid.velocity = Vector2.zero;
        StopCoroutine(_comeBackCor);
        _comeBackCor = null;
    }

    /// <summary>
    /// 패링 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator ParryCoroutine()
    {
        int monsterLayer = LayerMask.GetMask("Monster");
        _isParrying = true;
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {

            Collider2D hit = Physics2D.OverlapCircle(transform.position, 1.5f, monsterLayer);

            if (hit != null)
            {
                if (MonsterRegistry.ColliderMonster.TryGetValue(hit, out Monster targetMonster))
                {
                    Debug.Log("패링 성공: " + targetMonster.name);
                    SuccessParry(targetMonster);
                    yield break;
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _isParrying = false;
    }

    private void SuccessParry(Monster target)
    {
        _isParrying = false;
        _isDash = false;

        // 일반 몬스터가 아닌 경우 해당하는 몬스터만 밀기
        if (target.GetMonsterType() != MonsterType.Normal)
        {
            target.Knockback(player.PlayerStats.Push);
        }

        // 모든 일반 몬스터 밀기
        for (int i = MonsterRegistry.NormalMonsters.Count - 1; i >= 0; i--)
        {
            MonsterRegistry.NormalMonsters[i].Knockback(player.PlayerStats.Push);
        }
        if (_comeBackCor == null)
        {
            _comeBackCor = StartCoroutine(ComebackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        float currentTime = 0.2f;
        attackColl.enabled = true;
        while (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        attackColl.enabled = false;
        StopCoroutine(_attackCor);
        _attackCor = null;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (!_isParrying)
            {
                player.Rigid.velocity = Vector2.zero;
                Debug.Log("몬스터와 분리됨: 관성 제거");
            }
        }
    }
}
