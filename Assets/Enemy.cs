using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public int Damage;
    private GameObject Player;
    private Rigidbody rb;
    public GameObject Dogo;
    public followPlayer myReferenceToDogo;
    Animator MyAnim;
    AnimationClip Animation;
    public BoxCollider AtackCollider;
    public LayerMask maskLayer;
    //private GameObject Loot;

    public bool IsSearch;
    [SerializeField]bool isAtacking = false;
    public float TimeAtack;

    public float calc = 0.8f;
    public float damageRadius = 0.3f;

    public delegate void MyAttack();
    public event MyAttack Attack;

    private enum EnemyState
    {

        wander,
        seach,
        searchDogo
    }

    private EnemyState MyState = EnemyState.wander;

    private void OnEnable()
    {
        //Score.enemyHelp += enemyHelp;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player").gameObject;
        Dogo = GameObject.FindGameObjectWithTag("Dogo").gameObject;
        myReferenceToDogo = Dogo.GetComponent<followPlayer>();
        MyAnim = GetComponent<Animator>();
        myReferenceToDogo.ActInBark += OnBarkDogo;
        myReferenceToDogo.AtionLeaveBarck += OnLeaveBarkDogo;
        //Loot = GenerateLoot();
    }

    void OnBarkDogo() {

        MyState = EnemyState.searchDogo;
    }

    void OnLeaveBarkDogo()
    {

        MyState = EnemyState.seach;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Life = 2;
        Speed = 100;

    }

    private void Update()
    {
        if (IsSearch)
        {

            StartCoroutine(search());
            IsSearch = false;
        }

        if (isAtacking)
        {
            WaitToNextAtack();
        }

        if (!isAtacking)
        {
            TimeAtack = 0;
        }
    }

    void Attacking() {

        if (Time.time >= TimeAtack)
        {
            MyAnim.SetTrigger("Atack");

            Collider[] hited = Physics.OverlapSphere(AtackCollider.transform.position, damageRadius, maskLayer);

            foreach (var i in hited)
            {

                i.gameObject.SendMessage("Damage", 1);
            }

            TimeAtack = Time.time + 1f / calc;
        }
    }

    void WaitToNextAtack() {

        TimeAtack += Time.deltaTime;

        if (TimeAtack >= calc && TimeAtack < calc + 0.1f)
        {
            Debug.Log("ohhh la la senior frances");
            Collider[] hited = Physics.OverlapSphere(AtackCollider.transform.position, damageRadius, maskLayer);

            Attack();
            //foreach (var i in hited)
            //{

            //    i.gameObject.SendMessage("Damage", 1);
            //}

        }

        if (TimeAtack >= 4f)
        {
            isAtacking = false;
            TimeAtack = 0f;
        }


    }

    private void FixedUpdate()
    {
        if (MyState == EnemyState.seach)
        {
            MyAnim.SetBool("Walk", true);
            SearchPlayer(Player);
        }
        else if (MyState == EnemyState.wander)
        {
            Move();
            MyAnim.SetBool("Walk", false);
        }
        else if (MyState == EnemyState.searchDogo)
        {
            MyAnim.SetBool("Walk", true);
            SearchDogo(Dogo);
        }
    }

    void LifeReduction(int force)
    {

        Life -= force;

        if (Life <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {

        //GameObject.Find("Score").SendMessage("ScoreUp");

        //if (Random.Range(0, 5) <= 1)
        //    Instantiate(Loot, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    protected override void Move() { }

    void SearchDogo(GameObject PlayerChar) {

        float dist = Mathf.Abs(Vector3.Distance(transform.position, PlayerChar.transform.position));

        if (dist < 10)
        {
            transform.LookAt(new Vector3(PlayerChar.transform.position.x, transform.position.y, PlayerChar.transform.position.z));

            Vector3 mov_forward = transform.forward * Speed * Time.deltaTime * 1.2f;

            if (dist > 1.3f)
                rb.velocity = new Vector3(mov_forward.x, rb.velocity.y, mov_forward.z);

        }
        else if (dist >= 10)
            MyState = EnemyState.wander;

        
    }

    void SearchPlayer(GameObject PlayerChar)
    {

        float dist = Mathf.Abs (Vector3.Distance(transform.position, PlayerChar.transform.position));

        if (dist <= 1.4f && !isAtacking) {

            MyAnim.SetBool("Atack",true);
            //Attack();
            //AtackCollider.enabled = true;
            isAtacking = true;
        }

        if (dist > 1.4)
        {
            MyAnim.SetBool("Atack", false);
            TimeAtack = 0;
            isAtacking = false;
        }

        if (dist < 10)
        {
            transform.LookAt(new Vector3(PlayerChar.transform.position.x, transform.position.y, PlayerChar.transform.position.z));

            Vector3 mov_forward = transform.forward * Speed * Time.deltaTime;

            if(dist > 1.3f)
            rb.velocity = new Vector3(mov_forward.x, rb.velocity.y, mov_forward.z);

        }
        else if (dist >= 10)
            MyState = EnemyState.wander;

    }

    //funcion creada por si se quieren poner mas estados a este enemigo
    IEnumerator search()
    {


        float dist = Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position));

        if (dist < 5 && MyState != EnemyState.searchDogo)
            MyState = EnemyState.seach;
        else if (dist >= 5 && MyState != EnemyState.searchDogo)
            MyState = EnemyState.wander;

        yield return new WaitForSeconds(0.01f);

        IsSearch = true;
    }

    void enemyHelp()
    {
        Speed += 5;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")  && TimeAtack == 0)
        {
            //MyAnim.SetBool("Atack",true);
            //AtackCollider.enabled = true;
            //isAtacking = true;
            
        }

        if (collision.gameObject.CompareTag("Dogo")) {

            collision.gameObject.SendMessage("Damage", 1);
        }

    }

    GameObject GenerateLoot()
    {


        int lootClue = Random.Range(0, 4);

        if (lootClue >= 0 && lootClue <= 2)
        {
            return Resources.Load("Ammo") as GameObject;
        }
        else
        {

            return Resources.Load("MedicKit") as GameObject;
        }
    }

    private void OnDestroy()
    {
        myReferenceToDogo.ActInBark -= OnBarkDogo;
        myReferenceToDogo.AtionLeaveBarck -= OnLeaveBarkDogo;
    }
}
