using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "goal")
        {
            print("You win");
        }

        else if (other.tag == "Player")
        {
            print("player trigger");
        }
        else
        {
            print("other trigger");
        }
    }
}
