using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsController;

public abstract class Weapon : MonoBehaviour
{
    protected int damagePoints;
    protected HoldType holdType;
    public abstract int DamagePoints { get; set; }
    public abstract bool IsEquipped { get; set; }

    // Start is called before the first frame update
    public abstract void Start();

    // Update is called once per frame
    public abstract void Update();


    public abstract void OnTriggerEnter(Collider other);

    public abstract void PlayWeaponSound();

    public abstract void UpdateWeaponSound();

    public abstract void Shoot(Transform trf, int side);
}
