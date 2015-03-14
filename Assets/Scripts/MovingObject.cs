using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
    // Abstract means that we can create incomplete classes that must be completed in the implementation

    public float moveTime = 0.1f;
    // Checking collisions
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    // Use this for initialization
    protected virtual void Start()
    {
        // protected virtual classes can be overridden
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;

    }
    // returning more than one value by using out (pointing to reference)
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        // Make sure we don't hit our own collider when casting ray
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    // Generic parameter T used to specify type of component we expect to interact with. 
    // With enemies -> player, with player -> walls
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        // since hit is an out-parameter allows us to check if the transform we checked in Move was null
        if (hit.transform == null)
            return;

        // get component reference to T attached to object that was hit
        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    // smooth movement coroutine
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // sqrMagnitude computationally cheaper
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            // Recalculate remaining distance after movement
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            // Wait for a frame before re-evaluating condition of loop
            yield return null;


        }
    }

    // smooth movement coroutine
    protected IEnumerator SmoothMovementThroughWalls(Vector3 end)
    {

        Debug.Log("Moving through walls");
        // sqrMagnitude computationally cheaper
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            // Recalculate remaining distance after movement
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            // Wait for a frame before re-evaluating condition of loop
            yield return null;

        }

    }
    protected abstract void OnCantMove<T>(T component)
        where T : Component;


}
