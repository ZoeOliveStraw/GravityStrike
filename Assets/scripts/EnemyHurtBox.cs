using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHurtBox : MonoBehaviour
{

    [SerializeField] private GameObject explosion;
    private void Start()
    {
    }


    public void death() 
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.GameObject());
    }
}
