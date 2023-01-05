using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 MovementDirection;
    private float elapsedSec;
    public GameObject explosion;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       

        if(rb != null)
        {
            rb.MovePosition(transform.position + MovementDirection * Time.deltaTime * 2f);
        }

    
        
    }

    void OnCollisionEnter(Collision collision)
    {
        print("Detected collision between " + gameObject.name + " and " + collision.collider.name);
        print("There are " + collision.contacts.Length + " point(s) of contacts");
        print("Their relative velocity is " + collision.relativeVelocity);


        GameObject explosionClone = (GameObject)Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);


        Destroy(gameObject);
    }
}
