using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference navigateUpAction;
    [SerializeField] private InputActionReference navigateDownAction;
    [SerializeField] private InputActionReference confirmAction;
    
    [SerializeField] private string sceneToLoad;  // Nom de la scène à lancer pour "Jouer"
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    // Tableau pour regrouper les boutons et gérer la navigation
    private Button[] menuButtons;
    // Index du bouton actuellement sélectionné (0 = Jouer, 1 = Quitter)
    private int currentButtonIndex = 0;

    private void Awake()
    {
        // Initialisation du tableau de boutons, similaire à votre PlayerController si besoin
        menuButtons = new Button[] { playButton, quitButton };
    }

    private void OnEnable()
    {
        // Activation et abonnement des actions
        navigateUpAction.action.performed += OnNavigateUp;
        navigateDownAction.action.performed += OnNavigateDown;
        confirmAction.action.performed += OnConfirm;

        navigateUpAction.action.Enable();
        navigateDownAction.action.Enable();
        confirmAction.action.Enable();

        // Sélection initiale du bouton
        UpdateButtonSelection();
    }

    private void OnDisable()
    {
        // Désabonnement et désactivation des actions
        navigateUpAction.action.performed -= OnNavigateUp;
        navigateDownAction.action.performed -= OnNavigateDown;
        confirmAction.action.performed -= OnConfirm;

        navigateUpAction.action.Disable();
        navigateDownAction.action.Disable();
        confirmAction.action.Disable();
    }

    private void OnNavigateUp(InputAction.CallbackContext context)
    {
        // Passage au bouton précédent (avec gestion du débordement)
        currentButtonIndex = (currentButtonIndex - 1 + menuButtons.Length) % menuButtons.Length;
        UpdateButtonSelection();
    }

    private void OnNavigateDown(InputAction.CallbackContext context)
    {
        // Passage au bouton suivant (avec gestion du débordement)
        currentButtonIndex = (currentButtonIndex + 1) % menuButtons.Length;
        UpdateButtonSelection();
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        ActivateButton();
    }

    // Met à jour la sélection visuelle des boutons
    private void UpdateButtonSelection()
    {
        // On sélectionne le bouton actuellement actif pour le mettre en surbrillance
        menuButtons[currentButtonIndex].Select();
    }

    // Active l’action correspondant au bouton sélectionné
    private void ActivateButton()
    {
        if (currentButtonIndex == 0)
        {
            // Lancer la scène si le nom est défini
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("Le nom de la scène n'est pas défini dans l'inspecteur !");
            }
        }
        else if (currentButtonIndex == 1)
        {
            // Quitter l'application
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
