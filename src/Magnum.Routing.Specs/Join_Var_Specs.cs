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

            var engine = new MagnumRoutingEngine<Uri>(x => x);
            engine.Match<RootNode<Uri>>().Single().AddSuccessor(segment);

            bool called = false;
            RouteVariable value = null;

            var uri = new Uri("http://localhost/1");
            engine.Route(uri, x =>
                {
                    called = true;
                    x.Route.ShouldEqual(routeA);
                    value = x.Data["version"];
                });

            called.ShouldBeTrue();
            value.Value.ShouldEqual("1");
        }



        [Test]
        public void MoreComplex()
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

            var captureNode = new EqualNode<Uri>(() => _id++);
            captureNode.AddCheck("1", alpha);

            var seg1 = new SegmentNode<Uri>(1);
            seg1.AddSuccessor(captureNode);

            engine.Root.AddSuccessor(seg1);

            //build up route two
            //    http://localhost/bb/show

            var routeB = new StubRoute<Uri>("b");
            var routeNodeB = new RouteNode<Uri>(routeB);

            var editAlpha = new AlphaNode<Uri>(_id++);

            var equalShow = new EqualNode<Uri>(() => _id++);
            equalShow.AddCheck("show", editAlpha);

            var seg2 = new SegmentNode<Uri>(2);
            seg2.AddSuccessor(equalShow);

            engine.Root.AddSuccessor(seg2);

            var j2 = new JoinNode<Uri>(_id++, editAlpha);
            j2.AddSuccessor(routeNodeB);



            var bbAlpha = new AlphaNode<Uri>(_id++);
            bbAlpha.AddSuccessor(j2);

            var equalBb = new EqualNode<Uri>(() => _id++);
            equalBb.AddCheck("bb", bbAlpha);

            engine.Match<SegmentNode<Uri>>().First(x=>x.Position == 1)
                .AddSuccessor(equalBb);



            var called = false;
            var uri = new Uri("http://localhost/bb/show");
            Route selectedRoute = null;
            engine.Route(uri, x =>
                {
                    called = true;
                    selectedRoute = x.Route;
                });

            called.ShouldBeTrue();
            selectedRoute.ShouldEqual(routeB); //because the route was 'aa' not 'bb';
        }
    }
}