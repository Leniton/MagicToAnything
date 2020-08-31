using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TopDown_Movement))]
public class AI_Move : MonoBehaviour
{
    TopDown_Movement Mov;

    [SerializeField] bool moving = false;
    Vector2 goal;
    [SerializeField] GameObject GoalObject;

    void Start()
    {
        Mov = GetComponent<TopDown_Movement>();

        //moving = true;
    }

    private void Update()
    {
        if (!moving) return;

        if (Vector2.Distance(transform.position, goal) > 0.2f)
        {
            Vector2 p = transform.position;
            resetMovement();
            if (Mathf.Abs(p.x - goal.x) > 0.1f)
            {
                if (p.x > goal.x)
                {
                    Mov.left = true;
                }
                else if (p.x < goal.x)
                {
                    Mov.right = true;
                }
            }

            if (Mathf.Abs(p.y - goal.y) > 0.1f)
            {
                if (p.y > goal.y)
                {
                    //print("back");
                    Mov.down = true;
                }
                else if (p.y < goal.y)
                {
                    //print("front");
                    Mov.up = true;
                }
            }
        }
        else
        {
            //print($"goal: {goal}");
            //print($"position: {transform.position}");
            transform.position.Set(goal.x, goal.y, transform.position.z);
            resetMovement();
            moving = false;
            if (GoalObject != null)
            {
                GoalObject = null;
            }
        }
    }

    private void resetMovement()
    {
        Mov.up = false;
        Mov.down = false;
        Mov.left = false;
        Mov.right = false;
    }

    public void SetGoal(Vector2 point)
    {
        goal = point;
        moving = true;
        //print($"Goal set: {goal}");
    }

    public void SetGoal(GameObject obj)
    {
        Vector3 closestSide;
        int x = 0, y = 0;
        if (Mathf.Abs(transform.position.x - obj.transform.position.x) >= Mathf.Abs(transform.position.y - obj.transform.position.y))
        {
            if (transform.position.x > obj.transform.position.x)
            {
                x = 1;
            }
            else if (transform.position.x < obj.transform.position.x)
            {
                x = -1;
            }
        }
        else
        {
            if (transform.position.y > obj.transform.position.y)
            {
                y = 1;
            }
            else if (transform.position.y < obj.transform.position.y)
            {
                y = -1;
            }
        }
        closestSide = new Vector2(x, y);
        moving = true;
        goal = obj.transform.position + closestSide;
        GoalObject = obj;
        //print($"Goal set: {goal}");
    }
}
