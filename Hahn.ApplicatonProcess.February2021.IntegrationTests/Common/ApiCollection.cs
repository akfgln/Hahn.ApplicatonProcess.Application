using Xunit;

namespace Hahn.ApplicatonProcess.February2021.IntegrationTests.Common
{
    [CollectionDefinition("ApiCollection")]
    public class DbCollection : ICollectionFixture<ApiServer>
    {
    }
}