using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GoldArea : MonoBehaviour
{

    public GameObject m_GoldPrefab;
    [Range(1, 6)]
    public int m_NumberOfCoins = 3;
    public float m_SpaceBetween = 1;
    public bool m_InLine = false;// este sera vertical...
    public bool m_InLineHorizontal = false;
    public bool m_InLinePerpendicular = false;

    private void Awake()
    {
        //Instantiate(m_GoldPrefab);
    }

    // Start is called before the first frame update
    void Start()
    {

        if (m_InLine)
        {
            m_InLineHorizontal = false;
            m_InLinePerpendicular = false;

            for (int i = 0; i < m_NumberOfCoins; i++)
            {
                Instantiate(m_GoldPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + (m_SpaceBetween * i)), transform.rotation);//Quaternion.identity

            }

        }
        else if (m_InLineHorizontal)
        {
            m_InLine = false;
            m_InLinePerpendicular = false;

            for (int i = 0; i < m_NumberOfCoins; i++)
            {
                Instantiate(m_GoldPrefab, new Vector3(transform.position.x + (m_SpaceBetween * i), transform.position.y, transform.position.z), transform.rotation);//Quaternion.identity
            }

        }
        else if (m_InLinePerpendicular)
        {
            m_InLine = false;
            m_InLineHorizontal = false;
            for (int i = 0; i < m_NumberOfCoins; i++)
            {
                Instantiate(m_GoldPrefab, new Vector3(transform.position.x, transform.position.y + (m_SpaceBetween * i), transform.position.z), transform.rotation);//Quaternion.identity
            }

        }
        else
        {
            m_InLine = false;
            m_InLineHorizontal = false;
            switch (m_NumberOfCoins)
            {
                case 1:

                    Instantiate(m_GoldPrefab, transform.position, transform.rotation);//Quaternion.identity
                    break;

                case 2:

                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - m_SpaceBetween, transform.position.y, transform.position.z), transform.rotation);
                    break;

                case 3:

                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - (m_SpaceBetween / 2), transform.position.y, transform.position.z + m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - (m_SpaceBetween / 2), transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);
                    break;

                case 4:

                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z + m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - m_SpaceBetween, transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - m_SpaceBetween, transform.position.y, transform.position.z + m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);

                    break;


                case 5:

                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z + m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - m_SpaceBetween, transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - m_SpaceBetween, transform.position.y, transform.position.z + m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);


                    break;

                case 6:

                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z + (m_SpaceBetween / 2)), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - m_SpaceBetween, transform.position.y, transform.position.z + (m_SpaceBetween / 2)), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x - m_SpaceBetween, transform.position.y, transform.position.z - (m_SpaceBetween / 2)), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z - m_SpaceBetween), transform.rotation);
                    Instantiate(m_GoldPrefab, new Vector3(transform.position.x + m_SpaceBetween, transform.position.y, transform.position.z - (m_SpaceBetween / 2)), transform.rotation);

                    break;

                default:
                    print("Numero de monedas introducidas no implementadas, prueba con otra cifra");
                    break;
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
