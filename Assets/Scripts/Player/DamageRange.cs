using UnityEngine;
using Cinemachine;


// 공격 범위에 대한 코드
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

    // 공격 성공 시 실행
    private void SuccessAtttack(Monster target)
    {
        // 몬스터에게 데미지를 줌
        target.GetDamage(player.PlayerStats.Damage);
        // 마나 획득
        player.PlayerStats.GetMana();
        // 화면 흔들림 효과 주기
        if (_impulseSource != null && !_hasShaked)
        {
            _impulseSource.GenerateImpulse(Vector3.right * 0.1f);
            _hasShaked = true;
        }
    }
}
