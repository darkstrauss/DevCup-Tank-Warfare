using UnityEngine;
using System.Collections;

public class StandardTank : Tank {

	// Use this for initialization
	void Start () {
	
	}

    protected override void AimAt(Vector3 lookAtTarget)
    {
        base.AimAt(lookAtTarget);
    }

    protected override void Fire()
    {
        base.Fire();
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
