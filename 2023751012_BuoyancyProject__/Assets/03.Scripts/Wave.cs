using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public static Wave instance = null;

    [SerializeField]
    public float density = 1.0f;

    [SerializeField]
    private float amplitude = 1.0f;

    [SerializeField]
    private float wavelength = 1.0f;

    [SerializeField]
    private float speed = 1.0f;

    private MeshFilter meshFilter = null;

    private float offset = 0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        offset = 0f;
    }

    private void Update()
    {
        offset += Time.deltaTime * speed;

        WaveWater();
    }

    public float GetOrSetHeight(float posX, float posZ)
    {
        return amplitude * Mathf.PerlinNoise((float)(posX / 10), (float)(posZ / 10)) * Mathf.Sin(posX / wavelength + offset);
    }

    private void WaveWater()
    {
        Vector3[] vertics = meshFilter.mesh.vertices;
        for (int i = 0; i < vertics.Length; i++)
        {
            vertics[i].y = GetOrSetHeight(transform.position.x + meshFilter.mesh.vertices[i].x, transform.position.z + meshFilter.mesh.vertices[i].z);
        }

        meshFilter.mesh.vertices = vertics;
        meshFilter.mesh.RecalculateNormals();
    }
}
