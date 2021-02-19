using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselHorseBarController : MonoBehaviour
{

    [SerializeField] float speed;

    void Update()
    {
        if(transform.localScale.x < 0)
        {
            MoveToLeftPoint();
        } else 
        {
            MoveToRightPoint();
        }
    }


    void MoveToRightPoint()
    {
        var x = transform.position.x + (speed * Time.deltaTime);

        if(x >= CarouselController.instance.limitRight.position.x){
            x = CarouselController.instance.limitRight.position.x;
            Flip();
        }

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void MoveToLeftPoint()
    {
        var x = transform.position.x - (speed * Time.deltaTime);

        if(x <= CarouselController.instance.limitLeft.position.x){
            x = CarouselController.instance.limitLeft.position.x;
            Flip();
        }

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void Flip()
    {
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sortingOrder = spriteRenderer.sortingOrder > 0 ? spriteRenderer.sortingOrder - 3 : spriteRenderer.sortingOrder + 3; 
        }
    }
}
