using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantValue
{
    public static readonly float player_attack_damage = 70f;

    #region 원기옥 관련 상수값
    public static readonly Vector3 lighting_big_origin_pos = new Vector3(0.55f, 7.00f, 0f);
    public static readonly Vector3 lighting_small_end_pos = new Vector3(0.00f, -2.00f, 0f); // 떨어지는 영역 중심
    public static readonly float lighting_small_witdh_x = 10f;    // 좌우 퍼지는 절대값 영역
    #endregion


    public static readonly float camera_shake_intensity = 0.007f; // 카메라 흔들림 감도
}
