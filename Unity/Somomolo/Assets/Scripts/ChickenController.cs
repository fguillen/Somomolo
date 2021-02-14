using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChickenController : MonoBehaviour
{
    [SerializeField] Transform[] jumpingPoints;
    Transform actualJumpingPoint;

    [SerializeField] LinearProportionConverter jumpingHighCalculator;
    

    string state;

    void Awake()
    {
        state = "idle";
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
    }

    Transform RandomJumpingPoint()
    {
        return jumpingPoints[Random.Range(0, jumpingPoints.Length)];
    }

    void GoToRandomJumpingPoint()
    {        
        actualJumpingPoint = RandomJumpingPoint();
        state = "jumping";

        StartCoroutine("JumpCorroutine");
    }

    IEnumerator JumpCorroutine()
    {
        float count = 0.0f;

        Vector3 actualPosition = transform.position;
        Vector3 jumpingPointPosition = actualJumpingPoint.position;
        Vector3 controlPosition = CalculateControlPoint(actualPosition, jumpingPointPosition);
        

        while (count < 1.0f) {
            count += 1.0f * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp( actualPosition, controlPosition, count );
            Vector3 m2 = Vector3.Lerp( controlPosition, jumpingPointPosition, count );
            transform.position = Vector3.Lerp(m1, m2, count);

            yield return null;
        }

        state = "idle";
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
