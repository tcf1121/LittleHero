using System.Collections;
using UnityEngine;

// 플레이어 입력 및 움직임
public class PlayerController : MonoBehaviour
{
    private Player player;
    private bool _isDash = false;
    private bool _isParrying = false;
    private Coroutine _attackCor;
    private Coroutine _dashCor;
    private Coroutine _comeBackCor;
    private Vector3 zeroZone = new Vector3(-7, 0, 0);
    [SerializeField] private Collider2D attackColl;
    [SerializeField] private AudioClip _attackSFX;
    [SerializeField] private AudioClip _dashSFX;
    [SerializeField] private AudioClip _parryingSFX;

    void Start()
    {
        player = GetComponent<Player>();
        _attackCor = null;
        _dashCor = null;
        _comeBackCor = null;
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

    public void Parrying()
    {
        Debug.Log("처절한 막기");
        transform.position = new Vector3(-7.5f, 0, 0);
        StopAllCoroutines();
        if (_isParrying) return;
        StartCoroutine(ParryCoroutine());
    }

    private void OnSkill1()
    {
        if (player.PlayerStats.CheckMana(10))
        {
            Debug.Log("스킬 1 사용");
            player.PlayerStats.UseMana(10);
        }
    }

    private void OnSkill2()
    {
        if (player.PlayerStats.CheckMana(20))
        {
            Debug.Log("스킬 2 사용");
            player.PlayerStats.UseMana(20);
        }
    }


    private void OnSkill3()
    {
        if (player.PlayerStats.CheckMana(30))
        {
            Debug.Log("스킬 3 사용");
            player.PlayerStats.UseMana(30);
        }
    }

    private void OnSkill4()
    {
        if (player.PlayerStats.CheckMana(20))
        {
            Debug.Log("스킬 4 사용");
            player.PlayerStats.UseMana(20);
        }
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

        // 오른쪽 보기
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, transform.right, 20f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Monster"))
        {
            // 만난 지점의 좌표 저장
            target = hit.point + Vector2.down;
            Debug.Log("몬스터 발견, 좌표 " + target);
        }

        transform.localScale = new Vector3(1.5f, 0.8f, 1f);
        float duration = 0.2f;
        float elapsedTime = 0f;
        // 소리 재생
        SoundManager.Instance.PlaySFX(_dashSFX);

        // 저장한 좌표로 대쉬
        while (elapsedTime < duration && _isDash)
        {
            float t = elapsedTime / duration;
            float easedT = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, target, easedT);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
        player.Rigid.velocity = Vector2.zero;
        // 딜레이 주기
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

        float duration = 0.15f;
        float elapsedTime = 0f;
        SoundManager.Instance.PlaySFX(_dashSFX);
        transform.localScale = new Vector3(0.7f, 1.3f, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float easedT = 1f - Mathf.Pow(1f - t, 3);
            transform.position = Vector3.Lerp(startPos, zeroZone, easedT);
            yield return null;
        }
        transform.position = zeroZone;
        transform.localScale = new Vector3(1.2f, 0.8f, 1f);
        yield return new WaitForSeconds(0.03f);

        transform.localScale = Vector3.one;
        player.Rigid.velocity = Vector2.zero;

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
            // 주변 확인
            Collider2D hit = Physics2D.OverlapCircle(transform.position, 1.5f, monsterLayer);
            // 뭔가 있다
            if (hit != null)
            {
                // 그게 몬스터라면
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
        // 소리 재생
        SoundManager.Instance.PlaySFX(_parryingSFX);
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

    // 공격하는 모션
    private IEnumerator AttackCoroutine()
    {
        // 앞으로 조금 나가는 듯한 효과를 주어 칼을 휘두르는 느낌
        Vector3 startPos = transform.position;
        Vector3 attackPos = startPos + transform.right * 0.4f;
        transform.localScale = new Vector3(1.3f, 0.8f, 1f);
        attackColl.enabled = true;
        float duration = 0.1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            float easedT = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, attackPos, easedT);

            yield return null;
        }
        // 소리 재생
        SoundManager.Instance.PlaySFX(_attackSFX);
        float returnTime = 0.1f;
        float eTime = 0f;
        Vector3 recoverPos = transform.position - transform.right * 0.1f;

        // 원래 자리로 돌아가기
        while (eTime < returnTime)
        {
            eTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(1.3f, 0.8f, 1f), Vector3.one, eTime / returnTime);
            yield return null;
        }
        transform.position = startPos;
        attackColl.enabled = false;

        _attackCor = null;
    }

    // 몬스터와 떨어지면 물리력 제거
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (!_isParrying)
            {
                player.Rigid.velocity = Vector2.zero;
            }
        }
    }
}
