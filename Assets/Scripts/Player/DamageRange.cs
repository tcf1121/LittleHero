using UnityEngine;
using Cinemachine;

public class DamageRange : MonoBehaviour
{
    [SerializeField] private Player player;
    private CinemachineImpulseSource _impulseSource;
    private bool _hasShaked = false;
    void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void LateUpdate()
    {
        _hasShaked = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (MonsterRegistry.ColliderMonster.TryGetValue(collision, out Monster targetMonster))
            {
                Debug.Log("공격 성공: " + targetMonster.name);
                SuccessAtttack(targetMonster);
            }
        }
    }

    private void SuccessAtttack(Monster target)
    {
        target.GetDamage(player.PlayerStats.Damage);
        player.PlayerStats.GetMana();
        if (_impulseSource != null && !_hasShaked)
        {
            _impulseSource.GenerateImpulse(Vector3.right * 0.1f);
            _hasShaked = true;
        }
    }
}
