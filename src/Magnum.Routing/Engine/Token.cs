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
    using System.Diagnostics;

    /// <summary>
    /// A key value pair - to store found items in the network
    /// like route variables. But we want a read only model.
    /// currently we are enforcing that through an interface.
    /// this may get removed.
    /// </summary>
    public interface Token
	{
		string Name { get; }
		string Value { get; }
	}

    [DebuggerDisplay("Token:{Name}:{Value}")]
	class SimpleToken :
		Token
	{
		public string Name { get; set; }
		public string Value { get; set; }

        public bool Equals(SimpleToken other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(other.Name, Name) && Equals(other.Value, Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(SimpleToken))
                return false;
            return Equals((SimpleToken)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
	}
}