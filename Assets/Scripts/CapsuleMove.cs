using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

public class CapsuleMove : Character
{
    public Animator playerAnimator;

    public float total;
    Rigidbody m_Rigidbody;
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_JumpPower = 1200f;
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;// the world-relative desired move direction, calculated from the camForward and user input.
    [SerializeField] float JumpForce = 2f;
    public bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    private bool Hited = false;
    bool OnAtack = false;
    float timeToAtack = 0f;

    float timeHited = 0;

    NavMeshAgent agent;
    Vector3 ToReach;
    public AudioSource MyAudio;
    [Range(1f, 10f)] [SerializeField] float m_TurnMultiplier = 2f;
    Camera MainCamera;
    GameObject[] enemies;

    Quaternion rotation;

    MenuScript MenuReference;

    bool ActiveTutorialUIOneTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if(GetComponent<AudioSource>() != null)
        MyAudio = GetComponent<AudioSource>();

        MainCamera = Camera.main.GetComponent<Camera>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        MenuReference = GameObject.FindObjectOfType<MenuScript>();

        Subscribe();
    }

    void Subscribe()
    {
        foreach (var i in enemies)
        {
            i.GetComponent<Enemy>().Attack += Damage;

        }
    }

    void UnSubscribe()
    {
        foreach (var i in enemies)
        {
            i.GetComponent<Enemy>().Attack -= Damage;

        }
    }


    private void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        playerAnimator.SetBool("CANMOVE", true);
        playerAnimator.SetFloat("HP", 10.0f);
        playerAnimator.SetBool("WEAPON", false);
        // get the transform of the main camera
        if (Camera.main != null)
        {
            //Debug.Log("MOvimiento");
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
    }

    void JumpByMouse() {

        if (Input.GetMouseButtonDown(1) && agent.destination != ToReach)
        {
            RaycastHit hit;
            //Debug.Log("MouseClick");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                //Debug.Log("MouseClick on something" + hit.normal);
                //if (hit.normal == Vector3.up)
                //{
                //Debug.Log("MouseClick on floor or something");    
                agent.destination = hit.point;
                ToReach = hit.point;
                //}
            }
        }
    }

    private void UpdateTutorial(bool Active)
    {


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tutorial"))
        {
            if (!ActiveTutorialUIOneTime)
            {
                MenuReference.TutorialSwitch(false);
                MenuReference.UpdateTutorialText(string.Empty);
                ActiveTutorialUIOneTime = true;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tutorial"))
        {
            if (ActiveTutorialUIOneTime) {

                MenuReference.TutorialSwitch(true);
                MenuReference.UpdateTutorialText(other.gameObject.GetComponent<Tutorial>().Text);
                ActiveTutorialUIOneTime = false;
            }
        }

    }


    private void Update()
    {

        this.transform.Rotate(new Vector3(0, m_TurnMultiplier * Input.GetAxis("Mouse X"), 0));

        rotation.x += -m_TurnMultiplier * Input.GetAxis("Mouse Y");

        rotation.x = Mathf.Clamp(rotation.x, -20, 30);

        MainCamera.transform.localRotation = Quaternion.Euler(rotation.x, MainCamera.transform.localRotation.y, MainCamera.transform.localRotation.z);

        //if (playerAnimator.GetBool("CANMOVE"))
        //{
        if (!m_Jump && m_IsGrounded && CrossPlatformInputManager.GetButtonDown("Jump"))
            {
            m_Rigidbody.AddForce(new Vector3(0, JumpForce, 0));
            m_Jump = true;
            }
        //}

        if (Input.GetMouseButtonDown(0) && !OnAtack) {

            Atack();

            OnAtack = true;
        }

        if (OnAtack) {

            TimeAtack();
        }

        if (Hited)
        {
            HitedWaitTime();
        }

    }

    void TimeAtack()
    {
        timeToAtack += Time.deltaTime;

        if (timeToAtack >= 1f)
        {
            playerAnimator.SetBool("WEAPON", false);
            OnAtack = false;
            timeToAtack = 0;
        }
    }

    void Atack() {

        playerAnimator.SetBool("WEAPON",true);
    }

    void HitedWaitTime()
    {
        timeHited += Time.deltaTime;

        if (timeHited > 1f) {
            Hited = false;
            timeHited = 0;
        }
        
        
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        //if (playerAnimator.GetBool("CANMOVE"))
        //{
        if (Life > 0)
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            //Debug.Log("CAN MOVE");
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            Move(m_Move, m_Jump, h, v);

            m_Jump = false;
        }
        else if (Life <= 0) {

            StartCoroutine(WaitToDeathAnim());
        }
        //}
    }

    IEnumerator WaitToDeathAnim() {


        yield return new WaitForSeconds(4.1f);

        Application.LoadLevel("SampleScene");
    }


    void Move(Vector3 move, bool jump, float Hh, float Yy)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        //Debug.Log("H: " + Hh + " Y: " + Yy);
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;
        CheckGroundStatus();
        //ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        //if (m_IsGrounded)
        //{
            HandleGroundedMovement(jump, Hh, Yy);
        //}
        //else
        //{
        //    HandleAirborneMovement();
        //}
    }

    void goForward(float H, float Y)
    {
        if (H != 0 || Y != 0)
        {
            //Debug.Log("MOVIMGGGG");
            playerAnimator.SetBool("MOVING", true);
            MyAudio.enabled = true;
            float axis = (Math.Abs(Y) + Math.Abs(H)) / 2;
            int mult = 5;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerAnimator.SetBool("RUN", true);
                mult = 7;
            }
            else
            {
                playerAnimator.SetBool("RUN", false);
            }
            total = axis * mult;

            Vector3 mov_side, mov_forward = Vector3.zero;

            mov_side = transform.right * CrossPlatformInputManager.GetAxis("Horizontal") * mult * Time.deltaTime * 0.8f;
            mov_forward = transform.forward * CrossPlatformInputManager.GetAxis("Vertical") * mult * Time.deltaTime * 0.8f;

            Vector3 totalDistanceTo = mov_side + mov_forward;
            //rb.velocity = new Vector3(total.x,rb.velocity.y,total.z);
            m_Rigidbody.MovePosition(transform.position + totalDistanceTo);
            //m_Rigidbody.MovePosition(new Vector3(transform.position.x + 10 * Time.deltaTime * Input.GetAxis("Horizontal"), transform.position.y, transform.position.z + 10 * Time.deltaTime * Input.GetAxis("Vertical")));
            //m_Rigidbody.velocity = transform.forward * total;
        }
        else
        {
            playerAnimator.SetBool("MOVING", false);
            MyAudio.enabled = false;
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier * 0.8f) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        //m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }


    void HandleGroundedMovement(bool jump, float h, float v)
    {
        // check whether conditions are right to allow a jump:
        //if (jump)
        //{
        //    // jump!
        //    m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
        //    m_IsGrounded = false;
        //    //m_Animator.applyRootMotion = false;
        //}
        //else
        //{
            goForward(h, v);
        //}
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            playerAnimator.SetBool("AIR", false);
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            playerAnimator.SetBool("AIR", true);
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }

    void Damage(int amount)
    {

        if (!Hited)
        {
            Life -= amount;
            Debug.Log("me golpearon");
        }
        //PushHit();
        //SoundManager.CreateSound("Hit");

        

        if (Life <= 0)
        {
            playerAnimator.SetBool("DIE",true);
            playerAnimator.SetBool("MOVING", false);
            MyAudio.enabled = false;
        }

        Hited = true;
    }

    void Damage()
    {

        if (!Hited)
        {
            Life -= 1;
            Debug.Log("me golpearon");
        }
        //PushHit();
        //SoundManager.CreateSound("Hit");



        if (Life <= 0)
        {
            playerAnimator.SetBool("DIE", true);
            playerAnimator.SetBool("MOVING", false);
            MyAudio.enabled = false;
        }

        Hited = true;
    }

    void PushHit()
    {

        m_Rigidbody.AddForce(-transform.forward * 200);
    }
    
}
