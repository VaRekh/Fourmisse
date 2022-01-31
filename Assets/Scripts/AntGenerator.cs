using UnityEngine;

namespace Assets.Scripts
{
    [DisallowMultipleComponent]
    public class AntGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject AntTemplate;

        void Start()
        {
            Generate(2);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void Generate(byte count)
        {
            for (byte i = 0; i < count; ++i)
            {
                var ant = Instantiate(AntTemplate, transform.position, transform.rotation);
                var controller = ant.GetComponentInChildren<Controller>();
                controller.Anthill = transform;
            }
        }
    }
}
