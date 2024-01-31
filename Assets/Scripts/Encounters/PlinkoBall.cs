using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoBall : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Vector3 prevPos;
    private readonly List<int> LOR = new List<int>() { -1, 1 };

    public bool hasDropped = false;

    public const float horizontalSpeed = 2f; 
    public const float adjustFactor = 50f;

    [HideInInspector] public string ballType;

    [SerializeField] private EnemyEncounter enemyEncounter;
 

    private void Start()
    {
        enemyEncounter = GameObject.Find("EnemyEncounter(Clone)").GetComponent<EnemyEncounter>();
    }

    //--------------------------------------------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDropped)
        {
            // Check if the ball has hit a peg
            if (collision.collider.name.Contains("Peg"))
            {
                GameObject peg = collision.collider.gameObject;
                StartCoroutine(ExpandPeg(peg));
            }
            
        }
    }

    //--------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Moving ball left and right
        if (!hasDropped)
        {
            switch (collision.name)
            {
                case "L Collider":
                    {
                        rb.velocity = new Vector2(horizontalSpeed, 0);
                        break;
                    }
                case "R Collider":
                    {
                        rb.velocity = new Vector2(-horizontalSpeed, 0);
                        break;
                    }
            }
        }
        // Check if ball has dropped into a drop zone or healing zone
        else
        {
            if (collision.name == "Drop Zone")
            {
                // Get troop
                // Pick a random enemy using range coefficient
                // Show troop attack
                // Damage enemy

                StartCoroutine(BallLanded());

            }
            else if (collision.name == "Healing Zone")
            {
                // Get troop
                // Heal troop
            }
        }
    }

    private void Update()
    {
        if (hasDropped)
        {
            Vector3 currPos = this.gameObject.transform.position;
            if (currPos.x == prevPos.x && currPos.y > prevPos.y)
            {
                // Add force to left or right of the ball
                Debug.Log("It is going up!");
                rb.AddForce(new Vector2(LOR[Random.Range(0,LOR.Count)] * adjustFactor, 0));
            }


            prevPos = this.gameObject.transform.position;
        }    
    }

    //--------------------------------------------------------------------------
    private IEnumerator ExpandPeg(GameObject peg)
    {
        Vector3 _scale = new Vector3(0.1f, 0.1f, 1f);
        peg.transform.localScale = new Vector3(_scale.x * 1.5f, _scale.y * 1.5f, 1);
        yield return new WaitForSeconds(0.1f);
        peg.transform.localScale = new Vector3(_scale.x, _scale.y, 1);

    }



    //--------------------------------------------------------------------------
    public IEnumerator BallLanded()
    {
        if (ballType == "Troop")
        {
           enemyEncounter.SpawnTroopBall();
        }
        else
        {
           enemyEncounter.SpawnEnemyBall();
        }
        rb.sharedMaterial.bounciness = 0.1f;
        yield return new WaitForSeconds(1);

        // Find which strip zone it has landed
        // Deal damage

        Destroy(this.gameObject);
    }

    //--------------------------------------------------------------------------
    public void MoveBall(int dir)
    {
        rb.velocity = new Vector2(horizontalSpeed * dir, 0);
    }
}
