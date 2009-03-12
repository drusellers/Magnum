// Copyright 2007-2008 The Apache Software Foundation.
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
namespace Magnum.Specs.Reflection
{
	using System;
	using System.Diagnostics;
	using Magnum.Reflection;
	using MbUnit.Framework;

	[TestFixture]
	public class When_invoking_a_generic_method
	{
		[Test]
		public void Invoking_the_method_directly_should_pass_the_appropriate_type()
		{
			MyMethod(new MyClass());
		}

		[Test]
		public void Invoking_with_an_object_should_not_properly_initialize_T()
		{
			object obj = ClassFactory.New(typeof (MyClass));

			MyOtherMethod(obj);
		}

		[Test]
		public void Invoking_using_the_generic_method_invoker_should_pass_the_appropriate_type()
		{
			object obj = ClassFactory.New(typeof(MyClass));

			Generic.Call(x => MyMethod(x), obj);
		}


		[Test]
		public void Invoking_using_the_generic_method_invoker_should_pass_the_appropriate_type_for_value_types()
		{
			Generic.Call(x => MyIntMethod(x), 27);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Invoking_a_regular_method_should_not_work_too()
		{
			object obj = ClassFactory.New(typeof (MyClass));

			Generic.Call(x => RegularMethod(x), obj);
		}

		[Test]
		public void Invoking_it_a_lot_should_be_fast()
		{
			object obj = ClassFactory.New(typeof(MyClass));

			Generic.Call(x => MyMethod(x), obj);

			Stopwatch count = Stopwatch.StartNew();
			for (int i = 0; i < 10000; i++)
			{
				Generic.Call(x => MyMethod(x), obj);
			}
			count.Stop();

			Console.WriteLine("time to run = " + count.ElapsedMilliseconds + "ms");
		}

		public void MyMethod<T>(T obj)
		{
			obj.GetType().ShouldEqual(typeof (MyClass));
			typeof (T).ShouldEqual(typeof (MyClass));
		}

		public void MyIntMethod<T>(T obj)
		{
			obj.GetType().ShouldEqual(typeof (int));
			typeof (T).ShouldEqual(typeof (int));
		}

		public void MyOtherMethod<T>(T obj)
		{
			obj.GetType().ShouldEqual(typeof (MyClass));
			typeof (T).ShouldEqual(typeof (object));
		}

		public void RegularMethod(object obj)
		{
		}

		public class MyClass
		{
		}
	}
}