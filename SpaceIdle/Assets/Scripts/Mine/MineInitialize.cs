using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Mine", fileName = "NewMine")]
public class MineInitialize : ScriptableObject
{
    public string mineName = "New Mine";
    public int health = 5;
    public int givenMoney = 1; 
    public int givenExp = 1;
}
