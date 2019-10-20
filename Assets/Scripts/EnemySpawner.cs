using System;
using crass;

[Serializable]
public class EnemyBag : BagRandomizer<EnemyShip> {}

public class EnemySpawner : OffscreenSpawner<EnemyShip, EnemyBag> {}
