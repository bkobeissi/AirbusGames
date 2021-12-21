using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    // Start is called before the first frame update

    //nous permet de faire une vague d'ennemi
    void Start()
    {
        transform.DetachChildren();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
