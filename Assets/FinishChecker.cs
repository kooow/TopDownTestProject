using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishChecker : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tags.Player.ToString()))
        {

            var playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.StopAndDisableMoveAndShoot();

            MenuController.Instance.ShowCongratulationMenu();

        }
    }

}
