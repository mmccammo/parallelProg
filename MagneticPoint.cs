using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticPoint : MonoBehaviour
{
    public GameObject m_Target;
    public GameObject m_Host;

    public Transform m_MagnetTargetTransform;
    public Transform m_MagnetHostTransform;
    public Rigidbody m_MagnetTargetRB;
    public Rigidbody m_MagnetHostRB;

    public Vector3 m_3DDistanceBetweenPoints;
    public Vector3 m_DirectionFromTargetToHost;
    public float m_DistanceBetweenPoints;

    public float m_MinDistance = 0.01f;
    public float m_RelativeDistance;
    public float m_MaxDistance = 100;
    public float m_PreviousMaxDistance = 100;
    public float m_CurrentMagnetStrength = 0;
    public float m_MaxMagnetStrength = 5;

    public MagnetMode m_PullMode;
    public Vector3 m_FinalForce;

    public bool m_UsePhysics;
    public Vector3 m_LerpedPosition;
    public float m_LerpMod = 1;

    public Vector3 m_LineStartVec;
    public float m_LineHeightOverride = 5;
    float m_LineHeight1;
    float m_LineHeight2;
    float m_LineHeight3;

    public bool m_DrawRadius;
    public bool m_Restart;
    public bool m_Ready;
    public bool m_Active;

    public enum MagnetMode
    {
        MASS = 0,
        PULL,
        PUSH,
    }

    void Start()
    {
        ExternalStart();
    }

    void ExternalStart()
    {
        m_Ready = false;

        m_Host = this.gameObject;
        m_MagnetHostTransform = m_Host.transform;
        m_MagnetHostRB = m_Host.GetComponent<Rigidbody>();

        if(m_Target)
        {
            m_MagnetTargetTransform = m_Target.transform;
            m_MagnetTargetRB = m_Target.GetComponent<Rigidbody>();
        }

        if (m_MagnetTargetTransform && m_MagnetHostTransform && m_MagnetTargetRB)
        {
            m_LineHeight1 = m_LineHeightOverride;
            m_LineHeight2 = m_LineHeightOverride + 1;
            m_LineHeight3 = m_LineHeightOverride + 2;

            m_Ready = true;
        }
    }

    void Update()
    {
        if(m_Restart)
        {
            ExternalStart();
            m_Restart = false;
            return;
        }
        if(m_Ready)
        {
            Debug.DrawLine(m_MagnetHostTransform.position, m_MagnetTargetTransform.position, Color.green);

            if (m_Active)
            {
                GetDistance();

                if (m_DrawRadius) DrawMagRadius();

                if (m_DistanceBetweenPoints < m_MaxDistance)
                {
                    AppyMagnet();
                }
            }
        }
    }

    void GetDistance()
    {
        m_3DDistanceBetweenPoints = m_MagnetHostTransform.position - m_MagnetTargetTransform.position;
        m_DistanceBetweenPoints = Vector3.Distance(m_MagnetHostTransform.position, m_MagnetTargetTransform.position);
        m_DirectionFromTargetToHost = (m_MagnetTargetTransform.position - m_MagnetHostTransform.position).normalized;
    }

    void DrawMagRadius()
    {
        m_LineStartVec.x = m_MagnetHostTransform.position.x;
        m_LineStartVec.z = m_MagnetHostTransform.position.z;

        m_LineStartVec.y = m_LineHeight1;
        Debug.DrawRay(m_LineStartVec, m_DirectionFromTargetToHost * m_MaxDistance, Color.green);

        m_LineStartVec.y = m_LineHeight2;
        Debug.DrawRay(m_LineStartVec, m_DirectionFromTargetToHost * (m_MaxDistance * 0.67f), Color.yellow);

        m_LineStartVec.y = m_LineHeight3;
        Debug.DrawRay(m_LineStartVec, m_DirectionFromTargetToHost * (m_MaxDistance * 0.34f), Color.red);
    }

    void AppyMagnet()
    {
        m_RelativeDistance = Mathf.InverseLerp(m_MaxDistance, 0f, m_DistanceBetweenPoints);
        m_CurrentMagnetStrength = Mathf.Lerp(0f, m_MaxMagnetStrength, m_RelativeDistance);
        m_FinalForce = m_DirectionFromTargetToHost * m_CurrentMagnetStrength;  

        if (m_UsePhysics) ApplyMagnetPhysics();
        else ApplyMagnetLerp();
    }

    void ApplyMagnetPhysics()
    {
        switch (m_PullMode)
        {
            case MagnetMode.PULL:
                m_FinalForce = m_DirectionFromTargetToHost * m_CurrentMagnetStrength;
                break;
            case MagnetMode.PUSH:
                m_FinalForce = m_DirectionFromTargetToHost * m_CurrentMagnetStrength * -1;
                break;
            default:
                break;
        }

        m_MagnetTargetRB.AddForce(m_FinalForce, ForceMode.Force);
    }

    void ApplyMagnetLerp()
    {

        if(m_DistanceBetweenPoints < m_MinDistance)
        {
            m_MagnetTargetRB.isKinematic = false;
        }
        else
        {
            m_MagnetTargetRB.isKinematic = true;
        }

        m_LerpedPosition = Vector3.Lerp(m_MagnetTargetTransform.position, m_MagnetHostTransform.position, Time.deltaTime * m_CurrentMagnetStrength * m_LerpMod);
        m_MagnetTargetTransform.position = m_LerpedPosition;
    }
}
