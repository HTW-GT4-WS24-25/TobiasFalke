using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    private  float SceneHeight = 10;
    private  float SceneWidth = 15;
    private float _objectFallSpeed = 5f;

    private void Start()
    {
        Camera cam = Camera.main;

         // Get the camera's height and width in world units
         SceneHeight = cam.orthographicSize * 2;
         SceneWidth = SceneHeight * cam.aspect;
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            _objectFallSpeed = GameManager.Instance.gameSpeed * 5 ;
        }
    }
        
    protected void SpawnObject(GameObject objectPrefab, Transform objectParent)
    {
        var randomX = Random.Range(-SceneWidth / 2, SceneWidth / 2);
        var randomPosition = new Vector2(randomX, SceneHeight);
        var spawningObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity, objectParent);
        // var objectRb = spawningObject.GetComponent<Rigidbody2D>();
        // objectRb.linearVelocity = new Vector2(0f, -_objectFallSpeed);
    }
}