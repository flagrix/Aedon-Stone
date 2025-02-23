using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableauRcords : MonoBehaviour
{
    private List<int> RecordSolo = new List<int> { -1, -1, -1, -1, -1, -1, -1 };
    private List<int> RecordDuo = new List<int> { -1, -1, -1, -1, -1, -1, -1 };
    [SerializeField] private Text FirstSolo;
    [SerializeField] private Text SecondSolo;
    [SerializeField] private Text ThirdSolo;
    [SerializeField] private Text FourthSolo;
    [SerializeField] private Text FifthSolo;
    [SerializeField] private Text SixthSolo;
    [SerializeField] private Text SeventhSolo;
    [SerializeField] private Text FirstDuo;
    [SerializeField] private Text SecondDuo;
    [SerializeField] private Text ThirdDuo;
    [SerializeField] private Text FourthDuo;
    [SerializeField] private Text FifthDuo;
    [SerializeField] private Text SixthDuo;
    [SerializeField] private Text SeventhDuo;
    [SerializeField] private Button Histoire;
    [SerializeField] private Button Record;
    [SerializeField] private Button Infini;
    [SerializeField] private GameObject Tableau;
    [SerializeField] private GameObject Fond;
    [SerializeField] private Button Croix;

    public static TableauRcords instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            RecordSolo[i] = PlayerPrefs.GetInt("RecordSolo_" + i, -1); // -1 si aucune valeur enregistrée
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
            instance = this;

    }

    public void NewScoreSolo(int score)
    {
        for (int i = 0; i < 7; i++)
            if (score > RecordSolo[i])
            {
                RecordSolo.RemoveAt(6);
                RecordSolo.Add(score);
                for (int y = 5; y >= i; y--)
                {
                    (RecordSolo[y], RecordSolo[y + 1]) = (RecordSolo[y + 1], RecordSolo[y]);
                }
                PlayerPrefs.SetInt("RecordSolo_" + i, RecordSolo[i]);
                PlayerPrefs.Save(); // Important pour enregistrer sur le disque
                break;
            }
    }
    public void ResetScores()
    {
        for (int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetInt("RecordSolo_" + i, -1);
            PlayerPrefs.SetInt("RecordDuo_" + i, -1);
        }
        PlayerPrefs.Save(); // N'oublie pas pour enregistrer les changements
        Debug.Log("Scores réinitialisés dans PlayerPrefs !");
    }

    public void NewScoreDuo(int score)
    {
        for (int i = 0; i < 7; i++)
            if (score > RecordDuo[i])
            {
                RecordDuo.RemoveAt(6);
                RecordDuo.Insert(i, score);
                break;
            }
    }

    public void AffichageScores()
    {
        ColorBlock colors = Histoire.colors;
        colors.normalColor = new Color(0.3f, 0.3f, 0.3f); // Assombrir les boutons
        Histoire.colors = colors;
        Histoire.interactable = false; // Empêcher les clics
        ColorBlock colors_inf = Infini.colors;
        colors_inf.normalColor = new Color(0.3f, 0.3f, 0.3f); 
        Infini.colors = colors_inf;
        Infini.interactable = false;
        ColorBlock colors_rec = Record.colors;
        colors_rec.normalColor = new Color(0.3f, 0.3f, 0.3f); 
        Record.colors = colors_rec;
        Record.interactable = false;
        Image fondImage = Fond.GetComponent<Image>();
        fondImage.color = new Color(0.3f, 0.3f, 0.3f);
        Tableau.SetActive(true);
        Croix.gameObject.SetActive(true);
        FirstSolo.gameObject.SetActive(true);
        SecondSolo.gameObject.SetActive(true);
        ThirdSolo.gameObject.SetActive(true);
        FourthSolo.gameObject.SetActive(true);
        FifthSolo.gameObject.SetActive(true);
        SixthSolo.gameObject.SetActive(true);
        SeventhSolo.gameObject.SetActive(true);
        FirstDuo.gameObject.SetActive(true);
        SecondDuo.gameObject.SetActive(true);
        ThirdDuo.gameObject.SetActive(true);
        FourthDuo.gameObject.SetActive(true);
        FifthDuo.gameObject.SetActive(true);
        SixthDuo.gameObject.SetActive(true);
        SeventhDuo.gameObject.SetActive(true);
        Debug.Log($"{RecordSolo[0]}");
        if (RecordSolo[0] == -1)
        {
            Debug.Log("Mauvais affichage");
            FirstSolo.text = ".........";
        }
        else
        {
            FirstSolo.text = RecordSolo[0].ToString() + " pts";
            Debug.Log("Bon affichage");
        }
        if (RecordSolo[1] == -1)
            SecondSolo.text = ".........";
        else SecondSolo.text = RecordSolo[1].ToString() +  " pts";
        if (RecordSolo[2] == -1)
            ThirdSolo.text = ".........";
        else ThirdSolo.text = RecordSolo[2].ToString() + " pts";
        if (RecordSolo[3] == -1)
            FourthSolo.text = ".........";
        else FourthSolo.text = RecordSolo[3].ToString() + " pts";
        if (RecordSolo[4] == -1)
            FifthSolo.text = ".........";
        else FifthSolo.text = RecordSolo[4].ToString() + " pts";
        if (RecordSolo[5] == -1)
            SixthSolo.text = ".........";
        else SixthSolo.text = RecordSolo[5].ToString() + " pts";
        if (RecordSolo[6] == -1)
            SeventhSolo.text = ".........";
        else SeventhSolo.text = RecordSolo[6].ToString() + " pts";
        if (RecordDuo[0] == -1)
        {
            FirstDuo.text = ".........";
        }
        else FirstDuo.text = RecordDuo[0].ToString() + " pts";
        if (RecordDuo[1] == -1)
            SecondDuo.text = ".........";
        else SecondDuo.text = RecordDuo[1].ToString() + " pts";
        if (RecordDuo[2] == -1)
            ThirdDuo.text = ".........";
        else ThirdDuo.text = RecordDuo[2].ToString() + " pts";
        if (RecordDuo[3] == -1)
            FourthDuo.text = ".........";
        else FourthDuo.text = RecordDuo[3].ToString() + " pts";
        if (RecordDuo[4] == -1)
            FifthDuo.text = ".........";
        else FifthDuo.text = RecordDuo[4].ToString() + " pts";
        if (RecordDuo[5] == -1)
            SixthDuo.text = ".........";
        else SixthDuo.text = RecordDuo[5].ToString() + " pts";
        if (RecordDuo[6] == -1)
            SeventhDuo.text = ".........";
        else SeventhDuo.text = RecordDuo[6].ToString() + " pts";
    }

    public void Croix_Rouge()
    {
        Tableau.SetActive(false);
        Croix.gameObject.SetActive(false);
        FirstSolo.gameObject.SetActive(false);
        SecondSolo.gameObject.SetActive(false);
        ThirdSolo.gameObject.SetActive(false);
        FourthSolo.gameObject.SetActive(false);
        FifthSolo.gameObject.SetActive(false);
        SixthSolo.gameObject.SetActive(false);
        SeventhSolo.gameObject.SetActive(false);
        FirstDuo.gameObject.SetActive(false);
        SecondDuo.gameObject.SetActive(false);
        ThirdDuo.gameObject.SetActive(false);
        FourthDuo.gameObject.SetActive(false);
        FifthDuo.gameObject.SetActive(false);
        SixthDuo.gameObject.SetActive(false);
        SeventhDuo.gameObject.SetActive(false);
        Histoire.interactable = true;
        ColorBlock colors = Histoire.colors;
        colors.normalColor = Color.white; // Rétablir la couleur d'origine
        Histoire.colors = colors;
        ColorBlock colors_rec = Record.colors;
        colors_rec.normalColor = Color.white; 
        Record.colors = colors_rec;
        Record.interactable = true;
        ColorBlock colors_inf = Infini.colors;
        colors_inf.normalColor = Color.white;
        Infini.colors = colors_inf;
        Infini.interactable = true;
        Image fondImage = Fond.GetComponent<Image>();
        fondImage.color = Color.white;
    }
}
