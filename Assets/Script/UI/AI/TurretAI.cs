using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretAI : MonoBehaviour
{
   
    public static TurretAI instance;
    public GameObject insideObject;
    private SpriteRenderer spriteRenderer;
    public Color newColor;
    public Vector3 targetPlayerPosition;
    public float fireRate = 2f;
    public GameObject bulletPrefab;
    private float fireCountdown = 0f;
    private bool enableToShoot;

    private Quaternion rot;
   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = (targetPlayerPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        transform.rotation = Quaternion.Euler(0, 0, angle);


        if (enableToShoot)
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, transform.rotation, 0);



        Debug.Log("were shoot from the turret");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == insideObject)
        {
          //  insideObject.transform.position = targetPlayerPosition;

        }


        if (other.CompareTag("Player"))
        {

            targetPlayerPosition = other.transform.position;

            enableToShoot = true;
            spriteRenderer.color = newColor;
            Debug.Log("Player entered the trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enableToShoot = false;
            spriteRenderer.color = Color.white;
            Debug.Log("Player out the trigger");
        }
    }

}
