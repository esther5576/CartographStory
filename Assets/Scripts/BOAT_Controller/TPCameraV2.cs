using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCameraV2 : MonoBehaviour {

    #region Variables
    //Pointeurs
    GameObject m_pMyCharacter;
    GameObject m_pDummy;
    BoatNavigation m_pMoveClass;

    //Exposed variables
    [Header("Height & Distances")]
    [Space(10)]
    [Range(1, 100)]
    public float m_fDesiredHeightFromAvatar = 10f;
    [Range(0, 50)]
    public float m_fDesiredYOffsetFromAvatar = 10f;
    [Range(0, 100)]
    public float m_fDescendingHeight = 10f;
    [Range(0, 100)]
    public float m_fAscendingHeight = 10f;
    [Range(1, 100)]
    public float m_fDesiredDescendingDistance = 10f;
    [Range(1, 100)]
    public float m_fDesiredAscendingDistance = 10f;
    [Range(1, 100)]
    public float m_fDesiredDistance = 10f;
    [Range(1, 200)]
    public float m_fMaxAllowedHeight = 10f;


    [Header("Dampings & Speeds")]
    [Range(1, 200)]
    [Space(10)]
    public float m_fDesiredRotationDamping = 100f;
    [Range(0f, 200f)]
    public float m_fPlayerInputSpeed = 100f;
    [Range(0f, 20f)]
    public float m_fContextualSpeed = 2f;
    [Range(0f, 20f)]
    public float m_fBehaviourSwiftDamping = 1f;
    [Range(1, 10)]
    public float m_fDesiredDistanceDamping = 4f;
    [Range(1, 10)]
    public float m_fDesiredHeightDamping = 10f;
    [Range(1, 100)]
    public float m_fDesiredDescendingDistanceSpeed = 10f;
    [Range(1, 100)]
    public float m_fDesiredAscendingDistanceSpeed = 10f;


    [Header("Misc")]
    [Space(10)]
    [Range(0f, 1f)]
    public float m_fStartBehaviorThreshold = 0.01f;
    public Vector3 m_vPauseCamPos = new Vector3(0, 0, 0);

    public GameObject m_pCinematicTarget;
    [Range(0f, 10f)]
    public float m_fRefocusTime = 3f;


    //private variables
    GameObject m_pContextualTarget;
    bool m_bIsInPause;
    bool m_bIsAscending = false;
    bool m_bIsDescending = false;
    bool m_bIsInClassicFollow = true;
    bool m_bIsRefocusing = false;
    bool m_bIsInCinematic = true;
    bool m_bOverrideBehaviors = false;

    Vector3 m_vCurrentCharacterPos;
    const int m_iNoMovementFrames = 10;
    Vector3[] m_avPreviousCharacterPos = new Vector3[m_iNoMovementFrames];

    float m_fActualDesiredHeight = 0;
    float m_fActualDesiredDistance = 0;
    float m_fActualDistanceDamping = 0;
    float m_fActualRotationDamping = 0;

    Vector3 m_vContextualPos = new Vector3(0, 0, 0);
    Quaternion m_qEntryQuaternion = new Quaternion(0, 0, 0, 0);

    #endregion


    void Awake()
    {
        SetPointers();
        CleanPosArray();
    }

    // Use this for initialization
    void Start()
    {

        m_fActualDesiredHeight = m_fDesiredHeightFromAvatar;
        m_fActualDesiredDistance = m_fDesiredDistance;
        m_fActualDistanceDamping = m_fDesiredDistanceDamping;
        m_fActualRotationDamping = m_fDesiredRotationDamping;


    }

    // Update is called once per frame
    void Update()
    {


        if (GetPlayerIsMovingCam() && !GetIsOverridingBehavior() && !GetIsInPauseCam() && !GetIsInCinematic() && !GetIsRefocusing())
        {
            UpdateCameraRotation();
        }
        else if (GetIsOverridingBehavior())
        {
            FocusOnPlayerForward();
        }
        else if (GetIsInCinematic())
        {
            ApplyCinematicBehavior();
        }
        else if (GetIsInPauseCam() && !GetIsRefocusing())
        {
            ApplyContextualBehavior();
            MoveCamToPausePos();
        }
        else if (GetIsRefocusing())
        {
            UpdateDummyPos();
            RefocusOnPlayer();
        }
        else
        {
            UpdateCharacterPos();
            UpdateDummyPos();
            SmartFollow();
        }

    }

    void FixedUpdate()
    {
        if (!GetIsInCinematic() && !GetIsInPauseCam() && !GetIsRefocusing())
        {
            AntiClipping();
        }
    }

    #region StartMethods
    void SetPointers()
    {
        m_pMyCharacter = GameObject.FindGameObjectWithTag("Player");
        m_pMoveClass = m_pMyCharacter.GetComponent<BoatNavigation>();
        m_pDummy = GameObject.Find("DummyCam");
    }

    void CleanPosArray()
    {
        for (int i = 0; i < m_avPreviousCharacterPos.Length; i++)
        {
            m_avPreviousCharacterPos[i] = Vector3.zero;
        }
    }
    #endregion

    #region UpdatedMethods
    void SmartFollow()
    {
        if (m_pMyCharacter == null)
            return;

        Vector3 vCamPos = transform.position;
        Vector3 vDummyPos = m_pDummy.transform.position;
        Vector3 vLookAtDesiredPos = new Vector3(m_pMyCharacter.transform.position.x, (m_pMyCharacter.transform.position.y + m_fDesiredYOffsetFromAvatar), m_pMyCharacter.transform.position.z);

        float fSpeedRatio = (m_pMyCharacter.GetComponent<Rigidbody>().velocity.magnitude / m_pMoveClass.maxSpeed) + 1;

        float fXpos = Mathf.Lerp(transform.position.x, vDummyPos.x, Time.deltaTime * m_fActualDistanceDamping);
        float fZpos = Mathf.Lerp(transform.position.z, vDummyPos.z, Time.deltaTime * m_fActualDistanceDamping * fSpeedRatio);
        float fYpos = Mathf.Lerp(transform.position.y, vDummyPos.y, Time.deltaTime / m_fDesiredHeightDamping);

        Vector3 vDesiredPos = new Vector3(fXpos, fYpos, fZpos);
        if (m_fActualRotationDamping < 99)
            m_fActualRotationDamping = Mathf.Lerp(m_fActualRotationDamping, m_fDesiredRotationDamping, Time.deltaTime / 1.2f);
        transform.position = vDesiredPos; ;
        Quaternion qRot = Quaternion.LookRotation(vLookAtDesiredPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, qRot, Time.deltaTime * m_fActualRotationDamping);
        //transform.LookAt(Vector3.Lerp(vCamPos, vLookAtDesiredPos, Time.deltaTime * m_fActualRotationDamping));

    }

    void UpdateDummyPos()
    {
        m_pDummy.transform.localPosition = new Vector3(0, m_fActualDesiredHeight, -m_fActualDesiredDistance);
        //m_pDummy.transform.localPosition = new Vector3(0, m_fActualDesiredHeight, -m_fDesiredDistanceSpeed);
    }

    void UpdateCameraRotation()
    {
        Vector3 vDesiredLookAtPos = new Vector3(m_pMyCharacter.transform.position.x, m_pMyCharacter.transform.position.y + m_fDesiredYOffsetFromAvatar, m_pMyCharacter.transform.position.z);
        transform.RotateAround(m_pMyCharacter.transform.position, m_pMyCharacter.transform.up, /*Input.GetAxis("Right Horizontal")*/ Input.GetAxis("Mouse X") * Time.deltaTime * m_fPlayerInputSpeed);
        Vector3 vDesiredPos = new Vector3(m_pDummy.transform.position.x, m_pDummy.transform.position.y, m_pDummy.transform.position.z);
        transform.parent = m_pMyCharacter.transform;
        transform.LookAt(vDesiredLookAtPos);
    }

    void ApplyContextualBehavior()
    {
        if (m_pContextualTarget == null) return;
        Quaternion qTempRot = Quaternion.LookRotation(m_pContextualTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, qTempRot, Time.deltaTime * m_fContextualSpeed);
    }

    void UpdateCharacterPos()
    {
        if (m_pMyCharacter == null)
            return;

        m_vCurrentCharacterPos = m_pMyCharacter.transform.position;

        //Check if the Character has changed its altitude
        for (int i = 0; i < m_avPreviousCharacterPos.Length - 1; i++)
        {
            m_avPreviousCharacterPos[i] = m_avPreviousCharacterPos[i + 1];
        }

        m_avPreviousCharacterPos[m_avPreviousCharacterPos.Length - 1] = m_vCurrentCharacterPos;

        float fDiff = 0;

        for (int i = 0; i < m_avPreviousCharacterPos.Length - 1; i++)
        {
            fDiff += m_avPreviousCharacterPos[i].y - m_avPreviousCharacterPos[i + 1].y;
        }

        fDiff = fDiff / m_avPreviousCharacterPos.Length;

        if (fDiff < -m_fStartBehaviorThreshold)
        {
            m_fActualDesiredHeight = Mathf.Lerp(m_fActualDesiredHeight, m_fAscendingHeight, Time.deltaTime);
            m_fActualDesiredDistance = Mathf.Lerp(m_fActualDesiredDistance, m_fDesiredAscendingDistance, Time.deltaTime * 2f);
            m_fActualDistanceDamping = Mathf.Lerp(m_fActualDistanceDamping, m_fDesiredAscendingDistanceSpeed, Time.deltaTime);
            m_bIsAscending = true;
            m_bIsInClassicFollow = false;


        }
        else if (fDiff > m_fStartBehaviorThreshold)
        {
            m_fActualDesiredHeight = Mathf.Lerp(m_fActualDesiredHeight, m_fDescendingHeight, Time.deltaTime);
            m_fActualDesiredDistance = Mathf.Lerp(m_fActualDesiredDistance, m_fDesiredDescendingDistance, Time.deltaTime * 2f);
            m_fActualDistanceDamping = Mathf.Lerp(m_fActualDistanceDamping, m_fDesiredDescendingDistanceSpeed, Time.deltaTime);
            m_bIsDescending = true;
            m_bIsInClassicFollow = false;
        }
        else
        {
            m_fActualDesiredHeight = Mathf.Lerp(m_fActualDesiredHeight, m_fDesiredHeightFromAvatar, Time.deltaTime);
            m_fActualDesiredDistance = Mathf.Lerp(m_fActualDesiredDistance, m_fDesiredDistance, Time.deltaTime / 2f);
            m_fActualDistanceDamping = Mathf.Lerp(m_fActualDistanceDamping, m_fDesiredDistanceDamping, Time.deltaTime);
            m_bIsAscending = false;
            m_bIsDescending = false;
            m_bIsInClassicFollow = true;
        }

        //Debug.Log("Speed = " + m_fActualDistanceDamping);
        //Debug.Log("Distance = " + m_fActualDesiredDistance);
    }

    void AntiClipping()
    {

        if (GetHeightFromAvatar() < 0)
        {
            //version pauvre anticlip vertical
            transform.position = new Vector3(transform.position.x, m_pMyCharacter.transform.position.y, transform.position.z);
            //Vector3 vDesiredPos = new Vector3(transform.position.x, m_pMyCharacter.transform.position.y, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, vDesiredPos, Time.deltaTime * 100f);
            //Debug.Log("Je bloque en bas");
        }
        else if (GetHeightFromAvatar() > m_fMaxAllowedHeight)
        {
            transform.position = new Vector3(transform.position.x, m_pMyCharacter.transform.position.y + m_fMaxAllowedHeight, transform.position.z);
            //Vector3 vDesiredPos = new Vector3(transform.position.x, m_pMyCharacter.transform.position.y + m_fMaxAllowedHeight, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, vDesiredPos, Time.deltaTime * 100f);
            // Debug.Log("Je bloque en haut");
        }

        if (GetDistanceFromDummy() < 10 && GetIsAscending())
        {
            m_fDesiredYOffsetFromAvatar = Mathf.Lerp(m_fDesiredYOffsetFromAvatar, 0, Time.deltaTime);
            //Debug.Log("Je refocus !");
        }
        else
        {
            m_fDesiredYOffsetFromAvatar = Mathf.Lerp(m_fDesiredYOffsetFromAvatar, 10, Time.deltaTime);
        }

        //Debug.Log("Height = " + GetHeightFromAvatar());
        //Debug.Log("Distance = " + GetDistanceFromDummy());
    }

    void MoveCamToPausePos()
    {
        Vector3 vPos = new Vector3(m_vContextualPos.x, m_vContextualPos.y, m_vContextualPos.z);
        m_pDummy.transform.position = Vector3.Lerp(m_pDummy.transform.position, vPos, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, m_pDummy.transform.position, Time.deltaTime);
        //Debug.Log("MoveCamToPause");
    }

    void ApplyCinematicBehavior()
    {
        m_pContextualTarget = m_pCinematicTarget;
        m_fActualRotationDamping = 1f;
        if (m_pContextualTarget == null) return;
        Quaternion qTempRot = Quaternion.LookRotation(m_pContextualTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, qTempRot, Time.deltaTime);
        transform.position = transform.position = Vector3.Lerp(transform.position, m_pDummy.transform.position, Time.deltaTime);
        /*if (Input.anyKeyDown && VideoPLaying._VideoEnded)
        {
            //Debug.Log("Stop cinematic");
            m_bIsInCinematic = false;
            m_pContextualTarget = null;
            StartCoroutine(FocusOnPlayer());
        }*/
    }

    void FocusOnPlayerForward()
    {
        Vector3 vLookAtDesiredPos = new Vector3(m_pMyCharacter.transform.position.x, (m_pMyCharacter.transform.position.y + m_fDesiredYOffsetFromAvatar), m_pMyCharacter.transform.position.z);
        Quaternion qTemp = Quaternion.LookRotation(vLookAtDesiredPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, qTemp, Time.deltaTime / 2f);
        transform.position = Vector3.Lerp(transform.position, m_pDummy.transform.position, Time.deltaTime * 4f);
        //Debug.Log("FocusAfterBiome");
    }
    #endregion

    #region ContextualMethods

    IEnumerator FocusOnPlayer()
    {

        m_pContextualTarget = m_pMyCharacter;
        m_bIsRefocusing = true;
        if (m_bIsInPause)
            m_bIsInPause = false;
        yield return new WaitForSeconds(m_fRefocusTime);
        m_bIsRefocusing = false;
        m_pContextualTarget = null;



    }

    void RefocusOnPlayer()
    {
        m_fActualRotationDamping = 2f;
        Vector3 vLookAtDesiredPos = new Vector3(m_pMyCharacter.transform.position.x, (m_pMyCharacter.transform.position.y + m_fDesiredYOffsetFromAvatar), m_pMyCharacter.transform.position.z);
        Quaternion qTemp = Quaternion.LookRotation(vLookAtDesiredPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, qTemp, Time.deltaTime * m_fActualRotationDamping);
        transform.position = Vector3.Lerp(transform.position, m_pDummy.transform.position, Time.deltaTime);

        //Debug.Log("Refocusing");
    }

    public void StartBiomeCam()
    {
        m_qEntryQuaternion = Quaternion.LookRotation(m_pMyCharacter.transform.forward - transform.position);
    }

    public void StartPauseCam(GameObject _pTarget)
    {
        m_bIsRefocusing = false;
        m_pContextualTarget = _pTarget;
        m_bIsInPause = true;
        m_vContextualPos = transform.position;
        m_pDummy.transform.parent = null;
    }

    public void StopPauseCam()
    {
        //Debug.Log("StopPauseCam");
        StartCoroutine(FocusOnPlayer());
        m_pDummy.transform.parent = m_pMyCharacter.transform;

    }
    #endregion

    #region Gets
    float GetHeightFromAvatar()
    {
        return transform.position.y - m_pMyCharacter.transform.position.y;
    }

    float GetDistanceFromDummy()
    {
        Vector3 vDummyPos = m_pDummy.transform.position;
        Vector3 vPos = transform.position;

        return Vector3.Distance(vDummyPos, vPos);
    }

    bool GetPlayerIsMovingCam()
    {
        bool b = false;
        if (/*Input.GetAxis("Right Horizontal")*/Input.GetAxis("Mouse X") != 0)
            b = true;
        else
        {
            b = false;
            transform.parent = null;
        }
        return b;
    }

    public bool GetIsAscending()
    {
        return m_bIsAscending;
    }

    public bool GetIsDescending()
    {
        return m_bIsDescending;
    }

    public bool GetIsInClassicFollow()
    {
        return m_bIsInClassicFollow;
    }

    public bool GetIsInPauseCam()
    {
        return m_bIsInPause;
    }

    public bool GetIsRefocusing()
    {
        return m_bIsRefocusing;
    }

    public bool GetIsInCinematic()
    {
        return m_bIsInCinematic;
    }

    public bool SetOverrideBehavior(bool _b)
    {
        m_bOverrideBehaviors = _b;
        return m_bOverrideBehaviors;
    }
    public bool GetIsOverridingBehavior()
    {
        return m_bOverrideBehaviors;
    }
    #endregion
}