using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 직육면체 (넓적한 직육면체인 경우)
public class ForceCuboid : Force
{
    [SerializeField]
    private Rigidbody rigid = null;

    private Vector3 gravity = new Vector3(0, -9.81f, 0);

    private float radius = 0.5f;

    protected override void ForceObject()
    {
        rigid.AddForceAtPosition(gravity * rigid.mass, transform.position);
        rigid.AddForceAtPosition(Wave.instance.density * GetSubmergedVolume() * -gravity, transform.position);
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
                // (직육면체 부피) * (((구 부피) - (잘린 구의 작은 부분 부피)) / (구 부피))
                return GetCuboidVolume() * (GetSphereVolume(radius) - GetCutSphereVolume(radius, surfacedistance)) / GetSphereVolume(radius);
            }
            // 구의 중심 높이 > 물 표면 높이
            else
            {
                // (직육면체 부피) * ((잘린 구의 작은 부분 부피) / (구 부피))
                return GetCuboidVolume() * (GetCutSphereVolume(radius, -surfacedistance)) / GetSphereVolume(radius);
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
            // (직육면체 부피)
            return GetCuboidVolume();
        }
    }

    private float GetRadius()
    {
        return transform.lossyScale.x * 0.5f;
    }

    // 직육면체 부피
    private float GetCuboidVolume()
    {
        return rigid.transform.lossyScale.x * rigid.transform.lossyScale.x * rigid.transform.lossyScale.z;
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
