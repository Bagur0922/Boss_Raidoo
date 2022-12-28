using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ≤¢¿Ÿ 22-12-27
// ΩÃ±€≈œ¿∏∑Œ ∫Ø∞Ê
public class CameraShake : BehaviourSingleton<CameraShake>
{
    [SerializeField] float m_force = 0f;
    [SerializeField] Vector3 m_offset = Vector3.zero;

    public bool shake;

    bool isShaking = false;

    Quaternion m_originRot;
    // Start is called before the first frame update

    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;
    private Vector3 OriginalPos;
    private Quaternion OriginalRot;

    /*void Start()
    {
        // √ ±‚»≠
        Shaking = true;

        m_originRot = transform.rotation;

        OriginalPos = transform.position;
        OriginalRot = transform.rotation;
    }
    public float shaketime = 0.0f;
    public void DoShake(float time = 0.3f)
    {
        Debug.Log("DoShake ");

        Shaking = true;
        shaketime = time;

        ShakeIntensity = ConstantValue.camera_shake_intensity;
    }

    public void StopShake()
    {
        Debug.Log("StopShake ");

        ShakeIntensity = 0f;
        Shaking = false;
    }

    void Update()
    {
        if (shaketime <= 0f)
        {
            transform.position = OriginalPos;
            transform.rotation = OriginalRot;

            ShakeIntensity = 0f;
            Shaking = false;
            return;
        }
        else
        {
            transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
            transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity),
                                                OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity),
                                                OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity),
                                                OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity));

            shaketime -= Time.deltaTime;
        }
    }*/

    
    // Update is called once per frame
    void Update()
    {
        if (shake && !isShaking)
        {
            isShaking = true;
            StartCoroutine(ShakeCoroutine());
        }
        else if (!shake)
        {
            isShaking = false;
            StopAllCoroutines();
            StartCoroutine(Reset());
        }
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 t_originEuler = transform.eulerAngles;
        while (true)
        {
            float t_rotX = Random.Range(-m_offset.x, m_offset.x);
            float t_rotY = Random.Range(-m_offset.y, m_offset.y);
            float t_rotZ = Random.Range(-m_offset.z, m_offset.z);

            Vector3 t_randomRot = t_originEuler + new Vector3(t_rotX, t_rotY, t_rotZ);
            Quaternion t_rot = Quaternion.Euler(t_randomRot);

            while(Quaternion.Angle(transform.rotation, t_rot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, t_rot, m_force * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    IEnumerator Reset()
    {
        while(Quaternion.Angle(transform.rotation, m_originRot) > 0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, m_originRot, m_force * Time.deltaTime);
            yield return null;
        }
    }
    
}
