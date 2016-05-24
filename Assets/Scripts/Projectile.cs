using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    private float travelSpeed = 1.5f;
    private float travelTime = 5f;


	void Start ()
    {
        StartCoroutine(destroyTime());
    }

	void Update ()
    {
        fromTo();
    }

    void fromTo()
    {
        Vector3 moving = transform.forward * travelSpeed;
        transform.Translate(moving);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tank")
        {
            //other.GetComponent<Tank>().ReceiveDamage();
            Debug.Log("Need to do Damage");
        }
    }

    IEnumerator destroyTime()
    {
        yield return new WaitForSeconds(travelTime);
        Destroy(gameObject);
    }
}
