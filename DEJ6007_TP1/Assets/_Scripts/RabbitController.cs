using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour, IEnemy
{
    Rigidbody2D rabbitRb;
    SpriteRenderer rabbitSr;

    Vector3 startPosition;

    float travelDistance = 4f;
    float speed = 2f;
    float deathAnimationSpeed = 15f;
    float jumpForce = 500f;
    float jumpInterval = 0.7f;
    float maxDeathAnimationTime = 1f;
    int moveDirection = -1;
    bool isAlive = true;

    void Start()
    {
        rabbitRb = GetComponent<Rigidbody2D>();
        rabbitSr = GetComponent<SpriteRenderer>();  
        startPosition = transform.position;
        StartCoroutine(JumpLoop());
    }


    void FixedUpdate()
    {
        if(isAlive)
        {
            if(startPosition.x - transform.position.x > travelDistance && moveDirection == -1)
            {
                moveDirection = 1;
                rabbitSr.flipX = true;
            } else if (transform.position.x - startPosition.x > 0 && moveDirection == 1)
            {
                moveDirection = -1;
                rabbitSr.flipX = false;
            }
            rabbitRb.velocity = new Vector3(speed * moveDirection, rabbitRb.velocity.y);
        }
    }
    IEnumerator JumpLoop()
    {
        while (isAlive)
        {
            rabbitRb.AddForce(Vector3.up * jumpForce);
            yield return new WaitForSeconds(jumpInterval);
        }
    }
    IEnumerator IEnemy.EnemyDead()
    {
        rabbitRb.isKinematic = true;
        rabbitRb.freezeRotation = false;
        isAlive = false;
        rabbitRb.velocity = new Vector3(0, -deathAnimationSpeed);
        float deathAnimationTime = 0f;
        while(deathAnimationTime < maxDeathAnimationTime)
        {
            rabbitRb.rotation += 5f;
            yield return null;
            deathAnimationTime += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
public interface IEnemy
{
    IEnumerator EnemyDead();
}
