using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
#region variables

    [Header("Guns")]
    public List<GunInitialize> gunInits;
    public List<GameObject> wearedGuns;
    public List<GunInitialize> ownedGuns;
    public bool isGunsValid;

    [Header("Bullet")] [Space(15)]
    public Transform bulletPrefab;

    [Header("Attack")] [Space(15)]
    public float bulletSpeed = 5.0f;
    [SerializeField] private float _playerTapAttackThreshold = 0.1f;

    private SpaceShipController _shipController;
    private PlayerInputController _inputs;
    private float _playerTapAttackTime;
    private bool _canPlayerTap = true;
    
#endregion

    private void Awake()
    {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _inputs = GameObject.Find("Input_Manager").GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        #region player tap
        // if player tapped to screen than make the ship ready to attack
        if (_inputs.isTapped && _canPlayerTap)
        {
            foreach (GameObject gun in wearedGuns)
            {
                gun.GetComponent<GunProperty>().attackTimer = gun.GetComponent<GunProperty>().gunInit.attackSecond;
            }

            _canPlayerTap = false;
            _inputs.isTapped = false;
            _playerTapAttackTime = 0.0f;
        }

        // player must not be able to tap very fast, so we add time between every tap
        if (!_canPlayerTap)
        {
            _playerTapAttackTime += Time.deltaTime;
            if (_playerTapAttackTime >= _playerTapAttackThreshold)
            {
                _canPlayerTap = true;
                _playerTapAttackTime = 0.0f;
            }
        }
        #endregion

        // if ship reach to destination than attack
        if (_shipController.isReachToDestination && isGunsValid)
        {
            foreach (GameObject gun in wearedGuns)
            {
                gun.GetComponent<GunProperty>().Attack();
            }
        }

        // We want to attack immediately when ship reach the destination
        else
        {
            foreach (GameObject gun in wearedGuns)
            {
                gun.GetComponent<GunProperty>().attackTimer = gun.GetComponent<GunProperty>().gunInit.attackSecond;
            }
        }
    }

    public void AddNewGun(GunInitialize gunInit, int gunSlotIx, bool isGunNew)
    {
        // get the gun slots
        List<Transform> gunSlots = GetGunSlots();

        // create new gun and wear it
        GameObject newGun = Instantiate(gunInit.gunObject, Vector3.zero, Quaternion.identity);
        SetObjectsParent(newGun.transform, gunSlots[gunSlotIx]);

        // add gun to weared guns list
        wearedGuns.Add(newGun);
        isGunsValid = true;

        if (isGunNew) ownedGuns.Add(gunInit);

        // set all guns attack timer to 0
        foreach (GameObject gun in wearedGuns)
        {
            gun.GetComponent<GunProperty>().attackTimer = 0.0f;
        }
    }

    public void UpgradeGun(GunInitialize gunInit, int gunSlotIx)
    {
        List<Transform> gunSlots = GetGunSlots();

        // remove old gun
        Destroy(gunSlots[gunSlotIx].GetChild(0).gameObject);
        wearedGuns.Remove(gunSlots[gunSlotIx].GetChild(0).gameObject);

        // wear new gun
        GameObject newGun = Instantiate(gunInit.gunObject, Vector3.zero, Quaternion.identity);
        SetObjectsParent(newGun.transform, gunSlots[gunSlotIx]);

        wearedGuns.Add(newGun);
        isGunsValid = true;

        // add new gun to owned guns list
        ownedGuns.Add(gunInit);

        // set all guns attack timer to 0
        foreach (GameObject gun in wearedGuns)
        {
            gun.GetComponent<GunProperty>().attackTimer = 0.0f;
        }
    }

    public List<Transform> GetGunSlots(){
        // take only gunSlots
        List<Transform> gunSlots = new List<Transform>();

        for(int i = 0; i < _shipController.ship.transform.childCount; i++){

            // if child's tag is gunSlot than add its transform to the list
            if(_shipController.ship.transform.GetChild(i).tag == "GunSlot"){
                gunSlots.Add(_shipController.ship.transform.GetChild(i));
            }
        }

        return gunSlots;
    }

    private void SetObjectsParent(Transform childObject, Transform parent){
        childObject.SetParent(parent);
        childObject.localPosition = Vector3.zero;
        childObject.localRotation = Quaternion.identity;
    }
}
