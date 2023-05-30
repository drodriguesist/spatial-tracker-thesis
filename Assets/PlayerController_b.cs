using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerController_b : MonoBehaviour
{
    #region properties

    #region private
    [SerializeField] private AudioClip[] breathingSounds; //0 fast 1 medium 2 slow
    [SerializeField] private AudioClip[] heartbeatSounds; //0 fast 1 medium 2 slow
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioSource breathingAudioSource;
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private TMP_Text pHealthText;
    private GameObject[] oneHandedWeapons;

    private int pHealth;
    #endregion

    #endregion

    #region methods

    #region health_methods

    /// <summary>
    /// Get Players Health
    /// </summary>
    public int GetHealth()
    {
        return pHealth;
    }

    /// <summary>
    /// Set Players Health
    /// </summary>
    /// <param name="h_value"></param>
    public void SetHealth(int h_value)
    {
        pHealth = h_value;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        oneHandedWeapons = new GameObject[2]; // 0 Right 1 Left
        pHealth = 100;
        pHealthText.text = "Health: " + pHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        pHealthText.text = "Health: " + pHealth.ToString();
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            DecreaseHealth();
        }
        else if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            IncreaseHealth();
		}
    }

    private void FixedUpdate()
    {
        UpdatePlayerHealthSounds();
        PlayHealthSounds();
    }

    private void UpdatePlayerHealthSounds()
	{
		if(pHealth > 0 && pHealth <= 25)
        {
            breathingAudioSource.clip = breathingSounds[0];
            heartbeatAudioSource.clip = heartbeatSounds[0];
        }
        else if(pHealth > 25 && pHealth <= 50)
		{
            breathingAudioSource.clip = breathingSounds[1];
            heartbeatAudioSource.clip = heartbeatSounds[1];
        }
        else if (pHealth > 50 && pHealth <= 75)
		{
            breathingAudioSource.clip = breathingSounds[2];
            heartbeatAudioSource.clip = heartbeatSounds[2];
        }
        else if (pHealth > 75 && pHealth <= 100)
        {
            breathingAudioSource.clip = null;//none
        }
    }

    private void PlayHealthSounds()
    {
        if (!breathingAudioSource.isPlaying)
		{
            breathingAudioSource.volume = 0.4f;
            breathingAudioSource.PlayOneShot(breathingAudioSource.clip);
        }
        if (!heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.PlayOneShot(heartbeatAudioSource.clip);
        }
    }

    public void IncreaseHealth()
	{
        if(pHealth <= 100) pHealth += 5;
	}

    public void DecreaseHealth()
	{
        if (pHealth > 0)
        {
            pHealth -= 5;
            breathingAudioSource.clip = damageSound;
            breathingAudioSource.Stop();
            breathingAudioSource.PlayOneShot(breathingAudioSource.clip);
        }
    }

    public (bool, int) AddOneHandedWeapon(GameObject weapon)
    {
        for (int i = 0; i <= oneHandedWeapons.Length; i++)
        {
            //if its empty add new weapon
            if (oneHandedWeapons[i] == null)
            {
                oneHandedWeapons[i] = weapon;
               return (true, i);
            }
        }
        //All slots are full
        return (false, 9);
    }
    #endregion
}
