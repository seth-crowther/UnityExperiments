using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCover : MonoBehaviour
{
    public class CoverPoint
    {
        private Vector3 point;
        private bool valid = false;
        private bool coverDir = false;

        public CoverPoint(Vector3 point, bool coverDir)
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

        public bool GetCoverDir()
        {
            return coverDir;
        }

        public static bool ParseCoverDir(Transform coverPoint, Transform parent)
        {
            return Vector3.Dot(coverPoint.forward, parent.right) >= 0;
        }
    }

    private CoverPoint[] coverPoints;
    [SerializeField] private Transform gunOrigin;
    private RaycastHit hit;
    [SerializeField] private LayerMask obstacles;

    void Start()
    {
        coverPoints = new CoverPoint[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            coverPoints[i] = new CoverPoint(transform.GetChild(i).position, CoverPoint.ParseCoverDir(transform.GetChild(i), transform));
        }

        DoCoverPointValidity();
    }

    void Update()
    {
        DoCoverPointValidity();
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

    public void DoCoverPointValidity()
    {
        foreach (CoverPoint cp in coverPoints)
        {
            Ray ray = new Ray(gunOrigin.position, (cp.GetPoint() - gunOrigin.position).normalized);

            if (Physics.Raycast(ray, out hit, 1000f, obstacles))
            {
                // If the ray hits this object
                if (ReferenceEquals(hit.collider.gameObject, gameObject))
                {
                    cp.SetValid(true);
                }
                else
                {
                    cp.SetValid(false);
                }
            }
        }
    }
}
