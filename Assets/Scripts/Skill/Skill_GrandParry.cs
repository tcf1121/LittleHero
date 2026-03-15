using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "GrandParry", menuName = "Skills/GrandParry")]
public class Skill_GrandParry : SkillBase
{
    public override IEnumerator Execute(PlayerController playerC)
    {
        //시간 정지 연출
        playerC.GetOverlay().SetActive(true);
        Time.timeScale = 0f;
        SoundManager.Instance.PlaySFX(ActivationSound); // 사운드 재생

        // unscaledDeltaTime을 사용한 대기 (여기서 연출 가능)
        float elapsed = 0f;
        while (elapsed < 0.5f)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // 3. 효과 적용
        playerC.GetOverlay().SetActive(false);
        Time.timeScale = 1f;

        // GameManager 등에 저장된 몬스터 리스트에 접근하여 로직 수행
        foreach (var monster in MonsterRegistry.ColliderMonster)
        {
            monster.Value.Knockback(playerC.Player.PlayerStats.Push * 3);
        }
    }
}