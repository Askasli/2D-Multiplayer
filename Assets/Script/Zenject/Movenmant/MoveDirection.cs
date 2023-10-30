using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirection : IMoveDirection
{
    public Vector3 moveDirection()
    {
        float hMove = Input.GetAxisRaw("Horizontal");
        float vMove = Input.GetAxisRaw("Vertical");
        Vector3 moveDir = new Vector3(hMove, vMove, 0f).normalized;
        return moveDir;
    }
}
