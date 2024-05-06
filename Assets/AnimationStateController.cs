using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    bool Angry = false;
    bool Sad = false;
    bool Happy = false;

    public GameObject AngryButton;
    public GameObject SadButton;
    public GameObject HappyButton;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetState(int state)
    {
        Angry = false;
        Sad = false;
        Happy = false;

        if(state == 0){
            Happy = true;
        } else if(state == 1){
            Sad = true;
        } else if(state == 2){
            Angry = true;
        }
    }

    void DisableMenu(){
        AngryButton.SetActive(false);
        SadButton.SetActive(false);
        HappyButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Angry){
            animator.SetBool("isAngry",true);
        }  
        if (!Angry){
            animator.SetBool("isAngry",false);
        }
         if (Sad){
            animator.SetBool("isSad",true);
        }  
        if (!Sad){
            animator.SetBool("isSad",false);
        }
         if (Happy){
            animator.SetBool("isHappy",true);
        }  
        if (!Happy){
            animator.SetBool("isHappy",false);
        }
    }
}
