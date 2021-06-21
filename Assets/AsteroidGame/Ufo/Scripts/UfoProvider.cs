using System.Collections;
using UnityEngine;

public class UfoProvider : MonoBehaviour
{
    [SerializeField]
    private Ufo ufo;
    [SerializeField]
    private float minSapwnDelay = 20f;
    [SerializeField]
    private float maxSapwnDelay = 40f;

    private IEnumerator coroutineSpawnUfoDelay;

    public void Init()
    {
        ufo.OnExplode += StartSpawnUfoDelay;
        coroutineSpawnUfoDelay = SpawnUfoDelay();
        StartCoroutine(coroutineSpawnUfoDelay);
    }

    private void StartSpawnUfoDelay()
    {
        ufo.OnExplode -= StartSpawnUfoDelay;
        Init();
    }

    private IEnumerator SpawnUfoDelay()
    {
        float deleyRatio = Random.value;
        float deley = Mathf.Lerp(minSapwnDelay, maxSapwnDelay, deleyRatio);
        yield return new WaitForSeconds(deley);
        ufo.Init();
    }

    public void StopSpawn()
    {
        ufo.OnExplode -= StartSpawnUfoDelay;
        if(coroutineSpawnUfoDelay != null)
        {
            StopCoroutine(coroutineSpawnUfoDelay);
        }
    }

    public void DeleteUfo()
    {
        ufo.gameObject.SetActive(false);
    }
}
