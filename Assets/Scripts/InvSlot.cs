using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour
{
    public GameObject imageObject;
    public Texture texture;

    RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        image = imageObject.GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(Texture tex)
	{
        this.texture = tex;
        image.texture = tex;
	}
}
