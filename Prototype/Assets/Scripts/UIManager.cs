using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance { get; set; }
    public Slider healthUI;
    public Slider staminaUI;
    public Text displayMessage;

    private PlayerController player;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        healthUI.maxValue = player.maxHealth;
        healthUI.value = healthUI.maxValue;
        staminaUI.maxValue = player.maxStamina;
        staminaUI.value = staminaUI.maxValue;

    }


    void Update()
    {

    }

    public void UpdateHealth(int amount)
    {
        healthUI.value = amount;
    }

    public void UpdateStamina(int amount)
    {
        staminaUI.value = amount;
    }

    public void UpdateDisplayMessage(string message)
    {
        displayMessage.text = message;
    }
}
