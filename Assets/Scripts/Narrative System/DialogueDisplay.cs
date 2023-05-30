using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

public class DialogueDisplay : MonoBehaviour
{
    public GameObject background;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    bool active;
    int lineIndex;
    DialogueConversation currentConvo;

    public UnityEvent OnDialogueCompletion;

    private static DialogueDisplay instance;

    // text lerp in effect variables
    Mesh mesh;
    Vector3[] vertices;
    int counter = 0;
    float xTracker = 0f;
    float shiftDistance = 5f;
    bool typing = false;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    public static DialogueDisplay Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        lineIndex = 0;
        currentConvo = null;

        background.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("next text");
                if (typing)
                {
                    counter = dialogueText.textInfo.characterCount - 1;

                    // make sure to play noise when skipping to end of dialogue
                    if ((counter + 1)% 2 != 0)
                        AudioManager.Instance().PlaySound(currentConvo.GetVoiceSoundName());
                }
                else
                    ShowNextLine();
            }


            if (!typing)
                return;

            dialogueText.ForceMeshUpdate();
            mesh = dialogueText.mesh;
            vertices = mesh.vertices;

            Color[] colors = mesh.colors;

            // keep already typed characters black
            for (int i = 0; i < counter; i++)
            {
                TMP_CharacterInfo completedChar = dialogueText.textInfo.characterInfo[i];

                int completedCharIndex = completedChar.vertexIndex;
                colors[completedCharIndex] = Color.black;
                colors[completedCharIndex + 1] = Color.black;
                colors[completedCharIndex + 2] = Color.black;
                colors[completedCharIndex + 3] = Color.black;
            }

            // lerp in current character
            if (dialogueText.textInfo.characterInfo[counter].isVisible)
            {
                xTracker -= Time.deltaTime * currentConvo.GetTalkSpeed();
                float lerpProgress = xTracker / shiftDistance;

                Vector3 offset = new Vector3(Mathf.Lerp(0, shiftDistance, lerpProgress), 0f, 0f);
                int index = dialogueText.textInfo.characterInfo[counter].vertexIndex;

                // slide character over
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;

                // lerp transparent -> black
                colors[index] = Color.Lerp(Color.black, Color.clear, lerpProgress);
                colors[index + 1] = Color.Lerp(Color.black, Color.clear, lerpProgress);
                colors[index + 2] = Color.Lerp(Color.black, Color.clear, lerpProgress);
                colors[index + 3] = Color.Lerp(Color.black, Color.clear, lerpProgress);

                if (xTracker <= 0)
                {
                    NextCharacter();
                    if (counter % 2 == 0)
                        AudioManager.Instance().PlaySound(currentConvo.GetVoiceSoundName());
                }
            }
            else
                NextCharacter();

            mesh.vertices = vertices;
            mesh.colors = colors;
            dialogueText.canvasRenderer.SetMesh(mesh);
        }
    }

    public void ActivateDisplay(DialogueConversation convo, string portraitAnimatorPath)
    {
        Debug.Log("activatign text display");
        lineIndex = 0;
        currentConvo = convo;
        //if (convo.GetCharacterName() == "")
            // switch to item UI
        nameText.text = convo.GetCharacterName();
        RuntimeAnimatorController animController = Resources.Load(portraitAnimatorPath) as RuntimeAnimatorController;
        if (animController == null)
            Debug.Log("NO ANIMATOR FOUND");
        GetComponent<Animator>().runtimeAnimatorController = animController;
        GetComponent<Animator>().SetBool("play", true);
        background.gameObject.SetActive(true);
        background.GetComponent<Animator>().SetBool("open", true);
        StartCoroutine(BeginText());
    }

    private IEnumerator BeginText()
    {
        yield return new WaitForSeconds(0.5f);
        dialogueText.gameObject.SetActive(true);
        ShowNextLine();
        active = true;
    }

    private void ShowNextLine()
    {
        if (currentConvo.GetDialogueLines().Length - 1 < lineIndex)
        {
            CloseDisplay();
            return;
        }

        typing = true;
        counter = 0;
        dialogueText.color = Color.clear; // need this?
        dialogueText.text = currentConvo.GetDialogueLines()[lineIndex];
        lineIndex++;
    }

    private void NextCharacter()
    {
        counter++;
        if (counter >= dialogueText.textInfo.characterCount)
            typing = false;
        else
            xTracker = shiftDistance;
    }

    private void CloseDisplay()
    {
        background.GetComponent<Animator>().SetBool("open", false);
        dialogueText.gameObject.SetActive(false);
        active = false;
        AudioManager.Instance().PlaySound("DialogueBoxClose");
        StartCoroutine(DelayedClose());
    }

    private IEnumerator DelayedClose()
    {
        yield return new WaitForSeconds(0.4f);
        background.gameObject.SetActive(false);
        OnDialogueCompletion.Invoke();
    }
}
