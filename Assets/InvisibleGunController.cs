using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleGunController : MonoBehaviour
{
    #region PROPERTIES

    #region PRIVATE

    AudioSource weaponAS;

    bool needsToReset;
    float range;
    #endregion

    #region PUBLIC
    #endregion

    #endregion

    #region UNITY METHODS
    // Start is called before the first frame update
    void Start()
    {
        range = 85f;
        weaponAS = gameObject.GetComponent<AudioSource>();
        weaponAS.volume = 0.75f;
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponAS.isPlaying)
        {
            needsToReset = true;
        }
        else
        {
            if (needsToReset)
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
                needsToReset = false;
            }
        }
    }
    #endregion

    #region SHOOT METHODS
    /// <summary>
    /// This method casts a ray along the forward vector of current transform, 
    /// if there is a hit and its an enemy it takes the Damage Points amount.
    /// </summary>
    /// <param name="DamagePoints">Amount of damage points that is taken from enemy.</param>
    public void FireGun(float damagePoints, int ammoCapacity, AudioClip[] weaponSounds)
    {
        PlayShootAudio(ammoCapacity, weaponSounds);
        Debug.DrawRay(transform.position, transform.forward * range, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
        {
            var target = hit.transform.GetComponent<SuicideBomberController>();
            if (null != target)
            {
                Debug.Log("Suicide Bomber was hit by shot");
                target.TakeDamage(damagePoints);
            }
            else
            {
                var target2 = hit.transform.GetComponent<DummyRobotCpntroller>();
                if (null != target2)
                {
                    Debug.Log("Dummy Robot was hit by shot");
                    target2.TakeDamage(damagePoints);
                    ammoCapacity = 15;
                }
            }
        }
    }

    public void NoAmmo(int ammoCapacity, AudioClip[] weaponSounds)
    {
        Debug.Log("Player pulled the trigger but no ammo");
        PlayShootAudio(ammoCapacity, weaponSounds);
    }
    #endregion

    #region SFX METHODS
    private IEnumerator WaitForSound()
    {
        yield return new WaitUntil(() => !weaponAS.isPlaying);
    }


    public void PlayWeaponSound(AudioClip clip)
    {
        weaponAS.Stop();
        weaponAS.PlayOneShot(clip);
        if (weaponAS.clip.name == "glock_reload") StartCoroutine(WaitForSound());
    }

    public void PlayShootAudio(int ammoCapacity, AudioClip[] weaponSounds)
    {
        weaponAS.clip = ammoCapacity > 0 ? weaponSounds[2] : weaponSounds[0];
        PlayWeaponSound(weaponAS.clip);
    }

    public bool PlayReloadAudio(AudioClip reloadSound)
    {
        weaponAS.clip = reloadSound;
        PlayWeaponSound(weaponAS.clip);
        return true;
    }
    #endregion
}
