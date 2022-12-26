using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� 22-12-27
// BossMovementŬ���� partial �� ��
public partial class BossMovement : MonoBehaviour
{
    [Header("BossMovement.Lighting")]
    // ����� Prefab
    public GameObject prefab_Lighting_Big;
    public GameObject prefab_Lighting_Small;
    public Transform LightingSpawnPos;

    // ���� 22-12-27
    // BossMovementŬ������ �ִ����� �̰����� �̰�
    void SpecialSkillStart()
    {
        // ����������� ������ �̶�� return
        if (isPlayspecialSkill)
            return;

        stopUpdate = true;
        specialSkillUsed = true;
        hitBoxCol.enabled = false;
        anim.SetTrigger("specialStart");
        anim.SetBool("Force", true);

        // ���� 22-12-27
        // ���߿� �ߴ°��� ����� ������ �� �ֵ��� ����
        transform.position = new Vector3(0f, transform.position.y, transform.position.z);

        // ����� �ڷ�ƾ ���� üũ
        isPlayspecialSkill = true;
        StartCoroutine(SpecialSkill());
    }

    bool isPlayspecialSkill = false;
    public IEnumerator SpecialSkill()
    {
        // ���⼭ �ִϸ��̼� ���ؼ� ������

        // ������ ���� ���ð� 1��
        yield return new WaitForSeconds(2f);

        // ���� ū ����� ��ȯ
        GameObject big = Instantiate(prefab_Lighting_Big);
        big.transform.position = ConstantValue.lighting_big_origin_pos;

        yield return new WaitForSeconds(2f);



        for (int a = 0; a < 15; a++)
        {
            GameObject small = Instantiate(prefab_Lighting_Small);

            small.transform.position = big.transform.position; 

            LightingMove script = small.GetComponent<LightingMove>();
            Vector3 calcEndPos = new Vector3(
                ConstantValue.lighting_small_end_pos.x + UnityEngine.Random.Range(-ConstantValue.lighting_small_witdh_x, +ConstantValue.lighting_small_witdh_x),
                ConstantValue.lighting_small_end_pos.y,
                ConstantValue.lighting_small_end_pos.z);
            script.MoveStart(big.transform.position, calcEndPos);

            /*
            small.transform.position = new Vector3(
                big.transform.position.x + UnityEngine.Random.Range(-5f, 5f),
                big.transform.position.y + UnityEngine.Random.Range(-5f, 5f),
                big.transform.position.z);
                */
            yield return new WaitForSeconds(0.1f);
        }

        Vector3 BigcalcEndPos = new Vector3(
                player.transform.position.x,
                ConstantValue.lighting_small_end_pos.y,
                player.transform.position.z);
        LightingMove script_big = big.GetComponent<LightingMove>();
        script_big.MoveStart(big.transform.position, BigcalcEndPos);

        yield return new WaitForSeconds(2f);

        stopUpdate = false;
        specialSkillUsed = false;
        hitBoxCol.enabled = true;
        anim.SetBool("Force", false);
        anim.SetTrigger("specialEnd");
    }
}
