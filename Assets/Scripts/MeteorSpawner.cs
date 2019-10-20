using System;
using crass;

[Serializable]
public class MeteorBag : BagRandomizer<Orbiter> {}

public class MeteorSpawner : OffscreenSpawner<Orbiter, MeteorBag> {}
