using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditPlayer : MonoBehaviour
{
    private Animator animator;
    public GameObject mainmenu;

    void Start()
    {
        animator = GetComponent<Animator>() as Animator;
    }


    void Update()
    {
        AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);

        if (!asi.IsName("Anim_Credits") || asi.normalizedTime >= 1)
        {
            gameObject.SetActive(false);
            mainmenu.SetActive(true);
            Destroy(gameObject);
        }
    }
}