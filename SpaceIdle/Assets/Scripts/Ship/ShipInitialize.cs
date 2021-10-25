using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "New Ship", fileName = "NewShip")]
public class ShipInitialize : ScriptableObject
{
    [Header("Properties")]
    public int health = 10;
    public float speed = 5.0f;
    public float speedDuration = 2.0f;
    public float turnDuration = 0.35f;
    public int price = 15;
    public int gunCapacity = 1;

    [Header("Appearance")]
    public GameObject shipObject = null;

    [Header("UI")]
    public string shipName = "New Ship";
    public Image shipIcon = null;
}
