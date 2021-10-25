using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProperties : MonoBehaviour
{
#region variables
    public MineInitialize mineInit;

    [SerializeField] private float _turnSpeed = 0.5f;

    [SerializeField] private int _health;
    public int Health{
        get => _health;
        set => _health = value;
    }
#endregion

    private void Start() {
        Health = mineInit.health;
    }

    private void Update() {
        TurnAround();
    }

    private void TurnAround(){
        transform.Rotate(_turnSpeed, _turnSpeed, _turnSpeed);
    }
}
