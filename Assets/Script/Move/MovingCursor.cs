using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is created for player to move.
 * It will be replaced simple function.
 */

public class MovingCursor : MonoBehaviour
{
    
    private bool    isKeyDown = false;
    private float   keyDownTime = 0f;
    private float   waitTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int x = 0, y = 0;

        float tmpX = Input.GetAxisRaw("Horizontal");
        if (tmpX > 0.1)
            x = 1;
        else if (tmpX < -0.1)
            x = -1;

        float tmpY = Input.GetAxisRaw("Vertical");
        if (tmpY > 0.1)
            y = 1;
        else if (tmpY < -0.1)
            y = -1;

        if (x != 0 || y != 0)
        {
            if (isKeyDown && (Time.time - keyDownTime < waitTime))
            {
                x = 0;
                y = 0;
            }
            if (!isKeyDown)
            {
                keyDownTime = Time.time;
                isKeyDown = true;
            }
        }
        else
        {
            isKeyDown = false;
        }


        Vector3 direction = new Vector3(x, y, 0).normalized;

        this.transform.position += direction;
    }
}
