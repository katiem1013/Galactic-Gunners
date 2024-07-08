using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private List<Joycon> joycons;
    private int jc_ind = 0;

    public WeaponSwitching equippedWeapon;

    [Header("Shooting")]
    public Camera fpsCam;
    public float damage = 1f;
    public float range = 100f;
    public float fireRate = 10f;
    private float nextTimeToFire = 0f;
    public bool isShooting = false;
    public bool isShootingBlaster = false;

    [Header("Bullet Trail")]
    public TrailRenderer bulletTrail;
    public ParticleSystem shootingSystem;
    public ParticleSystem impactParticle;
    public Transform trailSpawnPoint;


    [Header("Sniper Scope")]
    public Camera playerCamera;
    public GameObject reticle;
    public GameObject sniperScope;

    [Header("Ammo")]
    public int maxAmmo = 10;
    public float currentAmmo;
    public float currentMaxAmmo;
    public Text ammoText;
    public Text maxAmmoText;

    [Header("Reloading")]
    public Animator animator;
    public float reloadTime = 1f;
    private bool isReloading = false;

    PlayerControls controls;

    [Header("Audio")]
    public AudioClip[] gunShotSounds;
    public AudioClip[] reloadingSounds;
    public AudioSource audioSource;
    public float soundVolume;


    // Start is called before the first frame update
    void Start()
    {
        // sets the starting variables
        joycons = JoyconManager.Instance.j;
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    // called when the gameObject is Enabled
    private void OnEnable()
    {
        // enables the gun when the gun is active
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        // checks this gun is active
        if (gameObject.activeInHierarchy == true)
        {
            // updates the ammo text based on which gun is active
            ammoText.text = currentAmmo.ToString();
            maxAmmoText.text = currentMaxAmmo.ToString();
        }

        Joycon j = joycons[jc_ind];
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            // stops the player being able to shoot while reloading
            if (isReloading)
                return;

            // checks if the player is out of current ammo but still has ammo
            if (currentAmmo <= 0 && currentMaxAmmo > 0)
            {
                StartCoroutine(Reload()); // reloads the gun
                return;
            }

            // checks if the zinger is not equipped and if the player is shooting
            if (equippedWeapon.equippedWeapon != "Zinger")
                if (j.GetButtonDown(Joycon.Button.SHOULDER_2) && Time.time >= nextTimeToFire)
                {
                    // shoots the gun
                    isShooting = true;
                    nextTimeToFire = Time.time + 1f / fireRate; // stops the player from shooting too fast
                    Invoke("Shoot", 0.1f);
                }

            // checks if the zinger is quipped and if the player is shooting
            if (equippedWeapon.equippedWeapon == "Zinger")
                if (j.GetButton(Joycon.Button.SHOULDER_2) && Time.time >= nextTimeToFire)
                {
                    // shoots the gun
                    isShooting = true;
                    nextTimeToFire = Time.time + 1f / fireRate; // stops from all bullets firing at once
                    Invoke("Shoot", 0.1f);
                }

            // checks if the vaporizer is equipped and if the player is trying to scope
            if (equippedWeapon.equippedWeapon == "Vaporizer" && j.GetButtonDown(Joycon.Button.DPAD_UP))
            {
                // zooms the camera in and changes the reticle
                playerCamera.fieldOfView = 20;
                sniperScope.SetActive(true);
                reticle.SetActive(false);
            }

            // checks if the vaporizer is equipped and if the player is trying to unscope
            if (equippedWeapon.equippedWeapon == "Vaporizer" && j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                // zooms the camera out and changes the reticle back
                playerCamera.fieldOfView = 60;
                sniperScope.SetActive(false);
                reticle.SetActive(true);
            }

            // checks if the player is shooting the blaster
            if (equippedWeapon.equippedWeapon == "Blaster" && isShooting == true)
                isShootingBlaster = true; // this is so the payload and continue to move while the blaster is being shot

            else
                isShootingBlaster = false; // if the blaster isn't being shot
        }
    }

    IEnumerator Reload()
    {
        // reloads the gun
        isReloading = true;
        PlayReload();
        animator.SetBool("Reloading", true);

        // delays reload time
        yield return new WaitForSeconds(reloadTime - 0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = maxAmmo; // gives the player the max amount of bullets they are allowed
        currentMaxAmmo -= currentAmmo; // takes away the reload from the max ammo
        isReloading = false; // stops the reload
    }

    IEnumerator Shoot()
    {
        if (currentAmmo > 0) // checks if the player has ammo
        {
            Joycon j = joycons[jc_ind];
            currentAmmo -= 1f; // takes away ammo
            PlaySound();

            // checks if the vaporizer is not equipped
            if (equippedWeapon.equippedWeapon != "Vaporizer")
            {
                // sends out a raycast as a bullet, and checks if it hit
                RaycastHit gunHit; 
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out gunHit, range))
                {
                    j.SetRumble(160, 320, 0.6f, 200); // makes the joycon rumble

                    TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, gunHit.point, gunHit.normal, true));

                    // gets the hit targets
                    Target target = gunHit.transform.GetComponent<Target>();
                    Shield shield = gunHit.transform.GetComponent<Shield>();
                    Destroy destroyObj = gunHit.transform.GetComponent<Destroy>();
                    MissionSelect button = gunHit.transform.GetComponent<MissionSelect>();
                    ExplosiveBarrel explosiveBarrel = gunHit.transform.GetComponent<ExplosiveBarrel>();
                    DestroyDoor destroyDoor = gunHit.transform.GetComponent<DestroyDoor>();
                    Boss boss = gunHit.transform.GetComponent<Boss>();

                    // deals damage to boss
                    if (boss != null)
                        boss.TakeDamage(damage);

                    // deals damage to enemies
                    if (target != null)
                        target.TakeDamage(damage);

                    // deals damage to shield and half damage to enemy
                    if (shield != null)
                    {
                        shield.TakeDamage(damage);
                        target.TakeDamage(damage / 2); // this line doesn't work because the shield doesn't have it, need to get the parent?
                    }

                    // deals damage to the destroy objective
                    if (destroyObj != null)
                        destroyObj.TakeDamage(damage);

                    // lets the objective buttons be pressed
                    if (button != null)
                        button.buttonPressed = true;

                    // explodes barrels if the zinger is equipped
                    if (explosiveBarrel != null && equippedWeapon.equippedWeapon == "Zinger")
                        explosiveBarrel.exploded = true;

                    // destroys doors if the disruptor is equipped
                    if (destroyDoor != null && equippedWeapon.equippedWeapon == "Disruptor")
                        destroyDoor.DoorDestroy();
                }

                else
                {
                    j.SetRumble(160, 160, 1f, 200); // makes the joycon rumble
                    TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, trailSpawnPoint.position, Vector3.zero, false));
                }
            }

            // checks if the vaporizer is equipped
            else if (equippedWeapon.equippedWeapon == "Vaporizer")
            {
                // sends out a raycast as a bullet, and checks if it hit
                RaycastHit gunHit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out gunHit, range)) // needs to be able to multi shoot
                {
                    j.SetRumble(160, 320, 0.6f, 200); // makes the joycon rumble

                    TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, gunHit.point, gunHit.normal, true));

                    // gets the hit targets
                    Target target = gunHit.transform.GetComponent<Target>();
                    Shield shield = gunHit.transform.GetComponent<Shield>();
                    Destroy destroyObj = gunHit.transform.GetComponent<Destroy>();
                    MissionSelect button = gunHit.transform.GetComponent<MissionSelect>();
                    Boss boss = gunHit.transform.GetComponent<Boss>();

                    // deals damage to boss
                    if (boss != null)
                        boss.TakeDamage(damage);

                    // deals damage to enemies
                    if (target != null)
                        target.TakeDamage(damage);

                    // deals damage to shield and half damage to enemy
                    if (shield != null)
                    {
                        shield.TakeDamage(damage);
                        target.TakeDamage(damage / 2); // this line doesn't work because the shield doesn't have it, need to get the parent?
                    }

                    // deals damage to the destroy objective
                    if (destroyObj != null)
                        destroyObj.TakeDamage(damage);

                    // lets the objective buttons be pressed
                    if (button != null)
                        button.buttonPressed = true; ;
                }

                else
                {
                    j.SetRumble(160, 160, 1f, 200); // makes the joycon rumble
                    TrailRenderer trail = Instantiate(bulletTrail, trailSpawnPoint.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, trailSpawnPoint.position, Vector3.zero, false));
                }
            }

            // stops the player shooting
            isShooting = false;
            CancelInvoke(); 
        }

        return null;
    }

    void PlaySound()
    {
        // chooses a random sound from the list
        int soundChance = Random.Range(1, gunShotSounds.Length);
        audioSource.clip = gunShotSounds[soundChance];
        audioSource.PlayOneShot(audioSource.clip, soundVolume); // plays the sound once

        // moves the played sound to index 0 to stop it being repeated
        gunShotSounds[soundChance] = gunShotSounds[0];
        gunShotSounds[0] = audioSource.clip;
    }

    void PlayReload()
    {
        // chooses a random sound from the list
        int soundChance = Random.Range(1, reloadingSounds.Length);
        audioSource.clip = reloadingSounds[soundChance];
        audioSource.PlayOneShot(audioSource.clip, soundVolume); // plays the sound once

        // moves the played sound to index 0 to stop it being repeated
        reloadingSounds[soundChance] = reloadingSounds[0];
        reloadingSounds[0] = audioSource.clip;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPos, Vector3 hitNormal, bool hitObject)
    {
        float time = 100;
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPos);
        float remainingDistance = distance;

        // spawns the trail if theres still distance to travel
        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPos, 1 - (remainingDistance / distance));
            remainingDistance -= time * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = hitPos;
        if (hitObject)
            Instantiate(impactParticle, hitPos, Quaternion.LookRotation(hitNormal)); // spawns a particle on impact

        Destroy(trail.gameObject, trail.time); // destroys the trail
    }
}
