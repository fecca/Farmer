using UnityEngine;

namespace ARPG.Moving
{
    public class Raycaster
    {
        private readonly LayerMask _groundLayer = 1 << LayerMask.NameToLayer("Level");

        public bool RaycastLayer(Vector3 position, out RaycastHit raycastHit, LayerMask layer)
        {
            var ray = Camera.main.ScreenPointToRay(position);
            var raycast = Physics.Raycast(ray, out raycastHit, 1000.0f, layer);

            Debug.DrawRay(ray.origin, ray.direction, Color.magenta, 100f);

            return raycast;
        }

        public bool RaycastAllLayers(Vector3 position, out RaycastHit raycastHit)
        {
            var ray = Camera.main.ScreenPointToRay(position);
            var raycast = Physics.Raycast(ray, out raycastHit, 1000.0f);

            Debug.DrawRay(ray.origin, ray.direction, Color.magenta, 100f);

            return raycast;
        }

        public bool RaycastGround(Vector3 position, out RaycastHit raycastHit)
        {
            return RaycastLayer(position, out raycastHit, _groundLayer);
        }

        public bool Raycast(Vector3 from, Vector3 to, out RaycastHit raycastHit)
        {
            var direction = to - from;
            var ray = new Ray(from, direction);
            var raycast = Physics.Raycast(ray, out raycastHit, 1000.0f, _groundLayer);

            Debug.DrawRay(ray.origin, ray.direction, Color.magenta, 100f);

            return raycast;
        }
    }
}