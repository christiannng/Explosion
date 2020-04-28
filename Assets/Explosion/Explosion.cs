using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float cubeSize = .2f;
    public int cubeInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        //Calculate pivot distance
        cubesPivotDistance = cubeSize * cubeInRow / 2;

        //Use this value to create pivot vector
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Explode();
        }
       
    }

    public void Explode()
    {
        gameObject.SetActive(false);

        //Loop 3 times to create 5x5x5 pieces in x, y, z coordinate
        for (int x = 0; x < cubeInRow; x++)
        {
            for (int y = 0; y < cubeInRow; y++)
            {
                for (int z = 0; z < cubeInRow; z++)
                {
                    CreatePiece(x, y, z);
                }
            }
        }

        //Get explosion position
        Vector3 explosionPos = transform.position;

        //Get collider in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        //Add explostion fource to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            //Get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if(rb != null)
            {
                //Add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    private void CreatePiece(int x, int y, int z)
    {
        //Create piece
        GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //Set piece's postion and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //Add rigidbody and set mass
        Rigidbody pieceRigid = piece.AddComponent<Rigidbody>();
        pieceRigid.mass = cubeSize;

    }
}
