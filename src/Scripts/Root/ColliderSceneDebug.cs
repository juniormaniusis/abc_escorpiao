using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    /// <summary>
    /// Script de debug que desenha caixas de colisão invisíveis no Scene View.
    /// Desenha apenas os colliders do GameObject que possui este script e seus filhos.
    /// </summary>
    public class ColliderSceneDebug : MonoBehaviour
    {
        [Header("Configurações de Debug")]
        [SerializeField] private bool visualizarBoxColliders = true;
        [SerializeField] private bool visualizarSphereColliders = true;
        [SerializeField] private bool visualizarCapsuleColliders = true;
        [SerializeField] private bool visualizarMeshColliders = true;
        
        [Header("Cores")]
        [SerializeField] private Color corBoxCollider = Color.green;
        [SerializeField] private Color corSphereCollider = Color.blue;
        [SerializeField] private Color corCapsuleCollider = Color.yellow;
        [SerializeField] private Color corMeshCollider = Color.magenta;

        private void OnDrawGizmos()
        {
            // BoxCollider
            if (visualizarBoxColliders)
            {
                DesenharBoxColliders();
            }

            // SphereCollider
            if (visualizarSphereColliders)
            {
                DesenharSphereColliders();
            }

            // CapsuleCollider
            if (visualizarCapsuleColliders)
            {
                DesenharCapsuleColliders();
            }

            // MeshCollider
            if (visualizarMeshColliders)
            {
                DesenharMeshColliders();
            }
        }

        private void DesenharBoxColliders()
        {
            BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider collider in colliders)
            {
                if (!collider.enabled) continue;

                Gizmos.color = corBoxCollider;
                Matrix4x4 matrix = Matrix4x4.TRS(
                    collider.transform.position + collider.center,
                    collider.transform.rotation,
                    collider.transform.lossyScale
                );
                Gizmos.matrix = matrix;
                Gizmos.DrawWireCube(Vector3.zero, collider.size);
            }
        }

        private void DesenharSphereColliders()
        {
            SphereCollider[] colliders = GetComponentsInChildren<SphereCollider>();
            foreach (SphereCollider collider in colliders)
            {
                if (!collider.enabled) continue;

                Gizmos.color = corSphereCollider;
                Vector3 posicao = collider.transform.position + collider.center;
                float raio = collider.radius * Mathf.Max(collider.transform.lossyScale.x, 
                                                          collider.transform.lossyScale.y,
                                                          collider.transform.lossyScale.z);
                Gizmos.DrawWireSphere(posicao, raio);
            }
        }

        private void DesenharCapsuleColliders()
        {
            CapsuleCollider[] colliders = GetComponentsInChildren<CapsuleCollider>();
            foreach (CapsuleCollider collider in colliders)
            {
                if (!collider.enabled) continue;

                Gizmos.color = corCapsuleCollider;
                Vector3 posicao = collider.transform.position + collider.center;
                
                // Desenha uma esfera no topo e outra no fundo
                float altura = collider.height;
                float raio = collider.radius;
                
                Vector3 offset = Vector3.zero;
                switch (collider.direction)
                {
                    case 0: // X-axis
                        offset = Vector3.right * (altura / 2 - raio);
                        break;
                    case 1: // Y-axis
                        offset = Vector3.up * (altura / 2 - raio);
                        break;
                    case 2: // Z-axis
                        offset = Vector3.forward * (altura / 2 - raio);
                        break;
                }

                Vector3 topo = posicao + offset;
                Vector3 fundo = posicao - offset;

                Gizmos.DrawWireSphere(topo, raio);
                Gizmos.DrawWireSphere(fundo, raio);
                Gizmos.DrawLine(topo + Vector3.right * raio, fundo + Vector3.right * raio);
                Gizmos.DrawLine(topo - Vector3.right * raio, fundo - Vector3.right * raio);
            }
        }

        private void DesenharMeshColliders()
        {
            MeshCollider[] colliders = GetComponentsInChildren<MeshCollider>();
            foreach (MeshCollider collider in colliders)
            {
                if (!collider.enabled) continue;

                Gizmos.color = corMeshCollider;
                
                // MeshColliders são mais difíceis de desenhar, então fazemos um bounding box aprox.
                if (collider.sharedMesh != null)
                {
                    Bounds bounds = collider.sharedMesh.bounds;
                    Vector3 escala = collider.transform.lossyScale;
                    Vector3 tamanho = new Vector3(bounds.size.x * escala.x, 
                                                   bounds.size.y * escala.y, 
                                                   bounds.size.z * escala.z);
                    Vector3 centro = collider.transform.position + 
                                   Vector3.Scale(bounds.center, escala);
                    
                    Matrix4x4 matrix = Matrix4x4.TRS(centro, collider.transform.rotation, Vector3.one);
                    Gizmos.matrix = matrix;
                    Gizmos.DrawWireCube(Vector3.zero, tamanho);
                }
            }
        }
    }
}
