using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    GameObject character;
    Animator anim;
    bool target=false; // if character was spawned or not or smthg
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (target!=true){ //switch target for variable that indicates if man has been spawned
            character=GameObject.Find("personPrefab");
            //anim = character.gameObject.GetComponent<Animation>();

        }
    }
}
