using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerCall : MonoBehaviour {

    public bool triggered = false;
    public List<GameObject> enemies;

    public List<GameObject> Get()
    {
        return enemies;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("something is staying in trigger area of: " + transform.parent.name);
        if (other.tag == "Tank")
        {
            triggered = true;
            enemies.Add(other.gameObject);
        }
        else
        {
            triggered = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemies.Contains(other.gameObject))
        {
            enemies.Remove(other.gameObject);
        }
    }
}
