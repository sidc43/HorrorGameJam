using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int magazineCap = 30;
    public int reserve = 200;
    public int currentAmmoInMag = 30;
    public float reloadSpeed = 2f;
    public float shootAnimationDuration = 0.1f; 
    public float recoilAngle = 10f;
    public float damage;
    public GameObject muzzleFlash;

    [Header("Reload Animation")]
    public float reloadAnimDuration = 0.2f;
    public float reloadDownY = -0.4f;
    public float reloadUpY = -0.15f;

    bool isReloading = false;
    bool isShooting = false;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    void Start()
    {
        currentAmmoInMag = magazineCap;
        originalRotation = transform.localRotation;
        originalPosition = transform.localPosition;
        originalPosition.y = reloadUpY;
        transform.localPosition = originalPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading && !isShooting)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmoInMag < magazineCap)
        {
            Reload();
        }

        muzzleFlash.SetActive(isShooting);
    }

    public void Shoot()
    {
        if (currentAmmoInMag > 0)
        {
            currentAmmoInMag--;
            Debug.Log("SHOOT");
            StartCoroutine(ShootingRoutine());

            Ray crosshair = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(crosshair, out RaycastHit hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag("enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                        enemy.TakeDamage(damage);
                }
            }
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
        yield return StartCoroutine(AnimateReloadPosition(reloadDownY, reloadAnimDuration));

        float holdTime = reloadSpeed - (reloadAnimDuration * 2f);
        if (holdTime > 0f)
            yield return new WaitForSeconds(holdTime);

        yield return StartCoroutine(AnimateReloadPosition(reloadUpY, reloadAnimDuration));

        int needed = magazineCap - currentAmmoInMag;
        int toLoad = Mathf.Min(needed, reserve);

        currentAmmoInMag += toLoad;
        reserve -= toLoad;

        isReloading = false;
    }

    IEnumerator ShootingRoutine()
    {
        isShooting = true;
        float halfDuration = shootAnimationDuration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            float zAngle = Mathf.Lerp(0f, -recoilAngle, elapsed / halfDuration);
            transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, zAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float zAngle = Mathf.Lerp(-recoilAngle, 0f, elapsed / halfDuration);
            transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, zAngle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = originalRotation;
        isShooting = false;
    }

    IEnumerator AnimateReloadPosition(float targetY, float duration)
    {
        Vector3 start = transform.localPosition;
        Vector3 end = new Vector3(start.x, targetY, start.z);
        float t = 0f;

        while (t < duration)
        {
            transform.localPosition = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = end;
    }
}
