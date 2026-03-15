using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "TripleSlash", menuName = "Skills/TripleSlash")]
public class Skill_TripleSlash : SkillBase
{
    public float range = 5f;       // 공격 범위
    public float slashDelay = 0.1f; // 각 공격 사이의 간격

    private Vector3 rangePos = new Vector3(2.5f, 1f, 0);
    private Vector3 rangescale = new Vector3(5f, 1.5f, 0);
    // 소리 재생 여부 체크
    private bool sound1 = false;
    private bool sound2 = false;
    private bool sound3 = false;

    public override IEnumerator Execute(PlayerController playerC)
    {
        //시간 정지 연출
        playerC.GetOverlay().SetActive(true);
        playerC.SkillRange.transform.localPosition = rangePos;
        playerC.SkillRange.transform.localScale = rangescale;
        playerC.SkillRange.SetActive(true);

        Time.timeScale = 0f;
        sound1 = false;
        sound2 = false;
        sound3 = false;

        // unscaledDeltaTime을 사용한 대기 (여기서 연출 가능)
        float elapsed = 0f;
        Vector3 startPos = playerC.transform.position;

        while (elapsed < 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;

            if (!sound1 && elapsed >= 0.1f)
            {
                SoundManager.Instance.PlaySFX(ActivationSound); sound1 = true;
            }
            if (!sound2 && elapsed >= 0.2f)
            {
                SoundManager.Instance.PlaySFX(ActivationSound); sound2 = true;
            }
            if (!sound3 && elapsed >= 0.3f)
            {
                SoundManager.Instance.PlaySFX(ActivationSound); sound3 = true;
            }
            // 0.1초, 0.2초, 0.3초마다 플레이어를 아주 짧게 "틱" 하고 순간이동 시켰다 돌아오기
            if (elapsed > 0.1f && elapsed < 0.12f) playerC.transform.position = startPos + Vector3.right * 0.5f;
            else if (elapsed > 0.2f && elapsed < 0.22f) playerC.transform.position = startPos + Vector3.left * 0.5f;
            else if (elapsed > 0.3f && elapsed < 0.32f) playerC.transform.position = startPos + Vector3.up * 0.5f;
            else playerC.transform.position = startPos;
            yield return null;
        }
        playerC.SkillRange.SetActive(false);
        playerC.transform.position = startPos;
        playerC.ShakeCam(Vector3.right, 0.5f);
        playerC.GetOverlay().SetActive(false);
        Time.timeScale = 1f;

        // 효과 적용
        int monsterLayer = LayerMask.GetMask("Monster");
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerC.transform.position, range, monsterLayer);
        foreach (Collider2D enemyCollider in hits)
        {
            if (MonsterRegistry.ColliderMonster.TryGetValue(enemyCollider, out Monster target))
            {
                target.GetDamage(playerC.Player.PlayerStats.Damage * (int)DamageMultiplier);
            }

        }
    }
}
