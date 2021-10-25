using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SpaceShipMovement : MonoBehaviour
{
#region variables

    [Header("Movement")]
    [SerializeField] private float _distanceThreshold = 11f;
    
    private SpaceShipController _shipController;
    private CharacterController _controller;
    private float _speed;
    private float _distance;

#endregion

    private void Start() {
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        Move();

        if(_shipController.target == null) return;

        _distance = FindDistance();
        LookToTarget();
    }

    private void LookToTarget(){
        // smoothly look at the target
        Vector3 lookDir = _shipController.target.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), _shipController.shipInit.turnDuration * Time.deltaTime);
    }

    private void Move(){
        // just go forward
        float targetSpeed = _shipController.isReachToDestination ? 0.0f : _shipController.shipInit.speed;
        _speed = Mathf.Lerp(_speed, targetSpeed, _shipController.shipInit.speedDuration * Time.deltaTime);
        _controller.Move(transform.forward * _speed * Time.deltaTime);
    }

    private float FindDistance(){
        float distance = Vector3.Distance(transform.position, _shipController.target.position);

        // if distance lower than distance threshold ship is reach the destination
        if(distance < _distanceThreshold) _shipController.isReachToDestination = true;
        else _shipController.isReachToDestination = false;        

        return distance;
    }
}
