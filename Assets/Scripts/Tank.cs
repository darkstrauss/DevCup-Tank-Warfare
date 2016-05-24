using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Tank : MonoBehaviour {

    [SerializeField]
    private GameObject turretRef;
    [SerializeField]
    private GameObject turretBulletSpawn;
    [SerializeField]
    private GameObject bodyRef;
    [SerializeField]
    private GameObject bullet;


    /// <summary>
    /// Deviation factor for shooting
    /// </summary>
    private float deviation;


    // Various bools for calculation deviation
    private bool rotatingBody;
    private bool moving;
    private bool rotatingTurrert;
    private bool reloadingComplete;


    public float movementSpeed = 10;
    public float backWardsSpeed;
    
    /// <summary>
    /// tank body turn rate
    /// </summary>
    private float turnSpeed = 10;

    /// <summary>
    /// tank turret turn rate
    /// </summary>
    private float turnRate = 10;

    /// <summary>
    /// tank sight range
    /// </summary>
    private float range = 5;

    /// <summary>
    /// turret fire frequency
    /// </summary>
    private float fireRate = 2.0f;

    /// <summary>
    /// tank durability
    /// </summary>
    public int health = 3;

    /// <summary>
    /// list of stored enemies
    /// </summary>
    public List<GameObject> enemies = new List<GameObject>();

    /// <summary>
    /// environment
    /// </summary>
    public List<GameObject> obstacles = new List<GameObject>();

    /// <summary>
    /// where is this tank moving?
    /// </summary>
    public Vector3 goal;

    /// <summary>
    /// what is this tank going to shoot at?
    /// </summary>
    public GameObject target;



    private void Start()
    {
        backWardsSpeed = movementSpeed / 2;
    }

    /// <summary>
    /// Moves the tank from its current position to the goal given.
    /// </summary>
    /// <param name="goal">Goal to move to.</param>
    protected virtual void Move(Vector3 goal)
    {

    }

    /// <summary>
    /// Points the turret at the target
    /// </summary>
    /// <param name="target">Target to aim at.</param>
    protected virtual void AimAt(Vector3 target)
    {

    }

    /// <summary>
    /// Spawns bullet and fires it at target
    /// </summary>
    /// <param name="to">Target to hit.</param>
    protected virtual void Fire(Vector3 to)
    {
        deviation = Random.Range(-deviation, deviation);

        Instantiate(bullet, turretBulletSpawn.transform.position, Quaternion.Euler(0, deviation, 0));

        Debug.DrawRay(turretBulletSpawn.transform.position, turretRef.transform.rotation.eulerAngles, Color.blue, 2.0f, false);
        Debug.DrawRay(turretBulletSpawn.transform.position, turretRef.transform.rotation.eulerAngles * deviation, Color.red, 2.0f, false);
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Fire(Vector3.zero);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            if (moving || rotatingBody || rotatingTurrert || !reloadingComplete)
            {
                deviation = Mathf.Lerp(deviation, 5, Time.deltaTime);
            }
            else
            {
                deviation = Mathf.Lerp(deviation, 0, Time.deltaTime);
            }


        }
    }

    public virtual void RecieveDamage()
    {
        health--;
    }

    IEnumerator reload()
    {

        yield return new WaitForSeconds(fireRate);

    }
}
