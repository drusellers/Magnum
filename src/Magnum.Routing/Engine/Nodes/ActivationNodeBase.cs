// Copyright 2007-2010 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.Routing.Engine.Nodes
{
	using System.Collections.Generic;
	using System.Linq;

	public abstract class ActivationNodeBase<TContext> :
		Node<TContext>,
        Activation<TContext>
	{
		readonly IList<Activation<TContext>> _successors;

		protected ActivationNodeBase()
		{
			_successors = new List<Activation<TContext>>();
		}

		protected ActivationNodeBase(IEnumerable<Activation<TContext>> successors)
		{
			_successors = new List<Activation<TContext>>(successors);
		}

		protected ActivationNodeBase(params Activation<TContext>[] successors)
		{
			_successors = new List<Activation<TContext>>(successors);
		}

        /// <summary>
        /// This method activates child nodes.
        /// </summary>
		protected void ActivateSuccessors(RouteContext<TContext> context, string value)
		{
			// not using LINQ, since the performance is much slower than iterating the list directly
			for (int i = 0; i < _successors.Count; i++)
				_successors[i].Activate(context, value);
		}

		public void AddSuccessor(Activation<TContext> successor)
		{
			_successors.Add(successor);
		}

		public IEnumerable<T> Match<T>()
			where T : class
		{
			if (typeof(T) == GetType())
				return ThisAsEnumerable<T>();

			return NextAsEnumerable<T>();
		}

		IEnumerable<T> ThisAsEnumerable<T>()
			where T : class
		{
			yield return this as T;
		}

		IEnumerable<T> NextAsEnumerable<T>()
			where T : class
		{
			return _successors.SelectMany(activation => activation.Match<T>());
		}

        /// AKA "Left Activation" 
	    public abstract void Activate(RouteContext<TContext> context, string value);

	}
}