using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterType _monsterType;
    [SerializeField] private int _maxHp;
    [SerializeField] private float moveSpeed;
    private Collider2D _collider;
    private Rigidbody2D _rigid;
    private int _curHp;
    private UnityAction _isDead;
    private bool _isKnockbacking = false;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.gravityScale = 0;
        _rigid.freezeRotation = true;
        _rigid.drag = 7f;
        _rigid.bodyType = RigidbodyType2D.Kinematic;
        _isDead += Dead;
    }


    void OnEnable()
    {
        MonsterRegistry.Register(this, _collider);
        _curHp = _maxHp;
    }

    void OnDisable()
    {
        MonsterRegistry.UnRegister(this, _collider);
    }

    void FixedUpdate()
    {
        if (!_isKnockbacking)
        {
            Vector2 nextPos = _rigid.position + Vector2.left * moveSpeed * Time.fixedDeltaTime;
            _rigid.MovePosition(nextPos);
        }
    }

    private void Dead()
    {
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
