using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool open = false;
    [SerializeField] private float smooth = 1.0f;
    private float DoorOpenAngle = -90.0f;
    private float DoorCloseAngle = 0.0f;
    [SerializeField] private Transform Player;

    private void Update()
    {
        if (Player == null) return;

        float distance = Vector3.Distance(transform.position, Player.position);

        if (distance <= 5f && !open)
        {
            OpenDoor();
        }
        else if (distance > 5f && open)
        {
            CloseDoor();
        }

        Quaternion targetRotation = open ? Quaternion.Euler(0, DoorOpenAngle, 0) : Quaternion.Euler(0, DoorCloseAngle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5 * smooth);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Victory();
        }
    }
    private void OpenDoor()
    {
        open = true;
        AudioManager.Instance.PlayOneShot(SoundType.DoorOpen);
    }

    private void CloseDoor()
    {
        open = false;
        AudioManager.Instance.PlayOneShot(SoundType.DoorClose);
    }
}
