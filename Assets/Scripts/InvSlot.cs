using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour
{
    public GameObject imageObject;
    public Texture texture;

    public IItemStack item;

    RawImage image;
    // Start is called before the first frame update
    void Awake()
    {
        image = imageObject.GetComponent<RawImage>();
        item = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(IItemStack item)
	{
        this.item = item;
        texture = item.ItemTexture;
        image.texture = texture;
        image.color = new Color(1, 1, 1, 1);
    }
}
