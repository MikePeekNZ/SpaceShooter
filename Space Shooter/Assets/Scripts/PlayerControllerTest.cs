using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoundaryTest
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerControllerTest : MonoBehaviour

{
    public float speed;
    public float tilt;
    public BoundaryTest boundaryTest;
    private Rigidbody rb;
    private AudioSource audioSource;
    private float speedEdge;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundaryTest.xMin, boundaryTest.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundaryTest.zMin, boundaryTest.zMax)
        );

        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);

        DecelerateWhenHittingXBorders();
        DecelerateWhenHittingZBorders();
    }

    void DecelerateWhenHittingXBorders()
    {
        if (rb.position.x >= boundaryTest.xMax || rb.position.x <= boundaryTest.xMin)
        {
            //reduce velocity indirectly by force
            //rb.AddForce(-speed * rb.velocity.x, 0, 0);

            //reduce velocity directly
            rb.velocity -= new Vector3(rb.velocity.x / 3f, 0, 0);
        }
    }

    void DecelerateWhenHittingZBorders()
    {
        if (rb.position.z >= boundaryTest.zMax || rb.position.z <= boundaryTest.zMin)
        {
            //reduce velocity indirectly by force
            //rb.AddForce(-speed * rb.velocity.x, 0, 0);

            //reduce velocity directly
            rb.velocity -= new Vector3(0, 0, rb.velocity.z / 3f);
        }
    }
}
