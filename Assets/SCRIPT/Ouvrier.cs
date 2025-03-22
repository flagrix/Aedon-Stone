using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Ouvrier : MonoBehaviour
{
    public float interactionDistance = 3f; // Distance d'interaction
    public List<string> dialoguePhrases = new List<string> { "Bonjour, aventurier !", "Comment puis-je vous aider ?" }; // Phrases du dialogue
    public Text dialogueUI; // R�f�rence au texte UI
    public LayerMask OuvrierLayer; // Calque pour l'ouvrier
    public Camera playerCamera; // R�f�rence � la cam�ra du joueur
    public float textSpeed = 0.05f; // Vitesse d'affichage du texte (en secondes par lettre)
    public GameObject DialoguePanel; // R�f�rence au panel de dialogue

    private GameObject player; // R�f�rence au joueur
    private bool isTyping = false; // Pour v�rifier si le texte est en train de s'afficher
    private int currentPhraseIndex = 0; // Index de la phrase actuelle

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Trouve le joueur par son tag
        dialogueUI.gameObject.SetActive(false); // Cache le texte au d�but
        DialoguePanel.SetActive(false); // Cache le panel au d�but
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // V�rifie si le clic gauche est press�
        {
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Rayon au centre de l'�cran
            RaycastHit hit;

            if (IsPlayerCloseEnough() && Physics.Raycast(ray, out hit, interactionDistance, OuvrierLayer)) // V�rifie la distance et le calque
            {
                Ouvrier ouvrier = hit.collider.GetComponent<Ouvrier>();
                if (ouvrier != null)
                {
                    if (!isTyping && currentPhraseIndex < dialoguePhrases.Count) // Si le texte n'est pas en train de s'afficher et qu'il reste des phrases
                    {
                        StartCoroutine(TypeText(dialoguePhrases[currentPhraseIndex])); // D�marre l'affichage de la phrase actuelle
                    }
                    else if (!isTyping && currentPhraseIndex >= dialoguePhrases.Count) // Si toutes les phrases ont �t� affich�es
                    {
                        CloseDialogue(); // Ferme le dialogue
                    }
                }
            }
        }
    }

    bool IsPlayerCloseEnough()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= interactionDistance;
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true; // Indique que le texte est en train de s'afficher
        DialoguePanel.SetActive(true); // Active le panel de dialogue
        dialogueUI.gameObject.SetActive(true); // Active le texte UI
        dialogueUI.text = ""; // R�initialise le texte

        foreach (char letter in text.ToCharArray()) // Parcourt chaque lettre du texte
        {
            dialogueUI.text += letter; // Ajoute la lettre au texte affich�
            yield return new WaitForSeconds(textSpeed); // Attend avant d'afficher la lettre suivante
        }

        isTyping = false; // Indique que l'affichage du texte est termin�
        currentPhraseIndex++; // Passe � la phrase suivante
    }

    void CloseDialogue()
    {
        DialoguePanel.SetActive(false); // D�sactive le panel de dialogue
        dialogueUI.gameObject.SetActive(false); // D�sactive le texte UI
        currentPhraseIndex = 0; // R�initialise l'index pour la prochaine conversation
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance); // Dessine une sph�re pour visualiser la distance d'interaction
    }
}
