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
            joinNode.AddActivation(route);

            var alpha = new AlphaNode<Uri>(_id++);
            alpha.AddActivation(joinNode);

            var captureNode = new CaptureSegmentValueNode<Uri>("version", () => _id++);
            captureNode.AddActivation(alpha);

            var segment = new SegmentNode<Uri>(1);
            segment.AddActivation(captureNode);

            var engine = new MagnumRoutingEngine<Uri>(x => x);
            engine.Match<RootNode<Uri>>().Single().AddActivation(segment);

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
            var route = new RouteNode<Uri>(new StubRoute<Uri>("1"));

            var joinNode = new JoinNode<Uri>(_id++, new ConstantNode<Uri>());
            joinNode.AddActivation(route);

            var alpha = new AlphaNode<Uri>(_id++);
            alpha.AddActivation(joinNode);

            var captureNode = new CaptureSegmentValueNode<Uri>("version", () => _id++);
            captureNode.AddActivation(alpha);

            var segment = new SegmentNode<Uri>(1);
            segment.AddActivation(captureNode);

            engine.Root.AddActivation(segment);

            //build up route two
            //    http://localhost/aa/edit
            
            var route2 = new RouteNode<Uri>(new StubRoute<Uri>("b"));


            //*************************
            var joinNodeAA = new JoinNode<Uri>(_id++, new ConstantNode<Uri>());

            var alphaAA = new AlphaNode<Uri>(_id++);
            alphaAA.AddActivation(joinNodeAA);

            var equalNodeAA = new EqualNode<Uri>(() => _id++);
            equalNodeAA.Add("aa", alphaAA);

            //shared node
            segment.AddActivation(equalNodeAA);


            //now start building up the next segment piece
            var joinNode2 = new JoinNode<Uri>(_id++, alphaAA);
            joinNode2.AddActivation(route2);

            var alpha2 = new AlphaNode<Uri>(_id++);
            alpha2.AddActivation(joinNode2);

            var equalNode2 = new EqualNode<Uri>(() => _id++);
            equalNode2.Add("edit", alpha2);

            var segment2 = new SegmentNode<Uri>(2);
            segment2.AddActivation(equalNode2);

            engine.Root.AddActivation(segment2);

            bool called = false;
            RouteVariable value = null;

            var uri = new Uri("http://localhost/a/edit");
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