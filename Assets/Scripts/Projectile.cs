using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    private float travelSpeed = 0.2f;
    private float travelTime = 5f;

    private float fadingProgress = 0f;

    public AudioClip gunSound;

	void Start ()
    {
        StartCoroutine(destroyTime());
        gameObject.GetComponent<AudioSource>().Play();
    }

	void Update ()
    {
        fromTo();
        fadingProgress += 0.01f;
        gameObject.GetComponent<Light>().intensity = floatlerp(0.25f, 0.0f, fadingProgress);
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

    float floatlerp(float start, float finish, float progress)
    {
        return (1 - progress) * start + progress * finish;
    }

}
