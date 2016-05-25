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
    private bool firing = true;

    // Various bools for calculation deviation
    private bool rotatingBody;
    private bool moving;
    private bool rotatingTurret;
    private bool reloadingComplete;

    public float movementSpeed = 10;
    public float backWardsSpeed;

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
    /// <param name="lookAtTarget">Target to aim at.</param>
    protected virtual void AimAt(Vector3 lookAtTarget)
    {
        StartCoroutine(RotateTurret(target.transform.position));
    }

    /// <summary>
    /// Rotates the turret
    /// </summary>
    /// <param name="rotateBy">Degrees of rotation</param>
    /// <param name="t">Over time</param>
    /// <returns></returns>
    private IEnumerator RotateTurret(Vector3 target)
    {
        rotatingTurret = true;
        Quaternion from = turretRef.transform.rotation;
        Quaternion lookRotation = Quaternion.LookRotation(turretRef.transform.position - target, Vector3.up);
        lookRotation.x = 0.0f;
        lookRotation.z = 0.0f;
        for (float i = 0; i < 1; i += Time.deltaTime/AIMTIME)
        {
            turretRef.transform.rotation = Quaternion.Lerp(from, lookRotation, i);
            yield return new WaitForEndOfFrame();
        }
        rotatingTurret = false;
    }

    /// <summary>
    /// Spawns bullet and fires it at target
    /// </summary>
    /// <param name="to">Target to hit.</param>
    protected virtual void Fire(Vector3 to)
    {
        if (!firing)
        {
            return;
        }
        firing = false;
        deviation = Random.Range(-deviation, deviation);
        StartCoroutine(reload());

        Quaternion rotation = new Quaternion(0, turretBulletSpawn.transform.rotation.y + deviation, 0, turretBulletSpawn.transform.rotation.w);
        Instantiate(bullet, turretBulletSpawn.transform.position, rotation);
        AimAt(target.transform.position);
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Fire(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 100));
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            if (moving || rotatingBody || rotatingTurret || !reloadingComplete)
            {
                deviation = Mathf.Lerp(deviation, 0.3f, 2 * Time.deltaTime);
            }
            else
            {
                deviation = Mathf.Lerp(deviation, 0, 2 * Time.deltaTime);
            }


        }
    }

    public virtual void RecieveDamage()
    {
        health--;
    }

    IEnumerator reload()
    {
        reloadingComplete = false;
        yield return new WaitForSeconds(fireRate);
        firing = true;
        yield return new WaitForSeconds(1f);
        reloadingComplete = true;
          
    }
}
