using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AndyCutscene : MonoBehaviour
{
    private GameObject player;
    private GameObject cutSceneCanvas;
    public GameObject andyPrefab;
    private GameObject andyInstance;

    public GameObject eliPrefab;
    private GameObject eliInstance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cutSceneCanvas = GameObject.FindGameObjectWithTag("CutSceneCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // player walks to middle of screen
    public void Part1()
    {
        // player walk
        AudioManager.Instance().TransitionAudio("");
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        cutSceneCanvas.GetComponent<Animator>().SetBool("PlayCutScene", true);
        GameObject.Find("ClockCanvas").SetActive(false);
        player.GetComponent<PlayerMovement>().QueueMovement(Vector2.right, 5f);
        player.GetComponent<PlayerMovement>().OnMoveQueueCompletion.AddListener(Part2);
    }

    // andy appears and walks to player, start convo
    private void Part2()
    {
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        andyInstance = Instantiate(andyPrefab, new Vector3(30.46f, -121.37f, 0.0f), Quaternion.identity);
        andyInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.down, 1f);
        andyInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(new Vector2(-1.0f, -1.0f), 0.5f);
        andyInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.left, 7f);
        andyInstance.GetComponent<NPCCutsceneMovement>().OnMoveQueueCompletion.AddListener(Part2Point5);
    }

    private void Part2Point5()
    {
        andyInstance.GetComponent<NPCCutsceneMovement>().OnMoveQueueCompletion.RemoveListener(Part2Point5);
        andyInstance.GetComponent<NPCDialogueManager>().StartConversation();
        DialogueDisplay.Instance().OnDialogueCompletion.AddListener(Part3);
    }

    private void Part3()
    {
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToDialogue);
        DialogueDisplay.Instance().OnDialogueCompletion.RemoveListener(Part3);
        StartCoroutine(Part3Helper());
    }

    private IEnumerator Part3Helper()
    {
        andyInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.right, 4f);
        yield return new WaitForSeconds(4.0f);
        andyInstance.GetComponent<NPCCutsceneMovement>().moveSpeed *= 2.0f;
        andyInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.right, 3f);
        andyInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(new Vector2(1.0f, 1.0f), 0.5f);
        andyInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.up, 1f);
        yield return new WaitForSeconds(1.1f);
        Destroy(andyInstance);
        AudioManager.Instance().PlaySound("Doorclose");
        yield return new WaitForSeconds(3.0f);
        Part4();
    }

    // game resumes
    private void Part4()
    {
        eliInstance = Instantiate(eliPrefab, new Vector3(18.12f, -117.04f, 0.0f), Quaternion.identity);
        eliInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.down, 1f);
        eliInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(new Vector2(1.0f, 0.7f), 1f);
        eliInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.down, 1f);
        eliInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(new Vector2(-1.0f, -0.7f), 0.5f);
        eliInstance.GetComponent<NPCCutsceneMovement>().QueueMovement(Vector2.down, 3f);
        eliInstance.GetComponent<NPCCutsceneMovement>().OnMoveQueueCompletion.AddListener(Part42);
    }

    private void Part42()
    {
        transform.Find("flashlight").gameObject.SetActive(true);
        AudioManager.Instance().PlaySound("FlashlightClick");
        StartCoroutine(Part5());
    }

    private IEnumerator Part5()
    {
        yield return new WaitForSeconds(2.0f);
        Camera.main.GetComponentInChildren<FlashlightCutscene>().BeginLerp();
        yield return new WaitForSeconds(6.0f);
        DialogueDisplay.Instance().OnDialogueCompletion.RemoveListener(Part3);
        player.GetComponent<PlayerStateController>().ChangeState(PlayerState.StateChangeInstruction.ChangeToRoam);
        SceneManager.LoadScene("Day2Scene");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Part1();
        }
    }
}
