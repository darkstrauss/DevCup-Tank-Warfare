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
    /// <param name="lookAtTarget">Target to aim at.</param>
    protected virtual void AimAt(Vector3 lookAtTarget)
    {
        Debug.Log(GetRotationDifference(turretRef, lookAtTarget));

        StartCoroutine(RotateTurret(Vector3.forward, GetRotationDifference(turretRef, lookAtTarget), 2));
    }

    /// <summary>
    /// Rotates the turret
    /// </summary>
    /// <param name="byAngle">Along what axis</param>
    /// <param name="rotateBy">Degrees of rotation</param>
    /// <param name="t">Over time</param>
    /// <returns></returns>
    private IEnumerator RotateTurret(Vector3 byAngle, float rotateBy, float t)
    {
        Quaternion fromAngle = turretRef.transform.rotation;
        Quaternion toAngle = Quaternion.Euler(turretRef.transform.eulerAngles + new Vector3(0, rotateBy, 0));
        for (float i = 0; i < 1; i += Time.deltaTime/t)
        {
            turretRef.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, i);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Gets the rotational difference between two points
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>returns a value in degrees</returns>
    private float GetRotationDifference(GameObject from, Vector3 to)
    {
        float oldRotation = from.transform.rotation.eulerAngles.y;
        from.transform.LookAt(to);
        float newRotation = from.transform.rotation.eulerAngles.y;
        from.transform.Rotate(0, -newRotation, 0);

        float rotation = newRotation - oldRotation;

        return rotation;
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

        Instantiate(bullet, turretBulletSpawn.transform.position, Quaternion.Euler(0, deviation, 0));
        AimAt(target.transform.position);
        Debug.DrawRay(turretBulletSpawn.transform.position, turretBulletSpawn.transform.forward * 1000, Color.red, 2.0f, true);
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
            if (moving || rotatingBody || rotatingTurrert || !reloadingComplete)
            {
                deviation = Mathf.Lerp(deviation, 5, 2 * Time.deltaTime);
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
