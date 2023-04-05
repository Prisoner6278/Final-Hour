using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public float speed;
    public int distanceX;        //how far do you want the NPC to move left and right

    private Vector2 moveDir;

    //negative and positive X values for transform
    private float endPoint1;
    private float endPoint2;

    // Start is called before the first frame update
    void Start()
    {

        moveDir = Vector2.right;        //default start walking right 

        endPoint1 = transform.position.x + distanceX;       //create X value  
        endPoint2 = transform.position.x - distanceX;

        Debug.Log(distanceX);
        Debug.Log(endPoint1);
        Debug.Log(endPoint2);

    }

    // Update is called once per frame
    void Update()
    {

        //check if the NPC has reached the desired X position
        if (transform.position.x == endPoint1)
        {
            moveDir = Vector2.right;
        }

        if(transform.position.x == endPoint2)
        {
            Debug.Log(transform.position);

            moveDir = Vector2.left;
        }

        Move();

    }

   void Move()
    {

        //move!
        transform.Translate(moveDir * Time.deltaTime * speed);

    }

}
