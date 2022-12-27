using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 깻잎 22-12-27
// BossMovement클래스 partial 로 분
public partial class BossMovement : MonoBehaviour
{
    [Header("BossMovement.Lighting")]
    // 원기옥 Prefab
    public GameObject prefab_Lighting_Big;
    public GameObject prefab_Lighting_Small;
    public GameObject particle_teleport_on;
    public GameObject particle_teleport_off;
    public GameObject particle_light_explosion;

    // 깻잎 22-12-27
    // BossMovement클래스에 있던것을 이곳으로 이관
    void SpecialSkillStart()
    {
        // 원기옥패턴이 진행중 이라면 return
        if (isPlayspecialSkill)
            return;

        stopUpdate = true;
        specialSkillUsed = true;
        
        hitBoxCol.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        // 원기옥 코루틴 시작 체크
        isPlayspecialSkill = true;
        StartCoroutine(SpecialSkill());
    }

    public bool isPlayspecialSkill = false;
    public bool special_skill_end = false;
    public IEnumerator SpecialSkill()
    {
        // 떠오를 동안 대기시간 1초
        anim.SetBool("isWalking", true);
        yield return new WaitForSeconds(1f);

        // 공중에 뜨는것을 가운데서 시작할 수 있도록 수정
        GameObject teleport_particle_off = Instantiate(particle_teleport_off);
        teleport_particle_off.transform.position = transform.position; // new Vector3(0f, transform.position.y, transform.position.z);
        sr.enabled = false;
        Destroy(teleport_particle_off, 0.7f);
        yield return new WaitUntil(() => teleport_particle_off == null);
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        GameObject teleport_particle_on = Instantiate(particle_teleport_on);
        teleport_particle_on.transform.position = transform.position; // new Vector3(0f, transform.position.y, transform.position.z);
        sr.enabled = true;
        Destroy(teleport_particle_on, 0.7f);
        yield return new WaitUntil(() => teleport_particle_on == null);

        // float walk_time_calc = 0.2f * Mathf.Abs(transform.position.x); // 위치에 따른 걸어가는 시간 비례 계산
        // ForceMoveInit(transform.position, new Vector3(0f, transform.position.y, transform.position.z), walk_time_calc);
        // 도착할때까지 대기
        // yield return new WaitUntil(() => isForceMoveStart == false);
        
        anim.SetTrigger("specialStart");
        anim.SetBool("Force", true);
        anim.SetBool("isWalking", false);
        // 떠오를 동안 대기시간 1초
        yield return new WaitForSeconds(1f);

        // 가장 큰 원기옥 소환
        CameraShake.I.DoShake(3f);
        GameObject big = Instantiate(prefab_Lighting_Big);
        big.transform.position = ConstantValue.lighting_big_origin_pos;

        yield return new WaitForSeconds(2f);

        /* 작은 원기옥 패턴 삭제
        for (int a = 0; a < 30; a++)
        {
            GameObject small = Instantiate(prefab_Lighting_Small);

            small.transform.position = big.transform.position; 

            LightingMove script = small.GetComponent<LightingMove>();
            Vector3 calcEndPos = new Vector3(
                ConstantValue.lighting_small_end_pos.x + UnityEngine.Random.Range(-ConstantValue.lighting_small_witdh_x, +ConstantValue.lighting_small_witdh_x),
                ConstantValue.lighting_small_end_pos.y,
                ConstantValue.lighting_small_end_pos.z);
            script.MoveStart(big.transform.position, calcEndPos);

            yield return new WaitForSeconds(0.5f);
        }*/

        anim.SetTrigger("specialHandMotion");
        // yield return new WaitForSeconds(1.0f);

        Vector3 BigcalcEndPos = new Vector3(
                player.transform.position.x,
                ConstantValue.lighting_small_end_pos.y,
                player.transform.position.z);
        LightingMove script_big = big.GetComponent<LightingMove>();
        script_big.MoveStart(big.transform.position, BigcalcEndPos);

        // 큰 원기옥이 사라질때까지 대기
        yield return new WaitUntil(() => big == null);

        GameObject explosion = Instantiate(particle_light_explosion);
        explosion.transform.position = BigcalcEndPos;
        Destroy(teleport_particle_on, 1.4f);

        // 내려올때까지 대기
        anim.SetTrigger("specialEnd");
        yield return new WaitForSeconds(2f);

        // 종료
        stopUpdate = false;
        hitBoxCol.enabled = true; 
        GetComponent<BoxCollider2D>().enabled = true;
        anim.SetBool("Force", false); 
        special_skill_end = true;
    }
}
