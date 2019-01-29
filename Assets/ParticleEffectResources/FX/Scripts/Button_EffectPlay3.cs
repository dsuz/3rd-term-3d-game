using UnityEngine;
using System.Collections;

public class Button_EffectPlay3 : MonoBehaviour {

	public GameObject Player;
	public GameObject Enemy;
	public GameObject Effect;

	void OnGUI () {
 		if(GUI.Button(new Rect(20,40,80,20), " Play")) {
			if(Player != null) {
				Player.GetComponent<Animation>().Play();
			}
			StartCoroutine("EnemyAnimPlay",  0.3f);
		}
	}
	private IEnumerator EnemyAnimPlay(float time)
	{
		yield return new WaitForSeconds(time);
		if(Enemy != null){
			Enemy.GetComponent<Animation>().Play();
		}
		if(Effect != null){
			Effect.active = false;
			Effect.active = true ;
		}
	} 

}