using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string instruction;
        public string keyPrompt;
        public KeyAction requiredAction;
    }

    public enum KeyAction
    {
        MoveForward,
        MoveBack,
        MoveLeft,
        MoveRight,
        Jump,
        Throw,
        StarAttack,
        Freeze
    }

    public TutorialStep[] steps;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI keyPromptText;
    public TextMeshProUGUI progressText;
    public cat player;
    public Transform dummyTarget;

    private int currentStep = 0;
    private bool stepComplete = false;

    void Start()
    {
        UserInput ui = FindObjectOfType<UserInput>();
        if (ui != null) ui.enabled = false;

        if (PlayerAbilities.instance != null)
            PlayerAbilities.instance.canThrowStars = true;

        ShowStep(0);
    }

    void Update()
    {
        if (currentStep >= steps.Length) return;

        Vector3 direction = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) direction.z += 1;
        if (Keyboard.current.sKey.isPressed) direction.z -= 1;
        if (Keyboard.current.aKey.isPressed) direction.x -= 1;
        if (Keyboard.current.dKey.isPressed) direction.x += 1;
        player.Move(direction);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            player.Jump();

        CheckStepComplete(steps[currentStep].requiredAction);
    }

    void ShowStep(int index)
    {
        if (index >= steps.Length)
        {
            TutorialComplete();
            return;
        }

        stepComplete = false;
        Debug.Log("Showing step: " + index + " - " + steps[index].instruction);
        instructionText.text = steps[index].instruction;
        keyPromptText.text = steps[index].keyPrompt;
        progressText.text = (index + 1) + " / " + steps.Length;
    }

    void CheckStepComplete(KeyAction action)
    {
        if (stepComplete) return;

        bool done = false;

        switch (action)
        {
            case KeyAction.MoveForward:
                done = Keyboard.current.wKey.isPressed;
                break;
            case KeyAction.MoveBack:
                done = Keyboard.current.sKey.isPressed;
                break;
            case KeyAction.MoveLeft:
                done = Keyboard.current.aKey.isPressed;
                break;
            case KeyAction.MoveRight:
                done = Keyboard.current.dKey.isPressed;
                break;
            case KeyAction.Jump:
                done = Keyboard.current.spaceKey.wasPressedThisFrame;
                break;
            case KeyAction.Throw:
                done = Keyboard.current.eKey.wasPressedThisFrame;
                if (done && dummyTarget != null)
                    player.ThrowRegularBall(dummyTarget);
                break;
            case KeyAction.StarAttack:
                done = Keyboard.current.qKey.wasPressedThisFrame;
                if (done && dummyTarget != null)
                    player.ThrowStars(dummyTarget);
                break;
            case KeyAction.Freeze:
                done = Keyboard.current.fKey.wasPressedThisFrame;
                if (done && dummyTarget != null)
                {
                    pig p = dummyTarget.GetComponent<pig>() ?? dummyTarget.GetComponentInParent<pig>();
                    if (p != null) p.Freeze(3f);

                    Penguin pen = dummyTarget.GetComponent<Penguin>() ?? dummyTarget.GetComponentInParent<Penguin>();
                    if (pen != null) pen.Freeze(3f);

                    Corgi c = dummyTarget.GetComponent<Corgi>() ?? dummyTarget.GetComponentInParent<Corgi>();
                    if (c != null) c.Freeze(3f);
                }
                break;
        }

        if (done)
        {
            stepComplete = true;
            currentStep++;
            StartCoroutine(NextStepDelay());
        }
    }

    IEnumerator NextStepDelay()
    {
        yield return new WaitForSeconds(0.5f);
        ShowStep(currentStep);
    }

    void TutorialComplete()
    {
        instructionText.text = "Tutorial Complete!";
        keyPromptText.text = "Get ready...";
        progressText.text = "";
        StartCoroutine(LoadGameDelay());
    }

    IEnumerator LoadGameDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}