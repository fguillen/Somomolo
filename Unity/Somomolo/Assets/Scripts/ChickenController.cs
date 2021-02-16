using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChickenController : MonoBehaviour
{
    Vector3 jumpingPointPosition;
    Vector3 originalPosition;
    Animator animator;
    private IEnumerator jumpCoroutine;

    [SerializeField] LinearProportionConverter jumpingHighCalculator;
    

    string state;
    string facing;

    void Awake()
    {
        SetState("waiting");
        facing = "right";
        originalPosition = transform.position;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(state == "idle")
        {
            if(Random.Range(0, 200) == 0)
            {
                Cheep();
            } else if(Random.Range(0, 50) == 0)
            {
                Bite();
            } else if(Random.Range(0, 100) == 0)
            {
                GoToRandomJumpingPoint();
            }
        }    

        if(state == "jumping" && animator.GetBool("flyingUp"))
        {
            CheckFlyingDownState();
        }
    }

    void CheckFlyingDownState()
    {
        if(jumpingPointPosition.y < transform.position.y)
        {
            animator.SetBool("flyingUp", false);
            animator.SetBool("flyingDown", true);
        }
    }

    void CheckFacing()
    {
        if(jumpingPointPosition.x > transform.position.x)
        {
            facing = "right";
        } else
        {
            facing = "left";
        }

        if(
            (facing == "left" && transform.localScale.x > 0) ||
            (facing == "right" && transform.localScale.x < 0)
        ) {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    Vector3 RandomJumpingPointPosition()
    {
        return BackgroundController.instance.jumpingPoints[Random.Range(0, BackgroundController.instance.jumpingPoints.Length)].position;
    }

    void GoToRandomJumpingPoint()
    {        
        jumpingPointPosition = RandomJumpingPointPosition();
        SetState("jumping");
        GoToJumpingPoint();
    }

    void GoToJumpingPoint()
    {
        animator.SetBool("flyingUp", true);
        animator.SetBool("flyingDown", false);
        
        CheckFacing();

        jumpCoroutine = JumpCorroutine();
        StartCoroutine(jumpCoroutine);
    }

    IEnumerator JumpCorroutine()
    {
        float count = 0.0f;

        Vector3 actualPosition = transform.position;
        Vector3 controlPosition = CalculateControlPoint(actualPosition, jumpingPointPosition);

        float distance = Vector3.Distance(actualPosition, jumpingPointPosition);
        float jumpingTime = distance / 12.0f;
        
        var time = Time.time;

        while (count < 1.0f) {
            count += (1.0f / jumpingTime) * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp( actualPosition, controlPosition, count );
            Vector3 m2 = Vector3.Lerp( controlPosition, jumpingPointPosition, count );
            transform.position = Vector3.Lerp(m1, m2, count);

            yield return null;
        }

        print("Jump took: " + (Time.time - time) + " seconds, distance: " + distance + ", jumpingTime: " + jumpingTime + ", jumpingPointPosition: " + jumpingPointPosition);

        animator.SetBool("flyingDown", false);
        animator.SetBool("flyingUp", false);

        if(state == "recycling")
        {
            Destroy(gameObject);
        } else
        {            
            SetState("idle");
        }
    }

    Vector3 CalculateControlPoint(Vector3 point1, Vector3 point2)
    {
        Vector3[] points = {point1, point2};
        var pointMinY = points.OrderBy(e => e.y).ToArray()[0];
        var pointMaxY = points.OrderBy(e => e.y).ToArray()[1];
        var pointMinX = points.OrderBy(e => e.x).ToArray()[0];
        var pointMaxX = points.OrderBy(e => e.x).ToArray()[1];

        jumpingHighCalculator.dimension2Max = (Mathf.Abs(pointMaxY.x - pointMinY.x) / 2.0f);

        var controlPointY = jumpingHighCalculator.CalculateDimension2Value(pointMaxY.y - pointMinY.y) + pointMaxY.y;
        var controlPointX = ((pointMaxX.x - pointMinX.x)/ 2.0f) + pointMinX.x;

        var controlPoint = new Vector3(controlPointX, controlPointY, 0);

        return controlPoint;
    }

    void Cheep()
    {
        SetState("cheeping");
        animator.SetTrigger("cheep");
    }

    void Bite()
    {
        SetState("biting");
        animator.SetTrigger("bite");
    }

    void AnimationFinished()
    {
        print("AnimationFinished");
        if(state != "recycling")
            SetState("idle");
    }

    public void GoToOriginalPosition()
    {
        jumpingPointPosition = originalPosition;
        animator.enabled = false;
        animator.enabled = true;

        if(jumpCoroutine != null)
        {
            print("Stoping Coroutine");
            StopCoroutine(jumpCoroutine);
        }

        SetState("recycling");
        GoToJumpingPoint();
    }

    public void GoToScene()
    {
        GoToRandomJumpingPoint();
    }

    public void SetSpritesRenderOrder(int renderOrder)
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(); 
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = renderOrder;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(jumpingPointPosition, 1);
    }

    void SetState(string newState)
    {
        print("SetState: " + newState);
        state = newState;
    }
}
