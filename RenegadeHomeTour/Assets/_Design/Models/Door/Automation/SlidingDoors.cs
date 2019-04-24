using UnityEngine;
using System.Collections;

public enum DoorState { Opened, Closed };
public enum DoorAnimationState { None, Opening, Closing };
public class SlidingDoors : MonoBehaviour
{
    public bool reverse = false;

    Animator anim;
    bool playerstillhere = false;

    DoorState doorState = DoorState.Closed;
    DoorAnimationState doorAnimationState = DoorAnimationState.None;

    bool animating = false;

    void Start()
    {
        anim = transform.parent.gameObject.GetComponent<Animator>();
        doorState = DoorState.Closed;
        doorAnimationState = DoorAnimationState.None;
    }

    IEnumerator MyFunction()
    {
        yield return new WaitForSeconds(10f);
        playerstillhere = false;
        anim.SetBool("PlayerIsThere", playerstillhere);
        // enter your code here
    }

    void Update()
    {
        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Layer_Door.CloseDoor")) || (anim.GetCurrentAnimatorStateInfo(0).IsName("Layer_Door.OpenDoor")))
        {
            animating = true;
        }
        else
        {
            animating = false;
            doorAnimationState = DoorAnimationState.None;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (doorState == DoorState.Closed) && (doorAnimationState == DoorAnimationState.None))
        {
            doorAnimationState = DoorAnimationState.Opening;
            doorState = DoorState.Opened;
            animating = true;
            if (reverse)
            {
                anim.SetTrigger("Open Door Reverse");
            }
            else
            {
                anim.SetTrigger("Open Door");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player") && (doorState == DoorState.Opened) && (doorAnimationState == DoorAnimationState.None))
        {
            doorAnimationState = DoorAnimationState.Closing;
            doorState = DoorState.Closed;
            animating = true;
            if (reverse)
            {
                anim.SetTrigger("Close Door Reverse");
            }
            else
            {
                anim.SetTrigger("Close Door");
            }
        }
    }
}