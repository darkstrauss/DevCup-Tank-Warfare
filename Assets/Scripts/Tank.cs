﻿using UnityEngine;
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
    protected bool rotatingBody = false;
    protected bool moving = false;
    protected bool rotatingTurret;
    protected bool reloadingComplete = true;
    protected bool firing = false;
    protected bool aiming = false;
    protected bool shouldMove = false;

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

    [SerializeField]
    /// <summary>
    /// what is this tank going to shoot at?
    /// </summary>
    protected GameObject target;

    protected virtual void Start()
    {
        bodyTriggerRef = bodyRef.GetComponent<TriggerCall>();
        turretTriggerRef = turretRef.GetComponent<TriggerCall>();
        backWardsSpeed = movementSpeed / 2;
    }

    private void FixedUpdate()
    {
        if (aiming)
        {
            if (target != null)
            {
                Aim(target.transform.position);
            }
        }

        if (shouldMove)
        {
            Move(goal);
        }
    }

    /// <summary>
    /// Use this function to move to a vector 3 coordinate
    /// </summary>
    /// <param name="destination">Where you want the tank to move to</param>
    protected virtual void MoveTo(Vector3 destination)
    {
        goal = destination;
        shouldMove = true;
    }

    /// <summary>
    /// Use this to rotate the tank's turret at a certain target
    /// </summary>
    /// <param name="targetLocation">Where the barrel should point</param>
    protected virtual void AimAt(GameObject targetLocation)
    {
        target = targetLocation;
        aiming = true;
    }

    /// <summary>
    /// Moves the tank from its current position to the goal given.
    /// </summary>
    /// <param name="goal">Goal to move to.</param>
    private void Move(Vector3 goal)
    {
        if (Vector3.Distance(transform.position, goal) >= acceptanceDistance)
        {
            float rotationSpeed = 45;

            Quaternion lookAtRotation = Quaternion.LookRotation(goal - transform.position);
            lookAtRotation.x = 0;
            lookAtRotation.z = 0;

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
    protected virtual void Aim(Vector3 lookAtTarget)
    {
        if (turretRef.transform.rotation != Quaternion.LookRotation(lookAtTarget - turretRef.transform.position))
        {
            aiming = true;
            float rotationSpeed = 30;

            Quaternion lookAtRotation = Quaternion.LookRotation(lookAtTarget - turretRef.transform.position);

            turretRef.transform.rotation = Quaternion.RotateTowards(turretRef.transform.rotation, lookAtRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            aiming = false;
        }
    }

    /// <summary>
    /// Spawns bullet and fires it
    /// </summary>
    protected virtual void Fire()
    {
        if (reloadingComplete)
        {
            deviation = Random.Range(-deviation, deviation);
            StartCoroutine(reload());

            Quaternion rotation = new Quaternion(0, turretBulletSpawn.transform.rotation.y + deviation, 0, turretBulletSpawn.transform.rotation.w);
            Instantiate(bullet, turretBulletSpawn.transform.position, rotation);
        }
    }

    protected virtual void Update()
    {

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
        for (int i = 0; i < bodyTriggerRef.GetEnemies().Count; i++)
        {
            if (!enemies.Contains(bodyTriggerRef.GetEnemies()[i]))
            {
                enemies.Add(bodyTriggerRef.GetEnemies()[i]);
            }
        }
        
        for (int i = 0; i < turretTriggerRef.GetEnemies().Count; i++)
        {
            if (!enemies.Contains(turretTriggerRef.GetEnemies()[i]))
            {
                enemies.Add(turretTriggerRef.GetEnemies()[i]);
            }
        }
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                List<GameObject> turretEnemyList = turretTriggerRef.GetEnemies();
                List<GameObject> bodyEnemyList = bodyTriggerRef.GetEnemies();
                if (!turretEnemyList.Contains(enemies[i]) && !bodyEnemyList.Contains(enemies[i]))
                {
                    Debug.Log("enemy no longer in vision distance");
                    enemies.Remove(enemies[i]);
                    enemies.TrimExcess();
                }
            }
        }



        for (int i = 0; i < bodyTriggerRef.GetWorldProps().Count; i++)
        {
            if (!obstacles.Contains(bodyTriggerRef.GetWorldProps()[i]))
            {
                obstacles.Add(bodyTriggerRef.GetWorldProps()[i]);
            }
        }


        for (int i = 0; i < turretTriggerRef.GetWorldProps().Count; i++)
        {
            if (!obstacles.Contains(turretTriggerRef.GetWorldProps()[i]))
            {
                obstacles.Add(turretTriggerRef.GetWorldProps()[i]);
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
        yield return new WaitForSeconds(reloadSpeed);
        reloadingComplete = true;
    }
}
