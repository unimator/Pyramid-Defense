using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 2.5f;
    public Hexagon NextHexagon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    float step = speed * Time.deltaTime;
	    this.transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(NextHexagon.transform.position.x,
                        transform.position.y,
                        NextHexagon.transform.position.z), step);
	    if (Math.Abs(this.transform.position.x - NextHexagon.transform.position.x) < 0.001 &&
	        Math.Abs(this.transform.position.z - NextHexagon.transform.position.z) < 0.001)
	    {
	        NextHexagon = NextHexagon.NextHexagon;
	    }
        if(NextHexagon == null)
            Destroy(this.gameObject);
	}
}
