using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GeometryTester : MonoBehaviour
{
    public bool m_RunInEditor;
    bool m_IsPlaying;

    public bool m_Line_1_Active;
    public LineRenderer m_LineRender_1;
    public Color m_Line_1_Color;
    bool m_Line_1_Enabled;

    public bool m_Line_2_Active;
    public LineRenderer m_LineRender_2;
    public Color m_Line_2_Color;
    bool m_Line_2_Enabled;

    public bool m_Line_3_Active;
    public LineRenderer m_LineRender_3;
    public Color m_Line_3_Color;
    bool m_Line_3_Enabled;

    public bool m_Line_4_Active;
    public LineRenderer m_LineRender_4;
    public Color m_Line_4_Color;
    bool m_Line_4_Enabled;

    public bool m_Line_5_Active;
    public LineRenderer m_LineRender_5;
    public Color m_Line_5_Color;
    bool m_Line_5_Enabled;

    public bool m_Line_6_Active;
    public LineRenderer m_LineRender_6;
    public Color m_Line_6_Color;
    bool m_Line_6_Enabled;

    public Transform m_Target;
    public Vector3 m_Offset;
    public Vector3 m_OffsetReversed;
    public Vector3 m_PositionReversed;
    public Vector3 m_RotationReversed;
    public float m_Distance;
    public Transform m_ReveresedObject;
    public float m_Time;
    public Gradient m_Line3_Gradient;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public Vector3 m_SineVector;
    public float m_SegmentLength;
    public float m_SineAlpha;
    public float m_SineYComponent;

    public Gradient m_Line4_Gradient;
    public int m_LengthOfSineLine = 20;

    public int m_LengthOfBezierLine = 20;

    public List<Vector3> m_Line5_PointsList;
    public Transform m_Line5_Midpoint;
    public Vector3 m_Line5_TangentVertex1;
    public Vector3 m_Line5_TangentVertex2;
    public Vector3 m_Line5_BezierPoint;
    public float m_Line5Midpoint_Height;
    public Vector3 m_Line5MidpointVector;

    public List<Vector3> m_Line6_PointsList;
    public Transform m_Line6_Midpoint;
    public Vector3 m_Line6_TangentVertex1;
    public Vector3 m_Line6_TangentVertex2;
    public Vector3 m_Line6_BezierPoint;
    public float m_Line6Midpoint_Height;
    public Vector3 m_Line6MidpointVector;

    void Start()
    {
        SetLinesEnabled(true);

        m_IsPlaying = Application.isPlaying;

        if(!m_LineRender_1)
        {
            GameObject l_NewObj1 = new GameObject();

            m_LineRender_1 = l_NewObj1.AddComponent<LineRenderer>();
            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;
        }

        if (!m_LineRender_2)
        {
            GameObject l_NewObj1 = new GameObject();

            m_LineRender_2 = l_NewObj1.AddComponent<LineRenderer>();
            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;
        }

        if (!m_LineRender_3)
        {
            GameObject l_NewObj1 = new GameObject();

            m_LineRender_3 = l_NewObj1.AddComponent<LineRenderer>();
            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;
        }

        if (!m_LineRender_4)
        {
            GameObject l_NewObj1 = new GameObject();

            m_LineRender_4 = l_NewObj1.AddComponent<LineRenderer>();
            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;
        }

        if (!m_LineRender_5)
        {
            GameObject l_NewObj1 = new GameObject();

            m_LineRender_5 = l_NewObj1.AddComponent<LineRenderer>();
            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;
        }

        if (!m_LineRender_6)
        {
            GameObject l_NewObj1 = new GameObject();

            m_LineRender_6 = l_NewObj1.AddComponent<LineRenderer>();
            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;
        }

        if(!m_ReveresedObject)
        {
            GameObject l_NewObj1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            Collider l_Collider = l_NewObj1.GetComponent<Collider>();
            
            if (l_Collider)
            {
                Destroy(l_Collider);
            }

            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;

            m_ReveresedObject = l_NewObj1.transform;
        }

        if(!m_Line5_Midpoint)
        {
            GameObject l_NewObj1 = new GameObject();

            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;

            m_Line5_Midpoint = l_NewObj1.transform;
        }

        if (!m_Line6_Midpoint)
        {
            GameObject l_NewObj1 = new GameObject();

            l_NewObj1.transform.SetParent(this.transform);
            l_NewObj1.transform.localPosition = Vector3.zero;

            m_Line6_Midpoint = l_NewObj1.transform;
        }

        m_LineRender_1.positionCount = 2;
        m_LineRender_1.useWorldSpace = true;
        m_LineRender_2.positionCount = 2;
        m_LineRender_2.useWorldSpace = true;
        m_LineRender_3.positionCount = m_LengthOfSineLine;
        m_LineRender_3.useWorldSpace = true;
        m_LineRender_4.positionCount = m_LengthOfSineLine;
        m_LineRender_4.useWorldSpace = true;
        m_LineRender_5.positionCount = m_LengthOfBezierLine;
        m_LineRender_5.useWorldSpace = true;
        m_LineRender_6.positionCount = m_LengthOfBezierLine;
        m_LineRender_6.useWorldSpace = true;

        m_SineAlpha = 1.0f;
        m_Line3_Gradient = new Gradient();
        m_Line3_Gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(m_SineAlpha, 0.0f), new GradientAlphaKey(m_SineAlpha, 1.0f) }
        );
        m_LineRender_3.colorGradient = m_Line3_Gradient;

        m_Line4_Gradient = new Gradient();
        m_Line4_Gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(m_SineAlpha / 2, 0.0f), new GradientAlphaKey(m_SineAlpha / 2, 1.0f) }
        );
        m_LineRender_4.colorGradient = m_Line4_Gradient;

        m_SineVector.x = 0;
        m_SineVector.y = 0;
        m_SineVector.z = 0;

        m_Line5_PointsList = new List<Vector3>(m_LineRender_5.positionCount);
        m_Line6_PointsList = new List<Vector3>(m_LineRender_6.positionCount);

        //m_Line5Midpoint_Height = m_Line5_Midpoint.transform.position.y;
        //m_Line6Midpoint_Height = m_Line6_Midpoint.transform.position.y;
    }

    void Update()
    {
        if(m_IsPlaying || m_RunInEditor)
        {
            m_Time = Time.time;

            SetLinesEnabled();
            UpdateLines();
        }
    }

    void UpdateLines()
    {
        if(m_Target)
        {
            m_Offset = this.transform.position - m_Target.transform.position;
            m_Distance = Vector3.Distance(this.transform.position, m_Target.transform.position);

            m_LineRender_1.SetPosition(0, this.transform.position);
            m_LineRender_1.SetPosition(1, m_Target.transform.position);

            m_LineRender_2.SetPosition(0, this.transform.position);
            m_PositionReversed = this.transform.position + Vector3.Reflect(m_Offset, Vector3.up);
            m_LineRender_2.SetPosition(1, m_PositionReversed);
            m_ReveresedObject.transform.position = m_PositionReversed;
            m_ReveresedObject.transform.rotation = m_Target.rotation;

            m_SegmentLength = m_Distance / m_LengthOfSineLine;

            for (int i = 0; i < m_LengthOfSineLine; i++)
            {
                m_SineYComponent = Mathf.Sin(i + m_Time) / 2;

                if(m_LineRender_3)
                {
                    if (m_LineRender_3.positionCount != m_LengthOfSineLine) m_LineRender_3.positionCount = m_LengthOfSineLine;

                    m_SineVector = Vector3.MoveTowards(this.transform.position, m_Target.transform.position, m_SegmentLength * i);
                    m_SineVector.y += m_SineYComponent;
                    m_LineRender_3.SetPosition(i, m_SineVector);
                }

                if(m_LineRender_4)
                {
                    if (m_LineRender_4.positionCount != m_LengthOfSineLine) m_LineRender_4.positionCount = m_LengthOfSineLine;

                    m_SineVector = Vector3.MoveTowards(this.transform.position, m_PositionReversed, m_SegmentLength * i);
                    m_SineVector.y += m_SineYComponent;
                    m_LineRender_4.SetPosition(i, m_SineVector);
                }
            }

            //m_Line5MidpointVector = Vector3.MoveTowards(this.transform.position, m_Target.transform.position, 0.5f);
            m_Line5MidpointVector = (this.transform.position + m_Target.transform.position) / 2;
            m_Line5MidpointVector.y = m_Line5Midpoint_Height;
            m_Line5_Midpoint.transform.position = m_Line5MidpointVector;

            m_Line5_PointsList.Clear();
            for (float ratio = 0; ratio <= 1; ratio += 1.0f / m_LengthOfBezierLine)
            {
                m_Line5_TangentVertex1 = Vector3.Lerp(this.transform.position, m_Line5_Midpoint.position, ratio);
                m_Line5_TangentVertex2 = Vector3.Lerp(m_Line5_Midpoint.position, m_Target.transform.position, ratio);
                m_Line5_BezierPoint = Vector3.Lerp(m_Line5_TangentVertex1, m_Line5_TangentVertex2, ratio);
                m_Line5_PointsList.Add(m_Line5_BezierPoint);
            }

            m_LineRender_5.positionCount = m_Line5_PointsList.Count;
            m_LineRender_5.SetPositions(m_Line5_PointsList.ToArray());

            //m_Line6MidpointVector = Vector3.MoveTowards(this.transform.position, m_PositionReversed, 0.5f);
            m_Line6MidpointVector = (this.transform.position + m_PositionReversed) / 2;
            m_Line6MidpointVector.y = m_Line6Midpoint_Height;
            m_Line6_Midpoint.transform.position = m_Line6MidpointVector;

            m_Line6_PointsList.Clear();
            for (float ratio = 0; ratio <= 1; ratio += 1.0f / m_LengthOfBezierLine)
            {
                m_Line6_TangentVertex1 = Vector3.Lerp(this.transform.position, m_Line6_Midpoint.position, ratio);
                m_Line6_TangentVertex2 = Vector3.Lerp(m_Line6_Midpoint.position, m_PositionReversed, ratio);
                m_Line6_BezierPoint = Vector3.Lerp(m_Line6_TangentVertex1, m_Line6_TangentVertex2, ratio);
                m_Line6_PointsList.Add(m_Line6_BezierPoint);
            }

            m_LineRender_6.positionCount = m_Line6_PointsList.Count;
            m_LineRender_6.SetPositions(m_Line6_PointsList.ToArray());
        }
    }

    void SetLinesEnabled(bool a_Override = false)
    {
        if(m_LineRender_1)
        {
            if ((m_Line_1_Active != m_Line_1_Enabled) || a_Override)
            {
                m_Line_1_Enabled = m_Line_1_Active;
                m_LineRender_1.gameObject.SetActive(m_Line_1_Enabled);
                m_LineRender_1.startColor = Color.white;
                m_LineRender_1.startColor = m_Line_1_Color;
            }
        }

        if (m_LineRender_2)
        {
            if ((m_Line_2_Active != m_Line_2_Enabled) || a_Override)
            {
                m_Line_2_Enabled = m_Line_2_Active;
                m_LineRender_2.gameObject.SetActive(m_Line_2_Enabled);
                m_LineRender_2.startColor = Color.white;
                m_LineRender_2.startColor = m_Line_2_Color;
            }
        }

        if (m_LineRender_3)
        {
            if ((m_Line_3_Active != m_Line_3_Enabled) || a_Override)
            {
                m_Line_3_Enabled = m_Line_3_Active;
                m_LineRender_3.gameObject.SetActive(m_Line_3_Enabled);
                m_LineRender_3.startColor = Color.white;
                m_LineRender_3.startColor = m_Line_3_Color;
            }
        }

        if (m_LineRender_4)
        {
            if ((m_Line_4_Active != m_Line_4_Enabled) || a_Override)
            {
                m_Line_4_Enabled = m_Line_4_Active;
                m_LineRender_4.gameObject.SetActive(m_Line_4_Enabled);
                m_LineRender_4.startColor = Color.white;
                m_LineRender_4.startColor = m_Line_4_Color;
            }
        }

        if (m_LineRender_5)
        {
            if ((m_Line_5_Active != m_Line_5_Enabled) || a_Override)
            {
                m_Line_5_Enabled = m_Line_5_Active;
                m_LineRender_5.gameObject.SetActive(m_Line_5_Enabled);
                m_LineRender_5.startColor = Color.white;
                m_LineRender_5.startColor = m_Line_5_Color;
            }
        }

        if (m_LineRender_6)
        {
            if ((m_Line_6_Active != m_Line_6_Enabled) || a_Override)
            {
                m_Line_6_Enabled = m_Line_6_Active;
                m_LineRender_6.gameObject.SetActive(m_Line_6_Enabled);
                m_LineRender_6.startColor = Color.white;
                m_LineRender_6.startColor = m_Line_6_Color;
            }
        }
    }
}
