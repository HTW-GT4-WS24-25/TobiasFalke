using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private const float SceneHeight = 10;
    private const float SceneWidth = 15;
    private float _objectFallSpeed = 5f;
    
    private void Update()
    {
        if (GameModel.Instance.IsPlaying)
        {
            _objectFallSpeed = GameModel.Instance.ScrollSpeed;
        }
    }
        
    protected void SpawnObject(GameObject objectPrefab, Transform objectParent)
    {
        var randomX = Random.Range(-SceneWidth / 2, SceneWidth / 2);
        var randomPosition = new Vector2(randomX, SceneHeight);
        var spawningObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity, objectParent);
        var objectRb = spawningObject.GetComponent<Rigidbody2D>();
        objectRb.linearVelocity = new Vector2(0f, 1f * -_objectFallSpeed);
    }
}