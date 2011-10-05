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
	using System;
	using System.Diagnostics;


    /// <summary>
	/// Matches segment content and if they 'equal', it passes to the next condition
    /// </summary>
    [DebuggerDisplay("Equals: {KeyList}")]
	public class EqualNode<TContext> :
		DictionaryNode<TContext>
	{
		readonly Func<long> _generateId;

		public EqualNode(Func<long> generateId)
		{
			_generateId = generateId;
		}

		public override void Activate(RouteContext<TContext> context, string value)
		{
            //calls to the base DictionaryNode to perform the equals. odd.
			Next(value, context, value);
		}

		public void Add(string value, Activation<TContext> activation)
		{
			Add(value, activation, _generateId);
		}
	}
}