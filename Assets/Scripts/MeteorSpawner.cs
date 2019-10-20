using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class MeteorSpawner : MonoBehaviour
{
    [System.Serializable]
    public class OrbiterBag : BagRandomizer<Orbiter> {}

    public float TimePerSpawn;
    public OrbiterBag Spawns;

    IEnumerator Start ()
    {
        while (true)
        {
            StartCoroutine(meteorSpawnLoop());
            yield return new WaitForSeconds(TimePerSpawn);
        }
    }

    IEnumerator meteorSpawnLoop ()
    {
        var meteor = Spawns.GetNext();
        bool success = trySpawnMeteor(meteor);

        while (!success)
        {
            success = trySpawnMeteor(meteor);
            yield return null;
        }
    }

    bool trySpawnMeteor (Orbiter meteor)
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

        var rad = meteor.GetComponent<CircleCollider2D>().radius;
        Vector2 meteorPos = edgeLocWorld + cardinal * rad * 2;

        if (Physics2D.CircleCast(meteorPos, rad, Vector2.zero)) return false;

        Instantiate(meteor, meteorPos, Quaternion.identity);
        return true;
    }
}
