using System.Collections;
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
    private float accuracy = 0.01f;
    Coroutine currentCoroutine = null;

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
            if (previousJumpingState)
                currentCoroutine = StartCoroutine(Smooth(targetPosition.y));

            else if (isSmoothing)
                transform.position = new Vector3(targetPosition.x, transform.position.y, foregroundZ + offset.z);

            else if (!isSmoothing) //&& (targetPosition.y - transform.position.y > accuracy || transform.position.y - targetPosition.y > accuracy))
                transform.position = new Vector3(targetPosition.x, targetPosition.y, foregroundZ + offset.z);
        }

        else if (!playerJumping && Yaxis && !Xaxis)
        {
            targetPosition = player.transform.position + offset;
            if (previousJumpingState)
                currentCoroutine = StartCoroutine(Smooth(targetPosition.y));

            else if (!isSmoothing) //&& (targetPosition.y - transform.position.y > accuracy || transform.position.y - targetPosition.y > accuracy))
                transform.position = new Vector3(Xpos, targetPosition.y, foregroundZ + offset.z);
        }

        else if (Xaxis && (!Yaxis || playerJumping))
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            isSmoothing = false;
            targetPosition = player.transform.position + offset;
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
}