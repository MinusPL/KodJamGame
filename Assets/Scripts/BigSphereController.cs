using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BigSphereController : MonoBehaviour
{
    public float growTime;

    private float gTime = 0.0f;

    public float startScale = 0.1f;
    public float endScale = 50.0f;

    public bool enabled = false;

    private Material m_Material;

    // Start is called before the first frame update
    void Awake()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (gTime >= growTime)
            {
                Destroy(gameObject);
            }

            gTime += Time.deltaTime;
            double scale = startScale + ((endScale - startScale) * (gTime / growTime));
            m_Material.SetFloat("rangeVal", (gTime / growTime) / 2);
            transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
        }
    }

    public void Enable()
    {
        enabled = true;
    }
}
