using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UtilsController;

namespace Pistol
{
    public class PistolController : Weapon
    {
        #region PROPERTIES

        #region PRIVATE
        [SerializeField] AudioClip[] weaponSounds;
        [SerializeField] AudioClip pickUpAC;
        AudioSource pickUpAS;
        [SerializeField] GameObject[] invisibleGuns;
        Coroutine AnimationCoroutine;
        GameObject GameManager;

        [SerializeField] readonly float rotationAmount = 45f;
        
        bool reloading;
        #endregion

        #region PUBLIC
        public int AmmoCapacity { get; set; }
        public override int DamagePoints { get; set; }
        public override bool IsEquipped { get; set; }
        #endregion

        #endregion

        public override void Start()
        {
            //TODO Convert this into a method
            GameManager = GameObject.FindGameObjectWithTag("GameController");
            pickUpAS = GetComponent<AudioSource>();
            var lanesObj = GameManager.GetComponent<GameManagerController>().Lanes;
            for (int i = 0; i < lanesObj.Length; i++)
            {
                invisibleGuns[i] = lanesObj[i];
            }
            AmmoCapacity = 15;
            holdType = UtilsController.HoldType.SINGLE_HANDED;
            IsEquipped = false;
            reloading = false;
            DamagePoints = 10;
        }

        public void InitInvisibleGuns()
        {
            var lanesObj = GameManager.GetComponent<GameManagerController>().Lanes;
            for (int i = 0; i < lanesObj.Length; i++)
            {
                invisibleGuns[i] = lanesObj[i];
            }
        }

        public override void Update()
        {
            
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //    if(!reloading)
            //    {
            //        ammoCapacity--;
            //        weaponAS.clip = ammoCapacity > 0 ? weaponSounds[2] : weaponSounds[0];
            //        PlayWeaponSound();
            //    }
            // }
            // if (Input.GetKeyDown(KeyCode.R))
            // {
            //    reloading = true;
            //    ammoCapacity = 15;
            //    weaponAS.clip = weaponSounds[1];
            //    PlayWeaponSound();
            // }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trf"></param>
        /// <param name="side"></param>
        public override void Shoot(Transform trf, int side)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }
            if (holdType == UtilsController.HoldType.SINGLE_HANDED)
            {
                SwitchArm(trf);
            }
            AnimationCoroutine = StartCoroutine(UndoArmRotation(trf, side));
        }

        void SwitchArm(Transform trf)
        {
            transform.parent = trf;
            transform.rotation = trf.rotation;
            transform.localPosition = new Vector3(0, 0, 0.5f);
            transform.localScale = new Vector3(1.04166675f, 1.31578958f, 0.520833254f);
        }

        private IEnumerator DoArmRotation(Transform trf, int side)
        {
            //step1 # check if weapon is equipped
            if (IsEquipped)
            {
                //step2 # check if weapon has ammo
                if(AmmoCapacity > 0)
                {
                    //step2 # check if isn't reloading
                    if (!reloading)
                    {
                        //step3 # do rotation
                        Quaternion startRotation = trf.rotation;
                        Quaternion endRotation = Quaternion.Euler(startRotation.x, startRotation.y + rotationAmount * side, startRotation.z);
                        float time = 0;
                        while (time < 1)
                        {
                            trf.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                            yield return null;
                            time += Time.deltaTime * 10;
                        }

                        //step4 # update ammo and play audio
                        AmmoCapacity--;
                        FireInvisibleGun(side);
                    }
                }
                else
                {
                    //To Trigger Sound
                    GunWithoutAmmo();
                }
            }
        }

        IEnumerator UndoArmRotation(Transform trf, int side)
        {
            yield return DoArmRotation(trf, side);
            Quaternion startRotation = trf.rotation;
            Quaternion endRotation = Quaternion.Euler(0, 0, 0);
            float time = 0;
            while (time < 1)
            {
                trf.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                yield return null;
                time += Time.deltaTime * 3;
            }
        }

        void FireInvisibleGun(int side)
        {
            switch (side)
            {
                case (int)ArmPosition.Left:
                    var gun = invisibleGuns[0].transform.Find("InvisibleGun");
                    gun.GetComponent<InvisibleGunController>().FireGun(DamagePoints, AmmoCapacity, weaponSounds);
                    break;
                case (int)ArmPosition.Center:
                    var gun1 = invisibleGuns[1].transform.Find("InvisibleGun");
                    gun1.GetComponent<InvisibleGunController>().FireGun(DamagePoints, AmmoCapacity, weaponSounds);
                    break;
                case (int)ArmPosition.Right:
                    var gun2 = invisibleGuns[2].transform.Find("InvisibleGun");
                    gun2.GetComponent<InvisibleGunController>().FireGun(DamagePoints, AmmoCapacity, weaponSounds);
                    break;
            }
            Debug.Log("Player pulled the trigger at lane: " + side.ToString());
        }

        void GunWithoutAmmo()
        {
            var gun1 = invisibleGuns[1].transform.Find("InvisibleGun");
            gun1.GetComponent<InvisibleGunController>().NoAmmo(AmmoCapacity, weaponSounds);
        }

        public void ReloadWeapon()
        {
            reloading = true;
            Transform invisibleGun = invisibleGuns[1].transform.Find("InvisibleGun");
            reloading = !invisibleGun.GetComponent<InvisibleGunController>().PlayReloadAudio(weaponSounds[1]);//assume zero central position
            AmmoCapacity = 15;
            Debug.Log("Player gained ammo");
        }

        public override void OnTriggerEnter(Collider other)
        {
            //step1 # add weapon to player inventory
            if (other.tag == "Player" && other.GetComponentInChildren<PlayerController>().EquipWeapon(gameObject))
            {
                //step2 # disable pistol box collider
                gameObject.GetComponent<BoxCollider>().enabled = false;

                //step3 # place weapon into players hand
                switch (holdType)
                {
                    case UtilsController.HoldType.SINGLE_HANDED:
                        Transform rightArm = other.transform.Find("ShootingArm");
                        transform.parent = rightArm;
                        transform.rotation = rightArm.rotation;
                        transform.localPosition = new Vector3(0, 0, 0.5f);
                        break;
                    case UtilsController.HoldType.TWO_HANDED:
                        //TODO
                        break;
                }
                IsEquipped = true;
                reloading = true;
                Transform invisibleGun = invisibleGuns[1].transform.Find("InvisibleGun");
                reloading = !invisibleGun.GetComponent<InvisibleGunController>().PlayReloadAudio(weaponSounds[1]);//assume zero central position
                GameManager.GetComponent<GameManagerController>().SetEnemiesCounter(0);
            }
        }

        private void OnTriggerExit(Collider other)
        {

            if (other.gameObject.name.StartsWith("Lane"))
            {
                GameManager.GetComponent<GameManagerController>().SetEnemiesCounter(0);
                Destroy(gameObject);
            }
        }

        public void PlayPickUpAmmoAudio()
        {
            pickUpAS.PlayOneShot(pickUpAC);
            Invoke("ReloadWeapon", pickUpAC.length);
        }

        public override void UpdateWeaponSound()
        {
        }

        public override void PlayWeaponSound()
        {
            throw new NotImplementedException();
        }
    }
}