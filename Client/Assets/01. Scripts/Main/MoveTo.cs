using UnityEngine;

namespace Main
{
    public class MoveTo : MonoBehaviour
    {
        [SerializeField] Vector3 dir;

        private void Update()
        {
            DoMove();
        }

        private void DoMove()
        {
            transform.Translate(dir * Time.deltaTime);
        }
    }
}
