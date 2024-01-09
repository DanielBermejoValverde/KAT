using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{
    private KAT_Movement kat;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        kat= GetComponentInParent<KAT_Movement>();


    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition =(PointerScript.CursorToWorld(Input.mousePosition) - kat.transform.position).normalized*range;
        transform.right = PointerScript.CursorToWorld(Input.mousePosition) - transform.position;
    }
    public void StopAttack()
    {
        transform.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("EnergyPole") && collision.gameObject.GetComponent<EnergyPoleBehaviour>().hasEnergy)
        {

            kat.dashCurrentEnergy = 100;
            EnergyPoleBehaviour energyPole=collision.gameObject.GetComponent<EnergyPoleBehaviour>();
            energyPole.StartCoroutine(energyPole.Deactivate());

        }
    }

}
