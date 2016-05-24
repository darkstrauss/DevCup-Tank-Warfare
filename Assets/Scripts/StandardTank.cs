using UnityEngine;
using System.Collections;

public class StandardTank : Tank {

	// Use this for initialization
	void Start () {
	
	}

    protected override void AimAt(Vector3 target)
    {
        base.AimAt(target);
    }

    protected override void Fire(Vector3 to)
    {
        base.Fire(to);
    }

    protected override void Move(Vector3 goal)
    {
        base.Move(goal);
    }

    protected override void Update()
    {
        base.Update();
    }
}
