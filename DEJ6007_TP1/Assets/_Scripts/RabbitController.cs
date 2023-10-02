using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour, IEnemy
{
    protected Rigidbody2D thisRb;
    SpriteRenderer rabbitSr;

    Vector3 startPosition;

    float travelDistance = 4f;
    public float speed;
    float jumpForce = 500f;
    float jumpInterval = 0.7f;
    protected float maxDeathAnimationTime = 1f;
    protected float deathAnimationSpeed = 15f;
    public int moveDirection = -1;
    protected bool isAlive = true;

    void Start()
    {
        thisRb = GetComponent<Rigidbody2D>();
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
            thisRb.velocity = new Vector3(speed * moveDirection, thisRb.velocity.y);
        }
    }
    IEnumerator JumpLoop()
    {
        while (isAlive)
        {
            thisRb.AddForce(Vector3.up * jumpForce);
            yield return new WaitForSeconds(jumpInterval);
        }
    }
    IEnumerator IEnemy.EnemyDead()
    {
        thisRb.isKinematic = true;
        thisRb.freezeRotation = false;
        isAlive = false;
        thisRb.velocity = new Vector3(0, -deathAnimationSpeed);
        float deathAnimationTime = 0f;
        while(deathAnimationTime < maxDeathAnimationTime)
        {
            thisRb.rotation += 5f;
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
