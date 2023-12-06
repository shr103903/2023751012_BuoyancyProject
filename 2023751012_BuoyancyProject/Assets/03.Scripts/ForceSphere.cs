using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구
public class ForceSphere : Force
{
    private Rigidbody rigid = null;

    private Vector3 gravity = new Vector3(0, -9.81f, 0);

    private float radius = 0.5f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    protected override void ForceObject()
    {
        rigid.AddForce(gravity * rigid.mass);
        rigid.AddForce(Wave.instance.density * GetSubmergedVolume() * -gravity);
    }

    protected override float GetSubmergedVolume()
    {
        float waterHeight = Wave.instance.GetOrSetHeight(transform.position.x, transform.position.z);

        // 일부가 잠긴 상태
        if (Mathf.Abs(waterHeight - transform.position.y) < transform.lossyScale.y * 0.5f)
        {
            // 구 중심에서 물 표면까지의 거리
            float surfacedistance = waterHeight - transform.position.y;

            radius = GetRadius();
            // 구의 중심 높이 < 물 표면 높이
            if (surfacedistance >= 0)
            {
                // (구 부피) - (잘린 구의 작은 부분 부피)
                return GetSphereVolume(radius) - GetCutSphereVolume(radius, surfacedistance);
            }
            // 구의 중심 높이 > 물 표면 높이
            else
            {
                // 잘린 구의 작은 부분 부피
                return GetCutSphereVolume(radius, -surfacedistance);
            }
        }
        // 안 잠긴 상태
        else if (waterHeight < transform.position.y)
        {
            return 0;
        }
        //완전히 잠긴 상태
        else
        {
            radius = GetRadius();
            return GetSphereVolume(radius);
        }
    }

    private float GetRadius()
    {
        return transform.lossyScale.x * 0.5f;
    }

    // 구의 부피
    private float GetSphereVolume(float radius)
    {
        return (4 * Mathf.PI * radius * radius * radius) / 3;
    }

    // 평면으로 잘려진 구의 작은 부분 부피
    private float GetCutSphereVolume(float radius, float surfacedistance)
    {
        float curRadius = Mathf.Sqrt(radius * radius - surfacedistance * surfacedistance);
        float cutHeight = radius - surfacedistance;

        return (3 * curRadius * curRadius + cutHeight * cutHeight) / 6;
    }
}
