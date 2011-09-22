The routing lib is built on the Rete algorithm.
http://en.wikipedia.org/wiki/Rete_algorithm

This is a specialized version of the Rete algorithm
which is why we are not currently reusing an already
built lib. 

NOTE: The rete algorithem trades memory for speed.

Our version does the following things differently.
- The network should be stateless, and instead store 'memory' in the passed context
- - this is done, because as a routing lib we don't want the network to remember things.