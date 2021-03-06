using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numFish= 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;


    //seta a velocida e movimentçao
    [Header("Configuraçao do cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;


    void Start()
    {
        allFish = new GameObject[numFish];

        //Cria lista de peixes
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), 
                                                               Random.Range(-swinLimits.y, swinLimits.y), 
                                                               Random.Range(-swinLimits.z, swinLimits.z));

            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = this.transform.position;    
        
        
        void Update()
        {
            //Checa a posicao e movimenta o peixe para uma nova
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                Random.Range(-swinLimits.y, swinLimits.y),
                Random.Range(-swinLimits.z, swinLimits.z));


        }
    }
}
