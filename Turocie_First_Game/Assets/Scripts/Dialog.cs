using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject dialoguePanel;
   // public Animator textDisplayAnim;
    private bool canOpenDialog = false;
    private bool isTyping = false;
    private bool dialogActive = false;
   
    void Start()
    {
        // dialoguePanel.SetActive(false);
        // continueButton.SetActive(false);
        StartCoroutine(Type());
    }

    void Update() {
        // if(textDisplay.text == sentences[index])
        // {
        //     dialoguePanel.SetActive(true);
        //     continueButton.SetActive(true);
        // }
        //  if(index == sentences.Length - 1)
        // {
        //     dialoguePanel.SetActive(false);
        // }


        if (textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }

        if (index == sentences.Length - 1 && textDisplay.text == sentences[index])
        {
            dialoguePanel.SetActive(false);
            continueButton.SetActive(false);
            canOpenDialog = false; // Yeni eklendi
        }
        if (!dialogActive && canOpenDialog && Input.GetKeyDown(KeyCode.E))
        {
            StartDialog();
        }

        if (dialogActive && Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            if (index < sentences.Length - 1)
            {
                NextSentence();
            }
            else
            {
                EndDialog();
            }
        }
    }

    IEnumerator Type()
    {
        isTyping = true; // Yeni eklendi
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false; // Yeni eklendi
    }

    public void NextSentence()
    {
       // textDisplayAnim.SetTrigger("Change");
        continueButton.SetActive(false);

        if (isTyping)
        {
            StopCoroutine(Type()); // Yazıyı tamamlamak için yazma işlemini durdurun.
            textDisplay.text = sentences[index]; // Tamamlanan yazıyı hemen gösterin.
        }

        if(index < sentences.Length -1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }

    public void StartDialog() // Değiştirildi
    {
        dialoguePanel.SetActive(true);
        continueButton.SetActive(false);
        index = 0; // Yeni eklendi
        textDisplay.text = "";
        StartCoroutine(Type());
        canOpenDialog = false; // Yeni eklendi
    }

     public void EndDialog()
    {
        dialoguePanel.SetActive(false);
        continueButton.SetActive(false);
        dialogActive = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Doctor") || collision.gameObject.CompareTag("Boy"))
        {
            canOpenDialog = true; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Doctor") || collision.gameObject.CompareTag("Boy"))
        {
            canOpenDialog = false; 
        }
    }
}
