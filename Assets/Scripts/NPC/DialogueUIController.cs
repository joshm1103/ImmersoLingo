using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// pure UI renderer
public class DialogueUIController : MonoBehaviour
{
    public TMP_Text dialogueText;          // shows NPC's line
    public TMP_Text hintText;              // shows englishHint
    public TMP_Text feedbackText;          // "Inténtalo de nuevo" message
    public Transform buttonContainer;      // parent object that holds response buttons
    public Button buttonPrefab;            // a simple button prefab to spawn per option

    public Button hintButton;              // button to display the hint
    public Button backButton;              // button to return to the question

    // NPCController subscribes to this to find out which option was picked
    public event Action<int> OnOptionSelected;

    private readonly List<GameObject> spawnedButtons = new List<GameObject>();
    private bool currentNodeHasHint;
    private bool initialized;
    private bool showingWrongFeedback;

    private void Awake()
    {
        Initialize();
    }

    // Safe to call multiple times; only wires up the buttons once
    private void Initialize()
    {
        if (initialized) return;
        initialized = true;

        if (hintButton != null)
            hintButton.onClick.AddListener(() => SetShowingHint(true));
        else
            Debug.LogWarning("DialogueUIController: Hint Button is not assigned.", this);

        if (backButton != null)
            backButton.onClick.AddListener(() => SetShowingHint(false));
        else
            Debug.LogWarning("DialogueUIController: Back Button is not assigned.", this);

        if (hintText == null)
            Debug.LogWarning("DialogueUIController: Hint Text is not assigned.", this);
    }

    // called by NPCController whenever a new node needs to be shown
    public void DisplayNode(DialogueNode node)
    {
        Initialize();

        showingWrongFeedback = false;

        if (node == null)
        {
            Debug.LogError("DialogueUIController: DisplayNode called with a null node.", this);
            return;
        }

        if (dialogueText != null) dialogueText.text = node.spanishLine;
        if (hintText != null) hintText.text = node.englishHint;

        currentNodeHasHint = !string.IsNullOrWhiteSpace(node.englishHint);

        ClearButtons();

        if (node.options != null)
        {
            for (int i = 0; i < node.options.Count; i++)
            {
                int optionIndex = i;
                DialogueOption option = node.options[i];

                Button newButton = Instantiate(buttonPrefab, buttonContainer);
                TMP_Text label = newButton.GetComponentInChildren<TMP_Text>();
                if (label != null) label.text = option.responseText;

                newButton.onClick.AddListener(() => OnOptionSelected?.Invoke(optionIndex));
                spawnedButtons.Add(newButton.gameObject);
            }
        }

        // every new node starts on the question view: options + hint button
        SetShowingHint(false);
    }

    // The single place that decides what is visible.
    // showHint = true  -> English hint + back button only
    // showHint = false -> question + options + hint button (if the node has a hint)
    private void SetShowingHint(bool showHint)
    {
        if (dialogueText != null) dialogueText.gameObject.SetActive(!showHint);
        if (hintText != null) hintText.gameObject.SetActive(showHint);

        foreach (GameObject button in spawnedButtons)
        {
            if (button != null) button.SetActive(!showHint);
        }

        if (hintButton != null) hintButton.gameObject.SetActive(!showHint && currentNodeHasHint);
        if (backButton != null) backButton.gameObject.SetActive(showHint);
        if (feedbackText != null) feedbackText.gameObject.SetActive(!showHint && showingWrongFeedback);
    }

    // removes last conversation's buttons before showing new set
    private void ClearButtons()
    {
        foreach (GameObject button in spawnedButtons)
        {
            if (button != null) Destroy(button);
        }

        spawnedButtons.Clear();
    }
    // wrong answer: show the message and grey out that button so they try another
    public void ShowWrongAnswer(int optionIndex)
    {
        showingWrongFeedback = true;
        SetShowingHint(false);   // make sure we're on the question view

        if (optionIndex >= 0 && optionIndex < spawnedButtons.Count && spawnedButtons[optionIndex] != null)
            spawnedButtons[optionIndex].GetComponent<Button>().interactable = false;
    }
}
