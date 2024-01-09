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
    public GameObject attackGO;
    public float attackSpeed = 0.1f;
    public float attackCooldown = 1;
    public float lastAttack;
    private Collider2D hitBox;
    //Dashing Variables
    public TrailRenderer trailRenderer;
    private bool isDashing;
    private bool canDash=true;
    public float dashPower;
    public float dashCurrentEnergy=100;
    public float dashMaxEnergy=100;
    public float dashEnergyGain=1;
    public float dashTime;
    private Image dashEnergyUI,dashEnergyLightning;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackGO=transform.GetChild(0).gameObject;
        dashEnergyUI = GameObject.FindGameObjectWithTag("DashEnergyUi").GetComponent<Image>();
        dashEnergyLightning = GameObject.FindGameObjectWithTag("DashEnergyLightning").GetComponent<Image>();
        hitBox = GetComponent<Collider2D>();
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
            Debug.Log(Time.time);
            Console.WriteLine(Time.deltaTime+attackCooldown);
            lastAttack = Time.time+attackCooldown;
        }
        if (isDashing) return;
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
        isFacingLeft = GameObject.FindGameObjectWithTag("Pointer").transform.position.x - transform.position.x < 0;
        anim.SetBool("isFacingLeft", isFacingLeft);
        anim.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, (isFacingLeft) ? 180f : 0f, 0f));
        //Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashCurrentEnergy==dashMaxEnergy) StartCoroutine(Dash());
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
        dashCurrentEnergy -= 100;
        anim.SetBool("isDashing", isDashing);
        hitBox.isTrigger = true;
        Vector3 dashDirection = (GameObject.FindGameObjectWithTag("Pointer").transform.position - transform.position).normalized;
        rb.velocity = dashDirection * dashPower;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);
        hitBox.isTrigger = false;
        trailRenderer.emitting = false;
        isDashing = false;
        canDash = true;
        anim.SetBool("isDashing", isDashing);

    }
    private void CheckDashEnergy()
    {
        dashEnergyUI.fillAmount = dashCurrentEnergy / dashMaxEnergy;
        bool active=(dashCurrentEnergy==dashMaxEnergy) ? true : false;
        dashEnergyLightning.gameObject.SetActive(active);
    }
}
