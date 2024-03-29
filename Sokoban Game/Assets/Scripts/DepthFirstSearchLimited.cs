using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DepthFirstSearchLimited : SearchAlgorithm {

	private Stack<SearchNode> stack = new Stack<SearchNode>();
	private HashSet<object> closedSet = new HashSet<object> ();
	public int maximumDepth;

	void Start ()
	{
		problem = GameObject.Find ("Map").GetComponent<Map> ().GetProblem();
		SearchNode start = new SearchNode (problem.GetStartState (), 0);
		stack.Push(start);
	}

	protected override void Step()
	{
		if (stack.Count > 0)
		{
			SearchNode cur_node = stack.Pop();
			closedSet.Add (cur_node.state);

			if (problem.IsGoal (cur_node.state)) {
				solution = cur_node;
				finished = true;
				running = false;
			} else if(cur_node.depth < maximumDepth){
				Successor[] sucessors = problem.GetSuccessors (cur_node.state);
				foreach (Successor suc in sucessors) {
					if (!closedSet.Contains (suc.state)) {
						SearchNode new_node = new SearchNode (suc.state, suc.cost + cur_node.g, suc.action, cur_node);
						stack.Push(new_node);
					}
				}
			}
		}
		else
		{
			finished = true;
			running = false;
		}
	}
}
