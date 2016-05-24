using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Tank : MonoBehaviour {

    public float movementSpeed = 10;
    public float backWardsSpeed;
    
    /// <summary>
    /// tank body turn rate
    /// </summary>
    public float turnSpeed = 10;

    /// <summary>
    /// tank turret turn rate
    /// </summary>
    public float turnRate = 10;

    /// <summary>
    /// tank sight range
    /// </summary>
    public float range = 5;

    /// <summary>
    /// turret fire frequency
    /// </summary>
    public float fireRate = 2.0f;

    /// <summary>
    /// tank durability
    /// </summary>
    public int health = 3;

    /// <summary>
    /// list of stored enemies
    /// </summary>
    public List<GameObject> enemies;

    /// <summary>
    /// environment
    /// </summary>
    public List<GameObject> obstacles;

    /// <summary>
    /// where is this tank moving?
    /// </summary>
    public Vector3 goal;

    /// <summary>
    /// what is this tank going to shoot at?
    /// </summary>
    public GameObject target;

    /// <summary>
    /// Moves the tank from its current position to the goal given.
    /// </summary>
    /// <param name="goal">Goal to move to.</param>
    public void Move(Vector3 goal)
    {

    }

    /// <summary>
    /// Points the turret at the target
    /// </summary>
    /// <param name="target">Target to aim at.</param>
    public void AimAt(Vector3 target)
    {

    }

    /// <summary>
    /// Spawns bullet and fires it at target
    /// </summary>
    /// <param name="to">Target to hit.</param>
    public void Fire(Vector3 to)
    {

    }

    private void Update()
    {

    }
}
