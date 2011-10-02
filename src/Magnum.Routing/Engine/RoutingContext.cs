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
namespace Magnum.Routing.Engine
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Model;


    /// <summary>
    /// The built in routing context used by magnum.routing.
    /// </summary>
    [DebuggerDisplay("Context:{_uri}")]
	public class RoutingContext<TContext> :
		RouteContext<TContext>
	{
		readonly IList<Action> _actions;
		readonly TContext _context;
		readonly HashSet<long> _rights; //TODO: this isn't really used - as in nothing is reading it.
		readonly IList<Route<TContext>> _routes;
		readonly string[] _segments;
		readonly Uri _uri;

		public RoutingContext(TContext context, Uri uri)
		{
			_context = context;
			_uri = uri;

			_segments = _uri.Segments;
		    Data = new Dictionary<string, object>();

			_rights = new HashSet<long>();
			_routes = new List<Route<TContext>>();
			_actions = new List<Action>();
		}

		public RouteMatch<TContext> Match
		{
			get
			{
				if (_routes.Count == 0)
					return null;

			    var vars = Data.Select(x => new RouteVariableImpl(x.Key, x.Value));
				return new RouteMatchImpl<TContext>(_context, _routes[0], new RouteVariablesImpl(vars));
			}
		}

		public TContext Context
		{
			get { return _context; }
		}

        public IDictionary<string, object> Data { get; set; }

        public void AddRightActivation(long id)
		{
			_rights.Add(id);
		}

		public bool HasRightActivation(long id)
		{
			return _rights.Contains(id);
		}

		public void AddRoute(Route<TContext> route)
		{
			_routes.Add(route);
		}

		public void AddAction(Action action)
		{
			_actions.Add(action);
		}

		public string Segment(int position)
		{
			if (position < 0)
				throw new ArgumentOutOfRangeException("position");

			if (position >= _segments.Length)
				return null;

			return _segments[position] ?? "";
		}

		public void Resolve()
		{
			for (int i = 0; i < _actions.Count; i++)
				_actions[i]();
		}
	}


    public class RouteVariableImpl :
        RouteVariable
    {
        public RouteVariableImpl(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }

    }
}