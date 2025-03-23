using UnityEngine;

public class GoalController : MonoBehaviour
{
    
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est la balle
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            // Vérifie quel but a été touché
            if (gameObject.name == "BlueGoal")
            {
                Debug.Log("But pour l'équipe Orange !");
                gameManager.GoalScored("Orange");
            }
            else if (gameObject.name == "OrangeGoal")
            {
                Debug.Log("But pour l'équipe Bleue !");
                gameManager.GoalScored("Blue");
            }
            
            gameManager.ResetPositions();
        }
    }
}