using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AprofundamentoProgressivo : SearchAlgorithm {

    private Stack<SearchNode> stack = new Stack<SearchNode>();
    private HashSet<object> closedSet = new HashSet<object> ();
    public int maximumDepth;
    // Use this for initialization
    void Start () {
	problem = GameObject.Find ("Map").GetComponent<Map> ().GetProblem();

    }

    protected override void Step()
    {
	while(!depthFirstSearch()){
	    maximumDepth++;
	}

    }
    
    bool depthFirstSearch()
    {
	closedSet.Clear();
	stack.Clear();
	
	SearchNode start = new SearchNode (problem.GetStartState (), 0);
	stack.Push(start);
	while (stack.Count > 0)
	    {
		SearchNode cur_node = stack.Pop();
		closedSet.Add (cur_node.state);

		if (problem.IsGoal (cur_node.state)) {
		    solution = cur_node;
		    finished = true;
		    running = false;
		    return true;
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

	finished = true;
	running = false;

	return false;
    }


}
