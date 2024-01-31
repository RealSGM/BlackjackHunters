using UnityEngine;

public class CoinFlip : MonoBehaviour
{
    [SerializeField] Animator animator;
    string currentState = "Heads";

    private void Start()
    {
        // Hide coin flip
        // Show hole approaching
        // Stop troops
        // Loop through each troop, bring them forward
        // Make each troop jump over hole
    }

    public void OnButtonPress(string prediction)
    {
        switch (currentState)
        {
            // Flipping from heads
            case "Heads":
                {
                    if (Random.Range(0, 2) == 0) animator.SetTrigger("HeadsToHeads");
                    else
                    {
                        animator.SetTrigger("HeadsToTails");
                        currentState = "Tails";
                    }
                    break;
                }
            // Flipping from Tails
            case "Tails":
                {
                    if (Random.Range(0, 2) == 0) animator.SetTrigger("TailsToTails");
                    else
                    {
                        animator.SetTrigger("TailsToHeads");
                        currentState = "Heads";
                    }
                    break;
                }        
        }

        // Prediction outcome
        if (prediction == currentState) Debug.Log("Well done, you got it correct!");
        else Debug.Log("Better luck next time!");
    }
}
