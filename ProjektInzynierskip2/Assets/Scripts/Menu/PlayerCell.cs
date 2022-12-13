using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PlayerCell : MonoBehaviour
{
    [SerializeField] private Button removeBtn;
    [SerializeField] private TMP_InputField inputField;
    [HideInInspector] public MenuSystem menuSystem;

    public bool IsReadyToPlay()
    {
        return !string.IsNullOrEmpty(inputField.text);
    }

    public void Setup(MenuSystem menuSystem, UnityAction removeButtonListener,bool isRemoveButtonActive=true)
    {
        this.menuSystem = menuSystem;
        removeBtn.onClick.AddListener(removeButtonListener);
        inputField.onDeselect.AddListener((text) => { menuSystem.HandlePlayButton(); });
        inputField.onEndEdit.AddListener((text) => { menuSystem.HandlePlayButton(); });
        //inputField.onSu.AddListener((text) => { menuSystem.HandlePlayButton(); });

        if (!isRemoveButtonActive)
            removeBtn.gameObject.SetActive(isRemoveButtonActive);
    }

    public string GetName()
    {
        return inputField.text;
    }
}
