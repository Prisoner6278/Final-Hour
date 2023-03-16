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
    public TMP_Text text;

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
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (typing)
                    counter = text.textInfo.characterCount - 1;
                else
                    ShowNextLine();
            }
                

            if (!typing)
                return;

            text.ForceMeshUpdate();
            mesh = text.mesh;
            vertices = mesh.vertices;

            Color[] colors = mesh.colors;

            // keep already typed characters black
            for (int i = 0; i < counter; i++)
            {
                TMP_CharacterInfo completedChar = text.textInfo.characterInfo[i];

                int completedCharIndex = completedChar.vertexIndex;
                colors[completedCharIndex] = Color.black;
                colors[completedCharIndex + 1] = Color.black;
                colors[completedCharIndex + 2] = Color.black;
                colors[completedCharIndex + 3] = Color.black;
            }

            // lerp in current character
            if (text.textInfo.characterInfo[counter].isVisible)
            {
                xTracker -= Time.deltaTime * currentConvo.GetTalkSpeed();
                float lerpProgress = xTracker / shiftDistance;

                Vector3 offset = new Vector3(Mathf.Lerp(0, shiftDistance, lerpProgress), 0f, 0f);
                int index = text.textInfo.characterInfo[counter].vertexIndex;

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
                    AudioManager.Instance().PlaySound(currentConvo.GetVoiceSoundName());
                    NextCharacter();
                }
            }
            else
                NextCharacter();

            mesh.vertices = vertices;
            mesh.colors = colors;
            text.canvasRenderer.SetMesh(mesh);
        }
    }

    public void ActivateDisplay(DialogueConversation convo, string portraitAnimatorPath)
    {
        active = true;
        lineIndex = 0;
        currentConvo = convo;
        RuntimeAnimatorController animController = Resources.Load(portraitAnimatorPath) as RuntimeAnimatorController;
        if (animController == null)
            Debug.Log("NO ANIMATOR FOUND");
        GetComponent<Animator>().runtimeAnimatorController = animController;
        GetComponent<Animator>().SetBool("play", true);
        background.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        ShowNextLine();
    }

    private void ShowNextLine()
    {
        if (currentConvo.GetDialogueLines().Length - 1 < lineIndex)
        {
            CloseDisplay();
            OnDialogueCompletion.Invoke();
            return;
        }

        typing = true;
        counter = 0;
        text.color = Color.clear; // need this?
        text.text = currentConvo.GetDialogueLines()[lineIndex];
        lineIndex++;
    }

    private void NextCharacter()
    {
        counter++;
        if (counter >= text.textInfo.characterCount)
            typing = false;
        else
            xTracker = shiftDistance;
    }

    private void CloseDisplay()
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        active = false;
    }
}