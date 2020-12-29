using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHidingController : MonoBehaviour
{
    public GameObject player;
    public GameObject shownInDark;
    public GameObject hiddenInDark;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.IsDarkDimension())
        {
            hiddenInDark.SetActive(false);
            shownInDark.SetActive(true);
        }
        else
        {
            hiddenInDark.SetActive(true);
            shownInDark.SetActive(false);
        }
    }
}
