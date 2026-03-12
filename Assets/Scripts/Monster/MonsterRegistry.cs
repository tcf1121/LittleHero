using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterRegistry
{
    public static Dictionary<Collider2D, Monster> ColliderMonster = new Dictionary<Collider2D, Monster>();
    public static List<Monster> NormalMonsters = new List<Monster>();

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

    public static void UnRegister(Monster monster, Collider2D collider)
    {
        ColliderMonster.Remove(collider);
        NormalMonsters.Remove(monster);
    }
}
