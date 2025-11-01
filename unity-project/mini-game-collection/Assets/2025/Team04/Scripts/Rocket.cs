using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    bool player2;
    bool fired = true;
    float rocketForce = 10f;
    Vector2 up = Vector2.left;
    public Transform centerLine;
    Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if (player2)
        {
            up = Vector2.right;
        }
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (fired)
        {
            rigidbody.AddForce(up * rocketForce);
        }

        float distanceFromCenter = centerLine.position.x - transform.position.x;
    }
}

