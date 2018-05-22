using Microsoft.AspNetCore.Authorization;

namespace AspNetCore2AuthNZ
{
    internal class MinExistingOrderRequirement : IAuthorizationRequirement
    {
        public int Count { get; }

        public MinExistingOrderRequirement(int count)
        {
            Count = count;
        }
    }
}