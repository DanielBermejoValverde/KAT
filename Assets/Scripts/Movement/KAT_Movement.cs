using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KAT_Movement : MonoBehaviour
{
    //Movement Variables
    private Rigidbody2D rb;
    public float movementSpeed;
    public float x, y;
    public bool isWalking;
    public bool isFacingLeft;
    public Animator anim;
    private Vector3 moveDir;
    //Combat Variables
    private GameObject attackGO;
    public float attackSpeed = 0.1f;
    public float attackCooldown = 1;
    public float lastAttack;
    private BoxCollider2D walkBox;
    public CapsuleCollider2D hitBox;
    public Vector3 dashDirection;
    
    //Dashing Variables
    public TrailRenderer trailRenderer;
    public bool isDashing;
    private bool canDash=true;
    public float dashPower;
    public float dashCurrentEnergy=100;
    public float dashMaxEnergy=100;
    public float dashEnergyGain=1;
    public float dashTime;
    private Image dashEnergyUI,dashEnergyLightning;

    //Midair Variables
    public bool isMidair=false;
    private float timeMidair;
    public float maxTimeMidair;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackGO=transform.GetChild(0).gameObject;
        dashEnergyUI = GameObject.FindGameObjectWithTag("DashEnergyUi").GetComponent<Image>();
        dashEnergyLightning = GameObject.FindGameObjectWithTag("DashEnergyLightning").GetComponent<Image>();
        walkBox = GetComponent<BoxCollider2D>();
        //hitBox = GetComponent<CapsuleCollider2D>();
        lastAttack = Time.deltaTime;
    }
    
    // Update is called once per frame
    void Update()
    {

        CheckDashEnergy();
        //Attack

        if (Input.GetMouseButton(0)&&attackGO.activeInHierarchy==false&&Time.time>lastAttack)
        {
            attackGO.SetActive(true);
            lastAttack = Time.time+attackCooldown;
        }

        if (isDashing) return;

        //Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashCurrentEnergy == dashMaxEnergy) StartCoroutine(Dash());


        //Midair, cant move, has x seconds to dash again
        //isMidair=Physics.Raycast(new Vector3(walkBox.offset.x, walkBox.offset.y, 0.0f),)

        //pseudocodigo: si no detecto terreno detrás mia isMidair=true, luego si midair=true && no estoy dasheando (ya se comprueba antes) compruebo que el tiempo que llevo mid air no supere x,
        //problema: reiniciar de manera correcta y eficiente el tiempo midair, no quiero que este comprobando continualmente
        if (isMidair) {
            if (timeMidair < Time.time)
            {

                moveDir=new Vector3(0.0f,0.0f,0.0f);
                //Fall
                Debug.Log("Fall");
            }
            return; }
        //Movement
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (x != 0 || y != 0)
        {
            anim.SetFloat("x", x);
            if (!isWalking)
            {
                isWalking = true;
                anim.SetBool("isWalking", isWalking);
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                anim.SetBool("isWalking", isWalking);
                StopWalking();
            }
        }
        moveDir = new Vector3(x, y).normalized;
        //Flip
        isFacingLeft = PointerScript.CursorToWorld(Input.mousePosition).x - transform.position.x < 0;
        anim.SetBool("isFacingLeft", isFacingLeft);
        anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, (isFacingLeft) ? 180f : 0f, 0f));
        hitBox.offset = (isFacingLeft) ? new Vector2(-0.1f,-0.05f) : new Vector2(0.1f, -0.05f);
    }


    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.velocity = moveDir * movementSpeed * Time.deltaTime;
        if (dashCurrentEnergy < dashMaxEnergy&&rb.velocity.magnitude>0) dashCurrentEnergy += dashEnergyGain;
    }
    private void StopWalking()
    {
        rb.velocity = Vector3.zero;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        anim.SetBool("isDashing", isDashing);
        dashCurrentEnergy -= 100;
        walkBox.isTrigger = true;
        dashDirection = (PointerScript.CursorToWorld(Input.mousePosition) - transform.position).normalized;
        rb.velocity = dashDirection * dashPower;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        walkBox.isTrigger = false;
        isDashing = false;
        anim.SetBool("isDashing", isDashing);
        canDash = true;
        //Detect is midair, timeMidair=Time.time+maxTimeMidair
        if (isMidair) timeMidair = Time.time + maxTimeMidair;
    }
    private void CheckDashEnergy()
    {
        dashEnergyUI.fillAmount = dashCurrentEnergy / dashMaxEnergy;
        bool active=(dashCurrentEnergy==dashMaxEnergy) ? true : false;
        dashEnergyLightning.gameObject.SetActive(active);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDashing&& collision.gameObject.tag.Equals("Roof"))
        {
            isMidair = !isMidair;
            Debug.Log(isMidair);
        }
    }
}
