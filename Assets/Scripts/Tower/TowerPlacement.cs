using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public MainTower mainTower;
    public Renderer rangeRenderer;
    public SphereCollider towerCollider;
    private Camera mainCamera;

    private float roadHeight = 1f; // Hauteur du sol
    private bool isTooCloseToRoad = false;

    void Start()
    {
        mainCamera = Camera.main;

        if (mainTower == null || rangeRenderer == null)
        {
            Debug.LogError("Prefab de Tower ou rendu de la portée non assigné !");
            enabled = false;
        }
    }

    void Update()
    {
        MoveTowerToCursor();
        UpdateShaderBasedOnPlacement();

        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceTower();
        }
    }

    // Déplace la Tower à l'emplacement du curseur t'en qu'elle n'ai pas placer
    void MoveTowerToCursor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 hitPoint = ray.GetPoint(rayDistance);
            transform.position = new Vector3(hitPoint.x, roadHeight + towerCollider.radius, hitPoint.z);
        }
    }

    // Met à jour le shader en fonction de l'emplacement de la Tower
    void UpdateShaderBasedOnPlacement()
    {
        isTooCloseToRoad = IsTooCloseToRoad();

        if (isTooCloseToRoad)
        {
            rangeRenderer.material.SetInt("_canPlace", 0);
        }
        else
        {
            rangeRenderer.material.SetInt("_canPlace", 1);
        }
    }

    // Essai de placer la Tower et la place si les conditions de distances sont respecter
    void TryPlaceTower()
    {
        if (CanPlace())
        {
            rangeRenderer.enabled = false;
            mainTower.isPlaced = true;
            Destroy(this);
        }
    }

    // Vérifie si la Tower peut être placée
    bool CanPlace()
    {
        return !isTooCloseToRoad;
    }

    // Vérifie si la Tower est trop proche de la route
    bool IsTooCloseToRoad()
    {
        float minDistance = GetMinDistanceFromRoad();
        return minDistance < 2.2f; // Distance entre la Tower et le centre de la route
    }

    // Obtient la distance minimale de la tower par rapport à la route
    float GetMinDistanceFromRoad()
    {
        float minDist = float.MaxValue;
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector3 waypointPos = waypoints[i].transform.position;
            waypointPos.y = roadHeight;

            Vector3 nextWaypointPos = waypoints[i + 1].transform.position;
            nextWaypointPos.y = roadHeight;

            float dist = DistanceToPath(waypointPos, nextWaypointPos, transform.position);
            minDist = Mathf.Min(minDist, dist);
        }
        return minDist - towerCollider.radius;
    }

    // Calcule la distance entre un point et un chemin défini par deux points
    float DistanceToPath(Vector3 pathStart, Vector3 pathEnd, Vector3 point)
    {
        Vector3 pathDirection = (pathEnd - pathStart).normalized;
        Vector3 pointDirection = point - pathStart;
        float distance = Vector3.Dot(pointDirection, pathDirection);

        distance = Mathf.Clamp(distance, 0f, Vector3.Distance(pathStart, pathEnd));
        Vector3 closestPoint = pathStart + distance * pathDirection;
        float distToPath = Vector3.Distance(point, closestPoint);

        return distToPath;
    }
}

