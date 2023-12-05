using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Force : MonoBehaviour
{
    protected virtual void FixedUpdate()
    {
        ForceObject();
    }

    protected abstract void ForceObject();

    protected abstract float GetSubmergedVolume();
}
