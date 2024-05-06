using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        bool AngryButton = Input.GetKey("a"); //change for button condition
        bool SadButton = Input.GetKey("s");
        bool HappyButton = Input.GetKey("h");
        if (AngryButton){
            animator.SetBool("isAngry",true);
        }  
        if (!AngryButton){
            animator.SetBool("isAngry",false);
        }
         if (SadButton){
            animator.SetBool("isSad",true);
        }  
        if (!SadButton){
            animator.SetBool("isSad",false);
        }
         if (HappyButton){
            animator.SetBool("isHappy",true);
        }  
        if (!HappyButton){
            animator.SetBool("isHappy",false);
        }
    }
}
