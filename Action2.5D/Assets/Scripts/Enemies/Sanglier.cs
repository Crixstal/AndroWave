using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sanglier : MonoBehaviour
{
    [SerializeField] private DropHeart getHeart = null;
    [SerializeField] private Player player = null;
    [SerializeField] private GameObject grenade = null;
    [SerializeField] private float life = 0f;
    [SerializeField] private int score = 0;
    public float laserDamage = 0f;
    [SerializeField] private float laserDelay = 0f;
    [SerializeField] private float grenadeDamage = 0f;
    [SerializeField] private float grenadeDestructionDelay = 0f;
    [SerializeField] private float grenadeBlastDelay = 0f;
    [SerializeField] private float delayPerShot = 0f;
    [SerializeField] private float delayBeforeShoot = 0f;
    [SerializeField] private bool dropGrenade = false;
    [SerializeField] private AudioSource damageSound = null;
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] private Color blinkingColor = new Color(255, 255, 255);

    private GameObject currentGrenade = null;

    private Material material;
    private Color baseColor;

    private LineRenderer aimLine;
    private LineRenderer shootLine;
    private Ray laserRay;
    private RaycastHit laserHit;
    private float shotTimer = 0f;
    private bool firstAim = true;

    private Vector3 relativePos = Vector3.zero;
    private Vector3 enemyPos = Vector3.zero;

    private bool barrelHit = false;
    private Vector3 bulletSpawn = Vector3.zero;

    readonly private int layerMask = ~(1 << 10); // ignore layer 10 = Platform
    private Camera cam;


    public void Start()
    {
        grenade.GetComponent<GrenadeEnemy>().damage = grenadeDamage;
        grenade.GetComponent<GrenadeEnemy>().destructionDelay = grenadeDestructionDelay;
        grenade.GetComponent<GrenadeEnemy>().blastDelay = grenadeBlastDelay;

        aimLine = GetComponent<LineRenderer>();
        shootLine = transform.GetChild(0).GetComponent<LineRenderer>();

        material = meshRenderer.materials[0];
        baseColor = material.GetColor("_BaseColor");

        cam = Camera.main;
    }

    public void FixedUpdate()
    {
        if (material.GetColor("_BaseColor") != baseColor)
            material.SetColor("_BaseColor", baseColor);

        enemyPos = transform.position;

        bulletSpawn = transform.GetChild(0).GetChild(0).position;
        relativePos = player.transform.position - bulletSpawn;

        laserRay = new Ray(bulletSpawn, relativePos.normalized);

        if (life <= 0)
        {
            cam.GetComponent<ScreenShake>().StartShake();

            if (dropGrenade)
                currentGrenade = Instantiate(grenade, enemyPos, Quaternion.identity);

            if (getHeart != null)
                getHeart.ItemDrop();

            player.GetComponent<Player>().playerScore += score;

            Destroy(gameObject);
        }
    }

    public void Aim()
    {
        aimLine.enabled = true;
        shootLine.enabled = false;

        if (Physics.Raycast(laserRay, out laserHit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            aimLine.SetPosition(0, laserRay.origin);
            aimLine.SetPosition(1, laserHit.point);
        }

        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        if (Time.time > shotTimer)
        {
            if (!firstAim)
            {
                shootLine.enabled = true;
                aimLine.enabled = false;

                if (Physics.Raycast(laserRay, out laserHit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
                {
                    shootLine.SetPosition(0, laserRay.origin);
                    shootLine.SetPosition(1, laserHit.point);

                    if (laserHit.transform == player.transform)
                        player.Hurt(laserDamage);
                }

                yield return new WaitForSeconds(laserDelay);
            }

            shotTimer = Time.time + delayPerShot;
        }
        
        firstAim = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Barrel") && other.GetType() == typeof(BoxCollider) && !barrelHit)
        {
            life -= other.gameObject.GetComponent<Barrel>().damage;
            damageSound.Play();
            material.SetColor("_BaseColor", blinkingColor);
            barrelHit = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 = Player
        {
            delayBeforeShoot -= Time.deltaTime;
            
            if (delayBeforeShoot <= 0f)
                Aim();
        }
    }

    void OnTriggerExit(Collider other)
    {
        shootLine.enabled = false;
        aimLine.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9) // 9 = BulletPlayer
        {
            life -= collision.gameObject.GetComponent<BulletPlayer>().damage;
            damageSound.Play();
            material.SetColor("_BaseColor", blinkingColor);
        }
    }
}
