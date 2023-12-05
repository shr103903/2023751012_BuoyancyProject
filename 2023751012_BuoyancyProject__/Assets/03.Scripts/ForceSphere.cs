using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��
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

        // �Ϻΰ� ��� ����
        if (Mathf.Abs(waterHeight - transform.position.y) < transform.lossyScale.y * 0.5f)
        {
            // �� �߽ɿ��� �� ǥ������� �Ÿ�
            float surfacedistance = waterHeight - transform.position.y;

            radius = GetRadius();
            // ���� �߽� ���� < �� ǥ�� ����
            if (surfacedistance >= 0)
            {
                // (�� ����) - (�߸� ���� ���� �κ� ����)
                return GetSphereVolume(radius) - GetCutSphereVolume(radius, surfacedistance);
            }
            // ���� �߽� ���� > �� ǥ�� ����
            else
            {
                // �߸� ���� ���� �κ� ����
                return GetCutSphereVolume(radius, -surfacedistance);
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
            return GetSphereVolume(radius);
        }
    }

    private float GetRadius()
    {
        return transform.lossyScale.x * 0.5f;
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
