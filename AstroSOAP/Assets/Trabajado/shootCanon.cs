using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootCanon : MonoBehaviour
{
    public Transform m_Start;
    [Range(0, 99999)]
    public float m_Force;
    public GameObject m_BallCanon;

    // Start is called before the first frame update
    void Start() { 
        shoot();
    }
    
    private void shoot()
    {
        GameObject ball = Instantiate(m_BallCanon, m_Start.position, m_Start.rotation, transform);
        ball.GetComponent<Rigidbody>().AddForce(new Vector3(0, m_Force *0.5f, -m_Force));
    }
}
