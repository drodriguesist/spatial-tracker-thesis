using Pistol;
using Rail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UtilsController;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region properties

        #region private
        GameObject GameManager;
        bool isDead;
        bool isWalking;
        bool isWalkingKeyboard;
        bool isWeaponEquiped;

        bool healthStateChanged;
        int pHealth;
        int cantUseCounter;
        HealthAudioState healthState;
        #endregion

        #region public
        public List<GameObject> weaponInventory;
        public static PlayerController instance;
        #endregion

        #endregion

        #region unity methods

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager = GameObject.FindGameObjectWithTag("GameController");
            isDead = false;
            weaponInventory = new List<GameObject>();
            isWalking = true;
            isWalkingKeyboard = false;
            isWeaponEquiped = false;
            healthStateChanged = false;
            healthState = HealthAudioState.None;
            cantUseCounter = 0;
        }
        #endregion

        #region public methods
        public void InitPlayer()
        {
        }
        #endregion

        #region getters and setters

        #region getters
        /// <summary>
        /// gets isWalking value
        /// </summary>
        /// <returns></returns>
        public bool GetIsWalking()
        {
            return isWalking;
        }

        /// <summary>
        /// get isWalkingKeyboard
        /// </summary>
        /// <returns></returns>
        public bool GetIsWalkingKeyboard()
        {
            return isWalkingKeyboard;
        }

        /// <summary>
        /// get isWeaponEq  uiped
        /// </summary>
        /// <returns></returns>
        public bool GetIsWeaponEquiped()
        {
            return isWeaponEquiped;
        }

        /// <summary>
        /// indicates if health counters has changed
        /// </summary>
        /// <returns></returns>
        public bool GetHealthStateChanged()
        {
            return healthStateChanged;
        }

        /// <summary>
        /// return current health state damage/recover
        /// </summary>
        /// <returns></returns>
        public HealthAudioState GetHealthState()
        {
            return healthState;
        }

        /// <summary>
        /// return current value of p_health
        /// </summary>
        /// <returns></returns>
        public int GetPlayerHealth()
        {
            return pHealth;
        }
        #endregion

        #region setters
        /// <summary>
        /// Set isWalking bool
        /// </summary>
        /// <param name="value"></param>
        public void SetIsWalking(bool value)
        {
            isWalking = value;
        }

        /// <summary>
        /// set isWallkingKeyboard bool
        /// </summary>
        /// <param name="value"></param>
        public void SetIsWalkingKeyboard(bool value)
        {
            isWalkingKeyboard = value;
        }

        /// <summary>
        /// set health state changed
        /// </summary>
        /// <param name="value"></param>
        public void SetHealthStateChanged(bool value)
        {
            healthStateChanged = value;
        }

        public void SetPlayerHealth(int value)
        {
            pHealth = value;
        }

        public void SetHealthAudioState(HealthAudioState value)
        {
            healthState = value;
        }
        #endregion

        #endregion

        #region INTERACTION
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(int amount)
        {
            Debug.Log("Player was hit, lost " + amount.ToString() + " pts of life");
            //step1 update player life
            pHealth -= amount;
            healthState = HealthAudioState.Damage;
            healthStateChanged = true;
            //PlayPainAudio();
            if(pHealth < 0) isDead = true;
        }

        public void PickObjectPressed()
        {
            if(!GameManagerController.instance.objectPicker.GetComponent<ObjectPickerController>().PickObject())
            {
                var audioTfr = transform.Find("Audio");
                AudioSource cantUseAS = audioTfr.Find("CantUse").GetComponent<AudioSource>();
                cantUseAS.clip = Resources.Load<AudioClip>("Audio/Player/CantUse/cantuse_sfx"); //restoresfx
                cantUseCounter++;
                if(!cantUseAS.isPlaying) cantUseAS.Play();
            }
        }
        #endregion

        #region WEAPON
        public bool EquipWeapon(GameObject weapon)
        {
            try
            {
                //step1 # check if weapon already exists in the inventory
                var match = weaponInventory.FirstOrDefault(wpn => wpn.name == weapon.name);
                if (match is null)
                {
                    //step2 # add weapon to weapon inventory
                    weaponInventory.Add(weapon);

                    //step3 # everything went well end process
                    isWeaponEquiped = true;
                    return true;
                }
                else return false;
            }
            catch
            {
                //something went wrong
                return false;
            }
        }
        
        public void ReloadWeapon()
        {
            if(GetIsWeaponEquiped())
            {
                //assume for now the only weapon
                weaponInventory[0].GetComponent<PistolController>().PlayPickUpAmmoAudio();
            }
        }
        #endregion

        #region SHOOTING
        public void ShootCenterPressed()
        {
            if(weaponInventory.Count()>0)
            {
                PistolController weapon = weaponInventory.FirstOrDefault().GetComponent<PistolController>();
                weapon.Shoot(transform.Find("ShootingArm"), 0);
            }
            else
            {
                Debug.Log("Player tried to shoot right but there was weapon");
                var audioTfr = transform.Find("Audio");
                AudioSource cantUseAS = audioTfr.Find("CantUse").GetComponent<AudioSource>();
                cantUseAS.clip = Resources.Load<AudioClip>("Audio/Player/CantUse/cantuse_sfx"); //restoresfx
                if(!cantUseAS.isPlaying) cantUseAS.Play();
            }
        }

        public void ShootRightPressed()
        {
            if(weaponInventory.Count()>0)
            {
                PistolController weapon = weaponInventory.FirstOrDefault().GetComponent<PistolController>();
                weapon.Shoot(transform.Find("ShootingArm"), 1);
            }
            else
            {
                Debug.Log("Player tried to shoot straight but there was weapon");
                var audioTfr = transform.Find("Audio");
                AudioSource cantUseAS = audioTfr.Find("CantUse").GetComponent<AudioSource>();
                cantUseAS.clip = Resources.Load<AudioClip>("Audio/Player/CantUse/cantuse_sfx"); //restoresfx
                if(!cantUseAS.isPlaying) cantUseAS.Play();
            }
        }

        public void ShootLeftPressed()
        {
            if(weaponInventory.Count()>0)
            {
                PistolController weapon = weaponInventory.FirstOrDefault().GetComponent<PistolController>();
                weapon.Shoot(transform.Find("ShootingArm"), -1);
            }
            else
            {
                Debug.Log("Player tried to shoot left but there was weapon");
                var audioTfr = transform.Find("Audio");
                AudioSource cantUseAS = audioTfr.Find("CantUse").GetComponent<AudioSource>();
                cantUseAS.clip = Resources.Load<AudioClip>("Audio/Player/CantUse/cantuse_sfx"); //restoresfx
                if(!cantUseAS.isPlaying) cantUseAS.Play();
            }
        }
        #endregion
    }
}