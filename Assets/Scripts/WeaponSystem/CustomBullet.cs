using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject explosion;
    [SerializeField] LayerMask enemy;

    [Header("Stats")]
    [Range(0f,1f)]
    [SerializeField] float bounciness;
    [SerializeField] bool useGravity;

    [Header("Damage")]
    [SerializeField] int explosionDamage;
    [SerializeField] float explosionRange;
    [SerializeField] float explosionForce;
    [SerializeField] int maxCollisions;
    [SerializeField] float maxLifeTime;
    [SerializeField] bool impactExplode = true;

    int collisions;
    PhysicMaterial physicMaterial;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (collisions > maxCollisions)
        {
            Explode();
        }

        maxLifeTime -= Time.deltaTime;
        if (maxLifeTime <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, enemy);
        for (int i = 0; i < enemies.Length; i++)
        {
            //enemies[i].GetComponent<ShootingAi>().TAkeDamage(explosionDamage);

            if (enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            return;
        }

        collisions++;

        if (collision.collider.CompareTag("Enemy") & impactExplode)
        {
            Explode();
        }


    }

    private void Setup()
    {
        physicMaterial = new PhysicMaterial();
        physicMaterial.bounciness = bounciness;
        physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physicMaterial;

        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explosionRange);
    }
}
