using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRange : MonoBehaviour
{
    [SerializeField] private Player player;

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
    }
}
