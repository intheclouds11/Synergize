using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    public Transform playerTransform;
    public Animator animator;
    public Rigidbody slimeRB;
    public float jumpForceMultiplier;

    public enum SlimeState
    {
        NONE, // 0
        IDLE, // 1
        SQUAT, //2
        JUMP, // 3
        LAND, // 4
    };

    private SlimeState slimeState = SlimeState.NONE;
    private bool playerSeen;
    private static string stateParameterName = "state";
    private int stateParameterNameHash = Animator.StringToHash(stateParameterName);
    private int stateParameterValue;
    private static string normalTimeParameterName = "normalTime";
    private int normalTimeParameterNameHash = Animator.StringToHash(normalTimeParameterName);
    private float normalTimeParameterValue;
    private float timer;
    private float timerLength;
    private bool timerComplete;
    private readonly int jumpTimeLength = 2;
    private readonly int squatTimeLength = 1;
    private readonly int landTimeLength = 1;
    private float startTime;
    private bool isHit;

    void Awake()
    {
        if (playerTransform == null)
        {
            Debug.LogError($"The playerTransform shouldn't be null.");
            return;
        }

        animator = GetComponent<Animator>();
        slimeRB = GetComponent<Rigidbody>();
        if (animator == null)
        {
            Debug.LogError($"The animator shouldn't be null.");
            return;
        }

        var hasStateParameter = false;
        var hasNormalParameter = false;
        for (int i = 0; i < animator.parameterCount; i++)
        {
            if (animator.parameters[i].nameHash == stateParameterNameHash)
            {
                hasStateParameter = true;
            }

            if (animator.parameters[i].nameHash == normalTimeParameterNameHash)
            {
                hasNormalParameter = true;
            }
        }

        if (!hasStateParameter)
        {
            Debug.LogError($"The animator should have an int parameter called {stateParameterName}");
            return;
        }

        if (!hasNormalParameter)
        {
            Debug.LogError($"The animator should have an int parameter called {normalTimeParameterName}");
            return;
        }

        SetState(SlimeState.IDLE);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSeen = true; // Always Seen for now
    }

    // Update is called once per frame
    void Update()
    {
        // Player Checking
        IterateTimer();
        if (!playerSeen) return;
        // Jump Towards Player
        switch (slimeState)
        {
            case SlimeState.IDLE:
                SetState(SlimeState.SQUAT);
                break;
            case SlimeState.SQUAT:
                // code
                if (timerComplete)
                {
                    SetState(SlimeState.JUMP);
                    slimeRB.AddForce(jumpForceMultiplier * (playerTransform.position - transform.position), ForceMode.Impulse);
                }
                break;
            case SlimeState.JUMP:
                if (timerComplete)
                {
                    SetState(SlimeState.LAND);
                }
                break;
            case SlimeState.LAND:
                if (timerComplete)
                {
                    SetState(SlimeState.SQUAT);
                }
                break;
        }
    }

    private void IterateTimer()
    {
        if (timerComplete || timerLength == 0)
        {
            return;
        }

        timer = Time.time - startTime;
        if (timer >= timerLength)
        {
            timerComplete = true;
        }

        normalTimeParameterValue = timer / timerLength;
        animator.SetFloat(normalTimeParameterName, normalTimeParameterValue);
    }

    private void SetState(SlimeState state)
    {
        Debug.Log($"Slime is now {state}");
        slimeState = state;
        stateParameterValue = (int)slimeState;
        animator.SetInteger(stateParameterName, stateParameterValue);
        // Timer
        switch (slimeState)
        {
            case SlimeState.SQUAT:
                timerLength = squatTimeLength;
                break;
            case SlimeState.JUMP:
                timerLength = jumpTimeLength;
                break;
            case SlimeState.LAND:
                timerLength = landTimeLength;
                break;
        }

        startTime = Time.time;
        timerComplete = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        throw new NotImplementedException();
    }
}