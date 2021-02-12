using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class followPlayer : Character
{
    public Transform transformToFollow;
    //NavMesh Agent variable
    NavMeshAgent agent;
    Animator agentAnimator;
    Animator targetAnimator;
    public AudioSource MyBark;
    bool order = false;

    public delegate void Bark();
    public event Bark ActInBark;
    public event Bark AtionLeaveBarck;
    private bool Hited = false;
    Rigidbody m_Rigidbody;
    public LayerMask DogoGoLayer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agentAnimator = GetComponent<Animator>();
        targetAnimator = transformToFollow.gameObject.GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        MyBark = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            //Debug.Log("MouseClick");
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100,DogoGoLayer))
            {
                //Debug.Log("MouseClick on something" + hit.normal);
                //if (hit.normal == Vector3.up)
                //{
                //Debug.Log("MouseClick on floor or something");    

                if (hit.transform.gameObject.CompareTag("Floor"))
                {
                    order = true;
                    agent.destination = hit.point;
                }
                //}
            }
        }

        if (Hited)
        {
            StartCoroutine(HitedWaitTime());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (agent.isStopped)
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(ActInBark != null)
            ActInBark();
            MyBark.enabled = true;

            StartCoroutine(WaitBark());

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            agent.destination = transformToFollow.position;
            if (AtionLeaveBarck != null)
                AtionLeaveBarck();
            order = false;
        }

        agent.stoppingDistance = order ? 0.1f : 3.0f;
        //Follow the player
        if (!order)
        {
            agent.destination = transformToFollow.position;
        }
        float velocity = agent.velocity.magnitude;
        if (velocity > 0)
        {
            agentAnimator.SetBool("MOVE", true);
        }
        else
        {
            agentAnimator.SetBool("MOVE", false);
        }
        if (targetAnimator.GetBool("RUN"))
        {
            agentAnimator.SetBool("RUN", true);
            agent.speed = 6.5f;
        }
        else
        {
            agent.speed = 4.5f;
            agentAnimator.SetBool("RUN", false);
        }
    }

    IEnumerator WaitBark() {


        yield return new WaitForSeconds(3);

        MyBark.enabled = false;
    }

    IEnumerator HitedWaitTime()
    {

        yield return new WaitForSeconds(1);
        Hited = false;
    }

    void Damage(int amount)
    {  
        Life -= amount;
        PushHit();
        //SoundManager.CreateSound("Hit");

        //if (Life <= 0)
        //    PausedData.DeadPause();

        Hited = true;
    }

    void PushHit()
    {
        m_Rigidbody.AddForce(-transform.forward * 200);
    }
}
