using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using DG.Tweening;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandSetPlayerPosition : SequencerCommand
    {
        public void Awake()
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {

                var x = GetParameterAsFloat(0, player.transform.position.x);
                var y = GetParameterAsFloat(1, player.transform.position.y);
                var z = GetParameterAsFloat(2, player.transform.position.z);
                var targetPosition = new Vector3(x, y, z);

                var rotationX = GetParameterAsFloat(3, player.transform.rotation.eulerAngles.x);
                var rotationY = GetParameterAsFloat(4, player.transform.rotation.eulerAngles.y);
                var rotationZ = GetParameterAsFloat(5, player.transform.rotation.eulerAngles.z);
                var targetRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

                player.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InOutQuad)
                    .OnComplete(() =>
                    {
                        Debug.Log("Player position set to: " + targetPosition);
                        // player.GetComponent<KinematicCharacterController.KinematicCharacterMotor>()
                        //         .SetTransientPosition(targetPosition);
                        player.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().SetPositionAndRotation(
                            targetPosition,
                            targetRotation
                        );


                        Stop();
                    });


            }
            else
            {
                Debug.LogWarning("Player GameObject not found. Cannot set position.");
            }

        }



    }

}


/**/
