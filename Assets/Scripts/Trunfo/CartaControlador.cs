using UnityEngine;

public class CartaControlador : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AtivarAnimacaoComparacao()
    {
        animator.SetTrigger("Comparacao");
    }
}