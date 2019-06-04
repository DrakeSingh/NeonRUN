using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public ParticleSystem partical;
    public ParticleSystem burst;

    public SkinnedMeshRenderer joints; 
    public SkinnedMeshRenderer surface;

    private MovementController speed;

    public bool playing;

    void Start()
    {
        speed = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && speed.isWalking)
            playing = true;

        else
            playing = false;



        if (playing)
        {
            if (!partical.isPlaying)
            {
                burst.Play();
                partical.Play();
                joints.enabled = false;
                surface.enabled = false;
                speed.gravity = 0.2f;
                speed.movementSpeed = 20.0f;
                speed.jumpSpeed = 15.0f;
            }
        }
        else
        {
            if (partical.isPlaying)
            {
                burst.Play();
                partical.Stop();
                joints.enabled = true;
                surface.enabled = true;
                speed.movementSpeed = 10.0f;
                speed.gravity = 0.5f;
                speed.jumpSpeed = 8.0f;
            }
        }
    }
}
