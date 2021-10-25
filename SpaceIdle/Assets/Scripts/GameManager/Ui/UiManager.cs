using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
#region variables

    [Header("Main UI Panels")]
    public GameObject playerStatsPanel;
    public GameObject mainBtnsPanel;
    
    // main UI privates
    private bool _isMainUIActive = true;

    [Header("Player Stats")] [Space(15)]
    public TMP_Text levelTxt;
    public TMP_Text healthTxt;
    public TMP_Text moneyTxt;

    [Header("Shop Panel")] [Space(15)]
    public Animator shopAnimator;
    public TMP_Text shopGoldTxt;

    // shop privates
    private int _shopPanelBool;

    [Header("Upgrade Panel")] [Space(15)]
    public Animator upgradeAnimator;
    public TMP_Text upgradeGoldTxt;

    //upgrade privates
    private int _upgradePanelBool;

    // require privates
    private SpaceShipController _shipController;
    private GunController _gunController;
    private ShipShopUI _shipShopUI;
    private GunShopUI _gunShopUI;
    private ShipUpgradeUI _shipUpgradeUI;
    private GunUpgradeUI _gunUpgradeUI;

#endregion

    private void Start() {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();
        _shipShopUI = GetComponent<ShipShopUI>();
        _gunShopUI = GetComponent<GunShopUI>();
        _shipUpgradeUI = GetComponent<ShipUpgradeUI>();
        _gunUpgradeUI = GetComponent<GunUpgradeUI>();

        _shopPanelBool = Animator.StringToHash("isOpen");
        _upgradePanelBool = Animator.StringToHash("isOpen");

        // initialize ships & wearedGuns in the shop
        _shipShopUI.AutoInitialize();
        _gunShopUI.AutoInitialize();

        // shop middle panel infos
        _shipShopUI.SelectButton(0);
        _gunShopUI.SelectButton(0);

        UpdateMoneyUI();
        UpdateLevelUI();
        UpdateHealthUI();
    }

    private void Update() {
        // if we want to make main UI active, than check the animation is playing closePose, if so activate the main UI
        if(_isMainUIActive){
            if(shopAnimator.GetCurrentAnimatorStateInfo(0).IsName("ShopPanelClosePose")){
                isMainUIActive(true);
            }
        }
        else{
            isMainUIActive(false);
        }
    }

    public void UpdateMoneyUI() {
        moneyTxt.SetText("Money: " + _shipController.money);
        shopGoldTxt.SetText("Money: " + _shipController.money);
        upgradeGoldTxt.SetText("Money: " + _shipController.money);
    }
    public void UpdateLevelUI() => levelTxt.SetText("Level: " + _shipController.level);
    public void UpdateHealthUI() => healthTxt.SetText("Health: " + _shipController.Health);

    // Shop
    public void OpenCloseShop(){
        if(shopAnimator.GetBool(_shopPanelBool)){
            shopAnimator.SetBool(_shopPanelBool, false);
            _isMainUIActive = true;
        }
        else{
            shopAnimator.SetBool(_shopPanelBool, true);
            _isMainUIActive = false;
        }
    }

    // Upgrade
    public void OpenCloseUpgrade(){
        if(upgradeAnimator.GetBool(_upgradePanelBool)){
            upgradeAnimator.SetBool(_upgradePanelBool, false);
            _isMainUIActive = true;
        }
        else{
            upgradeAnimator.SetBool(_upgradePanelBool, true);
            _isMainUIActive = false;

            _shipUpgradeUI.AutoInitialize();
            _gunUpgradeUI.AutoInitialize();
        }
    }

    // Main UI
    private void isMainUIActive(bool isActive){
        playerStatsPanel.SetActive(isActive);
        mainBtnsPanel.SetActive(isActive);
    }
}
