using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ü (������ ������ü�� ���)
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

        // �Ϻΰ� ��� ����
        if (Mathf.Abs(waterHeight - transform.position.y) < transform.lossyScale.y * 0.5f)
        {
            // �� �߽ɿ��� �� ǥ������� �Ÿ�
            float surfacedistance = waterHeight - transform.position.y;

            radius = GetRadius();
            // ���� �߽� ���� < �� ǥ�� ����
            if (surfacedistance >= 0)
            {
                // (������ü ����) * (((�� ����) - (�߸� ���� ���� �κ� ����)) / (�� ����))
                return GetCuboidVolume() * (GetSphereVolume(radius) - GetCutSphereVolume(radius, surfacedistance)) / GetSphereVolume(radius);
            }
            // ���� �߽� ���� > �� ǥ�� ����
            else
            {
                // (������ü ����) * ((�߸� ���� ���� �κ� ����) / (�� ����))
                return GetCuboidVolume() * (GetCutSphereVolume(radius, -surfacedistance)) / GetSphereVolume(radius);
            }
        }
        // �� ��� ����
        else if (waterHeight < transform.position.y)
        {
            return 0;
        }
        //������ ��� ����
        else
        {
            radius = GetRadius();
            // (������ü ����)
            return GetCuboidVolume();
        }
    }

    private float GetRadius()
    {
        return transform.lossyScale.x * 0.5f;
    }

    // ������ü ����
    private float GetCuboidVolume()
    {
        return rigid.transform.lossyScale.x * rigid.transform.lossyScale.x * rigid.transform.lossyScale.z;
    }

    // ���� ����
    private float GetSphereVolume(float radius)
    {
        return (4 * Mathf.PI * radius * radius * radius) / 3;
    }

    // ������� �߷��� ���� ���� �κ� ����
    private float GetCutSphereVolume(float radius, float surfacedistance)
    {
        float curRadius = Mathf.Sqrt(radius * radius - surfacedistance * surfacedistance);
        float cutHeight = radius - surfacedistance;

        return (3 * curRadius * curRadius + cutHeight * cutHeight) / 6;
    }
}
