using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MineProperties))]
public class MineCollisionDetection : MonoBehaviour
{
#region variables

    private SpaceShipController _shipController;
    private UiManager _uiManager;
    private GunController _gunController;
    private MineProperties _mineProperties;
    
#endregion

    private void Start() {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UiManager>();
        _mineProperties = GetComponent<MineProperties>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("Bullet")){
            // get damage
            _mineProperties.Health -= other.GetComponent<BulletProperty>().damage;

            if(_mineProperties.Health <= 0){
                // give money and exp
                _shipController.money += _mineProperties.mineInit.givenMoney;
                _uiManager.UpdateMoneyUI();

                _shipController.UpdateLevel(_mineProperties.mineInit.givenExp);

                Destroy(transform.gameObject);
            }

            if(other != null)
                Destroy(other.gameObject);
        }
    }
}
