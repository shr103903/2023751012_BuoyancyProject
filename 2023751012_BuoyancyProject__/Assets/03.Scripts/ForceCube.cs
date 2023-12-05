using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ü
public class ForceCube : Force
{
    private Rigidbody rigid = null;

    private Vector3 gravity = new Vector3(0, -9.81f, 0);

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
        float submergedHeight = 0;

        // �Ϻΰ� ��� ����
        if (Mathf.Abs(waterHeight - transform.position.y) < transform.lossyScale.y * 0.5f)
        {
            if (waterHeight < transform.position.y)
            {
                submergedHeight = transform.position.y - waterHeight;
            }
            else
            {
                submergedHeight = transform.lossyScale.y * 0.5f + (waterHeight - transform.position.y);
            }
        }
        // �� ��� ����
        else if(waterHeight < transform.position.y)
        {
            submergedHeight = 0;
        }
        //������ ��� ����
        else
        {
            submergedHeight = transform.lossyScale.y;
        }

        return transform.lossyScale.x * transform.lossyScale.x * submergedHeight;
    }
}
