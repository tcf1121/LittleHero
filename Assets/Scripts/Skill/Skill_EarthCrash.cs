using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EarthCrash", menuName = "Skills/EarthCrash")]
public class Skill_EarthCrash : SkillBase
{
    [SerializeField] private float _range = 7f;            // 충격파 범위
    [SerializeField] private float _slowDuration = 5f;     // 슬로우 지속 시간
    [SerializeField] private float _jumpHeight = 2f;       // 도약 높이
    [SerializeField] private Color _slowColor;

    private Vector3 rangePos = new Vector3(3.5f, 0, 0);
    private Vector3 rangescale = new Vector3(7f, 1f, 0);

    public override IEnumerator Execute(PlayerController playerC)
    {
        Vector3 startPos = playerC.transform.position;
        Vector3 peakPos = startPos + Vector3.up * _jumpHeight;
        playerC.SkillRange.transform.localPosition = rangePos;
        playerC.SkillRange.transform.localScale = rangescale;
        playerC.SkillRange.SetActive(true);

        // 1. 시간 정지 및 도약 연출
        Time.timeScale = 0f;
        playerC.GetOverlay().SetActive(true);
        SoundManager.Instance.PlaySFX(ActivationSound); // 기 모으는 소리

        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            // 위로 솟구치는 연출
            playerC.transform.position = Vector3.Lerp(startPos, peakPos, elapsed / 0.5f);
            yield return null;
        }

        // 2. 급강하 및 지면 충돌
        elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.unscaledDeltaTime;
            // 아래로 빠르게 찍기
            playerC.transform.position = Vector3.Lerp(peakPos, startPos, elapsed / 0.1f);
            yield return null;
        }

        // 3. 시간 재개 및 충격파 발생
        Time.timeScale = 1f;
        playerC.SkillRange.SetActive(false);
        playerC.ShakeCam(Vector3.down, 1.5f);
        playerC.GetOverlay().SetActive(false);

        int monsterLayer = LayerMask.GetMask("Monster");
        Collider2D[] hits = Physics2D.OverlapCircleAll(startPos, _range, monsterLayer);

        foreach (Collider2D enemyCollider in hits)
        {
            if (MonsterRegistry.ColliderMonster.TryGetValue(enemyCollider, out Monster target))
            {
                // 데미지 2배 적용
                target.GetDamage(playerC.Player.PlayerStats.Damage * (int)DamageMultiplier);

                // 슬로우 디버프 (코루틴 실행)
                playerC.StartCoroutine(ApplySlowDebuff(target));
            }
        }
    }

    // 슬로우 디버프 로직
    private IEnumerator ApplySlowDebuff(Monster target)
    {
        if (target == null) yield break;

        float originalSpeed = target.GetMoveSpeed();
        // 속도 절반으로 변경 및 디버프 효과 색 변경
        target.SetMoveSpeed(originalSpeed * 0.5f);
        target.SetColor(_slowColor);

        yield return new WaitForSeconds(_slowDuration);

        if (target != null)
        {
            // 속도와 색 복구
            target.SetMoveSpeed(originalSpeed);
            target.SetColor(Color.white);
        }
    }
}
