using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.15f;

    [SerializeField] private float nextTimeToFire = 0f;
    private GameObject bulletsHolder;

    private void Start()
    {
        CreateBulletsHolder();
    }

    private void CreateBulletsHolder()
    {
        if (bulletsHolder == null)
        {
            bulletsHolder = new GameObject("BulletsHolder");
        }
    }

    public void Shoot(Transform cameraTransform)
    {
        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            RaycastHit hit;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity, bulletsHolder.transform);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * 100;
                bulletController.hit = true;
            }
        }
    }
}