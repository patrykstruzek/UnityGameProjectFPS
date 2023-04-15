using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [Header("Assignables")]
    [SerializeField] private GameObject bullet;

    [SerializeField] Camera cam;
    [SerializeField] Transform attackPoint;

    [SerializeField] GameObject muzzleFlash;
    [SerializeField] TextMeshProUGUI ammoDisplay;

    [Header("Stats")]
    [SerializeField] private float shootForce;
    [SerializeField] private float upwardForce;

    [SerializeField] float timeBetweenShooting;
    [SerializeField] float spread;
    [SerializeField] float reloadTime;
    [SerializeField] float timeBetweenShots;
    [SerializeField] int magazineSize;
    [SerializeField] int bulletsPerTap;
    [SerializeField] bool allowButtonHold;

    int bulletsLeft;
    int bulletsShot;

    bool shooting;
    bool readyToShoot;
    bool reloading;

    [SerializeField] bool allowInvoke = true;

    Animator shotAnim;

    private void Awake()
    {
        shotAnim = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        PlayerInput(); 

        if (ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft / bulletsPerTap + "I" + magazineSize / bulletsPerTap);
        }
    }

    private void PlayerInput()
    {
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) & bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        if (readyToShoot & shooting & !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        if (readyToShoot && shooting & !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        shotAnim.SetTrigger("shoot");

        readyToShoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x= Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);


        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);


        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShoot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("ResetShoot", timeBetweenShots);
        }
    }

    private void ResetShoot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
    
}
