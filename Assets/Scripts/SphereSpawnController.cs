using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawnController : MonoBehaviour
{
    public GameObject SpherePrefab;
    public GameObject starter;

    public float smallScale = 0.1f;
    public float bigScale = 50.0f;
    public float growTime = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnBigSphere(bool growing)
    {
        var obj = Instantiate(SpherePrefab);
        obj.GetComponent<BigSphereController>().growTime = growTime;
        obj.GetComponent<BigSphereController>().endScale = growing ? bigScale : smallScale;
        obj.GetComponent<BigSphereController>().startScale = growing ? smallScale : bigScale;
        obj.transform.parent = starter.transform;
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.GetComponent<BigSphereController>().Enable();
    }
}
