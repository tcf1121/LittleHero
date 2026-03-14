using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    [SerializeField] private Material _whiteFlashMat;
    [SerializeField] private MonsterType _monsterType;
    [SerializeField] private int _maxHp;
    [SerializeField] private float _moveSpeed;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }
    private Material _originalMat;
    private int _curHp;
    private UnityAction _isDead;
    private bool _isKnockbacking = false;
    private Vector3 chestPos;
    private Coroutine _flashCor;

    private List<IBossPattern> _cachedPatterns = new List<IBossPattern>();

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMat = _spriteRenderer.material;
        _isDead += Die;

        _rigid.gravityScale = 0;
        _rigid.freezeRotation = true;
        _rigid.drag = 7f;
        _rigid.bodyType = RigidbodyType2D.Kinematic;
        chestPos = new Vector3(0, 0.7f, 0);

        var patterns = GetComponents<IBossPattern>();
        foreach (var p in patterns)
        {
            _cachedPatterns.Add(p);
            (p as MonoBehaviour).enabled = false;
        }
    }

    public void Init(MonsterData data)
    {
        // 기본 스탯 복사
        this._monsterType = data.type;
        this._maxHp = data.maxHp;
        this._curHp = _maxHp;
        this._moveSpeed = data.moveSpeed;

        // 외형 변경 및 죵류에 따른 레이어 변경
        if (_spriteRenderer != null && data.monsterSprite != null)
        {
            _spriteRenderer.sprite = data.monsterSprite;
            if (_monsterType == MonsterType.Boss) _spriteRenderer.sortingOrder = 0;
            else _spriteRenderer.sortingOrder = 1;
        }

        // 애니메이션 정보 복사
        // if (_animator != null && data.animatorController != null)
        // {
        //     _animator.runtimeAnimatorController = data.animatorController;
        // }

        // 콜라이더 정보 복사
        if (_collider != null)
        {
            _collider.offset = data.colliderOffset;
            _collider.size = data.colliderSize;
        }

        foreach (var p in _cachedPatterns)
        {
            p.OnDespawn();
            (p as MonoBehaviour).enabled = false;
        }

        foreach (var p in _cachedPatterns)
        {
            if (p.GetType().Name.Contains(data.monsterName))
            {
                (p as MonoBehaviour).enabled = true;
                p.OnSpawn();
                break;
            }
        }
    }

    void OnEnable()
    {
        _isKnockbacking = false;
        if (_rigid != null) _rigid.velocity = Vector2.zero;
        MonsterRegistry.Register(this, _collider);
    }

    void OnDisable()
    {
        MonsterRegistry.UnRegister(this, _collider);
    }

    void FixedUpdate()
    {
        if (!_isKnockbacking)
        {
            Vector2 nextPos = _rigid.position + Vector2.left * _moveSpeed * Time.fixedDeltaTime;
            _rigid.MovePosition(nextPos);
        }
    }



    private void Die()
    {
        InGameManager.Instance.DieMonster?.Invoke();
        DropChest();
        gameObject.SetActive(false);
    }

    // 피해 입었을 때 
    public void GetDamage(int damage)
    {
        _curHp -= damage;
        if (_flashCor != null) StopCoroutine(_flashCor);
        _flashCor = StartCoroutine(HitFlashRoutine());
        if (_curHp <= 0)
        {
            _isDead?.Invoke();
        }
    }

    // 흰색으로 번쩍이는 효과

    private IEnumerator HitFlashRoutine()
    {
        _spriteRenderer.material = _whiteFlashMat;
        yield return new WaitForSeconds(0.05f);
        _spriteRenderer.material = _originalMat;
        _flashCor = null;
    }

    public void Knockback(int push)
    {
        float pushForce = 0;

        switch (_monsterType)
        {
            case MonsterType.Normal:
                pushForce = push * 1f;
                break;
            case MonsterType.Elite:
                pushForce = push * 0.5f;
                break;
            case MonsterType.Boss:
                pushForce = push * 0.25f;
                break;
        }
        StartCoroutine(KnockbackProcess(pushForce));
    }

    private IEnumerator KnockbackProcess(float pushForce)
    {
        _isKnockbacking = true;
        _rigid.bodyType = RigidbodyType2D.Dynamic;

        _rigid.velocity = Vector2.zero;
        _rigid.AddForce(Vector2.right * pushForce * 5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        _isKnockbacking = false;
    }


    public MonsterType GetMonsterType()
    {
        return _monsterType;
    }


    public void DropChest()
    {
        int looting = 5;
        switch (_monsterType)
        {
            case MonsterType.Normal:
                looting = 5;
                break;
            case MonsterType.Elite:
                looting = 10;
                break;
            case MonsterType.Boss:
                looting = 100;
                break;
        }

        bool drop = Random.Range(0, 100) < looting;

        if (drop)
        {
            GameObject chest = ChestPool.Instance.Get();
            chest.transform.position = transform.position + chestPos;
            chest.SetActive(true);
        }
    }

    public void SetMoveSpeed(float newSpeed)
    {
        _moveSpeed = newSpeed;
    }

    public float GetInitialMoveSpeed()
    {
        // 필요하다면 SO 데이터에서 원래 속도를 가져올 수 있습니다.
        return _moveSpeed;
    }
}
