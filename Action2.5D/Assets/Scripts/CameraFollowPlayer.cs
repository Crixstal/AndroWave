﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject player = null;
    [SerializeField]
    private float smoothTime = 0.1f, cameraShift = 2f;

    [SerializeField] private float horizontalInputSensitivity = 0.5f;

    [HideInInspector]
    public bool Yaxis = true, Xaxis = true;
    [HideInInspector]
    public float Xpos;
    public Vector3 offset = new Vector3(0, 4, -15);

    private Rigidbody body;
    private Vector3 velocity = Vector3.zero;
    private float foregroundZ;
    private float xCameraShift = 0;
    private float epsilon = 0.5f;
    internal bool playerJumping;
    private bool previousJumpingState = false;
    private bool isSmoothing = false;
    private bool teleport = false;
    private bool isTeleporting = false;
    private float accuracy = 0.01f;
    private Coroutine currentCoroutine = null;
    private Vector3 positionBeforeTeleport;

    private IEnumerator Smooth(float target)
    {
        isSmoothing = true;
        float velocityF = 0;
        for (int i = 0; i < 40; i++)
        {
            if (target - transform.position.y > accuracy)
            {
                transform.position = new Vector3(transform.position.x, Mathf.SmoothDamp(transform.position.y, target, ref velocityF, 0.2f), transform.position.z);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                isSmoothing = false;
                yield break;
            }
        }
        isSmoothing = false;
    }

    private IEnumerator SmoothTeleport(Vector3 target)
    {
        float velocityF = 0;
        Vector3 velocityVec3 = Vector3.zero;
        for (int i = 0; i < 40; i++)
        {
            if (target.y - transform.position.y > accuracy || transform.position.y - target.y > accuracy)
            {
                transform.position = new Vector3(transform.position.x, Mathf.SmoothDamp(positionBeforeTeleport.y, target.y, ref velocityF, 0.3f), transform.position.z);
                positionBeforeTeleport.y = transform.position.y;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                teleport = false;
                isTeleporting = false;
                yield break;
            }
        }
        teleport = false;
        isTeleporting = false;
    }

    private void Start()
    {
        foregroundZ = player.GetComponent<Player>().posForeground;
        body = player.GetComponent<Rigidbody>();
        playerJumping = false;
    }

    void LateUpdate()
    {
        Vector3 targetPosition;

        if ((!playerJumping && Yaxis && Xaxis) || (Yaxis && Xaxis &&
            (player.transform.position.y < player.GetComponent<Player>().jumpStartY)))
        {
            targetPosition = player.transform.position + offset;
            transform.position = new Vector3(targetPosition.x, transform.position.y, foregroundZ + offset.z);
            if (previousJumpingState && !playerJumping)
                currentCoroutine = StartCoroutine(Smooth(targetPosition.y));

            else if (teleport)
                StartCoroutine(SmoothTeleport(targetPosition));

            else if (isSmoothing || isTeleporting)
                transform.position = new Vector3(targetPosition.x, transform.position.y, foregroundZ + offset.z);

            else if (!isSmoothing)
                transform.position = new Vector3(targetPosition.x, targetPosition.y, foregroundZ + offset.z);
        }

        else if (!playerJumping && Yaxis && !Xaxis || (Yaxis && !Xaxis &&
            (player.transform.position.y < player.GetComponent<Player>().jumpStartY)))
        {
            targetPosition = player.transform.position + offset;
            if (previousJumpingState && !playerJumping)
                currentCoroutine = StartCoroutine(Smooth(targetPosition.y));

            else if (teleport)
                StartCoroutine(SmoothTeleport(targetPosition));

            else if (!isSmoothing)
                transform.position = new Vector3(Xpos, targetPosition.y, foregroundZ + offset.z);
        }

        else if (Xaxis && (!Yaxis || playerJumping))
        {
            targetPosition = player.transform.position + offset;
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            else if (teleport)
                StartCoroutine(SmoothTeleport(targetPosition));

            isSmoothing = false;
            transform.position = new Vector3(targetPosition.x, transform.position.y, foregroundZ + offset.z);
        }

        else
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            isSmoothing = false;
            transform.position = new Vector3(Xpos, transform.position.y, foregroundZ + offset.z);
        }

        previousJumpingState = playerJumping;

        float horizontalInput = Input.GetAxis("HorizontalInput");

        if (horizontalInput > horizontalInputSensitivity || horizontalInput < horizontalInputSensitivity - epsilon)
        {
            xCameraShift = cameraShift * body.velocity.x;
        }

        targetPosition = transform.position + new Vector3(xCameraShift, 0, 0);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public Vector3 OnTeleport
    {
        get
        {
            return positionBeforeTeleport;
        }
        set
        {
            if (positionBeforeTeleport != value)
            {
                positionBeforeTeleport = value;
                teleport = true;
            }
        }
    }
}