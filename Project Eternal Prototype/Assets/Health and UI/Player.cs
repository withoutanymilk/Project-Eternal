using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	
	float x, y;

	public float speed;
	
	Rigidbody2D myRigidbody;
	
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
		y = Input.GetAxis("Vertical");
		
		//transform.position += new Vector3(x * speed * Time.deltaTime, y * speed * Time.deltaTime, 0);
		myRigidbody.AddForce(new Vector2(x * speed * Time.deltaTime, y * speed * Time.deltaTime));
	}
}
