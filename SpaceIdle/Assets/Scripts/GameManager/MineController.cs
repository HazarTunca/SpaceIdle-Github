using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
#region variables

    [Header("Requirements")]
    public Transform ship;
    public GameObject minePrefab;
    public SpaceShipMovement shipMovement;

    [Header("Lists")] [Space(15)]
    public List<MineInitialize> mineInitializes;

    [Header("Mine")]
    [SerializeField] private float minRangeCreation = 25.0f;
    [SerializeField] private float maxRangeCreation = 35.0f;

    private GameObject mineClone;
    private SpaceShipController _shipController;
    private GunController _gunController;
    private UiManager _uiManager;
    private float _randomValue;
#endregion

    private void Start() {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UiManager>();
    }

    private void Update() {
        // if there no mine on the scene create one
        if(mineClone != null || ship == null) return;

        CreateMine();
        _shipController.target = mineClone.transform;
        
    }
    
    private void CreateMine(){
        _randomValue = GetRandomNumber();

        // get random location and create the mine
        Vector3 createLocation = new Vector3(ship.position.x + _randomValue, ship.position.y + _randomValue, ship.position.z + _randomValue);
        mineClone = Instantiate(minePrefab, createLocation, Quaternion.identity);

        int mineIx = Random.Range(0, mineInitializes.Count);
        mineClone.GetComponent<MineProperties>().mineInit = mineInitializes[mineIx];

        // play creation effect
    }

    private float GetRandomNumber(){
        // get random value but it must be between 10 -> 20 or -20 -> -10
        float randomValue = Random.Range(-1.0f, 1.0f);

        if( randomValue >= 0.0f) randomValue = Random.Range(minRangeCreation, maxRangeCreation);
        else randomValue = Random.Range(-maxRangeCreation, -minRangeCreation);
        
        return randomValue;
    }
}
