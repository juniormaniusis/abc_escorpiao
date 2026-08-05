using Assets.Scripts.GameManager.Assets.Scripts.GameManager;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    public class CreateHollowParallelepiped : MonoBehaviour
    {
        private Vector3 boxSize;

        void Awake()
        {
            boxSize = gameObject.transform.localScale;
            if (gameObject.GetComponent<MeshRenderer>() != null)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            if (gameObject.GetComponent<MeshFilter>() != null)
            {
                gameObject.GetComponent<MeshFilter>().mesh = null;
            }

            if (gameObject.GetComponent<BoxCollider>() != null)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }

        void Start()
        {
            CreateFaces();
        }

        void CreateFaces()
        {
            // Definindo as dimensões
            Vector3 halfSize = boxSize / 2;

            // Criando cada face com o ajuste correto
            CreatePlane(new Vector3(0, halfSize.y, 0), Vector3.up, new Vector2(boxSize.x, boxSize.z));  // Face superior
            CreatePlane(new Vector3(0, -halfSize.y, 0), Vector3.down, new Vector2(boxSize.x, boxSize.z)); // Face inferior

            CreatePlane(new Vector3(0, 0, halfSize.z), Vector3.forward, new Vector2(boxSize.x, boxSize.y));    // Face frontal
            CreatePlane(new Vector3(0, 0, -halfSize.z), Vector3.back, new Vector2(boxSize.x, boxSize.y));   // Face traseira

            CreatePlane(new Vector3(halfSize.x, 0, 0), Vector3.right, new Vector2(boxSize.z, boxSize.y)); // Face direita
            CreatePlane(new Vector3(-halfSize.x, 0, 0), Vector3.left, new Vector2(boxSize.z, boxSize.y)); // Face esquerda
        }

        void CreatePlane(Vector3 position, Vector3 normal, Vector2 size)
        {
            // Criar o plano
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad); // Troquei para Quad, que é mais simples e ajustável
            plane.transform.position = transform.position + position;

            // Orientar o plano na direção correta
            plane.transform.rotation = Quaternion.LookRotation(normal);

            // Ajustar a escala do plano para corresponder ao tamanho da face
            plane.transform.localScale = new Vector3(size.x, size.y, 1);

            // Anexar o plano ao paralelepípedo
            plane.transform.parent = transform;

            // remove o render
            if (plane.GetComponent<MeshRenderer>() != null)
            {
                plane.GetComponent<MeshRenderer>().enabled = false;
            }

            // adiciona um box collider como trigger
            BoxCollider collider = plane.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(1, 1, 1);

            // adiciona um script para interação
            plane.AddComponent<WallBlockCollisionDetector>();
        }
    }
}