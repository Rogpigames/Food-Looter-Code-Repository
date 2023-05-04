using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTestScript : MonoBehaviour
{
    private Animator raccoon;

    // Start is called before the first frame update
    void Start()
    {
        raccoon = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
                raccoon.SetBool("Eat_b", true);       
        }
        else
        {
            raccoon.SetBool("Eat_b", false);
        }
    }
}
