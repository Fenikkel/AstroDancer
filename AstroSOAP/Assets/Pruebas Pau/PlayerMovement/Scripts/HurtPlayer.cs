using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{

    public int m_DamageToGive = 1;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "m_Player")
        {
            Vector3 hitDirection = other.transform.position - transform.position; //asi cojemos la direccion contraria a la que el jugador estaba iendo

            hitDirection = hitDirection.normalized; //normalizamos para que sea unitario y el impuso que pille no depenga del tamaño del vector

            FindObjectOfType<HealthManager>().HurtPlayer(m_DamageToGive,hitDirection);
            //el knock back lo llama la funcion hurt player para que unity no este buscando mas cosas
        }
    }

}
