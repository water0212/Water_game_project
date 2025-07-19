using System;
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
    private List<KeyValuePair<string, Action>> pendingBindings = new List<KeyValuePair<string, Action>>();
    [SerializeField]public bool dialogueIsPlaying { get ; private set; }
    private static DialogManager instance;
    
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAITS_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    public void BindExternalFunction(string functionName, Action fn)
    {
        // 加到 pending
        pendingBindings.Add(new KeyValuePair<string, Action>(functionName, fn));
        // 如果對話已經開始，也立即綁一次
        if (currentStory != null)
            currentStory.BindExternalFunction(functionName, fn);
    }

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
        foreach (var kv in pendingBindings)
            currentStory.BindExternalFunction(kv.Key, kv.Value);
        
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }
    private IEnumerator ExitDialogMode(){
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentStory = null;
        pendingBindings.Clear();
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

        // 1. 如果根本沒有選項，全部隱藏後直接 return
        if (currentChoices.Count == 0){
            for(int i = 0; i < choices.Length; i++){
                choices[i].SetActive(false);
            }
            return;
        }

        // 2. 如果選項太多，只取前面 choices.Length 個
        int count = Mathf.Min(currentChoices.Count, choices.Length);

        // 3. 顯示並設定文字
        for(int i = 0; i < count; i++){
            choices[i].SetActive(true);
            choicesText[i].text = currentChoices[i].text;
        }
        // 4. 多餘的按鈕全部隱藏
        for(int i = count; i < choices.Length; i++){
            choices[i].SetActive(false);
        }

        // 5. 選中第一個（用於鍵盤／手把操作）
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
        ContinueStory();  // 確保選擇之後繼續故事
    } else {
        Debug.LogWarning("Invalid choice index: " + choicesIndex);
    }
    }
}
