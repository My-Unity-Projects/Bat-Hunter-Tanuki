using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour {

    GameObject menu;
    GameObject aboutSection;
    GameObject controlsSection;

    GameObject[] options;

    GameObject selectedOption;
    int selectedOptionIndex;

    string sceneName;

    Animator transitionAni;
    Animator aboutAni;
    Animator controlsAni;

    bool isAbout;
    bool isControls;

    AudioManager am;

    // Use this for initialization
    void Start () {

        am = GameObject.FindObjectOfType<AudioManager>();

        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            menu = GameObject.Find("Menu");
            options = new GameObject[6];

            aboutSection = GameObject.Find("About Section");
            controlsSection = GameObject.Find("Controls Section");

            aboutAni = aboutSection.GetComponent<Animator>();
            controlsAni= controlsSection.GetComponent<Animator>();

            aboutSection.SetActive(false);
            controlsSection.SetActive(false);
        }

        else
        {
            menu = GameObject.Find("Death Menu");
            options = new GameObject[2];
        }

        GetOptions();

        selectedOption = options[0];
        selectedOptionIndex = 0;

        sceneName = "SampleScene";

        transitionAni = GameObject.Find("Transition Canvas").transform.GetChild(0).GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if(!isAbout && !isControls)
        {
            SelectOption();
            ChangeOption();
            TriggerOption();
        }

        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CloseSection();
        }
	}


    public void GetOptions()
    {
        for(int i = 0; i < options.Length; i++)
        {
            options[i] = menu.transform.GetChild(i + 1).gameObject;
        }
    }


    public void SelectOption()
    {
        for(int i = 0; i < options.Length; i++)
        {
            if(options[i] == selectedOption)
            {
                options[i].GetComponent<Animator>().enabled = true;
                options[i].transform.GetChild(1).gameObject.SetActive(true);

                if(options[i].tag == "Button")
                {
                    options[i].transform.GetChild(0).GetComponent<Text>().color = new Color(0.8679245f, 0.7265431f, 0, 1);
                }
            }

            else
            {
                options[i].GetComponent<Animator>().enabled = false;
                options[i].transform.GetChild(1).gameObject.SetActive(false);


                if (options[i].tag == "Button")
                {
                    options[i].transform.GetChild(0).GetComponent<Text>().color = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1);
                }
            }
        }
    }


    public void ChangeOption()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            am.Play("Change Option");
            selectedOptionIndex++;

            if(selectedOptionIndex == options.Length)
            {
                selectedOptionIndex = 0;
            }

            selectedOption = options[selectedOptionIndex];
        }
    }
   

    public void TriggerOption()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            string optionName = selectedOption.name;

            am.Play("Select Option");

            if (optionName == "Play Button")
            {
                sceneName = "Kinematic One";
                StartCoroutine(LoadScene());
            }

            else if(optionName == "Controls Button")
            {
                aboutSection.SetActive(false);

                isControls = true;
                controlsSection.SetActive(true);
                controlsAni.SetTrigger("Appear");

                menu.SetActive(false);
            }

            else if(optionName == "About Button")
            {
                controlsSection.SetActive(false);

                isAbout = true;
                aboutSection.SetActive(true);
                aboutAni.SetTrigger("Appear");
               
                menu.SetActive(false);
            }

            else if(optionName == "Exit Button")
            {
                Application.Quit();
            }

            else if(optionName == "YouTube Button")
            {
                Application.OpenURL("https://www.youtube.com/channel/UCqb1lPfba-OI3xX0rHBPOCA?view_as=subscriber");
            }

            else if(optionName == "Twitter Button")
            {
                Application.OpenURL("https://twitter.com/StalerMax");
            }

            else if(optionName == "Restart Button")
            {
                sceneName = "SampleScene";
                StartCoroutine(LoadScene());
            }

            else if(optionName == "MainMenu Button")
            {
                sceneName = "MainMenu";
                StartCoroutine(LoadScene());
            }
        }
    }


    public void CloseSection()
    {
        menu.SetActive(true);

        aboutAni.SetTrigger("Disappear");
        controlsAni.SetTrigger("Disappear");

        isAbout = false;
        isControls = false;
    }


    IEnumerator LoadScene()
    {
        transitionAni.SetTrigger("End");
        StartCoroutine(FadeSound());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeSound()
    {
        Sound s = am.GetSound("Music");

        while(s.volume > 0f)
        {
            s.volume -= Time.deltaTime / 1.5f;
            s.source.volume = s.volume;
            yield return null;
        }
    }
}
