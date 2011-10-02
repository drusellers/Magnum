namespace Magnum.Routing.Specs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Engine;
	using Engine.Nodes;
	using Model;
	using NUnit.Framework;
	using TestFramework;


	[TestFixture]
	public class Join_Specs
	{
		long _id = 1;

		[Test]
		public void FirstTestName()
		{
			var route = new RouteNode<Uri>(new StubRoute<Uri>());

			var joinNode = new JoinNode<Uri>(_id++, new ConstantNode<Uri>());
			joinNode.Add(route);

			var alpha = new AlphaNode<Uri>(_id++);
			alpha.Add(joinNode);

			var equal = new EqualNode<Uri>(() => _id++);
			equal.Add("version", alpha);

			var segment = new SegmentNode<Uri>(1);
			segment.Add(equal);

			var engine = new MagnumRoutingEngine<Uri>(x => x);
			engine.Match<RootNode<Uri>>().Single().Add(segment);

			bool called = false;

			var uri = new Uri("http://localhost/version");
			engine.Route(uri, x =>
				{
					called = true;
				});

			called.ShouldBeTrue();
		}
	}

	public class Join_Var_Specs
	{
		long _id = 1;

		[Test]
		public void FirstTestName()
		{
			var route = new RouteNode<Uri>(new StubRoute<Uri>());

			var joinNode = new JoinNode<Uri>(_id++, new ConstantNode<Uri>());
			joinNode.Add(route);

			var alpha = new AlphaNode<Uri>(_id++);
			alpha.Add(joinNode);

			var captureNode = new CaptureSegmentValueNode<Uri>("version", () => _id++);
			captureNode.Add(alpha);

			var segment = new SegmentNode<Uri>(1);
			segment.Add(captureNode);

			var engine = new MagnumRoutingEngine<Uri>(x => x);
			engine.Match<RootNode<Uri>>().Single().Add(segment);

			bool called = false;
		    RouteVariable value = null;

			var uri = new Uri("http://localhost/1");
			engine.Route(uri, x =>
				{
					called = true;

				    value = x.Data["version"];
				});

			called.ShouldBeTrue();
		    value.Value.ShouldEqual("1");
		}
	}
}
