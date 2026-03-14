using UnityEngine;

// 몬스터 정보에 대한 SO
[CreateAssetMenu(fileName = "NewMonsterData", menuName = "SO/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public MonsterType type;
    public int maxHp;
    public float moveSpeed;

    [Header("Visual Settings")]
    public Sprite monsterSprite;

    [Header("Collider Settings")]
    public Vector2 colliderOffset;
    public Vector2 colliderSize;
}
