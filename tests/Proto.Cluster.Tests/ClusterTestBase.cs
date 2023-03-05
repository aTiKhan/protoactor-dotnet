﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Proto.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Proto.Cluster.Tests;

[Collection("ClusterTests")]
public abstract class ClusterTestBase
{
    private readonly string _runId;
    protected readonly IClusterFixture ClusterFixture;

    protected ClusterTestBase(IClusterFixture clusterFixture)
    {
        ClusterFixture = clusterFixture;
        _runId = Guid.NewGuid().ToString("N").Substring(0, 6);
    }

    protected Task Trace(Func<Task> test, ITestOutputHelper output, [CallerMemberName]string testName = "")
    {
        return ClusterFixture.Trace(test, testName);
    }

    protected LogStore LogStore => ClusterFixture.LogStore;

    protected IList<Cluster> Members => ClusterFixture.Members;

    /// <summary>
    ///     To be able to re-use the fixture across tests, we can make sure the other test identities do not collide
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    protected string CreateIdentity(string baseId) => $"{_runId}-{baseId}-";

    protected IEnumerable<string> GetActorIds(int count) =>
        Enumerable.Range(1, count)
            .Select(i => CreateIdentity(i.ToString()));
}