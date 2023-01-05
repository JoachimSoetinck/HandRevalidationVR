using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulllet : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public GameObject spawn;
        public Vector3 Direction;
    }

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public GameObject bullet;
    private float elapsedSec;
    public float SpawnTime = 5.0f;

    void Fire()
    {
        if(spawnPoints.Count > 0)
        {
           int random = Random.Range(0, spawnPoints.Count);

            print(random);
            MovingScript mv = bullet.GetComponent<MovingScript>();
            mv.MovementDirection = spawnPoints[random].Direction;

            GameObject bulletClone = (GameObject)Instantiate(bullet, spawnPoints[random].spawn.transform.position, spawnPoints[random].spawn.transform.rotation);
        }
       
       
    }

    void Update()
    {
        elapsedSec += Time.deltaTime;

        if (elapsedSec >= SpawnTime)
        {
            Fire();

           

            elapsedSec = 0;
        }
           
    }
}