using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// DO NOT EDIT THIS CLASS!
/// </summary>
public class TriggerCall : MonoBehaviour {

    public bool triggered = false;

    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField]
    private List<GameObject> worldProps = new List<GameObject>();

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    public List<GameObject> GetWorldProps()
    {
        return worldProps;
    }

    private void FixedUpdate()
    {
        if (worldProps.Count <= 0 && enemies.Count <= 0)
        {
            triggered = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Tank")
        {
            triggered = true;
            if (!enemies.Contains(other.gameObject))
            {
                enemies.Add(other.gameObject);
            }
        }
        else if (other.tag == "Prop")
        {
            triggered = true;
            if (!worldProps.Contains(other.gameObject))
            {
                worldProps.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemies.Contains(other.gameObject))
        {
            enemies.Remove(other.gameObject);
        }
        else if (worldProps.Contains(other.gameObject))
        {
            worldProps.Remove(other.gameObject);
        }
    }
}
