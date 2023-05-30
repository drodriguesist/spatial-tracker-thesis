using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickerController : MonoBehaviour
{
    public bool PickObject()
    {
        foreach(Transform trf in transform)
        {
            if (Physics.Raycast(trf.position, trf.forward, out RaycastHit hit))
            {
                if(null != hit.transform.GetComponent<HealthkitController>())
                {
                    hit.transform.GetComponent<HealthkitController>().GainLife();
                    return true;
                }
                else if(null != hit.transform.GetComponent<AmmoController>())
                {
                    ObjectSpawnerController.FindObjectOfType<ObjectSpawnerController>().SetAmmoCaughtCounter(1);
                    hit.transform.GetComponent<AmmoController>().ReloadWeapon();
                    return true;
                }
            }
        }
        return false;
    }
}
