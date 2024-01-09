using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPoleBehaviour : MonoBehaviour
{
    public float cooldown;
    public bool hasEnergy=true;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag == "PlayerSword" && hasEnergy)
        //{
        //    StartCoroutine(Deactivate());
        //}
    }

    public IEnumerator Deactivate()
    {
        hasEnergy = false;
        anim.SetBool("hasEnergy",false);
        yield return new WaitForSeconds(cooldown);

        hasEnergy = true;
        anim.SetBool("hasEnergy", hasEnergy);
    }
}
