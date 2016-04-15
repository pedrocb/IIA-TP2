using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DepthLimitedSearch : SearchAlgorithm {

	/*private Queue<SearchNode> openQueue = new Queue<SearchNode> ();*/
	private HashSet<object> closedSet = new HashSet<object> ();
	public int depth_limit;
	private Stack<object> stack = new Stack<object> ();

	void Start () 
	{
		problem = GameObject.Find ("Map").GetComponent<Map> ().GetProblem();
		SearchNode start = new SearchNode (problem.GetStartState (), 0);
		/*openQueue.Enqueue (start);*/
		stack.Push(start);
	}

	protected override void Step()
	{
		if (stack.Count > 0) { // Enquanto a stack tiver elementos
			SearchNode cur_node = (SearchNode) stack.Pop (); // Retorna e remove o primeiro elemento da lista
			closedSet.Add (cur_node.state); // mete-o na lista de elementos visitados

			if (problem.IsGoal (cur_node.state)) { // se tiver no nó final
				solution = cur_node; // grava a solução
				finished = true;
				running = false;
			} else if (cur_node.depth >= depth_limit) {
				//nada acontece

			} else {
				Successor[] sucessors = problem.GetSuccessors (cur_node.state);// vai buscar a lista do nós que se sucedem
				foreach (Successor suc in sucessors) { // Enquanto tiver vertices seguintes
					if (!closedSet.Contains (suc.state)) { // Se ainda tiver por visitar
						SearchNode new_node = new SearchNode (suc.state, suc.cost + cur_node.g, suc.action, cur_node); 
						stack.Push (new_node); // cria um nó e adiciona-o à stack
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
