using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunShopUI : MonoBehaviour, IShop
{
#region variables

    [Header("Middle Panels")]
    public GameObject middlePanelShipDetail;
    public GameObject middlePanelGunDetail;
    public Transform gunSlotButtonContent;
    [SerializeField] private TMP_Text _gunDetailTxt;

    [Header("Bottom Panels")] [Space(15)]
    public GameObject shipPanel;
    public GameObject gunPanel;
    public Transform gunContent;

    // require privates
    private int _gunSlotIx = 0;

    private UiManager _uiManager;
    private SpaceShipController _shipController;
    private GunController _gunController;

#endregion

    private void Start() {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();
        _uiManager = GetComponent<UiManager>();
    }

    public void OpenPanel(){
        // shop panels
        shipPanel.SetActive(false);
        gunPanel.SetActive(true);

        // middle detail panel
        middlePanelShipDetail.SetActive(false);
        middlePanelGunDetail.SetActive(true);
    }

    public void SelectButton(int cardIx){
        _gunDetailTxt.SetText("Damage: " + _gunController.gunInits[cardIx].damage + "\n" +
                              "Attack Second: " + _gunController.gunInits[cardIx].attackSecond);
    }

    public void Purchase(int cardIx){
        if(_shipController.money < _gunController.gunInits[cardIx].price){
            // not enough money
            Debug.Log("Not enough Money!");

            return;
        }

        // if there is a gun under the gunSlot, upgrade the gun. If not add it as a new gun
        if(_shipController.ship.transform.GetChild(_gunSlotIx).childCount > 0){
            _gunController.UpgradeGun(_gunController.gunInits[cardIx], _gunSlotIx);
        }
        else{
            _gunController.AddNewGun(_gunController.gunInits[cardIx], _gunSlotIx, true);
        }
        
        _shipController.money -= _gunController.gunInits[cardIx].price;
        _uiManager.UpdateMoneyUI();

        CheckGunsAreBuyable();
        
        Invoke("UpdateGunSlotUI", 0.05f);
    }

    public void AutoInitialize(){
        for(int i = 0; i < _gunController.gunInits.Count; i++){
            gunContent.GetChild(i).gameObject.SetActive(true);

            // Set Name
            gunContent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().SetText(_gunController.gunInits[i].gunName);

            // Set Image !!!!!!!!
            // gunContent.GetChild(i).GetChild(1).GetComponent<Image>().sprite = gunInits[i].gunIcon.sprite;

            // Set Gold Amount
            gunContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<TMP_Text>().SetText(_gunController.gunInits[i].price + " Gold");
        }

        CheckGunsAreBuyable();
        ActivateGunSlotButtons();
    }

    // used for onClickMethod on gun slots
    public void GunSlotButton(int slotIx) => _gunSlotIx = slotIx;

    public void CheckGunsAreBuyable(){
        // get the gun cards
        List<Transform> gunCards = new List<Transform>();
        for(int i = 0; i < gunContent.childCount; i++){
            gunCards.Add(gunContent.GetChild(i));
        }

        string gunName = "";
        int sameGunCount = 0;

        // check the gun names in the ownedguns list
        foreach(GunInitialize gun in _gunController.ownedGuns){
            foreach(GunInitialize otherGun in _gunController.ownedGuns){
                if(gun.gunName.Equals(otherGun.gunName)){
                    sameGunCount++;
                    gunName = gun.gunName;
                }
            }

            // if there is a same gun equal amount of the guncapacity than disable the button
            if(sameGunCount + 1 > _shipController.shipInit.gunCapacity){
                foreach(Transform gunCard in gunCards){
                    if(gunCard.GetChild(0).GetComponent<TMP_Text>().text.Equals(gunName)){
                        gunCard.GetChild(2).GetComponent<Button>().interactable = false;
                    }
                }
                sameGunCount = 0;
            }
            // but there is no equality than enable the button again
            else{
                foreach(Transform gunCard in gunCards){
                    if(gunCard.GetChild(0).GetComponent<TMP_Text>().text.Equals(gunName)){
                        gunCard.GetChild(2).GetComponent<Button>().interactable = true;
                    }
                }
                sameGunCount = 0;
            }
        }
    }

    public void ActivateGunSlotButtons(){
        // get the gunSlotbuttons
        List<GameObject> gunSlotBtns = new List<GameObject>();
        for(int i = 0; i < gunSlotButtonContent.childCount; i++){
            gunSlotBtns.Add(gunSlotButtonContent.GetChild(i).gameObject);
        }

        // Deactive all
        foreach(GameObject gunSlotButton in gunSlotBtns){
            gunSlotButton.SetActive(false);
        }

        // activate require slots
        for(int i = 0; i < _shipController.shipInit.gunCapacity; i++){
            gunSlotBtns[i].SetActive(true);
        }

        UpdateGunSlotUI();
    }

    private void UpdateGunSlotUI(){
        List<Transform> gunSlots = _gunController.GetGunSlots();
        
        // get the gunSlotbuttons
        List<GameObject> gunSlotBtns = new List<GameObject>();
        for(int i = 0; i < gunSlotButtonContent.childCount; i++){
            gunSlotBtns.Add(gunSlotButtonContent.GetChild(i).gameObject);
        }

        // activate require slots
        for(int i = 0; i < gunSlots.Count; i++){
            // gun slot detail
            if(gunSlots[i].childCount == 0){
                gunSlotBtns[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("Empty Slot");
            }
            else{
                gunSlotBtns[i].transform.GetChild(0).GetComponent<TMP_Text>().SetText("Name: " + gunSlots[i].transform.GetChild(0).GetComponent<GunProperty>().gunInit.gunName + "\n" +
                                                                                    "Damage: " + gunSlots[i].transform.GetChild(0).GetComponent<GunProperty>().gunInit.damage + "\n" + 
                                                                                    "Attack Second: " + gunSlots[i].transform.GetChild(0).GetComponent<GunProperty>().gunInit.attackSecond);
            }
        }
    }
}
