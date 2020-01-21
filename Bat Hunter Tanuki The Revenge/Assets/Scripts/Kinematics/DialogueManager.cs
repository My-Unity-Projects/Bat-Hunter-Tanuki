using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Canvas dialogueCanvas;

    public Text nameText;
    public Text sentenceText;

    private Queue<string> sentences;

    Animator dialogueAni;
    public Animator talkingImageAni;
    public Animator transitionAni;

    bool started = false;

    void Start()
    {
        dialogueCanvas.enabled = false;
        sentences = new Queue<string>();
        dialogueAni = GameObject.Find("Dialogue Canvas").GetComponent<Animator>();

        if(SceneManager.GetActiveScene().name == "Kinematic Two")
        {
            talkingImageAni.SetBool("Staler", true);
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Kinematic Two" && started)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                DisplayNextSentence();
                GameObject.FindObjectOfType<AudioManager>().Play("Select Option");
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueCanvas.enabled = true;

        dialogueAni.SetTrigger("Appear");

        nameText.text = dialogue.name;

        sentences.Clear();

        started = true;

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        sentenceText.text = "";
        GameObject.FindObjectOfType<AudioManager>().Play("Type Text");
        foreach (char letter in sentence.ToCharArray())
        {
            sentenceText.text += letter;
            
            yield return null;
        }

        GameObject.FindObjectOfType<AudioManager>().Stop("Type Text");
    }

    public void EndDialogue()
    {
        dialogueAni.SetTrigger("Disappear");

        if(SceneManager.GetActiveScene().name == "Kinematic Two")
        {
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        transitionAni.SetTrigger("End");
        StartCoroutine(FadeSound());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }


    IEnumerator FadeSound()
    {
        Sound s = GameObject.FindObjectOfType<AudioManager>().GetSound("Music");

        while (s.volume > 0f)
        {
            s.volume -= Time.deltaTime / 1.5f;
            s.source.volume = s.volume;
            yield return null;
        }
    }
}
