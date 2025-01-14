using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpMessages : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI message;
   public void SetUpMessage(string message)
   {
        this.message.text = message;
   }

   private void OnEnable() {
      GameManager.Instance.GetPlayerController().HaltEverything();
   }
   private void OnDisable() {
      GameManager.Instance.GetPlayerController().UnHaltEverything();
   }
}
