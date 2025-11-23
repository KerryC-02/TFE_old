using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSound : MonoBehaviour
{
    AudioSource audio;
    private void Start()
    {
        audio = GetComponent<AudioSource>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ethan Player")
        {
            audio.Play();
            print("Åö×²Ìå²¥·ÅÉùÒô");
        }
    }
}