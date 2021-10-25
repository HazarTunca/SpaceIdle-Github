using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipUpgradeUI : MonoBehaviour, IUpgrade
{
#region variables

    [Header("Middle Panel")]
    public GameObject middlePanelShipDetail;
    public GameObject middlePanelGunDetail;
    public Transform detailContent;
    [SerializeField] private Image _shipTurningImage;

    [Header("Bottom Panel")]
    [Space(15)]
    public GameObject shipPanel;
    public GameObject gunPanel;
    public Transform shipContent;

    // require privates
    private UiManager _uiManager;
    private SpaceShipController _shipController;
    private GunUpgradeUI _gunUpgradeUI;
    private GunShopUI _gunShopUI;

    private bool _isShipNeedHealthRegen;

#endregion

    private void Start()
    {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _uiManager = GetComponent<UiManager>();
        _gunUpgradeUI = GetComponent<GunUpgradeUI>();
        _gunShopUI = GetComponent<GunShopUI>();
    }

    public void OpenPanel()
    {
        // upgrade panels
        shipPanel.SetActive(true);
        gunPanel.SetActive(false);

        // middle detail panel
        middlePanelShipDetail.SetActive(true);
        middlePanelGunDetail.SetActive(false);
    }

    public void Upgrade(int abilityBtnIx)
    {
        switch (abilityBtnIx)
        {
            // health
            case 0:
                _shipController.shipInit.health += 5;
                break;

            // speed
            case 1:
                _shipController.shipInit.speed += 0.25f;
                break;

            // speed duration
            case 2:
                _shipController.shipInit.speedDuration += 0.1f;
                break;

            // turn Duration
            case 3:
                _shipController.shipInit.turnDuration += 0.1f;
                break;
        }

        UpdateDetailTxt();
    }

    public void Wear(int itemCardIx)
    {
        if (_shipController.shipInit.health > _shipController.ownedShips[itemCardIx].health) _isShipNeedHealthRegen = true;
        else _isShipNeedHealthRegen = false;

        _shipController.shipInit = _shipController.ownedShips[itemCardIx];

        // if new ship's health is lower than old one's than regenerate the health
        if (_isShipNeedHealthRegen)
        {
            _shipController.CreateShipObject(true);
        }
        else
        {
            _shipController.CreateShipObject(false);
        }

        // update UI
        _uiManager.UpdateHealthUI();

        // initalize the cards
        _gunShopUI.AutoInitialize();
        AutoInitialize();
    }

    public void AutoInitialize()
    {
        for (int i = 0; i < _shipController.ownedShips.Count; i++)
        {
            shipContent.GetChild(i).gameObject.SetActive(true);

            // ship name
            shipContent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().SetText(_shipController.ownedShips[i].shipName);

            // ship image
            // shipContent.GetChild(i).GetChild(1).GetComponent<Image>().sprite = _shipController.ownedShips[i].shipIcon.sprite;

            // wear button interactablitiy
            if (shipContent.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text.Equals(_shipController.shipInit.shipName))
            {
                shipContent.GetChild(i).GetChild(2).GetComponent<Button>().interactable = false;
            }
            else
            {
                shipContent.GetChild(i).GetChild(2).GetComponent<Button>().interactable = true;
            }
        }

        UpdateDetailTxt();
    }

    // used at every image on upgrade panel
    public void UpdateDetailTxt()
    {
        // health
        detailContent.GetChild(0).GetChild(0).GetComponent<TMP_Text>().SetText("  Health : " + _shipController.shipInit.health);

        // speed
        detailContent.GetChild(1).GetChild(0).GetComponent<TMP_Text>().SetText("  Speed : " + _shipController.shipInit.speed);

        // speed duration
        detailContent.GetChild(2).GetChild(0).GetComponent<TMP_Text>().SetText("  Speed Duration : " + _shipController.shipInit.speedDuration);

        // turn duration
        detailContent.GetChild(3).GetChild(0).GetComponent<TMP_Text>().SetText("  Turn Duration : " + _shipController.shipInit.turnDuration);
    }
}
