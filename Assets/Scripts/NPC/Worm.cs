using Pathfinding;
using System.Collections;
using UnityEngine;

public class Worm : MonoBehaviour
{
	[SerializeField] AnimationClip ExitClip;

	private GridGraph grid;
	private GraphNode randomNode;
	private Vector3 destination1;
	private bool reroute;
	private Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		grid = AstarPath.active.data.gridGraph;
		randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
		destination1 = (Vector3)randomNode.position;
		gameObject.transform.position = destination1;

		reroute = true;
	}

	void Update()
	{
		if (transform.position == destination1 && reroute)
		{
			StartCoroutine(moveWorm());
			reroute = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//SoundManager.instance.EnemyDeathSound(hatchClip);
			//GameController.instance.endGame();
			Debug.Log("End");
			LevelManager.instance.LoadLevel(LevelManager.LoseLevelString);
		}
	}

	IEnumerator moveWorm()
	{
		randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
		destination1 = (Vector3)randomNode.position;
		gameObject.transform.position = destination1;
		animator.Play("entrance");



		yield return new WaitForSeconds(8);
		animator.SetTrigger("Exit");
		yield return new WaitForSeconds(ExitClip.length);
		reroute = true;
	}
}
