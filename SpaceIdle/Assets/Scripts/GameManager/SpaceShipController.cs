using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpaceShipController : MonoBehaviour
{
#region variables
    [Header("Ship Camera")]
    public CinemachineVirtualCamera shipCamFollow;

    [Header("Initialize")] [Space(15)]
    public ShipInitialize shipInit;
    public GunInitialize defaultGunInit;
    public GameObject ship;

    [Header("Ships")]
    public List<ShipInitialize> shipInits;
    public List<ShipInitialize> ownedShips;

    [Header("Player")] [Space(15)]
    public int level = 1;
    public int money = 0;
    public float exp = 0.0f;
    [SerializeField] private float _expLimit = 20;

    [SerializeField] private int _health;
    public int Health{ get => _health; set => _health = Mathf.Clamp(value, 0, shipInit.health); }

    // Enemy
    [Header("Target")]
    public bool isReachToDestination;
    public Transform target;

    // Requirements
    private UiManager _uiManager;
    private GunController _gunController;
    private MineController _mineController;
    
#endregion

    private void Start() {
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UiManager>();
        _mineController = GameObject.Find("Enemy_Manager").GetComponent<MineController>();

        CreateShipObject(true);
        ownedShips.Add(shipInit);
    }

    public void UpdateLevel(float amount){
        exp += amount;

        if(exp >= _expLimit){
            exp -= _expLimit;
            _expLimit += level * 21.0f;
            level++;
            _uiManager.UpdateLevelUI();
        }
    }

    public void HealthRegen(int amount) => Health += amount;

    public void CreateShipObject(bool healthRegenerate) {
        GameObject newShip = null;

        if(ship != null){
            newShip = Instantiate(shipInit.shipObject, ship.transform.position, ship.transform.rotation);
            newShip.AddComponent<SpaceShipMovement>();
            Destroy(ship);
        }
        else{
            newShip = Instantiate(shipInit.shipObject, Vector3.zero, Quaternion.identity);
            newShip.AddComponent<SpaceShipMovement>();
        }
        
        ship = newShip;
        ship.transform.SetParent(GameObject.Find("Player").transform);

        _mineController.ship = ship.transform;
        _mineController.shipMovement = ship.GetComponent<SpaceShipMovement>();

        shipCamFollow.m_Follow = ship.transform;
        shipCamFollow.m_LookAt = ship.transform;

        // add guns to the ship
        List<Transform> gunSlots = _gunController.GetGunSlots();

        _gunController.wearedGuns.Clear();
        if(_gunController.ownedGuns.Count > 0){
            for(int i = 0; i < gunSlots.Count; i++){
                if(_gunController.ownedGuns.Count - (i + 1) < 0) break;

                _gunController.AddNewGun(_gunController.ownedGuns[_gunController.ownedGuns.Count - (i + 1)], i, false);
            }
        }
        else{
            _gunController.AddNewGun(defaultGunInit, 0, true);
        }

        if(healthRegenerate)
            HealthRegen(shipInit.health);
    }
}
