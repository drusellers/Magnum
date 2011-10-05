namespace Magnum.Routing.Specs
{
    using System;
    using System.Linq;
    using Engine;
    using Engine.Nodes;
    using Model;
    using NUnit.Framework;
    using TestFramework;


    public class Join_Var_Specs
    {
        long _id = 1;

        [Test]
        public void FirstTestName()
        {
            var route = new RouteNode<Uri>(new StubRoute<Uri>());

            var joinNode = new JoinNode<Uri>(_id++, new ConstantNode<Uri>());
            joinNode.AddSuccessor(route);

            var alpha = new AlphaNode<Uri>(_id++);
            alpha.AddSuccessor(joinNode);

            var captureNode = new CaptureSegmentValueNode<Uri>("version", () => _id++);
            captureNode.AddSuccessor(alpha);

            var segment = new SegmentNode<Uri>(1);
            segment.AddSuccessor(captureNode);

            var engine = new MagnumRoutingEngine<Uri>(x => x);
            engine.Match<RootNode<Uri>>().Single().AddSuccessor(segment);

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
        [Test]
        public void TwoCaptures()
        {
            //build engine
            var engine = new MagnumRoutingEngine<Uri>(x => x);

            //build up route 1 test
            //     http://locahost/1
            var routeA = new StubRoute<Uri>("a");
            var route = new RouteNode<Uri>(routeA);

            var joinNode = new JoinNode<Uri>(_id++, new ConstantNode<Uri>());
            joinNode.AddSuccessor(route);

            var alpha = new AlphaNode<Uri>(_id++);
            alpha.AddSuccessor(joinNode);

            var captureNode = new CaptureSegmentValueNode<Uri>("version", () => _id++);
            captureNode.AddSuccessor(alpha);

            var segment = new SegmentNode<Uri>(1);
            segment.AddSuccessor(captureNode);

            engine.Root.AddSuccessor(segment);

            //build up route two
            //    http://localhost/aa/edit

            var routeB = new StubRoute<Uri>("b");
            var route2 = new RouteNode<Uri>(routeB);


            //*************************
            var joinNodeBB = new JoinNode<Uri>(_id++, new ConstantNode<Uri>());

            var alphaBB = new AlphaNode<Uri>(_id++);
            alphaBB.AddSuccessor(joinNodeBB);

            var equalNodeBB = new EqualNode<Uri>(() => _id++);
            equalNodeBB.Add("bb", alphaBB);

            //shared node
            segment.AddSuccessor(equalNodeBB);


            //now start building up the next segment piece
            var joinNode2 = new JoinNode<Uri>(_id++, alphaBB);
            joinNode2.AddSuccessor(route2);

            var alpha2 = new AlphaNode<Uri>(_id++);
            alpha2.AddSuccessor(joinNode2);

            var equalNode2 = new EqualNode<Uri>(() => _id++);
            equalNode2.Add("edit", alpha2);

            var segment2 = new SegmentNode<Uri>(2);
            segment2.AddSuccessor(equalNode2);

            engine.Root.AddSuccessor(segment2);

            bool called = false;
            RouteVariable value = null;

            var uri = new Uri("http://localhost/aa/edit");
            Route selectedRoute = null;
            engine.Route(uri, x =>
                {
                    called = true;
                    selectedRoute = x.Route;
                    value = x.Data["version"];
                });

            called.ShouldBeTrue();
            selectedRoute.ShouldEqual(routeA); //because the route was 'aa' not 'bb';
            //in this one there should be no match
            //value.Value.ShouldEqual("a");
            //but since the context is shared... :(
            //maybe put the context on each route?
        }
    }
}