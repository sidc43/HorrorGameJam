using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform orientation;

    public int magazineCap = 30;
    public int reserve = 200;
    public int currentAmmoInMag = 30;
    public float reloadSpeed = 2f;

    bool isReloading = false;

    void Start()
    {
        currentAmmoInMag = magazineCap;
    }

    void Update()
    {
        transform.rotation = orientation.rotation;

        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmoInMag < magazineCap)
        {
            Reload();
        }
    }

    public void Shoot()
    {
        if (currentAmmoInMag > 0)
        {
            Debug.Log("SHOOT");
            currentAmmoInMag--;
        }
        else
        {
            Reload();
        }
    }

    public void Reload()
    {
        if (reserve <= 0 || currentAmmoInMag == magazineCap) return;
        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadSpeed);

        int needed = magazineCap - currentAmmoInMag;
        int toLoad = Mathf.Min(needed, reserve);

        currentAmmoInMag += toLoad;
        reserve -= toLoad;

        Debug.Log($"Reload complete. Mag: {currentAmmoInMag}/{magazineCap}, Reserve: {reserve}");
        isReloading = false;
    }
}
