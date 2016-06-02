using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    ///<summary>
    /// float defining  the projectile travel speed in units per second, this is not to be changed.
    ///</summary>
    private float travelSpeed = 5f;

    ///<summary>
    /// float defining how long the projectile stays active, this is not to be changed.
    ///</summary>
    private float travelTime = 5f;

    ///<summary>
    /// float that when incremented, fades out the light attached to the projectile (used in floatlerp), this is not to be changed.
    ///</summary>
    private float fadingProgress = 0f;


    ///<summary>
    /// Audioclip that holds the sound to be played when the cannon is fired.
    ///</summary>
    public AudioClip gunSound;

    ///<summary>
    /// When the projectile is first activated it starts the coroutine for the destroy timer. Activates the AudioSource component.
    ///</summary>
    void Start ()
    {
        StartCoroutine(destroyTime());
        gameObject.GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Calls FromTo, for movement. Incremements fadingProgress, dimming the light using floatlerp.
    /// </summary>
    
    void Update ()
    {
        FromTo();
        fadingProgress += 0.01f;
        gameObject.GetComponent<Light>().intensity = floatlerp(0.25f, 0.0f, fadingProgress);
    }

    ///<summary>
    /// FromTo moves the projectile using its forward vector taking time.deltatime and speed into acount.
    ///</summary>
    void FromTo()
    {
        transform.Translate(0, 0, travelSpeed * Time.deltaTime);
    }

    ///<summary>
    /// Collider event calling damage on the other object if it is tagged as "Tank". If the collided Object is a wall or an obstacle, the projectile is destroyed.
    ///</summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tank" && !other.gameObject.Equals(gameObject))
        {
            other.GetComponent<Tank>().RecieveDamage();
            Debug.Log("Need to do Damage");
            Destroy(gameObject);
        }
        else if (other.tag =="Wall" || other.tag == "Prop")
        {
            Destroy(gameObject);
        }
    }

    ///<summary>
    /// IEnumerator that is called when the projectile is activated, if it wont hit anything for some odd reason, it will destroy the projectile after so many seconds defined by travelTime.
    ///</summary>
    IEnumerator destroyTime()
    {
        yield return new WaitForSeconds(travelTime);
        Destroy(gameObject);
    }

   

    /// <summary>
    ///  floatlerp lerps a float from start to finish with the use of progress for the use of fading the light component attached to the projectile.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="finish"></param>
    /// <param name="progress"></param>
    /// <returns></returns>
    float floatlerp(float start, float finish, float progress)
    {
        return (1 - progress) * start + progress * finish;
    }

}
