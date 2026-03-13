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
    [SerializeField] private MonsterType _monsterType;
    [SerializeField] private int _maxHp;
    [SerializeField] private float _moveSpeed;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;
    private int _curHp;
    private UnityAction _isDead;
    private bool _isKnockbacking = false;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _isDead += Die;

        _rigid.gravityScale = 0;
        _rigid.freezeRotation = true;
        _rigid.drag = 7f;
        _rigid.bodyType = RigidbodyType2D.Kinematic;
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
        gameObject.SetActive(false);
    }

    public void GetDamage(int damage)
    {
        _curHp -= damage;
        if (_curHp <= 0)
        {
            _isDead?.Invoke();
        }
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

}
