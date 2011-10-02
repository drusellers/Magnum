﻿namespace Magnum.Routing.Engine.Nodes
{
    using System;
    using System.Diagnostics;


    /// <summary>
    /// Captures only ONE segment's value
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    [DebuggerDisplay("Captures Segment: Pos {_position} As {_key}")]
    public class CaptureSegmentValueNode<TContext> : 
        ActivationNode<TContext>,
        Activation<TContext>
    {
        readonly Func<long> _generateId;
        string _key;

        public CaptureSegmentValueNode(string key, Func<long> generateId)
        {
            _key = key;
            _generateId = generateId;
        }

        public void Activate(RouteContext<TContext> context, string value)
        {
            //we found a uri segment at _position, so continue
            context.Data[_key] = value;

            Next(context, value);
        }
    }
}