using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChickenController : MonoBehaviour
{
    [SerializeField] Transform[] jumpingPoints;
    Transform jumpingPoint;
    Animator animator;

    [SerializeField] LinearProportionConverter jumpingHighCalculator;
    

    string state;
    string facing;

    void Awake()
    {
        state = "idle";
        facing = "right";

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GoToRandomJumpingPoint();
    }

    void Update()
    {
        if(state == "idle")
        {
            if(Random.Range(0, 100) == 0)
            {
                GoToRandomJumpingPoint();
            }
        }    

        CheckFacing();

        if(state == "jumping" && animator.GetBool("flyingUp"))
        {
            CheckFlyingDownState();
        }
    }

    void CheckFlyingDownState()
    {
        if(jumpingPoint.position.y < transform.position.y)
        {
            animator.SetBool("flyingUp", false);
            animator.SetBool("flyingDown", true);
        }
    }

    void CheckFacing()
    {
        if(
            (facing == "left" && transform.localScale.x > 0) ||
            (facing == "right" && transform.localScale.x < 0)
        ) {
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    Transform RandomJumpingPoint()
    {
        return jumpingPoints[Random.Range(0, jumpingPoints.Length)];
    }

    void GoToRandomJumpingPoint()
    {        
        jumpingPoint = RandomJumpingPoint();
        state = "jumping";
        animator.SetBool("flyingUp", true);
        animator.SetBool("flyingDown", false);

        if(jumpingPoint.position.x > transform.position.x)
        {
            facing = "right";
        } else
        {
            facing = "left";
        }

        StartCoroutine("JumpCorroutine");
    }

    IEnumerator JumpCorroutine()
    {
        float count = 0.0f;

        Vector3 actualPosition = transform.position;
        Vector3 jumpingPointPosition = jumpingPoint.position;
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

        print("Jump took: " + (Time.time - time) + " seconds, distance: " + distance + ", jumpingTime: " + jumpingTime);

        state = "idle";
        animator.SetBool("flyingDown", false);
        animator.SetBool("flyingUp", false);
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
}
