using UnityEngine;
using System.Collections;

public class DirectionTargeter : Targeter {

    protected override IEnumerator InternalExecuteRoutine(Effector effect, Result<bool> result) {
        DirectionCursor cursor = controller.SpawnDirCursor(actor.position);

        yield return cursor.SelectAdjacentUnitRoutine
    }
}
