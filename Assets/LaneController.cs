using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //// The step size is equal to speed times frame time.
        //float step = speed * Time.deltaTime;

        //// Move our position a step closer to the target.
        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        if(other.tag != "Player")
        {
            Vector3 v = transform.forward * Time.deltaTime * 30f;
            other.transform.Translate(v);
        }
    }
}
