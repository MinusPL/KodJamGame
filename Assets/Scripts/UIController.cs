using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    enum eUI_STATE
    {
        DEFAULT,
        OPEN_INV,
        INVENTORY
    }

    public GameObject player;

    public GameObject inventory;
    public GameObject mainUi;
    public GameObject healthIndicator;

    PlayerController playerController;
    InventoryController invController;

    eUI_STATE uiState;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        invController = inventory.GetComponent<InventoryController>();
        mainUi.SetActive(true);
        inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        healthIndicator.GetComponent<RawImage>().color = new Color(
                                                                    healthIndicator.GetComponent<RawImage>().color.r,
                                                                    healthIndicator.GetComponent<RawImage>().color.g,
                                                                    healthIndicator.GetComponent<RawImage>().color.b,
                                                                    1 - playerController.GetHealthRatio());
        
        bool invKey = Input.GetButtonDown("Inventory");
        bool cancelKey = Input.GetButtonDown("Cancel");
        switch (uiState)
        {
            case eUI_STATE.DEFAULT:
                if (invKey) uiState = eUI_STATE.OPEN_INV;
                
                break;
            case eUI_STATE.OPEN_INV:
                var itemList = playerController.Inventory;
                int c = 0;
                foreach(var item in itemList)
				{
                    
				}
                mainUi.SetActive(false);
                inventory.SetActive(true);
                //Move to INVENTORY STATE
                uiState = eUI_STATE.INVENTORY;
                break;
            case eUI_STATE.INVENTORY:
                if(cancelKey)
				{
                    mainUi.SetActive(true);
                    inventory.SetActive(false);
                    uiState = eUI_STATE.DEFAULT;
				}
                break;
        }
    }
}
