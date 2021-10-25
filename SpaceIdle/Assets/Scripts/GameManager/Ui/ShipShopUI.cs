using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipShopUI : MonoBehaviour, IShop
{
#region variables

    [Header("Middle Panels")]
    public GameObject middlePanelShipDetail;
    public GameObject middlePanelGunDetail;
    [SerializeField] private Image _shipTurningImage;
    [SerializeField] private TMP_Text _shipDetailTxt;

    [Header("Bottom Panels")] [Space(15)]
    public GameObject shipPanel;
    public GameObject gunPanel;
    public Transform shipContent;   

    // require privates
    private UiManager _uiManager;
    private SpaceShipController _shipController;
    private GunController _gunController;

    private GunShopUI _gunShopUI;

#endregion

    private void Start() {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();

        _uiManager = GetComponent<UiManager>();
        _gunShopUI = GetComponent<GunShopUI>();
    }

    public void OpenPanel(){
        // shop panels
        shipPanel.SetActive(true);
        gunPanel.SetActive(false);

        // middle detail panel
        middlePanelShipDetail.SetActive(true);
        middlePanelGunDetail.SetActive(false);
    }

    public void SelectButton(int cardIx){
        // _shipTurningImage.sprite = shipInits[cardIx].shipIcon.sprite;
        _shipDetailTxt.SetText("Health: " + _shipController.shipInits[cardIx].health + "\n" +
                               "Speed: " + _shipController.shipInits[cardIx].speed + "\n" +
                               "Speed Duration: " + _shipController.shipInits[cardIx].speedDuration + "\n" +
                               "Turn Duration: " + _shipController.shipInits[cardIx].turnDuration);
    }
    
    public void Purchase(int cardIx){
        if(_shipController.money < _shipController.shipInits[cardIx].price){
            // not enough money
            Debug.Log("Not enough money!");

            return;
        }

        // initialize ship properties
        _shipController.shipInit = _shipController.shipInits[cardIx];
        _shipController.money -= _shipController.shipInits[cardIx].price;

        _shipController.CreateShipObject(true);

        // disable purchase button
        shipContent.GetChild(cardIx).GetChild(2).GetComponent<Button>().interactable = false;

        _shipController.ownedShips.Add(_shipController.shipInit);

        // update gunShopUI
       _gunShopUI.AutoInitialize();

        // update UI
        _uiManager.UpdateMoneyUI();
        _uiManager.UpdateHealthUI();
    }
    
    public void AutoInitialize(){
        for(int i = 0; i < _shipController.shipInits.Count; i++){
            shipContent.GetChild(i).gameObject.SetActive(true);

            // Set Name
            shipContent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().SetText(_shipController.shipInits[i].shipName);

            // Set Image !!!!!!!!
            // shipContent.GetChild(i).GetChild(1).GetComponent<Image>().sprite = shipInits[i].shipIcon.sprite;

            // Set Gold Amount
            shipContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<TMP_Text>().SetText(_shipController.shipInits[i].price + " Gold");

            // Set purchase button's interactability
            if(shipContent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text.Equals(_shipController.shipInit.shipName)){
                shipContent.GetChild(i).GetChild(2).GetComponent<Button>().interactable = false;
            }
            else{
                shipContent.GetChild(i).GetChild(2).GetComponent<Button>().interactable = true;
            }
        }
    }

}
