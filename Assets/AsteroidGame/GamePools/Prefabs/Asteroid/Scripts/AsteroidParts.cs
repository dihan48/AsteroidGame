using System.Collections.Generic;
using UnityEngine;

public class AsteroidParts : MonoBehaviour
{
    [SerializeField]
    protected float minPartSpeed = 1f;

    [SerializeField]
    protected float maxPartSpeed = 6f;

    [SerializeField]
    protected float spreadAngle = 90f;

    private List<AsteroidProvider> asteroidProviders = new List<AsteroidProvider>();

    public delegate void OnExplod(AsteroidParts asteroidParts);
    public event OnExplod onAllClear;

    public void Init(Vector2 totalDirection, Vector3 partsStartPosition)
    {
        asteroidProviders.Clear();
        asteroidProviders.AddRange(this.ExtGetComponentsInChild<AsteroidProvider>());

        float partsSpeed = Random.Range(minPartSpeed, maxPartSpeed);
        float[] partsAngles = PartsAngles(asteroidProviders.Count);

        for (int i = 0; i < asteroidProviders.Count; i++)
        {
            asteroidProviders[i].onClear += RemoveProvider;
            asteroidProviders[i].Init(partsSpeed, totalDirection.Rotate(partsAngles[i]), partsStartPosition);
        }

        gameObject.SetActive(true);
    }

    private void RemoveProvider(AsteroidProvider asteroidProvider)
    {
        asteroidProvider.onClear -= RemoveProvider;
        asteroidProviders.Remove(asteroidProvider);

        if (asteroidProviders.Count == 0)
        {
            onAllClear?.Invoke(this);
        }
    }

    private float[] PartsAngles(int countParts)
    {
        float startAngle = 360 - spreadAngle / 2;
        float stepAngle = spreadAngle / (countParts - 1);

        float[] angles = new float[countParts];

        if (angles.Length > 0)
        {
            angles[0] = startAngle;
        }

        for (int i = 1; i < angles.Length; i++)
        {
            angles[i] = angles[i - 1] + stepAngle;
        }

        return angles;
    }
}
