using UnityEngine;

public class GameWorld : MonoBehaviour
{
    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private float minWorldSideSize = 20;

    [SerializeField]
    private float offsetRatioForSpawnUfo = 0.2f;

    public static GameWorld instance = null;

    public float MinWorldSideSize
    {
        get
        {
            return minWorldSideSize;
        }
    }

    public Vector3 BorderTop
    {
        get
        {
            return borderTop;
        }
    }

    public Vector3 BorderBottom
    {
        get
        {
            return borderBottom;
        }
    }

    public Vector3 BorderRight
    {
        get
        {
            return borderRight;
        }
    }

    public Vector3 BorderLeft
    {
        get
        {
            return borderLeft;
        }
    }

    private Vector3 borderTop, borderBottom, borderRight, borderLeft;

    private float heightArea, widthArea;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
        SetBorder();
    }

    private void Update()
    {
        SetBorder();
    }

    private void SetBorder()
    {
        heightArea = gameCamera.orthographicSize;
        widthArea = gameCamera.aspect * heightArea;

        borderTop = new Vector3(0, transform.position.y + heightArea, 0);
        borderBottom = new Vector3(0, transform.position.y - heightArea, 0);
        borderRight = new Vector3(transform.position.x + widthArea, 0, 0);
        borderLeft = new Vector3(transform.position.x - widthArea, 0, 0);
    }

    public void SetInsideScreenPosition(Transform objectTransform)
    {
        #region воняет
        if (objectTransform.position.y > borderTop.y)
        {
            objectTransform.position += borderBottom * 2;
        }
        if (objectTransform.position.y < borderBottom.y)
        {
            objectTransform.position += borderTop * 2;
        }
        if (objectTransform.position.x > borderRight.x)
        {
            objectTransform.position += borderLeft * 2;
        }
        if (objectTransform.position.x < borderLeft.x)
        {
            objectTransform.position += borderRight * 2;
        }
        #endregion
    }

    public Vector3 RandomAsteroidPosition()
    {
        Vector3 position = Vector3.zero;
        #region воняет
        float interpolationRatio = Random.value;
        int borderIndex = Random.Range(0, 4);

        switch (borderIndex)
        {
            case 0:
                position = Vector3.Lerp(borderTop + borderLeft, borderTop + borderRight, interpolationRatio);
                break;
            case 1:
                position = Vector3.Lerp(borderBottom + borderLeft, borderBottom + borderRight, interpolationRatio);
                break;
            case 2:
                position = Vector3.Lerp(borderRight + borderTop, borderRight + borderBottom, interpolationRatio);
                break;
            case 3:
                position = Vector3.Lerp(borderLeft + borderTop, borderLeft + borderBottom, interpolationRatio);
                break;
        }
        #endregion
        return position;
    }

    public Vector3 RandomUfoPosition(int borderIndex)
    {
        Vector3 position = Vector3.zero;
        #region воняет
        float interpolationRatio = Random.value;

        Vector3 offsetBorderForSpawnUfo = new Vector3(0, heightArea * offsetRatioForSpawnUfo * 2, 0);

        switch (borderIndex)
        {
            case 0:
                position = Vector3.Lerp(borderRight + borderTop - offsetBorderForSpawnUfo, borderRight + borderBottom + offsetBorderForSpawnUfo, interpolationRatio);
                break;
            case 1:
                position = Vector3.Lerp(borderLeft + borderTop - offsetBorderForSpawnUfo, borderLeft + borderBottom + offsetBorderForSpawnUfo, interpolationRatio);
                break;
        }
        #endregion
        return position;
    }
}