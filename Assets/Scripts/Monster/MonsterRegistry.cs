using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterRegistry
{
    public static Dictionary<Collider2D, Monster> ColliderMonster = new Dictionary<Collider2D, Monster>();
    public static List<Monster> NormalMonsters = new List<Monster>();

    // 몬스터를 생성할 때 실행할 함수
    public static void Register(Monster monster, Collider2D collider)
    {
        if (!ColliderMonster.ContainsKey(collider))
        {
            ColliderMonster.Add(collider, monster);
        }

        if (monster.GetMonsterType() == MonsterType.Normal)
        {
            if (!NormalMonsters.Contains(monster))
                NormalMonsters.Add(monster);
        }
    }

    // 몬스터가 죽었을 때 실행할 함수
    public static void UnRegister(Monster monster, Collider2D collider)
    {
        ColliderMonster.Remove(collider);
        NormalMonsters.Remove(monster);
    }

    // 게임 한 판이 끝나면 실행할 함수
    public static void ResetMonster()
    {
        ColliderMonster.Clear();
        NormalMonsters.Clear();
    }
}
