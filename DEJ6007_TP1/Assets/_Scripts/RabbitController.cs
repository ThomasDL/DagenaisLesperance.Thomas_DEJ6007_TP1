using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le lapin impl�mente une interface qui le d�signe comme ennemi et qui le
// force � int�grer une m�thode qui le d�truit avec une animation simple.
public class RabbitController : MonoBehaviour, IEnemy
{
    protected Rigidbody2D thisRb;
    SpriteRenderer rabbitSr;

    Vector3 startPosition;

    float travelDistance = 4f;
    public float speed;

    // La force de saut du lapin et l'interval de temps entre ses saut.
    float jumpForce = 500f;
    float jumpInterval = 0.7f;
    // Variables li�es � l'animation de destruction du lapin.
    protected float maxDeathAnimationTime = 1f;
    protected float deathAnimationSpeed = 15f;

    [Range(-1, 1)]
    public int moveDirection = -1;
    protected bool isAlive = true;

    bool IEnemy.isAlive { get { return isAlive; } }

    void Start()
    {
        thisRb = GetComponent<Rigidbody2D>();
        rabbitSr = GetComponent<SpriteRenderer>();  
        startPosition = transform.position;
        // Le lapin se met � sauter d�s le d�but.
        StartCoroutine(JumpLoop());
    }


    void FixedUpdate()
    {
        // Si le lapin est en vie...
        // ...et qu'il est rendu plus loin vers la gauche de sa position de d�part
        // que sa distance de d�placement maximale, il change de direction;
        // ...et qu'il d�passe sa position de d�part vers la droite, il change � nouveau de direction.
        // Comme �a, il fait une patrouille simple de gauche � droite.
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
    // Tant qu'il est en vie, il saute! C'est un lapin apr�s tout...
    IEnumerator JumpLoop()
    {
        while (isAlive)
        {
            thisRb.AddForce(Vector3.up * jumpForce);
            yield return new WaitForSeconds(jumpInterval);
        }
    }
    // S'il est tu� par le joueur, le lapin se met � tomber vers le bas en tournoyant puis s'autod�truit.
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
    bool isAlive { get; }
    IEnumerator EnemyDead();
}
