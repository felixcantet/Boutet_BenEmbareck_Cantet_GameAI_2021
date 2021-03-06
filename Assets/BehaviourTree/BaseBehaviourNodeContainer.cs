using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BehaviourTree
{

    /// <summary>
    /// Base class for every Container node like Sequence or Selector
    /// </summary>
    [System.Serializable]
    public abstract class BaseBehaviourNodeContainer : BaseBehaviourNode
    {
        // Contains all nodes of the Container Node
        // Nodes have to be in the right order (0 to N)
        [SerializeField] protected List<BaseBehaviourNode> nodes = new List<BaseBehaviourNode>();
        public List<BaseBehaviourNode> Nodes
        {
            get { return this.nodes; }
            set { this.nodes = value; }
        }
        
        
        public override void OnStart()
        {
            foreach (var n in nodes)
            {
                n.OnStart();
                n.tree = this.tree;
            }
        }

        public override void OnReset()
        {
            this.state = NodeState.NotExecuted;
            foreach (var n in nodes)
                n.OnReset();
        }

        protected abstract override void OnEnter();

        protected abstract override void OnExit();

        public abstract override Task<NodeState> Process();

        
        public BaseBehaviourNodeContainer(string nName = "node", BaseBehaviourNode child = null) : base(null, nName, child)
        {
            
        }

        /// <summary>
        /// Add a node to the specific sibbling index if possible
        /// else, add back
        /// </summary>
        /// <param name="node"></param>
        /// <param name="siblingIndex"></param>
        public void Add(BaseBehaviourNode node, int siblingIndex = -1)
        {
            Assert.IsNotNull(node, "Add null node to a container is not available !!");
            
            if(siblingIndex + 1 == nodes.Count && nodes.Count > 0)
                nodes.Insert(siblingIndex, node);
            else
                nodes.Add(node);
        }
    }
}
