using UnityEngine;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{
    [AddComponentMenu("")] // Esconde do menu
    public class SequencerCommandFlipPositions : SequencerCommand
    {
        public void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var playerTransform = player.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().Transform;
            var subjectB = GetSubject(1);
            if (player == null || subjectB == null)
            {
                if (DialogueDebug.logWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: FlipPosition() não encontrou '{1}' ou '{2}'.", DialogueDebug.Prefix, GetParameter(0), GetParameter(1)));
                Stop();
                return;
            }

            playerTransform.GetPositionAndRotation(out var originalAPos, out var originalARot);

            subjectB.GetPositionAndRotation(out var originalBPos, out var originalBRot);

            player.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().SetPositionAndRotation(
                originalBPos,
                originalBRot
            );

            Swap(subjectB, originalAPos, originalARot);

            Stop();
        }

        private void Swap(Transform t, Vector3 pos, Quaternion rot)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                rb.MovePosition(pos);
                rb.MoveRotation(rot);
            }
            else
            {
                t.position = pos;
                t.rotation = rot;
            }
        }
    }
}
//required FlipPositions(Pai do André);