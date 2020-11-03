using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float engineThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip onDeath;
    [SerializeField] AudioClip nextLevel;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem onDeathParticles;
    [SerializeField] ParticleSystem nextLevelParticles;

    Rigidbody rigidBody;
    AudioSource rocketAudio;
    
    
    enum State {Alive, Dying, Transcending};
    State state = State.Alive;
    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
         // RotateY();
        }
        
            RespondToDebugKeys();
        
    }

    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    private void StartSuccessSequence()
    {
        state = State.Transcending;
        nextLevelParticles.Play();
        rocketAudio.Stop();
        rocketAudio.PlayOneShot(nextLevel);
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        onDeathParticles.Play();
        rocketAudio.Stop();
        rocketAudio.PlayOneShot(onDeath);
        Invoke("LoadCurrentLevel", levelLoadDelay);  
    }

    private void LoadCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero;
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        
    }


   /* private void RotateY()
    {
        rigidBody.freezeRotation = true;   //extra unintended rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.right * rotationThisFrame);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(-Vector3.right * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;  //resume physics control of rotation
    }*/

    private void RespondToThrustInput()
    {
        float thrustThisFrame = engineThrust * Time.deltaTime;


        /* rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
         if (rocketAudio.isPlaying == false)        //so this doesnt layer
         {
             rocketAudio.Play();
         }
         */
        //^free thrust

        //regular space thrust
        if (Input.GetKey(KeyCode.Space))  //thrusters
        {
            ApplyThrust(thrustThisFrame);
        }
        else
            {
                rocketAudio.Stop();
                mainEngineParticles.Stop();
            }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame * Time.deltaTime);
        if (rocketAudio.isPlaying == false)        //so this doesnt layer
        {
            rocketAudio.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
