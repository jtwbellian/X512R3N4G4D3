/************************************************************************************

Copyright   :   Copyright 2017 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.4.1 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

https://developer.oculus.com/licenses/sdk-3.4.1

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

** Modified by James Bellian to support OVRClimbable and 2 handed grab

************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows grabbing and throwing of objects with the OVRGrabbable component on them.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class OVRGrabber : MonoBehaviour
{

    private GameManager gm;
    private GameObject currentPOI = null;
    private GameObject previousPOI = null;

    private Vector3 climbPos;

    [HideInInspector]
    public bool isLocked = false;

    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public float grabBegin = 0.55f;
    public float grabEnd = 0.35f;
    public bool m_secondaryGrabber = false;

    // Demonstrates parenting the held object to the hand's transform when grabbed.
    // When false, the grabbed object is moved every FixedUpdate using MovePosition. 
    // Note that MovePosition is required for proper physics simulation. If you set this to true, you can
    // easily observe broken physics simulation by, for example, moving the bottom cube of a stacked
    // tower and noting a complete loss of friction.
    [SerializeField]
    protected bool m_parentHeldObject = false;

    // Child/attached transforms of the grabber, indicating where to snap held objects to (if you snap them).
    // Also used for ranking grab targets in case of multiple candidates.
    [SerializeField]
    protected Transform m_gripTransform = null;
    // Child/attached Colliders to detect candidate grabbable objects.
    [SerializeField]
    protected Collider[] m_grabVolumes = null;

    // Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch.
    [SerializeField]
    public OVRInput.Controller m_controller;
    [SerializeField]
    public Collider [] bodyCols;
    [SerializeField]
    protected Transform m_parentTransform;

    protected bool m_grabVolumeEnabled = true;
    protected Vector3 m_lastPos;
    protected Quaternion m_lastRot;
    protected Quaternion m_anchorOffsetRotation;
    protected Vector3 m_anchorOffsetPosition;
    protected float m_prevFlex;
    protected OVRGrabbable m_grabbedObj = null;
    protected Vector3 m_grabbedObjectPosOff;
    protected Quaternion m_grabbedObjectRotOff;
    protected Dictionary<OVRGrabbable, int> m_grabCandidates = new Dictionary<OVRGrabbable, int>();
    protected bool operatingWithoutOVRCameraRig = true;
    protected Vector3 bodyPos;
    VRTool lastTool;

    protected Vector3 prevPos;

    protected Rigidbody rb;

    /// <summary>
    /// The currently grabbed object.
    /// </summary>
    public OVRGrabbable grabbedObject
    {
        get { return m_grabbedObj; }

    }

    public void ForceRelease(OVRGrabbable grabbable)
    {
        bool canRelease = (
            (m_grabbedObj != null) &&
            (m_grabbedObj == grabbable)
        );
        if (canRelease)
        {
            GrabEnd();
        }
    }

    protected virtual void Awake()
    {
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;

        // If we are being used with an OVRCameraRig, let it drive input updates, which may come from Update or FixedUpdate.

        OVRCameraRig rig = null;

        if (transform.parent != null && transform.parent.parent != null)
            rig = transform.parent.parent.GetComponent<OVRCameraRig>();

        if (rig != null)
        {
            rig.UpdatedAnchors += (r) => { OnUpdatedAnchors(); };
            operatingWithoutOVRCameraRig = false;
        }

        // Find rigid body for player
        rb = transform.root.GetComponentInChildren<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("No rigid body found.");
        }
    }

    protected virtual void Start()
    {
        gm = GameManager.GetInstance();

        m_lastPos = transform.position;
        m_lastRot = transform.rotation;
        bodyPos = prevPos = transform.root.position;

        if (m_parentTransform == null)
        {
            if (gameObject.transform.parent != null)
            {
                m_parentTransform = gameObject.transform.parent.transform;
            }
            else
            {
                m_parentTransform = new GameObject().transform;
                m_parentTransform.position = Vector3.zero;
                m_parentTransform.rotation = Quaternion.identity;
            }
        }
    }

    void FixedUpdate()
    {
        if (operatingWithoutOVRCameraRig)
            OnUpdatedAnchors();

    }

    // Hands follow the touch anchors by calling MovePosition each frame to reach the anchor.
    // This is done instead of parenting to achieve workable physics. If you don't require physics on 
    // your hands or held objects, you may wish to switch to parenting.
    void OnUpdatedAnchors()
    {
        Vector3 handPos = OVRInput.GetLocalControllerPosition(m_controller);
        Quaternion handRot = OVRInput.GetLocalControllerRotation(m_controller);
        Vector3 destPos = m_parentTransform.TransformPoint(m_anchorOffsetPosition + handPos);
        Quaternion destRot = m_parentTransform.rotation * handRot * m_anchorOffsetRotation;
        GetComponent<Rigidbody>().MovePosition(destPos);
        GetComponent<Rigidbody>().MoveRotation(destRot);

        if (isLocked && Vector3.Distance(climbPos, transform.position) > 0.1f)
        {
            GrabEnd();
            Unlock();
        }

        if (m_grabbedObj is OVRClimbable)
        {
            ClimbGrabbedObject(handPos);
        }
        else
        if (m_secondaryGrabber)
        {
            OrientGrabbedObject(destPos);
        }
        else
        if (!m_parentHeldObject)
        {
            MoveGrabbedObject(destPos, destRot);
        }

        m_lastPos = transform.position;
        m_lastRot = transform.rotation;

        float prevFlex = m_prevFlex;
        // Update values from inputs
        m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);

        CheckForGrabOrRelease(prevFlex);

        // Force grab weapons
        /*
        if (m_grabbedObj == null)
        {
            VRTool nearTool = GrabRay();

            if (nearTool != null && nearTool != lastTool)
            {
                if (lastTool != null)
                    lastTool.LinesOff();

                nearTool.LinesOn();
                lastTool = nearTool;
            }
        }
        */
    }

    void OnDestroy()
    {
        if (m_grabbedObj != null)
        {
            GrabEnd();
        }
    }

    void OnColliderEnter(Collider otherCollider)
    {
        OnTriggerEnter(otherCollider);
    }


    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("UNAVAILABLE"))
        {
            gm = GameManager.GetInstance();
            gm.CreatePopup(transform.position, "Content Currently Unvailable", 1f);
        }

        // Get the grab trigger
        OVRGrabbable grabbable = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
        if (grabbable == null) return;

        // Add the grabbable
        int refCount = 0;
        m_grabCandidates.TryGetValue(grabbable, out refCount);
        m_grabCandidates[grabbable] = refCount + 1;
    }

    void OnTriggerExit(Collider otherCollider)
    {
        OVRGrabbable grabbable = otherCollider.GetComponent<OVRGrabbable>() ?? otherCollider.GetComponentInParent<OVRGrabbable>();
        if (grabbable == null) return;

        // Remove the grabbable
        int refCount = 0;
        bool found = m_grabCandidates.TryGetValue(grabbable, out refCount);
        if (!found)
        {
            return;
        }

        if (refCount > 1)
        {
            m_grabCandidates[grabbable] = refCount - 1;
        }
        else
        {
            m_grabCandidates.Remove(grabbable);
        }

    }

    protected void CheckForGrabOrRelease(float prevFlex)
    {
        if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin))
        {
            //Check for nearby grabbables
            GrabBegin();
        }
        else if ((m_prevFlex <= grabEnd) && (prevFlex > grabEnd))
        {
            if (isLocked)
                Unlock();
                                                
            if (m_secondaryGrabber)
                m_secondaryGrabber = false;

            GrabEnd();
        }
    }

    public void Lock()
    {
        if (m_controller == OVRInput.Controller.LTouch)
        {
            var lhand = transform.root.GetComponentInChildren<IKPlayerController>().handL;
            #region errorHandling
            if (!lhand)
            {
                Debug.Log("Error: could not find left hand");
                return;
            }

            Debug.Log("lhand: " + lhand);
            #endregion
            var ikPos = lhand.GetComponent<BioIK.Position>();
            var ikRot = lhand.GetComponent<BioIK.Orientation>();
            #region errorHandling
            if (!ikPos || !ikRot)
            {
                Debug.Log("Error: Bio IK segments missing");
                return;
            }
            #endregion

            ikPos.SetTargetTransform(null);//m_grabbedObj.transform);
            ikRot.SetTargetTransform(null);
            //ikPos.SetTargetPosition();
        }
        else
        if (m_controller == OVRInput.Controller.RTouch)
        {
            var rhand = transform.root.GetComponentInChildren<IKPlayerController>().handR;

            Debug.Log("rhand: " + rhand);
            #region errorHandling
            if (!rhand)
            {
                Debug.Log("Error: could not find right hand");
                return;
            }
            #endregion
            var ikPos = rhand.GetComponent<BioIK.Position>();
            var ikRot = rhand.GetComponent<BioIK.Orientation>();
            #region errorHandling
            if (!ikPos || !ikRot)
            {
                Debug.Log("Error: Bio IK segments missing");
                return;
            }
            #endregion

            ikPos.SetTargetTransform(null);
            ikRot.SetTargetTransform(null);
        }
        isLocked = true; 
    }

    public void Unlock()
    {
        if (m_controller == OVRInput.Controller.LTouch)
        {
            var lhand = transform.root.GetComponentInChildren<IKPlayerController>().handL;
            #region errorHandling
            if (!lhand)
            {
                Debug.Log("Error: could not find left hand");
                return;
            }
            #endregion
            var ikPos = lhand.GetComponent<BioIK.Position>();
            var ikRot = lhand.GetComponent<BioIK.Orientation>();
            #region errorHandling
            if (!ikPos || !ikRot)
            {
                Debug.Log("Error: Bio IK segments missing");
                return;
            }
            #endregion
            var offset = transform.Find("LOffset").transform;

            ikPos.SetTargetTransform(offset);
            ikRot.SetTargetTransform(offset);
        }
        else
        if (m_controller == OVRInput.Controller.RTouch)
        {
            var rhand = transform.root.GetComponentInChildren<IKPlayerController>().handR;
            #region errorHandling
            if (!rhand)
            {
                Debug.Log("Error: could not find right hand");
                return;
            }
            #endregion
            var ikPos = rhand.GetComponent<BioIK.Position>();
            var ikRot = rhand.GetComponent<BioIK.Orientation>();
            #region errorHandling
            if (!ikPos || !ikRot)
            {
                Debug.Log("Error: Bio IK segments missing");
                return;
            }
            #endregion

            var offset = transform.Find("ROffset").transform;

            ikPos.SetTargetTransform(offset);
            ikRot.SetTargetTransform(offset);
        }
        isLocked = false;
    }

    // Grab an object from afar, still WIP
    protected virtual VRTool GrabRay()
    {

        RaycastHit hit;
        VRTool newTool = null;
        VRTool oldTool = null;


        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            currentPOI = hit.collider.gameObject;
            
            if (currentPOI.Equals(previousPOI))
            {
                return null;
            }

            // Get the grabbable
            OVRGrabbable grabbable = currentPOI.GetComponent<OVRGrabbable>() ?? currentPOI.GetComponentInParent<OVRGrabbable>();

            if (grabbable == null)
                return null;

            /* Add the grabbable
            int refCount = 0;
            m_grabCandidates.TryGetValue(grabbable, out refCount);
            m_grabCandidates[grabbable] = refCount + 1;
            */
            //Get Tool
            newTool = currentPOI.GetComponent<VRTool>();

            if (newTool != null)
                newTool.LinesOn();

            if (previousPOI != null)
            {
                oldTool = previousPOI.GetComponent<VRTool>();
            }

            if (oldTool != null)
                oldTool.LinesOff();

            previousPOI = currentPOI;

            //Debug.Log("old: " + previousPOI.ToString() + " new: " + currentPOI.ToString());
            return newTool;
        }

        return null;
    }

    protected virtual void GrabBegin()
    {
        float closestMagSq = float.MaxValue;
        OVRGrabbable closestGrabbable = null;
        Collider closestGrabbableCollider = null;

        // Iterate grab candidates and find the closest grabbable candidate
        foreach (OVRGrabbable grabbable in m_grabCandidates.Keys)
        {
            bool canGrab = !(grabbable.isGrabbed && !grabbable.allowOffhandGrab);

            if (!canGrab)
            {
                continue;
            }

            for (int j = 0; j < grabbable.grabPoints.Length; ++j)
            {
                Collider grabbableCollider = grabbable.grabPoints[j];
                // Store the closest grabbable
                Vector3 closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(m_gripTransform.position);
                float grabbableMagSq = (m_gripTransform.position - closestPointOnBounds).sqrMagnitude;
                if (grabbableMagSq < closestMagSq)
                {
                    closestMagSq = grabbableMagSq;
                    closestGrabbable = grabbable;
                    closestGrabbableCollider = grabbableCollider;
                }
            }
        }

        // Disable grab volumes to prevent overlaps
        GrabVolumeEnable(false);

        if (closestGrabbable != null)
        {
            if (closestGrabbable.isGrabbed)
            {
                // If two handed, do not change the objects grabbedBy or parent. 
                if (closestGrabbable.twoHanded)
                {
                    Debug.Log("second hand begin interaction");
                    m_grabbedObj = closestGrabbable;
                    m_lastPos = transform.position;
                    m_lastRot = transform.rotation;
                    m_secondaryGrabber = true;
                    return;
                }

                closestGrabbable.grabbedBy.OffhandGrabbed(closestGrabbable);
            }

            m_grabbedObj = closestGrabbable;

            m_grabbedObj.GrabBegin(this, closestGrabbableCollider);

            m_lastPos = transform.position;
            m_lastRot = transform.rotation;

            // Set up offsets for grabbed object desired position relative to hand.
            if (m_grabbedObj.snapPosition)
            {
                m_grabbedObjectPosOff = m_gripTransform.localPosition;
                if (m_grabbedObj.snapOffset)
                {
                    Vector3 snapOffset = m_grabbedObj.snapOffset.position;
                    if (m_controller == OVRInput.Controller.LTouch) snapOffset.x = -snapOffset.x;
                    m_grabbedObjectPosOff += snapOffset;
                }
            }
            else
            {
                Vector3 relPos = m_grabbedObj.transform.position - transform.position;
                relPos = Quaternion.Inverse(transform.rotation) * relPos;
                m_grabbedObjectPosOff = relPos;
            }

            if (m_grabbedObj.snapOrientation)
            {
                m_grabbedObjectRotOff = m_gripTransform.localRotation;
                if (m_grabbedObj.snapOffset)
                {
                    m_grabbedObjectRotOff = m_grabbedObj.snapOffset.rotation * m_grabbedObjectRotOff;
                }
            }
            else
            {
                Quaternion relOri = Quaternion.Inverse(transform.rotation) * m_grabbedObj.transform.rotation;
                m_grabbedObjectRotOff = relOri;
            }

            // Note: force teleport on grab, to avoid high-speed travel to dest which hits a lot of other objects at high
            // speed and sends them flying. The grabbed object may still teleport inside of other objects, but fixing that
            // is beyond the scope of this demo.

            if (m_grabbedObj != null)
            {
                iSpecial_Grabbable special = m_grabbedObj.GetComponent<VRTool>();

                if (special != null)
                {
                    special.OnGrab();
                }

                // Stop player and release other hand if climb begins
                if (m_grabbedObj is OVRClimbable)
                {
                    OVRGrabber[] hands = transform.parent.GetComponentsInChildren<OVRGrabber>();

                    // lift opposite hand to avoid blasting off 
                    if (hands[0] != this && hands[0].m_grabbedObj is OVRClimbable)
                    {
                        hands[0].GrabbableRelease(Vector3.zero, Vector3.zero);
                    }
                    else if (hands[1] != this && hands[1].m_grabbedObj is OVRClimbable)
                    {
                        hands[1].GrabbableRelease(Vector3.zero, Vector3.zero);
                    }

                    rb.velocity = Vector3.zero;

                    // set colliders to go through this object 
                    foreach (Collider col in m_grabbedObj.allColliders)
                    {
                        if (col.isTrigger)
                            continue;

                        if (bodyCols.Length > 2)
                        {
                            Physics.IgnoreCollision(col, bodyCols[0]);
                            Physics.IgnoreCollision(col, bodyCols[1]);
                            Physics.IgnoreCollision(col, bodyCols[2]);
                        }
                    }

                    climbPos = transform.position;

                    return;
                }

                MoveGrabbedObject(m_lastPos, m_lastRot, true);

                if (m_parentHeldObject)
                {
                    m_grabbedObj.transform.parent = transform;
                }

                // Ignore collisions with players body (to prevent objects you interact with from pushing and disorienting player)
                foreach (Collider col in m_grabbedObj.allColliders)
                {
                    if (col.isTrigger)
                        continue;

                    if (bodyCols.Length > 2)
                    { 
                        Physics.IgnoreCollision(col, bodyCols[0]);
                        Physics.IgnoreCollision(col, bodyCols[1]);
                        Physics.IgnoreCollision(col, bodyCols[2]);
                    }
                }
            }
        }
    }

    protected virtual void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
    {
        if (m_grabbedObj.twoHanded)
            return;

        if (m_grabbedObj == null)
        {
            return;
        }

        Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;
        Vector3 grabbablePosition = pos + rot * m_grabbedObjectPosOff;
        Quaternion grabbableRotation = rot * m_grabbedObjectRotOff;

        if (forceTeleport)
        {
            grabbedRigidbody.transform.position = grabbablePosition;
            grabbedRigidbody.transform.rotation = grabbableRotation;
        }
        else
        {
            grabbedRigidbody.MovePosition(grabbablePosition);
            grabbedRigidbody.MoveRotation(grabbableRotation);
        }
    }

    // Added by James Bellian to allow for climbing
    protected virtual void ClimbGrabbedObject(Vector3 pos)
    {
        if (!isLocked)
            Lock();

        if (m_grabbedObj == null)
        {
            return;
        }
        
        rb.AddForce((m_lastPos - transform.position) / Time.deltaTime, ForceMode.VelocityChange);
    }

    protected virtual void OrientGrabbedObject(Vector3 pos)
    {
        if (m_grabbedObj == null || m_grabbedObj.grabbedBy == null)
        { Debug.Log("error");
        return; }

        //m_grabbedObj.grabbedBy.Lock();
        //Lock();

        var pivot = m_grabbedObj.grabbedBy.transform.position;
        var point = transform.position;

        //var newRot = Quaternion.Euler(angleX, angleY, angleZ);
        var newRot = Quaternion.LookRotation(point - pivot);

        m_grabbedObj.transform.Rotate(pivot, Vector3.Angle(pivot, point));
        m_grabbedObj.transform.rotation = newRot;
        m_grabbedObj.transform.position = (pivot + point)/2f ;
    }

    // calculate hand velocity based on the last position it was recorded in
    public Vector3 GetHandVelocity()
    {
        return (m_lastPos - transform.position) / Time.deltaTime;
    }

    public void GrabEnd()
    {
        if (m_grabbedObj != null)
        {
            iSpecial_Grabbable special = m_grabbedObj.GetComponent<iSpecial_Grabbable>();

            if (special != null)
            {
                special.OnRelease();
            }

            foreach (Collider col in m_grabbedObj.allColliders)
            {
                if (col.isTrigger)
                    continue;

                if (bodyCols.Length > 2)
                {
                    Physics.IgnoreCollision(col, bodyCols[0], false);
                    Physics.IgnoreCollision(col, bodyCols[1], false);
                    Physics.IgnoreCollision(col, bodyCols[2], false);
                }
            }

            OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
            OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
            localPose = localPose * offsetPose;

			OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
			Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
			Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);

            GrabbableRelease(linearVelocity, angularVelocity);
        }

        // Re-enable grab volumes to allow overlap events
        GrabVolumeEnable(true);
    }

    protected void GrabbableRelease(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        m_grabbedObj.GrabEnd(linearVelocity, angularVelocity);
        if(m_parentHeldObject) m_grabbedObj.transform.parent = null;
        m_grabbedObj = null;
    }

    protected virtual void GrabVolumeEnable(bool enabled)
    {
        if (m_grabVolumeEnabled == enabled)
        {
            return;
        }

        m_grabVolumeEnabled = enabled;

        for (int i = 0; i < m_grabVolumes.Length; ++i)
        {
            Collider grabVolume = m_grabVolumes[i];
            grabVolume.enabled = m_grabVolumeEnabled;
        }

        if (!m_grabVolumeEnabled)
        {
            m_grabCandidates.Clear();
        }
    }

	protected virtual void OffhandGrabbed(OVRGrabbable grabbable)
    {
        // If the object grabbable is held in this hand
        if (m_grabbedObj == grabbable)
        {
            GrabbableRelease(Vector3.zero, Vector3.zero);
        }
    }
}
