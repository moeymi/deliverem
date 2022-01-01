using UnityEngine;

public class ZuAnimation : MonoBehaviour
{
    #region Attributes
    Animator animator;
    bool moving;
    bool makingAction = false;
    Action action;
    Vector2 destination;
    float actionTime;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        actionTime = 0.52f;
    }
    public void Act(Vector2Int destination, Action action = Action.Move)
    {
        if (moving || makingAction)
            return;
        this.action = action;
        moving = true;
        this.destination.x = destination.x + 0.5f;
        this.destination.y = destination.y + 0.5f;
        Vector2 currentPos = transform.position;
        if (this.destination.x - currentPos.x < -0.5f)
            ToLeft();
        else if (this.destination.x - currentPos.x > 0.5f)
            ToRight();
        else if (this.destination.y - currentPos.y < -0.5f)
            ToDown();
        else if (this.destination.y - currentPos.y > 0.5f)
            ToUp();
    }

    void Update()
    {
        //KeyBoard();
        if (moving)
        {
            Vector2 currentPos = transform.position;
            if(Mathf.Abs(Vector2.Distance(currentPos, destination)) <= 0.01 )
            {
                Stop();
                moving = false;
                if (action != Action.Move)
                {
                    if (action == Action.Pickup)
                        Pickup();
                    else if (action == Action.Deliver)
                        Deliver();
                    makingAction = true;
                }
            }
            else
            {
                Vector2 nextPosition = Vector2.MoveTowards(currentPos, destination, Time.deltaTime * 2);
                transform.localPosition = nextPosition;
            }
        }
        else if (makingAction)
        {
            actionTime -= Time.deltaTime;
            string clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if(actionTime <= 0)
            {
                Debug.Log("wa2el");
                makingAction = false;
                actionTime = 0.52f;
            }
        }
    }

    void KeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ToLeft();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ToDown();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ToRight();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ToUp();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Stop();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Pickup();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Deliver();
        }
    }

    void ToRight()
    {
        animator.SetBool("up", false);
        animator.SetBool("side", true);
        animator.SetBool("walking", true);
        transform.localScale = new Vector3(-1, 1, 1);
    }

    void ToLeft()
    {
        animator.SetBool("up", false);
        animator.SetBool("side", true);
        animator.SetBool("walking", true);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void ToUp()
    {
        animator.SetBool("up", true);
        animator.SetBool("side", false);
        animator.SetBool("walking", true);
        transform.localScale = new Vector3(1, 1, 1);
    }
    void ToDown()
    {
        animator.SetBool("up", false);
        animator.SetBool("side", false);
        animator.SetBool("walking", true);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void Pickup()
    {
        animator.SetTrigger("pickup");
    }

    void Deliver()
    {
        animator.SetTrigger("deliver");
    }

    void Stop()
    {
        animator.SetBool("walking", false);
    }

}

