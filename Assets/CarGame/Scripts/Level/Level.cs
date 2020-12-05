using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public const string DefaultBackgroundSpritePath = "DefaultBackground";

    [Header("Level Identifiers")]
    [SerializeField] Transform m_ObstacleContainer;
    [SerializeField] Transform m_EntranceExitPairContainer;
    [SerializeField] SpriteRenderer m_Background;

    [Header("Level Attributes")]
    [SerializeField] [Range(0f, 10f)] float m_CarSpeed = 1f;
    [SerializeField] [Range(10f, 360f)] float m_CarRotationSpeed = 40f;

    [Header("Object Prefabs")]
    [SerializeField] EntranceExitPair m_EntranceExitPairPrefab;
    [SerializeField] ObstacleBase m_DefaultStaticObstaclePrefab;
    [SerializeField] ObstacleBase m_DefaultMovingObstaclePrefab;
    
    int m_PairArraySize = 8;
    int[] m_PairUsageStatus;
    EntranceExitPair[] m_EntranceExitPairs;

    public float CarSpeed => m_CarSpeed;
    public float CarRotationSpeed => m_CarRotationSpeed;

    /* Picks a random entrance/exit pair.
     * A pair is chosen only once.
     */ 
    public EntranceExitPair NextRandomEntraceExitPair
    {
        get
        {
            int randomIndex = Random.Range(0, m_PairArraySize);
            randomIndex = m_PairUsageStatus.SwapIndices<int>(randomIndex, m_PairArraySize - 1);

            --m_PairArraySize;
            return m_EntranceExitPairs[randomIndex];
        }
    }

    private void ResetUsageStatus()
    {
        m_PairArraySize = m_PairUsageStatus.Length;

        for (int i = 0; i < m_PairArraySize; ++i)
            m_PairUsageStatus[i] = i;
    }

    private void DeactivatePairs()
    {
        for (int i = 0; i < m_EntranceExitPairs.Length; ++i)
            m_EntranceExitPairs[i].gameObject.SetActive(false);
    }

    private void Start()
    {
        m_EntranceExitPairs = GetComponentsInChildren<EntranceExitPair>();
        m_PairUsageStatus = new int[m_EntranceExitPairs.Length];

        ResetUsageStatus();
        DeactivatePairs();
    }
    
    public void Init(Transform obstacleContainer,
                     Transform pointsContainer,
                     SpriteRenderer background)
    {
        m_ObstacleContainer = obstacleContainer;
        m_EntranceExitPairContainer = pointsContainer;
        m_Background = background;

        m_Background.sprite = Resources.Load<Sprite>(DefaultBackgroundSpritePath) as Sprite;
    }

    public void AddEntranceExitPair()
    {
        if(m_EntranceExitPairPrefab != null)
            Instantiate(m_EntranceExitPairPrefab, m_EntranceExitPairContainer);
        else
        {
            GameObject go = new GameObject("EntranceExitPair");
            EntranceExitPair pair = go.AddComponent<EntranceExitPair>();
            pair.transform.SetParent(m_EntranceExitPairContainer);

            GameObject entrancePoint = new GameObject("Entrance");
            entrancePoint.transform.SetParent(pair.transform);
            entrancePoint.AddComponent<SpriteRenderer>();

            GameObject exitPoint = new GameObject("Exit");
            exitPoint.transform.SetParent(pair.transform);
            exitPoint.AddComponent<SpriteRenderer>();
            exitPoint.AddComponent<BoxCollider2D>();

            pair.SetPoints(entrancePoint.transform, exitPoint.transform);
        }
    }

    public void AddObstacle(ObstacleBase newObstacle)
    {
        if (newObstacle == null)
        {
            Debug.LogError("Please feed default obstacle for spawning action");
            return;
        }

        GameObject go = Instantiate(newObstacle, m_ObstacleContainer).gameObject;
    }

    public void AddStaticObstacle() =>
        AddObstacle(m_DefaultStaticObstaclePrefab);

    public void AddMovingObstacle() =>
        AddObstacle(m_DefaultMovingObstaclePrefab);
}
