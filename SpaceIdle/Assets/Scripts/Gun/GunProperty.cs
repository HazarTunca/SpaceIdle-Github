using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProperty : MonoBehaviour
{
#region variables

    [Header("Gun Initialize")]
    public GunInitialize gunInit;
    
    [Header("Attack")] [Space(15)]
    public float attackTimer;

    private GunController _gunController;
    private SpaceShipController _shipController;

#endregion

    private void Start()
    {
        _gunController = GameObject.Find("Gun_Manager").GetComponent<GunController>();
        _shipController = GameObject.Find("Ship_Manager").GetComponent<SpaceShipController>();
    }

    public void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= gunInit.attackSecond)
        {
            // create the bullet and set its damage
            Transform newBullet = Instantiate(_gunController.bulletPrefab, transform.position, transform.rotation);
            newBullet.GetComponent<BulletProperty>().damage = gunInit.damage;

            // set the direction and fire!
            Vector3 targetDir = _shipController.target.position - transform.position;
            newBullet.GetComponent<Rigidbody>().AddForce(targetDir * _gunController.bulletSpeed, ForceMode.Impulse);

            if (newBullet != null) Destroy(newBullet.gameObject, 4.0f);
            attackTimer = 0.0f;
        }
    }
}
