using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    //variavel
    public FlockManager myManager;
    
    //velocidade
    public float speed;
   
    //checa sw o peice esta virando
    bool turning = false;


    void Start()
    {
        // Ajusta a velocidade
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

 
    void Update()
    {
        //cria limites para aonde os peixes podem nadar
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);

        RaycastHit hit = new RaycastHit();
        
        //define a direçao
        Vector3 direction = myManager.transform.position - transform.position;

        if (!b.Contains(transform.position))
        {
            
            turning =true;
            direction = myManager.transform.position - transform.position;

        }
        else if(Physics.Raycast(transform.position, this.transform.forward*50, out hit))
        {

            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
            turning =false;

        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            
            //se for menor que 20
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }
                
                
            ApplyRules();
        transform.Translate(0, 0, Time.deltaTime * speed);

        ApplyRules();
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        //lista de objetos
        GameObject[] gos;

        //seta componentes
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                
                if (nDistance <= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;
                    
                    if (go != this.gameObject)
                    {
                        nDistance = Vector3.Distance(go.transform.position, this.transform.position);

                        if (nDistance <= myManager.neighbourDistance)
                        {
                            vcentre += go.transform.position;
                            groupSize++;

                            if (nDistance < 1.0f)
                            {
                                vavoid = vavoid + (this.transform.position - go.transform.position);
                            }
                            //pega o sxript flock
                            Flock anotherFlock = go.GetComponent<Flock>();

                            //aumenta a vrlocidade
                            gSpeed = gSpeed + anotherFlock.speed;
                        }
                    }
                }
                if (groupSize > 0)
                {
                    vcentre = vcentre / groupSize+(myManager.goalPos-this.transform.position);
                    speed = gSpeed / groupSize;

                    //calcula direçao
                    Vector3 direction = (vcentre + vavoid) - transform.position;
                    if (direction != Vector3.zero)
                        
                        //rotaciona
                        transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(direction),
                            myManager.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
