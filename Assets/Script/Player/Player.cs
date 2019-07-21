using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is created for player to move.
 * It will be replaced simple function.
 */

public class Player : MonoBehaviour
{
    
    private static bool    isKeyDown = false;
    private static float   keyDownTime = 0f;
    private static float   waitTime = 0.2f;
    private const float cursorOffsetX = -0.5f;
    private const float cursorOffsetY = -0.5f;
    private int x = 0, y = 0;
    private bool keyZ = false, keyX = false;

    //横軸と縦軸の両方のキーが入力された場合、横軸を優先する
    void GetKey()
    {
        if (Input.GetKey(KeyCode.Z))
            keyZ = true;
        else
            keyZ = false;

        if (Input.GetKey(KeyCode.X))
            keyX = true;
        else
            keyX = false;

        float tmpX = Input.GetAxisRaw("Horizontal");

        if (tmpX > 0.1)
            x = 1;
        else if (tmpX < -0.1)
            x = -1;
        else
            x = 0;

        if (x != 0)
        {
            y = 0;
            return;
        }

        float tmpY = Input.GetAxisRaw("Vertical");
        if (tmpY > 0.1)
            y = 1;
        else if (tmpY < -0.1)
            y = -1;
        else
            y = 0;
    }

    private Vector2 NowLocateTile()
    {
        Vector2 vector2 = new Vector2();
        vector2.x = this.transform.position.x + cursorOffsetX + ControlTile.tileOffsetX;
        vector2.y = this.transform.position.y + cursorOffsetY + ControlTile.tileOffsetY;

        return vector2;
    }


    void ActionZ()
    {
        Vector2 vector2 = NowLocateTile();
        int x = (int)vector2.x;
        int y = (int)vector2.y;
        ControlTile.Dig(x, y);
    }

    void MoveCursor()
    {
        Vector2 vector2 = NowLocateTile();
        int nextX = (int)(vector2.x + x);
        int nextY = (int)(vector2.y + y);
        //int nextX = (int)(this.transform.position.x + cursorOffsetX + ControlTile.tileOffsetX + x);
        //int nextY = (int)(this.transform.position.y + cursorOffsetY + ControlTile.tileOffsetY + y);
        if (ControlTile.IsWall(nextX, nextY))
            return;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetKey();
        MoveCursor();
        if (keyZ)
            ActionZ();
    }
}
