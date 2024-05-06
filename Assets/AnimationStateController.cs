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
    
    public float AnnoyDelay;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void isAngry(){
        Debug.Log("Setting trigger to isAngry");
        animator.SetTrigger("isAngry");
    }

    public void isHappy(){
        animator.SetTrigger("isHappy");
    }

    public void isSad(){
        animator.SetTrigger("isSad");
    }

    public void SetState(int state)
    {
        Debug.Log("Setting trigger to issad or happy");
        animator.SetBool("isHappy", false);
        animator.SetBool("isSad", false);
        animator.SetBool("isAngry", false);
        Debug.Log("Setting state from button");

        if (state == 0) {
            animator.SetBool("isHappy", true);
        } else if (state == 1) {
            animator.SetBool("isSad", true);
        } else if (state == 2) {
            animator.SetBool("isAngry", true);
        }
    }

    void DisableMenu(){
        AngryButton.SetActive(false);
        SadButton.SetActive(false);
        HappyButton.SetActive(false);
    }

    void Update()
    {
        if (Time.deltaTime > AnnoyDelay){
            animator.SetBool("isHappy", false);
            animator.SetBool("isSad", false);
            animator.SetBool("isAngry", false);
            animator.SetBool("charAnnoyed",true);
        }
    }
}
