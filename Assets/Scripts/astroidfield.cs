using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class astroidfield : MonoBehaviour
{
    public float m_Height, m_Width, m_Length, m_SpaceBetween;
    public int m_Seed;
    public GameObject[] m_AstroidSet;
    public float m_MinScale, m_MaxScale;
    public ArrayList m_RoidField = new ArrayList();
    private int m_RoidCount = 0;
    private Vector3[] m_Rotations;
    private float[] m_RotAngle;
    // Use this for initialization
    void Start()
    {
        Random.seed = m_Seed;
        GameObject containingFolder = new GameObject();
        containingFolder.name = "RoidField";
        m_RoidField.Clear();
        m_Rotations = new Vector3[(int)m_Width*(int)m_Length];
        m_RotAngle = new float[(int)m_Width * (int)m_Length];

        //initial loop for loading in the asteroids
        for (int w = 0; w < m_Width; w++)
        {
            for (int l = 0; l < m_Length; l++)
            {
                GameObject newRoid;
                m_Rotations[m_RoidCount] = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                m_Rotations[m_RoidCount].Normalize();
                m_RotAngle[m_RoidCount] = Random.Range(-0.5f, 0.5f);
                //newRoid.transform.parent = containingFolder.transform;
                //
                newRoid = (GameObject)Instantiate(m_AstroidSet[Random.Range(0,m_AstroidSet.Length)],new Vector3(0, 0, 0),Quaternion.identity);
                newRoid.transform.Translate(new Vector3((m_SpaceBetween* w), Random.Range(0.0f, m_Height), m_SpaceBetween * l));
                newRoid.transform.localRotation.SetLookRotation(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));
                float scale = Random.Range(m_MinScale, m_MaxScale);
                newRoid.transform.localScale = new Vector3(scale,scale,scale);
                m_RoidField.Add(newRoid);
                m_RoidCount++;
            }
        }
        int i = 0;
        foreach (GameObject item in m_RoidField)
        {
            item.transform.parent = containingFolder.transform;
            item.name = "asteroid " + i;
            i++;
        }
        containingFolder.transform.Translate(new Vector3(-(m_Width * m_SpaceBetween) / 2, -(m_Height / 2), -(m_Length * m_SpaceBetween) / 2));
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (GameObject item in m_RoidField)
        {
            item.transform.Rotate(m_Rotations[i],m_RotAngle[i]);
            i++;
        }
    }
}
