using UnityEngine;

[RequireComponent(typeof(Animator))]

public class UnitAnimator : MonoBehaviour
{
    [Header("Idle animations num")]

    internal Animator animator = null;

    private string newTriggerName = null;
    private string oldTriggerName = null;
    private bool newAnimation = false;
    private string newAnimationName;

    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!animator) Debug.LogError("No animator found inside " + gameObject.name);
    }

    void Update()
    {
        if (newAnimation)
        {
            newAnimation = false;
            animator.Play("Base Layer." + newAnimationName);
        }
    }

    private void ResetAllAnimatorTriggers()
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name);
            }
        }
    }

    //play animation
    public void PlayAnimation(string name)
    {
        newAnimation = true;
        newAnimationName = name;
    }

    //set a trigger in the animator
    public void SetAnimatorTrigger(string name)
    {
        newTriggerName = name;
        if (!newTriggerName.Equals(oldTriggerName))
        {
            if (oldTriggerName != null && oldTriggerName.Equals("Death")) return;
            ResetAllAnimatorTriggers();
            animator.SetTrigger(newTriggerName);
            oldTriggerName = newTriggerName;
        }
    }

    //set a bool in the animator
    public void SetAnimatorBool(string name, bool state)
    {
        animator.SetBool(name, state);
    }

    //set a float in the animator
    public void SetAnimatorFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }
}