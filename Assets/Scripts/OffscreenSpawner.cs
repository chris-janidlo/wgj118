using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class OffscreenSpawner<T, TBag> : MonoBehaviour
    where T : MonoBehaviour
    where TBag : BagRandomizer<T>
{
    public Vector2 TimePerSpawnRange;
    public TBag Spawns;
    public bool SpawnAtStart;

    IEnumerator Start ()
    {
        while (true)
        {
            if (SpawnAtStart) StartCoroutine(objectSpawnLoop());
            yield return new WaitForSeconds(RandomExtra.Range(TimePerSpawnRange));
            if (!SpawnAtStart) StartCoroutine(objectSpawnLoop());
        }
    }

    IEnumerator objectSpawnLoop ()
    {
        var spawn = Spawns.GetNext();
        bool success = trySpawnObject(spawn);

        while (!success)
        {
            success = trySpawnObject(spawn);
            yield return null;
        }
    }

    bool trySpawnObject (T spawn)
    {
        var w = CameraCache.Main.pixelWidth;
        var h = CameraCache.Main.pixelHeight;

        bool xIsStuck = RandomExtra.Chance(.5f);
        bool stuckOnUpper = RandomExtra.Chance(.5f);

        Vector2 edgeLocScreen = new Vector2
        (
             xIsStuck ? (stuckOnUpper ? w : 0) : Random.Range(0, w),
            !xIsStuck ? (stuckOnUpper ? h : 0) : Random.Range(0, h)
        );

        Vector2 edgeLocWorld = CameraCache.Main.ScreenToWorldPoint(edgeLocScreen);

        Vector2 cardinal = new Vector2
        (
             xIsStuck ? (stuckOnUpper ? 1 : -1) : 0,
            !xIsStuck ? (stuckOnUpper ? 1 : -1) : 0
        );

        var rad = spawn.GetComponent<CircleCollider2D>().radius;
        Vector2 meteorPos = edgeLocWorld + cardinal * rad * 2;

        if (Physics2D.CircleCast(meteorPos, rad, Vector2.zero)) return false;

        Instantiate(spawn, meteorPos, Quaternion.identity);
        return true;
    }
}
