using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCover : MonoBehaviour
{
    public class CoverPoint
    {
        private Vector3 point;
        private bool valid = false;

        public CoverPoint(Vector3 point)
        {
            this.point = point;
        }

        public void SetPoint(Vector3 value)
        {
            point = value;
        }

        public void SetValid(bool value)
        {
            valid = value;
        }

        public Vector3 GetPoint()
        {
            return point;
        }

        public bool IsValid()
        {
            return valid;
        }
    }

    private CoverPoint[] coverPoints;
    [SerializeField] private Transform player;
    private RaycastHit hit;
    [SerializeField] private LayerMask obstacles;

    void Start()
    {
        coverPoints = new CoverPoint[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            coverPoints[i] = new CoverPoint(transform.GetChild(i).position);
        }
    }

    void Update()
    {
        foreach (CoverPoint cp in coverPoints)
        {
            Ray ray = new Ray(player.position, (cp.GetPoint() - player.position).normalized);

            if (Physics.Raycast(ray, out hit, 1000f, obstacles))
            {
                // If the ray hits this object
                if (ReferenceEquals(hit.collider.gameObject, gameObject)) {
                    cp.SetValid(true);
                }
                else
                {
                    cp.SetValid(false);
                }
            }
        }
    }

    public HashSet<CoverPoint> GetValidCoverPoints()
    {
        HashSet<CoverPoint> output = new HashSet<CoverPoint>();
        foreach (CoverPoint cp in coverPoints)
        {
            if (cp.IsValid())
            {
                output.Add(cp);
            }
        }
        return output;
    }
}
