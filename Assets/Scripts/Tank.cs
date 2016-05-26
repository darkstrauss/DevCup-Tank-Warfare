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
    private float deviation = 0;
    private float acceptanceDistance = 1.0f;
    private float turretRotationAngleMax = 60.0f;
    private float bodyRotationAngleMax = 10.0f;
    private float movementSpeed = 1;
    private float backWardsSpeed;
    private float reloadSpeed = 2.0f;

    // Various bools for calculation deviation
    private bool rotatingBody;
    private bool moving;
    private bool rotatingTurret;
    private bool reloadingComplete;
    private bool firing;
    private bool aiming;
    private bool shouldMove;

    // Trigger references
    private TriggerCall bodyTriggerRef;
    private TriggerCall turretTriggerRef;

    /// <summary>
    /// How long it takes to aim the turret
    /// </summary>
    private static float AIMTIME = 2.0f;

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
    public Vector3 goal = Vector3.zero;

    /// <summary>
    /// what is this tank going to shoot at?
    /// </summary>
    public GameObject target;

    private void Start()
    {
        bodyTriggerRef = bodyRef.GetComponent<TriggerCall>();
        turretTriggerRef = turretRef.GetComponent<TriggerCall>();
        backWardsSpeed = movementSpeed / 2;
    }

    private void FixedUpdate()
    {
        if (aiming)
        {
            AimAt(target.transform.position);
        }

        if (shouldMove)
        {
            Move(goal);
        }
    }

    /// <summary>
    /// Moves the tank from its current position to the goal given.
    /// </summary>
    /// <param name="goal">Goal to move to.</param>
    protected virtual void Move(Vector3 goal)
    {
        if (Vector3.Distance(transform.position, goal) >= acceptanceDistance)
        {
            float rotationSpeed = (2 / Vector3.Distance(transform.position, goal)) * 360f;

            Quaternion lookAtRotation = Quaternion.LookRotation(goal - transform.position);
            if (Mathf.Abs(Quaternion.Angle(lookAtRotation, transform.rotation)) >= bodyRotationAngleMax)
            {
                rotationSpeed *= 2;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtRotation, rotationSpeed * Time.fixedDeltaTime);

            // Moving the tank.
            transform.Translate(0, 0, movementSpeed * Time.deltaTime);
        }
        else
        {
            shouldMove = false;
        }
    }

    /// <summary>
    /// Points the turret at the target
    /// </summary>
    /// <param name="lookAtTarget">Target to aim at.</param>
    protected virtual void AimAt(Vector3 lookAtTarget)
    {
        if (turretRef.transform.rotation != Quaternion.LookRotation(lookAtTarget - turretRef.transform.position))
        {
            aiming = true;
            float rotationSpeed = (0.5f / Vector3.Distance(turretRef.transform.position, lookAtTarget)) * 360f;

            Quaternion lookAtRotation = Quaternion.LookRotation(lookAtTarget - turretRef.transform.position);
            if (Mathf.Abs(Quaternion.Angle(lookAtRotation, transform.rotation)) >= turretRotationAngleMax)
            {
                rotationSpeed *= 2;
            }
            turretRef.transform.rotation = Quaternion.RotateTowards(turretRef.transform.rotation, lookAtRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            aiming = false;
        }
    }

    /// <summary>
    /// Spawns bullet and fires it at target
    /// </summary>
    /// <param name="to">Target to hit.</param>
    protected virtual void Fire()
    {
        deviation = Random.Range(-deviation, deviation);
        StartCoroutine(reload());
        
        Quaternion rotation = new Quaternion(0, turretBulletSpawn.transform.rotation.y + deviation, 0, turretBulletSpawn.transform.rotation.w);
        Instantiate(bullet, turretBulletSpawn.transform.position, rotation);
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            aiming = true;
            shouldMove = true;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            if (moving || rotatingBody || rotatingTurret || !reloadingComplete || aiming)
            {
                deviation = Mathf.Lerp(deviation, 0.2f, 2 * Time.deltaTime);
            }
            else
            {
                deviation = Mathf.Lerp(deviation, 0, 2 * Time.deltaTime);
            }
        }

        //need to get the enemies from the triggers in the body and turret. Use the getter function in the tirggercall
    }

    public virtual void RecieveDamage()
    {
        health--;
    }

    IEnumerator reload()
    {
        reloadingComplete = false;
        yield return new WaitForSeconds(reloadSpeed);
        reloadingComplete = true;
    }
}
