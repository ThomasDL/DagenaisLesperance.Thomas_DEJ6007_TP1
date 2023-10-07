using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le lapin implémente une interface qui le désigne comme ennemi et qui le
// force à intégrer une méthode qui le détruit avec une animation simple.
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
    // Variables liées à l'animation de destruction du lapin.
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
        // Le lapin se met à sauter dès le début.
        StartCoroutine(JumpLoop());
    }


    void FixedUpdate()
    {
        // Si le lapin est en vie...
        // ...et qu'il est rendu plus loin vers la gauche de sa position de départ
        // que sa distance de déplacement maximale, il change de direction;
        // ...et qu'il dépasse sa position de départ vers la droite, il change à nouveau de direction.
        // Comme ça, il fait une patrouille simple de gauche à droite.
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
    // Tant qu'il est en vie, il saute! C'est un lapin après tout...
    IEnumerator JumpLoop()
    {
        while (isAlive)
        {
            thisRb.AddForce(Vector3.up * jumpForce);
            yield return new WaitForSeconds(jumpInterval);
        }
    }
    // S'il est tué par le joueur, le lapin se met à tomber vers le bas en tournoyant puis s'autodétruit.
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
