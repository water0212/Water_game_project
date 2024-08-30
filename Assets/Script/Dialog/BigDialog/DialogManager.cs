using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO.Compression;

public class DialogManager : MonoBehaviour
{
    [Header("Dialog UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI SpeakerName;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;
    [Header("Choice UI")]
    [SerializeField] private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    [SerializeField]public bool dialogueIsPlaying { get ; private set; }
    private static DialogManager instance;
    
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAITS_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private void Awake() {
        if(instance == null){
            Debug.LogWarning("Found more than one Dialog Manager in the Scene");
        }
         instance = this;
    }
    private void Start() {
        dialoguePanel.SetActive(false);
        dialogueIsPlaying = false;
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        choicesText =  new TextMeshProUGUI[choices.Length];
        int index = 0 ;
        foreach (GameObject choice in choices){
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

    }
    private void Update() {
        if(!dialogueIsPlaying){
            return;
        }

        if(InputManager.GetInstance().GetSubmitPressed()){
            ContinueStory();
        }
    }
    public static DialogManager GetInstance() {
        return instance;
    }
    public void EnterDialogMode(TextAsset inkJSON){
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }
    private IEnumerator ExitDialogMode(){
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
    private void ContinueStory(){
        if(currentStory.canContinue){
            dialogueText.text = currentStory.Continue();
            DisPlayChoices();
            HandleTag(currentStory.currentTags);
        }else{
            StartCoroutine(ExitDialogMode());
        }
    }
    private void HandleTag(List<string> currentTags){

        foreach (string tag in currentTags){

            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2){
                Debug.Log("Tag could not be appropriately parsed" + tag);
            }
            string tagKay = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKay){
                case SPEAKER_TAG:
                SpeakerName.text = tagValue;
                break;
                case PORTRAITS_TAG:
                portraitAnimator.Play(tagValue);
                break;
                case LAYOUT_TAG:
                layoutAnimator.Play(tagValue);
                break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
    private void DisPlayChoices(){
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length){
            Debug.LogWarning("太多選擇要放了");
        }
        int index = 0;

        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for( int i =index; i<choices.Length; i++){
            choices[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice(){
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    public void MakeChoice(int choicesIndex){
        Debug.Log("logtest");
    Debug.Log("Choices Index: " + choicesIndex + " / Total Choices: " + currentStory.currentChoices.Count);
    
    if (choicesIndex >= 0 && choicesIndex < currentStory.currentChoices.Count) {
        currentStory.ChooseChoiceIndex(choicesIndex);
        //ContinueStory();  // 確保選擇之後繼續故事
    } else {
        Debug.LogWarning("Invalid choice index: " + choicesIndex);
    }
    }
}
