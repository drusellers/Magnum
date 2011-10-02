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
namespace Magnum.Routing.Configuration
{
    using System;
    using System.Linq;
    using Model;


    public class UrlPatternParser
	{
		const char BeginParameter = '{';
        string EncodedBeginParameter = "%7B";
		const char EndParameter = '}';
        string EncodedEndParameter = "%7D";
		const char SegmentSeparator = '/';

		public RouteSpecification Parse(UrlPattern pattern)
		{
		    var spec = new RouteSpecification();
		    int i = 0;
		    foreach (var segment in pattern.GetSegments().Skip(1)) //skipping the root '/'
		    {
		        i = i + 1;
		        var s = segment.Replace("/","");
                //this needs to be cleaned up.
		        if(s.StartsWith(EncodedBeginParameter))
		        {
                    //strip off encoded { }
		            s = s.Replace(EncodedBeginParameter,"").Replace(EncodedEndParameter,"");
		            spec.AddParameter(new VariableRouteParameter(i, s));
		        }
                else
		        {
		            spec.AddParameter(new StaticRouteParameter(i, s));
		        }
		    }

			return spec;
		}

		void ParsePattern(string value, ref int index, ref int segment, RouteSpecification spec)
		{
			int length = value.Length;

			while (index < length)
			{
				char ch = value[index];

				if (ch == BeginParameter)
				{
					bool doubled = index + 1 < length && value[index + 1] == BeginParameter;
					if (doubled)
						index++;
					else
						ParseParameter(value, ref index, spec);
				}
				else if (ch == SegmentSeparator)
					segment++;

				index++;
			}
		}

		void ParseParameter(string value, ref int index, RouteSpecification spec)
		{
			if (value[index] != BeginParameter)
				return;

			int start = index + 1;
			int length = value.Length;

			if(index + 1 < length && value[index+1] == EndParameter)
				throw new ArgumentException("A parameter name must not be empty");

			while (++index < length)
			{
				if (value[index] == EndParameter)
				{
					string name = value.Substring(start, index - start);
				    spec.AddParameter(new VariableRouteParameter(1, name));
					return;
				}

				if (value[index] == SegmentSeparator)
				{
					throw new ArgumentException("A parameter name must not contain a separator");
				}
			}

			throw new ArgumentException("A parameter name must be properly terminated");
		}
	}
}