using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "New Gun", fileName = "NewGun")]
public class GunInitialize : ScriptableObject
{
    public string gunName = "New Gun";
    public int damage = 1;
    public float attackSecond = 5;
    public int price = 1;
    public Image gunIcon;
    public GameObject gunObject;
}
