using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunUpgradeUI : MonoBehaviour, IUpgrade
{
#region variables

    [Header("Middle Panel")]
    public GameObject middlePanelShipDetail;
    public GameObject middlePanelGunDetail;
    public Transform detailContent;
    [SerializeField] private Image _gunImage;

    [Header("Bottom Panel")]
    [Space(15)]
    public GameObject shipPanel;
    public GameObject gunPanel;
    public Transform gunContent;

    // require privates
    private UiManager _uiManager;
    private SpaceShipController _shipController;
    private GunController _gunController;

    private int _gunSlotButtonIx = 0;

#endregion

    private void Start() {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();
        _uiManager = GetComponent<UiManager>();
    }

    public void OpenPanel()
    {
        // upgrade panels
        shipPanel.SetActive(false);
        gunPanel.SetActive(true);

        // middle detail panel
        middlePanelShipDetail.SetActive(false);
        middlePanelGunDetail.SetActive(true);
    }

    public void Upgrade(int abilityBtnIx)
    {
        switch (abilityBtnIx)
        {
            // damage
            case 0:
                _shipController.shipInit.health += 5;
                break;

            // attack second
            case 1:
                _shipController.shipInit.speed += 0.25f;
                break;
        }

        UpdateDetailTxt();
    }

    public void Wear(int itemCardIx)
    {
        // if player does not select any of gun slot buttons its deafult value is 0
    }

    public void AutoInitialize()
    {
        for (int i = 0; i < _gunController.ownedGuns.Count; i++)
        {
            gunContent.GetChild(i).gameObject.SetActive(true);

            // ship name
            gunContent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().SetText(_gunController.ownedGuns[i].gunName);

            // ship image
            // shipContent.GetChild(i).GetChild(1).GetComponent<Image>().sprite = _shipController.ownedShips[i].shipIcon.sprite;
        }

        UpdateDetailTxt();
    }

    public void UpdateDetailTxt()
    {
        // damage txt
        detailContent.GetChild(0).GetChild(0).GetComponent<TMP_Text>().SetText("  Damage : " + _gunController.ownedGuns[_gunSlotButtonIx].damage);

        // attack second txt
        detailContent.GetChild(1).GetChild(0).GetComponent<TMP_Text>().SetText("  Attack Second : " + _gunController.ownedGuns[_gunSlotButtonIx].attackSecond);
    }

    public void SelectGunSlotButton(int gunSlotBtnIx) => _gunSlotButtonIx = gunSlotBtnIx;
}
