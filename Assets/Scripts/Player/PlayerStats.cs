using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    private Player player;

    private int _curHp;
    private int _maxHp;
    private int _curMp;
    private int _maxMp;
    private int _damage;
    private int _push;
    private int _manaRegen;
    public int CurHp { get { return _curHp; } set { _curHp = value; _changeHp?.Invoke(); } }
    public int MaxHp { get { return _maxHp; } }
    public int CurMp { get { return _curMp; } set { _curMp = value; _changeMp?.Invoke(); } }
    public int MaxMp { get { return _maxMp; } }
    public int Damage { get { return _damage; } }
    public int Push { get { return _push; } }
    private UnityAction _changeHp;
    private UnityAction _changeMp;
    private UnityAction _isDead;
    private Coroutine _damageCor;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        _damageCor = null;
        _changeHp += SetHp;
        _changeMp += SetMp;
        _isDead += InGameManager.Instance.StageFail;
        GetStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GetStats()
    {
        _maxHp = 3;
        _curHp = _maxHp;
        _curMp = 0;
        _maxMp = 40;
        _damage = 3;
        _push = 1;
        _manaRegen = 1;
        _changeHp?.Invoke();
        _changeMp?.Invoke();
    }

    public void GetMana()
    {
        CurMp += _manaRegen;
        if (CurMp > MaxMp) CurMp = MaxMp;
    }

    public bool CheckMana(int mana)
    {
        return CurMp >= mana;
    }

    public void UseMana(int mana)
    {
        CurMp -= mana;
    }

    public void TakeDamage()
    {
        CurHp--;
        if (CurHp <= 0)
        {
            _isDead.Invoke();
        }
        else
        {
            player.PlayerUI.TakeDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamageZone"))
        {
            TakeDamage();
        }
    }

    private void SetHp()
    {
        player.PlayerUI.SetHp(_curHp, _maxHp);
    }

    private void SetMp()
    {
        player.PlayerUI.SetMp(_curMp, _maxMp);
    }
}
