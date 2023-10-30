using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDash
{
   void FxEnable(GameObject[] transforms);
   void DashController(Rigidbody2D rigi, Animator animator, Collider2D collider2D);
}
