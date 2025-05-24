using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Object = UnityEngine.Object;

public class OuvrierDialogue : MonoBehaviour
{
    public float interactionDistance = 5f; // Distance d'interaction
    public List<string> dialoguePhrases = new List<string> { "AAAAAAAAHHHHHH", "UN QWERTIEN A L'AIDE", "HEIN ?", "Oh excusez moi je vous avait pris pour une de ces saletes de qwertiens.", "Excusez moi je suis assez sur les nerfs depuis que la barriere a ete brisee et que les qwertiens attaquent." ,"Vous voyez cette tour ?", "C'est la ou est cache le saphir de vie.", "Vous m'avez l'air d'etre quelqu'un de fort. Pourriez vous eliminer les qwertiens qui tentent de s'en prendre a cette tour ?", "Si ils la detruise l'humanite est perdue.", "Essayez de poser des batiments pour nous aider a defendre la tour!" }; // Phrases du dialogue
    public Text dialogueUI; // R�f�rence au texte UI
    public LayerMask OuvrierLayer; // Calque pour l'ouvrier
    public Camera playerCamera; // R�f�rence � la cam�ra du joueur
    public float textSpeed = 0.05f; // Vitesse d'affichage du texte (en secondes par lettre)
    public GameObject DialoguePanel; // R�f�rence au panel de dialogue

    private GameObject player; // R�f�rence au joueur
    private bool isTyping = false; // Pour v�rifier si le texte est en train de s'afficher
    private int currentPhraseIndex = 0; // Index de la phrase actuelle
    public TableauRcords tableaurecords;

    [Space(10)]
    [Header("missionOrder")]
    [SerializeField]
    public float duree = 1f;
    public Image missionPanel;
    public TextMeshProUGUI missionText;
    public GameObject mission;
    bool isDone = false;
    bool abandon = false;
    bool parle = false;
    
    void Start()
    {
        try
        {
            tableaurecords = (GameObject.Find("Tableaurecords")).GetComponent<TableauRcords>();
            if (!enabled)
                Debug.LogWarning("SCRIPT OuvrierDialogue est desactive !");
            player = GameObject.FindGameObjectWithTag("Player"); // Trouve le joueur par son tag
            dialogueUI.gameObject.SetActive(false); // Cache le texte au d�but
            DialoguePanel.SetActive(false); // Cache le panel au d�but
            missionText.text = "Mission :\nA Villager Want to talk to you !\nPress T to interact with him !";
            if(!tableaurecords.isInfini)StartCoroutine(FadeIn());
        }
        catch (Exception e)
        {
            abandon = true;
            missionPanel.gameObject.SetActive(false);
        }
        
       
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T) && !abandon) // Verifie si le clic gauche est press�
        {
            if (isDone)
            {
                missionText.text = "Mission :\nWin 10 Waves At least and come back to talk.";
                StartCoroutine(FadeIn());
                       
            }
            Debug.Log("ouvrier");
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Rayon au centre de l'�cran
            RaycastHit hit;

            if (IsPlayerCloseEnough() && Physics.Raycast(ray, out hit, interactionDistance, OuvrierLayer)) // V�rifie la distance et le calque
            {
                Ouvrier ouvrier = hit.collider.GetComponent<Ouvrier>();
                Debug.Log(ouvrier);
                if (ouvrier != null && !tableaurecords.isInfini)
                {
                    Debug.Log("ouvrier qui parle");
                    if (!isTyping && currentPhraseIndex < dialoguePhrases.Count) // Si le texte n'est pas en train de s'afficher et qu'il reste des phrases
                    {
                        StartCoroutine(TypeText(dialoguePhrases[currentPhraseIndex])); // D�marre l'affichage de la phrase actuelle
                    }
                    else if (!isTyping && currentPhraseIndex >= dialoguePhrases.Count) // Si toutes les phrases ont �t� affich�es
                    {
                        CloseDialogue(); // Ferme le dialogue
                    }

                    if (isDone)
                    {
                        missionText.text = "Mission :\nWin 10 Waves At least and come back to talk.";
                        StartCoroutine(FadeIn());
                       
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
    public IEnumerator FadeIn()
    {
        float t = 0f;
        Color couleur = missionPanel.color;
        couleur.a = 0f;
        missionPanel.color = couleur;

        while (t < duree)
        {
            t += Time.deltaTime;
            couleur.a = Mathf.Clamp01(t / duree);
            missionPanel.color = couleur;
            yield return null;
        }

        // Optionnel : attendre puis fade-out
        yield return new WaitForSeconds(1f);
    }
    public IEnumerator FadeOut()
    {
        float t = 0f;
        Color couleur = missionPanel.color;
        couleur.a = 1f;
        missionPanel.color = couleur;

        while (t < duree)
        {
            t += Time.deltaTime;
            couleur.a = 1f - Mathf.Clamp01(t / duree);
            missionPanel.color = couleur;
            yield return null;
        }

        missionText.text = "";
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
        missionText.text = "Well Done ! ";
        StartCoroutine(FadeOut());
        isDone = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance); // Dessine une sph�re pour visualiser la distance d'interaction
    }
}
